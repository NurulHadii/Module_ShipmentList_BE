using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BooxApp.Api.Models
{
    public class Login
    {
        public string? email { get; set; }
        public string? Password { get; set; }
        public string? api_token { get; set; }
    }
}