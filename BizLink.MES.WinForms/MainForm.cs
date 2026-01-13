using AntdUI;
using AutoUpdaterDotNET;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Forms;
using BizLink.MES.WinForms.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AntdUI.Tabs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using MenuItem = AntdUI.MenuItem; // <--- 添加这一行别名


namespace BizLink.MES.WinForms
{
    public partial class MainForm : MesWindowForm
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFormFactory _formFactory; // 用于打开模态对话框（如修改密码）

        private System.Windows.Forms.Timer _updateTimer;
        private const string UpdateXmlUrl = "http://svcn5mesp01:8001/update.xml";

        private Dictionary<MenuItem, MenuItem> _menuParentMap;

        // 2. 构造函数：只注入基础设施，不注入业务 Service/Facade
        // 因为 MainForm 是长生命周期的，直接注入 Scoped 业务服务会导致连接过期
        public MainForm(IServiceProvider serviceProvider, IFormFactory formFactory)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _formFactory = formFactory;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            // --- AutoUpdater 配置 ---
            AutoUpdaterDotNET.AutoUpdater.Synchronous = true;
            AutoUpdaterDotNET.AutoUpdater.ShowSkipButton = false;
            AutoUpdaterDotNET.AutoUpdater.RunUpdateAsAdmin = false;

            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = (int)TimeSpan.FromHours(1).TotalMilliseconds;
            _updateTimer.Tick += (s, args) => AutoUpdaterDotNET.AutoUpdater.Start(UpdateXmlUrl);
            _updateTimer.Start();

            // --- 版本信息 ---
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
            {
                pageHeader1.SubText = $"Version:{version}";
            }

            // --- 用户与工厂信息 ---
            labelUserInfo.Text = $"当前用户：{AppSession.CurrentUser?.UserName ?? "未登录"}";
            dropDownFactory.Text = $"当前工厂：{AppSession.CurrentUser?.FactoryName}      ";

