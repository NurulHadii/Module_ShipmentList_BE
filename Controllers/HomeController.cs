using Microsoft.AspNetCore.Mvc;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DevExpress.DataAccess.Sql;

namespace BooxApp.Api.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            Models.ErrorModel model = new Models.ErrorModel();
            return View(model);
        }

        public IActionResult Designer()
        {
            Models.ReportDesignerModel model = new Models.ReportDesignerModel();
            // Create a SQL data source with the specified connection string.
            SqlDataSource ds = new SqlDataSource("DefaultConnection");
            // Create a SQL query to access the Products data table.
            SelectQuery query = SelectQueryFluentBuilder.AddTable("PlanningTask").SelectAllColumnsFromTable().Build("PlanningTask");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            model.DataSources = new Dictionary<string, object>();
            model.DataSources.Add("context", ds);
            return View(model);
        }

        public IActionResult Viewer()
        {
            return View();
        }
    }
}
