using DevExpress.XtraReports.UI;

namespace BooxApp.Api.Reporting
{
    public static class ReportsFactory
    {
        public static Dictionary<string, Func<XtraReport>> Reports = new Dictionary<string, Func<XtraReport>>()
        {
            ["TestReport"] = () => new XtraReport()
        };

    }   
}
