using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.WinForms.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class UserPermissionEditForm : MesWindowForm
    {
        private readonly UserModuleFacade _facade;
        private int _targetUserId;
        private List<MenuDto> _allMenus;

        // 构造函数注入 Facade
        public UserPermissionEditForm(UserModuleFacade facade)
        {
            _facade = facade;
            InitializeComponent();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData(int userId, string userName)
        {
            _targetUserId = userId;
            lblTitle.Text = $"分配权限 - {userName}";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // 使用 RunAsync 自动处理 Loading
            await RunAsync(async () =>
            {
                await LoadMenuTreeAsync();
            });
        }

        private async Task LoadMenuTreeAsync()
        {
            // 1. 并行获取“所有菜单”和“用户已有权限”
            //var allMenusTask = _facade.MenuService.GetAllAsync(); // 假设获取扁平列表或树形结构
            //var userMenusTask = _facade.PermissionService.GetMenuIdsByUserIdAsync(_targetUserId); // 获取用户已有的菜单ID集合

            //await Task.WhenAll(allMenusTask, userMenusTask);

            //_allMenus = allMenusTask.Result;
            //var userMenuIds = userMenusTask.Result ?? new List<int>();

            //// 2. 构建 UI 树
            //menuTree.Items.Clear();

            //// 假设 _allMenus 是扁平的，我们需要构建树 (如果是树形结构则直接递归)
            //// 这里假设后端返回的是树形结构 List<MenuDto>，其中包含 Children
            //// 如果是扁平结构，需要先 BuildTree
            //var rootMenus = BuildTree(_allMenus);

            //foreach (var menu in rootMenus)
            //{
            //    menuTree.Items.Add(CreateTreeItem(menu, userMenuIds));
            //}

            //// 展开所有节点
            //menuTree.ExpandAll();
        }

        // 递归创建 TreeItem
        private TreeItem CreateTreeItem(MenuDto menu, List<int> userMenuIds)
        {
            var item = new TreeItem
            {
                Text = menu.Name,
                Tag = menu.Id, // 绑定 ID
                Checked = userMenuIds.Contains(menu.Id) // 回显选中状态
            };

            if (menu.Children != null && menu.Children.Any())
            {
                foreach (var child in menu.Children)
                {
                    item.Sub.Add(CreateTreeItem(child, userMenuIds));
                }
            }

            return item;
        }

        // 简单的扁平转树形辅助方法 (如果后端已经是树形则不需要)
        private List<MenuDto> BuildTree(List<MenuDto> flatMenus)
        {
            // 简单实现：查找 ParentId 为空或0的作为根
            var lookup = flatMenus.ToLookup(x => x.ParentId);
            foreach (var menu in flatMenus)
            {
                if (lookup.Contains(menu.Id))
                    menu.Children = lookup[menu.Id].ToList();
            }
            return lookup[null].Concat(lookup[0]).ToList();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            //await RunAsync(btnSave, async () =>
            //{
            //    // 1. 获取所有选中的 ID
            //    var checkedIds = GetCheckedIds(menuTree.Items);

            //    // 2. 调用服务保存
            //    await _facade.PermissionService.SaveUserMenusAsync(_targetUserId, checkedIds);

            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}, "权限保存成功！");
        }

        // 递归获取选中项 ID
        private List<int> GetCheckedIds(TreeItemCollection items)
        {
            var ids = new List<int>();
            foreach (var item in items)
            {
                if (item.Checked && item.Tag is int id)
                {
                    ids.Add(id);
                }
                if (item.Sub != null && item.Sub.Count > 0)
                {
                    ids.AddRange(GetCheckedIds(item.Sub));
                }
            }
            return ids;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
