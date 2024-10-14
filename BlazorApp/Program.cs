using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp;
using WebApi.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.ConfigureOptions(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddAuthorizationCore(options => {});

await builder.Build().RunAsync();
