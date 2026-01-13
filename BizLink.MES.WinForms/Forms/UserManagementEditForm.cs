using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
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
    public partial class UserManagementEditForm : MesEditForm<UserDto>
    {
        private readonly UserModuleFacade _facade;

        // 标记位：是否处于“补充域用户信息”的特殊模式
        private bool _isDomainSupplementMode = false;

        public UserManagementEditForm(UserModuleFacade facade)
        {
            _facade = facade;
            InitializeComponent();

            // 【关键】告诉基类哪个是保存按钮，以便基类控制 Loading 状态
            this.SaveButton = btnSave;
        }

        // ============================================================
        // 1. 初始化与加载逻辑
        // ============================================================

        protected override async void OnLoad(EventArgs e)
        {
            // UI 样式初始化
            label1.PrefixColor = Color.Red;
            label3.PrefixColor = Color.Red;

            // 异步加载下拉菜单数据
            await LoadFactoriesAsync();

            // 调用基类 OnLoad (会触发 BindDataToUI)
            base.OnLoad(e);
        }

        /// <summary>
        /// 特殊初始化：用于“补充域用户信息”场景 (对应原代码 userId == -1)
        /// </summary>
        /// <param name="domainName">域账号</param>
        public void InitForDomainSupplement(string domainName)
        {
            _isDomainSupplementMode = true;

            // 构造一个预填了域账号的 DTO
            var dto = new UserDto { DomainAccount = domainName?.ToUpper() };

            // 调用基类标准初始化
            InitData(dto);

            // 针对此模式的特殊 UI 调整
            label8.Text = "补充域用户信息";
            label4.Prefix = " ";
            label5.Prefix = " ";
            passwordInput.PlaceholderText = "域用户可不填";
            passwordConfirmInput.PlaceholderText = "域用户可不填";
            btnCancel.Visible = false; // 强制填写，不允许取消
        }

        private async Task LoadFactoriesAsync()
        {
            try
            {
                // 使用 Facade 基类中的 FactoryService
                var pagedResult = await _facade.FactoryService.GetPagedListAsync(1, int.MaxValue, string.Empty, true);

                if (pagedResult?.Items != null)
                {
                    var factories = pagedResult.Items.OrderBy(x => x.FactoryCode).ToList();
                    FactorySelect.Items.Clear();
                    foreach (var factory in factories)
                    {
                        FactorySelect.Items.Add(new AntdUI.MenuItem
                        {
                            Name = factory.Id.ToString(),
                            Text = factory.FactoryName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, $"加载工厂数据失败: {ex.Message}");
            }
        }

        // ============================================================
        // 2. 基类抽象实现：数据绑定 (Model -> UI)
        // ============================================================

        protected override async void BindDataToUI()
        {
            // A. 编辑模式 (Id > 0)
            if (CurrentModel.Id > 0)
            {
                // 最佳实践：编辑时重新查询最新数据，防止列表页数据不全
                var fullUser = await _facade.UserService.GetByIdAsync(CurrentModel.Id);
                if (fullUser != null)
                    CurrentModel = fullUser;

                label8.Text = "编辑用户信息";
                empCodeInput.Enabled = false; // 禁止修改工号
                label4.Prefix = " ";
                label5.Prefix = " ";
            }
            // B. 普通新增模式 (非域补充)
            else if (!_isDomainSupplementMode)
            {
                label4.PrefixColor = Color.Red;
                label5.PrefixColor = Color.Red;
                FactorySelect.PlaceholderText = "非MES相关用户可不选";
            }

            // 通用绑定逻辑
            empCodeInput.Text = CurrentModel.EmployeeId;
            domainInput.Text = CurrentModel.DomainAccount;
            nameInput.Text = CurrentModel.UserName;
            statusSwitch.Checked = CurrentModel.IsActive;

            // 绑定工厂选中项
            if (!string.IsNullOrEmpty(CurrentModel.FactoryName) && FactorySelect.Items.Count > 0)
            {
                FactorySelect.SelectedValue = FactorySelect.Items
                    .Cast<dynamic>()
                    .FirstOrDefault(x => x.Text == CurrentModel.FactoryName);
            }
        }

        // ============================================================
        // 3. 基类虚方法重写：表单校验
        // ============================================================

        protected override bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(empCodeInput.Text))
            {
                AntdUI.Message.error(this, "工号不能为空！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(nameInput.Text))
            {
                AntdUI.Message.error(this, "姓名不能为空！");
                return false;
            }

            // 密码校验逻辑
            string pwd = passwordInput.Text.Trim();
            string pwdConfirm = passwordConfirmInput.Text.Trim();

            // 判定规则：如果是“新建普通用户”或者“用户正在修改密码”
            bool isNewUser = CurrentModel.Id <= 0 && !_isDomainSupplementMode;
            bool isChangingPwd = !string.IsNullOrEmpty(pwd);

            if (isNewUser || isChangingPwd)
            {
                if (isNewUser && string.IsNullOrEmpty(pwd))
                {
                    AntdUI.Message.error(this, "密码不能为空！");
                    return false;
                }
                if (string.IsNullOrEmpty(pwdConfirm))
                {
                    AntdUI.Message.error(this, "密码确认不能为空！");
                    return false;
                }
                if (pwd != pwdConfirm)
                {
                    AntdUI.Message.error(this, "两次输入的密码不一致！");
                    return false;
                }
            }

            return true;
        }

        // ============================================================
        // 4. 基类抽象实现：保存逻辑 (UI -> Model -> DB)
        // ============================================================

        protected override async Task SaveDataAsync(UserDto model)
        {
            // 1. 收集 UI 数据构建 UpdateDto
            var userDto = new UserUpdateDto
            {
                Id = model.Id,
                EmployeeId = empCodeInput.Text.Trim(),
                DomainAccount = domainInput.Text.Trim(),
                UserName = nameInput.Text.Trim(),
                IsActive = statusSwitch.Checked,
                FactoryId = FactorySelect.SelectedValue != null ? Convert.ToInt32(((AntdUI.MenuItem)FactorySelect.SelectedValue).Name) : 0
            };

            string pwd = passwordInput.Text.Trim();
            if (!string.IsNullOrEmpty(pwd))
            {
                userDto.PasswordHash = pwd;
            }

            // 2. 根据不同模式执行保存
            if (model.Id > 0)
            {
                // --- 场景 A: 明确的更新 ---
                await _facade.UserService.UpdateAsync(userDto);
            }
            else if (_isDomainSupplementMode)
            {
                // --- 场景 B: 域用户补充 (先查是否存在，存在则更新，不存在则新增) ---
                var existingUser = await _facade.UserService.GetByDomainAccountAsync(userDto.DomainAccount);
                if (existingUser != null)
                {
                    userDto.Id = existingUser.Id; // 修正 ID
                    await _facade.UserService.UpdateAsync(userDto);
                }
                else
                {
                    await _facade.UserService.CreateAsync(MapToCreateDto(userDto));
                }
            }
            else
            {
                // --- 场景 C: 普通新增 ---
                await _facade.UserService.CreateAsync(MapToCreateDto(userDto));
            }
        }

        private UserCreateDto MapToCreateDto(UserUpdateDto updateDto)
        {
            return new UserCreateDto
            {
                EmployeeId = updateDto.EmployeeId,
                DomainAccount = updateDto.DomainAccount,
                UserName = updateDto.UserName,
                PasswordHash = updateDto.PasswordHash,
                IsActive = updateDto.IsActive,
                FactoryId = updateDto.FactoryId
            };
        }

        // ============================================================
        // 5. 事件绑定
        // ============================================================

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // 调用基类模板方法，自动处理校验、Loading、错误捕获、结果回调
            await ExecuteSaveAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
