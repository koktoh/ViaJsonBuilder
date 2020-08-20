using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.SessionStorage;
using BlazorDownloadFile;
using ViaJsonBuilder.Models.Builder;
using ViaJsonBuilder.Models.Extractor;

namespace ViaJsonBuilder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient(sp => new KeyboardConfigInfoExtractor());
            builder.Services.AddTransient(sp => new ViaBuilder());
            builder.Services.AddTransient(sp => new KeymapBuilder());
            builder.Services.AddBlazorDownloadFile();
            builder.Services.AddBlazoredSessionStorage();

            await builder.Build().RunAsync();
        }
    }
}
