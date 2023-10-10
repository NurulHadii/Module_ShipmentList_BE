using DevExpress.XtraReports.Web.WebDocumentViewer;

namespace BooxApp.Api.Models
{
    public class ViewerModel
    {
        public string ReportUrl { get; set; }
        public WebDocumentViewerModel ViewerModelToBind { get; set; }
    }
}