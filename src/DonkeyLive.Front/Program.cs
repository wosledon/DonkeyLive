using DonkeyLive.Front;
using DonkeyLive.Front.Services.Global;
using DonkeyLive.Front.Setups;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Donkey",opts =>
{
#if DEBUG
    opts.BaseAddress = new Uri("http://localhost:8080");
#else
    opts.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
#endif
});

builder.Services.AddScoped<HttpRequestService>();
builder.Services.AddScoped<AlertService>();

builder.Services.AddHttpServices();

builder.Services.AddMasaBlazor();

await builder.Build().RunAsync();
