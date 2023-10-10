using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using BooxApp.Api.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using BooxApp.Core.Extensions;
using Serilog;
using BooxApp.Core.ThirdParty.Azure.CosmosDb;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(Program));
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
//register BooxApp Bootsraper
builder.Services.BooxAppBootsraper(builder.Configuration);

builder.Services.AddCors(options =>
{
   
    options.AddPolicy("DefaultCorsPolicy", builder =>
    {
        // Allow all ports on local host.
        builder.WithOrigins("http://localhost:8080")
                  .SetIsOriginAllowedToAllowWildcardSubdomains()
                  .AllowAnyHeader()
                  .AllowCredentials()
                  .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                  .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
    });
});

// reporting
builder.Services.AddDevExpressControls();
builder.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
builder.Services.ConfigureReportingServices(configurator => {
    configurator.ConfigureReportDesigner(designerConfigurator => {
        designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
    });
    configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
        viewerConfigurator.UseCachedReportSourceBuilder();
    });
});

var app = builder.Build();

app.UseCors("DefaultCorsPolicy");

app.UseSerilogRequestLogging();

// reporting
app.UseDeveloperExceptionPage();
app.UseDevExpressControls();

//register BooxApp App Builder
app.BooxAppBuilder();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

