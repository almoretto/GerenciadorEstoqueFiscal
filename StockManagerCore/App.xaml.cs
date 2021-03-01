#region --== Dependency declaration ==--
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Services;
#endregion

namespace StockManagerCore
{
    /// <summary>
    /// This aplication is for control stock of purchased and sold products with Nfe invoice.
    /// The primary objective of this is to load the data of the Nfe invoices
    /// from txt files of the Imported products sended from Import/Export Company 
    /// and csv file from solds order.
    /// After that the system carry on of process all data and calculate que total itens purchased and amount of them.
    /// and the total item sold and so the amount of them. Separated by group of product.
    /// Them user can see on a grid.
    /// The system has a sencond separated function that is to control shipping invoices and return invoices
    /// for each seller representative or client that could be necessary use this kind of invoice.
    /// its only a log and do not have any control of emmited or returned invoices if not informed.
    /// Other functios is to inputs manualy data that don´t have file to load.
    /// The system doesn´t have reports for printing pourpose.
    /// The system must calculate the total invoices of sales emited and register 
    /// a balance of the Max renenues less the total sales per company
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;
        public App()
        {
            /*Application inicialization and injection of dependency of services, DBContext and Seed Data service.
             * Any Service is from each kind of Model.
             */
            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<StockDBContext>(options =>
            {
                options.UseSqlServer("server=tcp:192.168.100.2,1433;Initial Catalog =StockKManagerDB_TST;User Id=sa;Password=$3nh@2018;");
            });
            services.AddScoped<InputService>();
            services.AddScoped<SaleService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<ProductService>();
            services.AddScoped<StockService>();
            services.AddScoped<PersonService>();
            services.AddScoped<ControlNFService>();
            services.AddScoped<CityService>();
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
