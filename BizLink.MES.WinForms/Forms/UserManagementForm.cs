using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
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
using static AntdUI.Table;

namespace BizLink.MES.WinForms.Forms
{
    // 1. 继承 MesListForm，指定 TModel 为 UserManagementView
    //public partial class UserManagementForm : Form

    public partial class UserManagementForm : MesListForm<UserManagementView>
    {
        private readonly UserModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        // 2. 构造函数注入 Facade 和 Factory
        public UserManagementForm(UserModuleFacade facade, IFormFactory formFactory)
        {
            _facade = facade;
            _formFactory = formFactory;
            InitializeComponent();

            // 3. 【关键】将 UI 控件绑定到基类属性
            this.TableControl = userTable;
            this.PaginationControl = paginationControl;
            this.SearchButton = searchButton;
        }

        // 4. 窗体加载初始化
        protected override void OnLoad(EventArgs e)
        {
            InitializeUserTable();
            LoadStatus();
            base.OnLoad(e); // 这行会触发基类的 RefreshListAsync -> GetDataAsync
        }

        // 5. 【核心实现】基类要求的数据获取逻辑
        protected override async Task<List<UserManagementView>> GetDataAsync()
        {
            // 获取分页参数
            var pageIndex = PaginationControl?.Current ?? 1;
            var pageSize = PaginationControl?.PageSize ?? 10;
            var keyword = keywordInput.Text?.Trim();

            // 状态转换逻辑
            bool? status = statusSelect.Text switch
            {
                "已启用" => true,
                "已禁用" => false,
                _ => null
            };

            // 调用 Facade 获取数据
            // 【修改点】使用 _facade.UserService 替代旧的 _facade.User
            var result = await _facade.UserService.GetPagedListAsync(pageIndex, pageSize, keyword, status);

            // 更新分页总数
            if (result != null && PaginationControl != null)
            {
                PaginationControl.Total = result.TotalCount;
            }

            // 转换 DTO -> ViewModel
            return result?.Items.Select(u => new UserManagementView(u)).ToList() ?? new List<UserManagementView>();
        }

        // 6. 新建用户
        private void btnNewUser_Click(object sender, EventArgs e)
        {
            _formFactory.Show<UserManagementEditForm>(form =>
            {
                form.InitData(new UserDto());
                form.OnSaved += async () => await RefreshListAsync();
            }, isModal: true);
        }

        // 7. 表格操作 (编辑/删除)
        private async void userTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is UserManagementView viewData)
            {
                if (e.Btn.Id == "edit")
                {
                    _formFactory.Show<UserManagementEditForm>(form =>
                    {
                        var dto = new UserDto { Id = viewData.Id };
                        form.InitData(dto);
                        form.OnSaved += async () => await RefreshListAsync();
                    }, isModal: true);
                }
                else if (e.Btn.Id == "delete")
                {
                    // 【修复点】第一个参数传 null，表示不需要特定按钮显示 Loading 动画
                    await RunAsync(null, async () =>
                    {
                        // 【修改点】使用 _facade.UserService 替代旧的 _facade.User
                        await _facade.UserService.DeleteAsync(viewData.Id);
                        await RefreshListAsync();
                    },
                    successMsg: "删除成功",
                    confirmMsg: "是否删除本条记录？");
                }
            }
        }

        // 8. 重置按钮
        private async void resetButton_Click(object sender, EventArgs e)
        {
            keywordInput.Text = null;
            statusSelect.Text = null;
            if (PaginationControl != null)
                PaginationControl.Current = 1;
            await RefreshListAsync();
        }

        // 9. 分页变更
        private async void paginationControl_ValueChanged(object sender, PagePageEventArgs e)
        {
            await RefreshListAsync();
        }

        // 10. 搜索按钮
        private async void searchButton_Click(object sender, EventArgs e)
        {
            if (PaginationControl != null)
                PaginationControl.Current = 1;
            await RefreshListAsync();
        }

        // --- 纯 UI 辅助方法 ---

        private void LoadStatus()
        {
            statusSelect.Items.Clear();
            statusSelect.Items.AddRange(new[] { "已启用", "已禁用" });
        }

        private void InitializeUserTable()
        {
            userTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("EmployeeId", "工号", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DomainAccount", "域账号", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("UserName", "姓名", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("FactoryName", "所属工厂", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreatedAt", "创建时间", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.ColumnSwitch("IsActive", "状态", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };

            paginationControl.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };
        }
    }

    public class UserManagementView : AntdUI.NotifyProperty 
    {

        public UserManagementView(UserDto userListItem)
        {
            _id = userListItem.Id;
            _employeeId = userListItem.EmployeeId;
            _domainAccount = userListItem.DomainAccount;
            _userName = userListItem.UserName;
            _factoryName = userListItem.FactoryName;
            _isActive = userListItem.IsActive;
            _createdAt = userListItem.CreatedAt;
            _check = false; // 默认未选中
            // 初始化按钮
            _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit", "编辑", AntdUI.TTypeMini.Primary),
                        new AntdUI.CellButton("delete", "删除", AntdUI.TTypeMini.Error)
                    };
        }


        int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        bool _check = false;
        public bool check
        {
            get => _check;
            set
            {
                if (_check == value) return;
                _check = value;
                OnPropertyChanged();
            }
        }
        string _employeeId;
        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                if (_employeeId == value) return;
                _employeeId = value;
                OnPropertyChanged();
            }
        }

        string? _domainAccount;
        public string DomainAccount
        {
            get => _domainAccount;
            set
            {
                if (_domainAccount == value) return;
                _domainAccount = value;
                OnPropertyChanged();
            }
        }

        string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName == value) return;
                _userName = value;
                OnPropertyChanged();
            }
        }

        string? _factoryName;
        public string FactoryName
        {
            get => _factoryName;
            set
            {
                if (_factoryName == value) return;
                _factoryName = value;
                OnPropertyChanged();
            }
        }
        bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value) return;
                _isActive = value;
                OnPropertyChanged();
            }
        }

        DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt == value) return;
                _createdAt = value;
                OnPropertyChanged();
            }
        }
        AntdUI.CellLink[] _btns;
        public AntdUI.CellLink[] btns
        {
            get => _btns;
            set
            {
                _btns = value;
                OnPropertyChanged();
            }
        }
    }
}
