using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GoodHamburger.Frontend;
using GoodHamburger.Frontend.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for Backend API
if (builder.HostEnvironment.IsDevelopment())
{
    // Local: Aponta direto para o backend
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5269/") });
}
else
{
    // Produção: Usa o Proxy do Nginx (mesma origem)
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
}

builder.Services.AddMudServices();
builder.Services.AddScoped<SearchService>();

await builder.Build().RunAsync();
