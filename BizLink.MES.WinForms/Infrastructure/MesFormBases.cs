using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Infrastructure
{
    // ========================================================================
    // 1. 清单/报表基类 (MesListForm)
    // ========================================================================
    /// <summary>
    /// 列表窗体基类
    /// <typeparam name="TModel">表格显示的数据模型 (DTO)</typeparam>
    /// </summary>
    public abstract class MesListForm<TModel> : MesBaseForm
    {
        // --- 子类配置区 ---
        // 请在子类构造函数中给这些属性赋值
        protected AntdUI.Table TableControl
        {
            get; set;
        }
        protected AntdUI.Pagination PaginationControl
        {
            get; set;
        }
        protected AntdUI.Button SearchButton
        {
            get; set;
        }

        // --- 抽象方法 ---
        /// <summary>
        /// 子类必须实现：获取数据的逻辑 (调用 Facade)
        /// </summary>
        protected abstract Task<List<TModel>> GetDataAsync();

        // --- 核心逻辑 ---
        /// <summary>
        /// 刷新列表 (包含 Loading、错误处理、数据绑定)
        /// </summary>
        protected async Task RefreshListAsync()
        {
            await RunAsync(SearchButton, async () =>
            {
                var data = await GetDataAsync();

                if (TableControl != null)
                {
                    TableControl.DataSource = data;
                }

                AfterDataLoaded(data);
            });
        }

        /// <summary>
        /// 扩展点：数据加载完成后执行 (如计算总计、更新状态栏)
        /// </summary>
        protected virtual void AfterDataLoaded(List<TModel> data)
        {
        }

        // 自动加载：窗体显示时自动查询
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                await RefreshListAsync();
            }
        }
    }

    // ========================================================================
    // 2. 编辑/对话框基类 (MesEditForm)
    // ========================================================================
    /// <summary>
    /// 编辑窗体基类
    /// <typeparam name="TModel">编辑的数据模型 (DTO)</typeparam>
    /// </summary>
    public abstract class MesEditForm<TModel> : MesBaseForm
    {
        // --- 子类配置区 ---
        protected AntdUI.Button SaveButton
        {
            get; set;
        }

        // --- 状态 ---
        protected TModel CurrentModel
        {
            get; set;
        }
        public event Action OnSaved; // 保存成功的事件回调

        // --- 抽象/虚方法 ---

        /// <summary>
        /// 初始化数据入口
        /// </summary>
        public virtual void InitData(TModel model)
        {
            CurrentModel = model;
            BindDataToUI();
        }

        /// <summary>
        /// 子类实现：将 Model 数据填入 UI 控件
        /// </summary>
        protected virtual void BindDataToUI()
        {
        }

        /// <summary>
        /// 子类实现：UI 校验逻辑 (默认返回 true)
        /// </summary>
        protected virtual bool ValidateInput() => true;

        /// <summary>
        /// 子类必须实现：保存数据的逻辑 (调用 Facade)
        /// </summary>
        protected abstract Task SaveDataAsync(TModel model);

        // --- 核心逻辑 ---
        /// <summary>
        /// 执行保存流程 (包含校验、Loading、错误处理、关闭窗体)
        /// </summary>
        protected async Task ExecuteSaveAsync()
        {
            if (!ValidateInput())
                return;

            await RunAsync(SaveButton, async () =>
            {
                // 调用子类具体的保存逻辑
                await SaveDataAsync(CurrentModel);

                // 触发回调
                OnSaved?.Invoke();

                // 标准行为：关闭窗体，返回 OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            },
            successMsg: "保存成功！",
            confirmMsg: "确定要保存当前修改吗？");
        }
    }
}
