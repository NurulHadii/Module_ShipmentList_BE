using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace BooxApp.Api.Models
{
    public class DashboardConfigurationModel
    {
        public string DashboardType { get; set; }
        public JArray Entity { get; set; }
    }
}
