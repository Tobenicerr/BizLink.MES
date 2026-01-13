using BizLink.MES.MAUI.View;

namespace BizLink.MES.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // 【在这里注册您的新页面的路由】
            Routing.RegisterRoute(nameof(StockTransferPage), typeof(StockTransferPage));
        }
    }
}
