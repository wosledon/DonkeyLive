using DonkeyLive.WebApi.Data;
using DonkeyLive.WebApi.Helpers;
using DonkeyLive.WebApi.Options;
using DonkeyLive.WebApi.Setups;
using LiveStreamingServerNet;
using LiveStreamingServerNet.AdminPanelUI;
using LiveStreamingServerNet.Flv.Installer;
using LiveStreamingServerNet.Networking.Helpers;
using LiveStreamingServerNet.Standalone;
using LiveStreamingServerNet.Standalone.Insatller;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Serilog.Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(setup =>
{
    setup.SerializerSettings.ContractResolver
        = new CamelCasePropertyNamesContractResolver();
    setup.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
})
        /*添加XML*/.AddXmlDataContractSerializerFormatters()
        .ConfigureApiBehaviorOptions(setup =>
        {
            setup.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = "http://www.baidu.com",
                    Title = "有错误",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = "请看详细信息",
                    Instance = context.HttpContext.Request.Path
                };

                problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        }); ;
builder.Services.AddRazorPages();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(builder => builder.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddRtmpServer(builder.Configuration);
builder.Services.AddFFmpeg(builder.Configuration);

builder.WebHost.UseUrls($"http://*:{builder.Configuration.GetSection("Application:Port").Value}");

builder.Services.AddGlobalServices();
builder.Services.AddEfCore<DonkeyDbContext>(builder.Configuration);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Application"));

var app = builder.Build();

app.UseCors("AllowAll");
app.UseGlobalExceptionMiddleware();
app.UseRtmpServer();

app.UseLogger();
app.UseEfCore<DonkeyDbContext>(opts =>
{
#if DEBUG
    //opts.Reset = true;
#endif
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseBlazorFrameworkFiles();
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
    //ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
    //        {
    //                { ".apk", "application/vnd.android.package-archive" }
    //        })
});

app.MapRazorPages();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

try
{
    app.Run();
}
catch (Exception ex)
{
    Serilog.Log.Error("=============================");
    Serilog.Log.Error($"application exit.\r\n", ex);
    Serilog.Log.Error("=============================");

}
finally
{
    await Serilog.Log.CloseAndFlushAsync();
}