            // --- 异步加载数据 ---
            // 使用 RunAsync 包装，虽然 Load 不需要 Loading 遮罩，但可以统一处理异常
            await RunAsync(async () =>
            {
                await LoadFactoriesAsync();
                await LoadMenusAsync();

                // 初始化当前工厂选中项
                if (!string.IsNullOrEmpty(AppSession.CurrentUser?.FactoryName))
                {
                    var factoryItem = dropDownFactory.Items.Cast<MenuItem>()
                        .FirstOrDefault(x => x.Text.Equals(AppSession.CurrentUser?.FactoryName));

                    if (factoryItem != null)
                    {
                        AppSession.CurrentFactoryId = int.Parse(factoryItem.Name);
                    }
                }
            });
        }

        #region Data Loading (使用临时 Scope)

        private async Task LoadMenusAsync()
        {
            // 【关键】：创建一个临时的 Scope 来获取数据
            // 这样使用完后立即释放 DbContext，避免长期占用
            using (var scope = _serviceProvider.CreateScope())
            {
                // 从 Scope 中获取 Facade
                var facade = scope.ServiceProvider.GetRequiredService<BaseAppFacade>();

                mainMenu.Items.Clear();

                // 使用 Facade 获取菜单
                var rootMenus = await facade.PermissionService.GetMenuTreeForUserAsync(AppSession.CurrentUser.Id);

                if (rootMenus == null || !rootMenus.Any())
                {
                    AntdUI.Modal.open(this, "权限不足", "当前用户没有任何菜单权限，请联系管理员。", TType.Error);
                    return;
                }

                foreach (var menuDto in rootMenus)
                {
                    mainMenu.Items.Add(CreateMenuItem(menuDto));
                }

                _menuParentMap = new Dictionary<MenuItem, MenuItem>();
                BuildParentMapRecursive(mainMenu.Items, null);

                // 尝试打开首页
                var homeMenuItem = mainMenu.Items.Cast<MenuItem>().FirstOrDefault(item => item.Name == "HomeForm");
                if (homeMenuItem != null)
                {
                    CreateAndShowForm(homeMenuItem);
                }
            }
        }

        private async Task LoadFactoriesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var facade = scope.ServiceProvider.GetRequiredService<BaseAppFacade>();

                dropDownFactory.Items.Clear();
                var factories = await facade.FactoryService.GetAllAsync();

                // 过滤逻辑
                if (!string.IsNullOrWhiteSpace(AppSession.CurrentUser?.FactoryName))
                {
                    factories = factories.Where(f => f.FactoryName == AppSession.CurrentUser.FactoryName).ToList();
                }

                foreach (var factory in factories)
                {
                    dropDownFactory.Items.Add(new MenuItem
                    {
                        Name = factory.Id.ToString(),
                        Text = factory.FactoryName
                    });
                }
            }
        }

        #endregion

        #region Form Management (核心：Tab 页 Scope 管理)

        private void mainMenu_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            if (e.Value == null)
                return;
            // 仅处理叶子节点
            if (e.Value.Sub == null || e.Value.Sub.Count == 0)
            {
                CreateAndShowForm(e.Value);
            }
        }

        private void CreateAndShowForm(MenuItem menuItem)
        {
            // 1. 基础校验
            if (AppSession.CurrentFactoryId == 0 && menuItem.Name != "HomeForm")
            {
                AntdUI.Message.error(this, "请先选择对应工厂！");
                return;
            }

            // 2. 检查 Tab 是否已存在
            foreach (AntdUI.TabPage page in mainTabs.Pages)
            {
                if (page.Name == menuItem.Name)
                {
                    mainTabs.SelectedTab = page;
                    return;
                }
            }

            // 3. 反射查找窗体类型
            var targetAssembly = Assembly.GetExecutingAssembly();
            var formType = targetAssembly.GetTypes()
                .FirstOrDefault(t => t.Name == menuItem.Name && typeof(Form).IsAssignableFrom(t) && !t.IsAbstract);

            if (formType == null)
            {
                AntdUI.Message.error(this, $"找不到名为 '{menuItem.Name}' 的窗体类。");
                return;
            }

            // 4. 【核心】：为新 Tab 页创建一个独立的 Scope
            // 这个 Scope 将伴随 Tab 页的整个生命周期
            var tabScope = _serviceProvider.CreateScope();

            try
            {
                // 从新 Scope 中解析窗体
                // 这样窗体内部注入的 Service/Facade 都是基于这个新 Scope 的
                var form = (Form)tabScope.ServiceProvider.GetRequiredService(formType);

                // 5. 嵌入设置
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;

                // 6. 【关键】：绑定释放逻辑
                // 当窗体关闭（Tab 关闭）时，释放对应的 Scope
                form.FormClosed += (s, e) => tabScope.Dispose();

                // 7. 创建 Tab 页
                var path = BuildBreadcrumbPath(menuItem);
                var newPage = new AntdUI.TabPage
                {
                    Text = menuItem.Text,
                    Name = menuItem.Name,
                    Tag = path // 存储面包屑路径
                };

                newPage.Controls.Add(form);
                mainTabs.Pages.Add(newPage);
                mainTabs.SelectedTab = newPage;

                form.Show();
            }
            catch (Exception ex)
            {
                tabScope.Dispose(); // 创建失败要立即释放
                AntdUI.Message.error(this, $"无法打开窗体: {ex.Message}");
            }
        }

        private void dropDownFactory_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (e.Value is MenuItem item)
            {
                dropDownFactory.Text = $"当前工厂：{item.Text}      ";
                AppSession.CurrentUser.FactoryName = item.Text;
                AppSession.CurrentFactoryId = int.Parse(item.Name);

                // 切换工厂时清空所有 Tab (除了首页可选)
                mainTabs.Pages.Clear();

                // 重新打开首页
                var homeItem = mainMenu.Items.Cast<MenuItem>().FirstOrDefault(i => i.Name == "HomeForm");
                if (homeItem != null)
                    CreateAndShowForm(homeItem);
            }
        }

        #endregion

        #region UI Helpers (Menu & Breadcrumb)

        private MenuItem CreateMenuItem(MenuDto menuDto)
        {
            System.Drawing.Image icon = null;
            if (!string.IsNullOrWhiteSpace(menuDto.Icon))
            {
                var property = typeof(Properties.Resources).GetProperty(menuDto.Icon);
                if (property != null)
                    icon = property.GetValue(null) as System.Drawing.Image;

                if (icon == null)
                {
                    var resObj = Properties.Resources.ResourceManager.GetObject(menuDto.Icon);
                    if (resObj is System.Drawing.Image img)
                        icon = img;
                }
            }

            var menuItem = new MenuItem
            {
                Name = menuDto.FormName,
                Text = menuDto.Name,
                Icon = icon
            };

            if (menuDto.Children != null && menuDto.Children.Any())
            {
                foreach (var child in menuDto.Children)
                {
                    menuItem.Sub.Add(CreateMenuItem(child));
                }
            }
            return menuItem;
        }

        private void BuildParentMapRecursive(IEnumerable<MenuItem> items, MenuItem parent)
        {
            foreach (var item in items)
            {
                _menuParentMap[item] = parent;
                if (item.Sub != null && item.Sub.Count > 0)
                {
                    BuildParentMapRecursive(item.Sub, item);
                }
            }
        }

        private List<string> BuildBreadcrumbPath(MenuItem targetItem)
        {
            var path = new LinkedList<string>();
            var current = targetItem;
            while (current != null)
            {
                path.AddFirst(current.Text);
                _menuParentMap.TryGetValue(current, out current);
            }
            return path.ToList();
        }

        private void mainTabs_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            if (mainTabs.SelectedTab?.Tag is List<string> path)
            {
                breadcrumb.Items.Clear();
                foreach (var text in path)
                {
                    breadcrumb.Items.Add(new BreadcrumbItem().SetText(text));
                }
            }
            else
            {
                breadcrumb.Items = null;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _updateTimer?.Stop();
            _updateTimer?.Dispose();
        }

        #endregion
    }

    // 假设的 DTO

}
