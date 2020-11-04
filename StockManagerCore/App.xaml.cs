using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Services;

namespace StockManagerCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<StockDBContext>(options =>
            {
                options.UseSqlServer("server=tcp:192.168.100.2,1433;Initial Catalog =StockKManagerDB;User Id=sa;Password=$3nh@2018;");
            });
            services.AddScoped<InputService>();
            services.AddScoped<SaleService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<ProductService>();
            services.AddScoped<StockService>();
            services.AddScoped<SeedDataService>();//Initiates the service in the injection dependecy of application
            services.AddSingleton<MainWindow>();
            serviceProvider = services.BuildServiceProvider();
        }
        private void OnStartup(object s, StartupEventArgs e)
        {
            SeedDataService seedDataService = new SeedDataService(serviceProvider.GetService<StockDBContext>());
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
            seedDataService.Seed();
        }

    }
}
