
#region --== Dependency Declaration ==--
using System.Windows;
using StockManagerCore.Services;
using StockManagerCore.UserInterface;
#endregion

namespace StockManagerCore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        #region --== Instances of Context, Provider and StringBuilder ==--
        /*Declatarions for dependency injection of services classes.
         * Each one of the services are responsible for the model thats concern to
         * Each method in the single service is designed to a single purpose.
         * trying to obey SOLID principles.
         */
        private readonly InputService _inputService;
        private readonly SaleService _saleService;
        private readonly ProductService _productService;
        private readonly CompanyService _companyService;
        private readonly StockService _stockService;
        #endregion

        public MainWindow(InputService inputService, SaleService saleService, ProductService productService,
            CompanyService companyService, StockService stockService)
        {
            //Constructor of the form MainWindow here we call the dependency injection

            _inputService = inputService;
            _saleService = saleService;
            _productService = productService;
            _companyService = companyService;
            _stockService = stockService;

            InitializeComponent();
        }
        
        #region --== Buttons Methods==--
        private void btnOpenStockInput_Click(object sender, RoutedEventArgs e)
        {
            WdnProcessFiles processFilesWindow = new WdnProcessFiles(_inputService, _saleService, _productService, 
                _companyService, _stockService);
            processFilesWindow.Show();
        }
        
        private void btnOpenStockCRUD_Click(object sender, RoutedEventArgs e)
        {
            WdnStockCrud stockCrud = new WdnStockCrud(_productService, _companyService, _stockService);
            stockCrud.Show();
        }
        
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        private void btnLogin_Click( object sender, RoutedEventArgs e )
        {
            
        }

        private void btnChangePass_Click( object sender, RoutedEventArgs e )
        {

        }
        #endregion

        #region --== Action  Methods ==--

        #endregion


    }
}
