using Microsoft.Extensions.DependencyInjection;
using TouristGuideAppWF;

namespace TouristGuideAppWF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var services = new ServiceCollection();
            services.AddHttpClient(); // Add HttpClientFactory to services
            services.AddTransient<Form1>(); // Add our main form
            var serviceProvider = services.BuildServiceProvider();
            Application.Run(serviceProvider.GetRequiredService<Form1>());
        }
    }
}