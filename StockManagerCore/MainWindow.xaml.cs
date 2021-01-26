
#region --== Dependency Declaration ==--
using System;
using System.Windows;
using StockManagerCore.Services;
using StockManagerCore.Services.Exceptions;
using StockManagerCore.Models;
using StockManagerCore.Models.Enums;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
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
        private readonly PersonService _personService;
        private readonly ControlNFService _controlNFService;
        private readonly CityService _cityService;

        //declaration of culture info for less verbose code
        CultureInfo provider = CultureInfo.InvariantCulture;
        //Declaration of the log string builder. This log is merely for information and its not stored in any place for a while.
        StringBuilder log = new StringBuilder();
        #endregion

        #region --== Local Variables ==--
        string filename;
        FileReader nfeReader;
        bool sales;
        DateTime dateInitial = new DateTime();
        DateTime dateFinal = new DateTime();
        int qteTot = 0;
        int qty = 0;
        double amount = 0.0;
        double totAmount = 0.0;
        int anyId = 0;
        #endregion

        #region --== Models instantitation and support Lists ==--
        /*Private declaration of the models extructure for local use only and in order to facilitate
         the management of data*/
        private Company SelectedCompany { get; set; } = new Company();
        private Product SelectedProduct { get; set; } = new Product();
        private Stock SelectedStock { get; set; } = new Stock();
        private InputProduct InputProduct { get; set; } = new InputProduct();
        private List<SoldProduct> ListOfSales { get; set; } = new List<SoldProduct>();
        private IEnumerable<Company> ListCompanies { get; set; }
        private IEnumerable<object> ListOfStocks { get; set; }
        private IEnumerable<Product> ListOfProducts { get; set; }
        private IEnumerable<Person> ListPeople { get; set; }
        private IEnumerable<NFControl> Notes { get; set; }
        private IEnumerable<City> Cities { get; set; }
        private NFControl NF { get; set; } = new NFControl();
        private City City { get; set; } = new City();
        private Person Person { get; set; } = new Person();

        #endregion
        public MainWindow(InputService inputService, SaleService saleService, ProductService productService,
            CompanyService companyService, StockService stockService, PersonService personService,
            ControlNFService controlNFService, CityService cityService)
        {
            //Constructor of the form MainWindow here we call the dependency injection
            
            _inputService = inputService;
            _saleService = saleService;
            _productService = productService;
            _companyService = companyService;
            _stockService = stockService;
            _personService = personService;
            _controlNFService = controlNFService;
            _cityService = cityService;

            InitializeComponent();
            //these is for populate de comboboxes.
            ListCompanies = _companyService.GetCompanies();
            ListOfProducts = _productService.GetProducts();
            ListPeople = _personService.GetPeople();
            Cities = _cityService.GetCities();

            foreach (Company c in ListCompanies)
            {
                CmbCompany.Items.Add(c.Name);
                CmbStkCompany.Items.Add(c.Name);
                cmbNFCompany.Items.Add(c.Name);
            }
            foreach (Product product in ListOfProducts)
            {
                CmbStkProduct.Items.Add(product.GroupP);
            }
            foreach (Person person in ListPeople)
            {
                cmbDestinatary.Items.Add(person.Name);
            }
            foreach (City c in Cities)
            {
                cmbCities.Items.Add(c.CityName);
            }

            InitializeComponent();

        }

        #region --== Functions of Tb_Functions TAB ==--
        private void BtnFileOpen_Click(object sender, RoutedEventArgs e)
        {
            //This method opens the dialog and permits to select a file to import.
            try
            {

                // Create OpenFileDialog
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension
                dlg.DefaultExt = ".txt";
                dlg.Filter = "TXT Document (.TXT;.CSV)|*.TXT;*.CSV";

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    filename = dlg.FileName;
                    FileNameTextBox.Text = filename;
                }

            }
            catch (Exception ex)
            {
                //writing log if there is any error and showing in an textBlock
                LogTextBlock.Text = "";
                log.AppendLine(ex.Message);
                LogTextBlock.Text = log.ToString();
            }
        }
        private void ProcessInputs_Click(object sender, RoutedEventArgs e)
        {
            //This Method is responsible to care of process the import files in input records.
            try
            {
                //Clean log
                LogTextBlock.Text = string.Empty;
                //Geting the Entity Company by mane selected in combo
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);

                if (filename.EndsWith("csv") || filename.EndsWith("CSV"))
                {
                    /*This test is simple in rule allways the file thats contains the inputs came in .TXT extention so, 
                     * if selected was diferent throw exception */
                    throw new ApplicationException("Não pode processar arquivo de venda como entrada!");
                }
                if (SelectedCompany == null)
                {
                    //Any company must be selected to create a link between products and company, so if its null, throw exception.
                    throw new ApplicationException("Selecione uma empresa!");
                }
                //Here just inform that imports is occurring
                LogTextBlock.Text = "Importando: " + filename;
                //Sets the bool variable to False to indicate to the service class where to process this file, 
                //because this file is input record.
                sales = false;
                log.Clear();
                if (!sales) //If Sales = False
                {
                    //Instance the service that process the file. 
                    //Calling the constructor passing the File and the Bool variable to indicate where to process
                    nfeReader = new FileReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(nfeReader.GetInputItens()); //Call the Method GetInputItens that returns an string as result.
                    LogTextBlock.Text = log.ToString(); // Showing the result.
                    //Reading all imported records
                    foreach (InputNFe item in nfeReader.Inputs)
                    {
                        //Instances a new Product
                        Product p = new Product();
                        //Getting the entity product by his group name in the service class.
                        p = _productService.FindByGroup(item.Group);
                        
                        item.AlternateNames(); //callcing an method to padronize the names of the products as groups.

                        //Instancing model class and calling constructor for each item record in service class to place data.
                        //Working in a private unique local instance of the model
                        InputProduct = new InputProduct
                            (item.NItem,
                            item.XProd,
                            item.QCom,
                            item.VUnCom,
                            item.UCom,
                            item.Vtotal,
                            item.VUnTrib,
                            item.VTotTrib,
                            p,
                            item.DhEmi,
                            SelectedCompany);
                        //calling the method to insert the data in model on db context
                        _inputService.InsertInputs(InputProduct);
                    }
                }
                else //Exception
                {
                    //When this occur user trying to procees sales as inputs
                    LogTextBlock.Text = "";
                    log.AppendLine("Não pode processar venda como Compra!");
                    LogTextBlock.Text = log.ToString();
                    throw new ApplicationException("Não pode processar venda como Compra!");
                }

            }
            catch (Exception ex)
            {
                //Error Exception for whole method above.
                LogTextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }

                LogTextBlock.Text = log.ToString();
            }
        }
        private void ProcessSales_Click(object sender, RoutedEventArgs e)
        {
            //This Method is responsible to care of process the import files in Sales records.
            try
            {
                //clean log
                LogTextBlock.Text = string.Empty;
                //Geting the Entity Company by mane selected in combo
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);

                if (filename.EndsWith("TXT") || filename.EndsWith("txt"))
                {
                    /*This test is simple in rule allways the file thats contains the inputs came in .CSV extention so, 
                    * if selected was diferent throw exception */
                    throw new ApplicationException("Não pode processar arquivo de entrada como venda!");
                }
                if (SelectedCompany == null)
                {
                    //Any company must be selected to create a link between products and company, so if its null, throw exception.
                    throw new ApplicationException("Selecione uma empresa!");
                }
                //Here just inform that imports is occurring
                LogTextBlock.Text = "Importando: " + filename;
                //Sets the bool variable to TRUE to indicate to the service class where to process this file, 
                //because this file is Sales record.
                sales = true;
                log.Clear();

                if (sales)
                {
                    //Instance the service
                    nfeReader = new FileReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(nfeReader.GetInputItens());
                    LogTextBlock.Text = log.ToString();
                    //Reading all imported records
                    foreach (InputNFe item in nfeReader.Inputs)
                    {
                        //Instancing a new product.
                        Product p = new Product();
                        //Getting the entity product by his group name in the service class.
                        p = _productService.FindByGroup(item.Group);

                        if (p == null)
                        {
                            //verifys if the return of the query above is null, and throw exceptios, because have to be a product
                            throw new ApplicationException(" Pelo menos um produto da Nota não foi encontrado! \n Importação abortada!");
                        }
                        //Instances a New Sale and call the Constructor 
                        ListOfSales.Add(new SoldProduct
                            (item.NItem,
                            item.XProd,
                            item.QCom,
                            item.VUnCom,
                            item.Vtotal,
                            item.DhEmi,
                            p,
                            SelectedCompany));
                    }
                    //call the method to insert into db context, on services layer
                    _saleService.InsertMultiSales(ListOfSales);
                }
                else
                {
                    //If isn´t any of the above, we throw an exception.
                    LogTextBlock.Text = "";
                    log.AppendLine("Não pode processar entrada como venda!");
                    LogTextBlock.Text = log.ToString();
                    throw new ApplicationException("Não pode processar entrada como venda!");
                }

            }
            catch (Exception ex)
            {
                //General method exception
                LogTextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                LogTextBlock.Text = log.ToString();
            }
        }
        private void Btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //counter for checking purpose
                int count = 0;
                //get the selected company in combobox
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);

                if (SelectedCompany == null)
                {
                    //If company is null then throw exception
                    throw new ApplicationException("Empresa deve ser selecionada!");
                }

                if (RdnIn.IsChecked == true)
                {
                    //Here is the separation for calcularion of inputs 
                   //Get the inicial date typed on textBox
                    dateInitial = DateTime.ParseExact(TxtDateInitial.Text, "dd/MM/yyyy", provider);
                    //Get a List of purchased products filtered by date and company
                    IEnumerable<InputProduct> listInputProduct = _inputService.GetInputsByDateAndCompany(dateInitial, SelectedCompany);
                    //transform the list in a grouping query, grouped by Product
                    var groupProducts = listInputProduct.GroupBy(p => p.XProd);

                    //moving through grouping for process
                    foreach (IGrouping<string, InputProduct> group in groupProducts)
                    {
                        //instances a new stock and gte the entity Stock related to the company and the product and assign to the instance.
                        Stock stock = new Stock();
                        stock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, group.Key);

                        //information on log
                        log.Append("Produto: " + group.Key);
                        TxtConsole.Text = log.ToString();

                        //sub loop moving through products in each group.
                        foreach (InputProduct item in group)
                        {
                            //Accumulating Quantity of purchased products
                            qty += item.QCom;
                            //accumulating value of the purchased products
                            amount += item.Vtotal;
                            //increment counter
                            count++;
                        }
                        //Call the inner method of stock for register de moviment.
                        stock.MovimentInput(qty, amount, dateInitial);
                        //Updates the Stock om db context using service layer
                        _stockService.Update(stock);

                        //Log to demonstrate what was carried.
                        log.Append(" | Qte: " + qty.ToString());
                        log.AppendLine(" | Valor: " + amount.ToString("C2", CultureInfo.CurrentCulture));
                        TxtConsole.Text = log.ToString();
                        qteTot += qty;
                        totAmount += amount;
                       //Zero fill the temporary variable.
                        qty = 0;
                        amount = 0.0;
                    }
                    //Final Log Sumarazing.
                    log.Append("QteTotal: " + qteTot.ToString());
                    log.AppendLine(" | ValorTotal: " + totAmount.ToString("C2", CultureInfo.CurrentCulture));
                    TxtConsole.Text = log.ToString();
                }
                if (RdnOut.IsChecked == true && SelectedCompany != null)//Continue from here 30/10
                {
                    //Here is the separation for calcularion of Sales 
                    //Get the inicial  and final date typed on textBox

                    dateInitial = DateTime.ParseExact(TxtDateInitial.Text, "dd/MM/yyyy", provider);
                    dateFinal = DateTime.ParseExact(TxtDateFinal.Text, "dd/MM/yyyy", provider);
                    // list of sales by date and company
                    IEnumerable<SoldProduct> salesByDateAndCompany = _saleService.GetSalesByDateAndCompany(dateInitial, dateFinal, SelectedCompany);
                    //Grouping query by product
                    var groupOfSales = salesByDateAndCompany.GroupBy(p => p.Product.GroupP);

                    //moving through grouping
                    foreach (IGrouping<string, SoldProduct> group in groupOfSales)
                    {
                        //Instances new Stock and gets the Entity Stock by company and Product and atributes to instance
                        Stock stock = new Stock();
                        stock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, group.Key);

                        //Log information
                        log.Append("Produto: " + group.Key);
                        TxtConsole.Text = log.ToString();

                        //Moving through the sales in groups
                        foreach (SoldProduct item in group)
                        {
                            //Accumulating Quantity and values of purchased products
                            qty += item.QCom;
                            amount += item.Vtotal;
                        }

                        //Call method for process moviment of sales in the stock
                        stock.MovimentSale(qty, amount, dateInitial);
                        //Recalculates the inputs 
                        stock.MovimentInput(qty, amount, dateInitial);
                        //update the stock in dbcontext
                        _stockService.Update(stock);

                        //Log demonstration of what been prossessed
                        log.Append(" | Qte: " + qty.ToString());
                        log.AppendLine(" | Valor: " + amount.ToString("C2", CultureInfo.CurrentCulture));
                        TxtConsole.Text = log.ToString();
                        qteTot += qty;
                        totAmount += amount;
                        qty = 0;
                        amount = 0.0;
                    }
                    log.Append("QteTotal: " + qteTot.ToString());
                    log.AppendLine(" | ValorTotal: " + totAmount.ToString("C2", CultureInfo.CurrentCulture));
                    TxtConsole.Text = log.ToString();

                }
                //Counting the two lists
                log.AppendLine("Lista Entradas: " + count.ToString());
                log.AppendLine("Lista Saídas: " + ListOfSales.Count);
            }
            catch (ApplicationException ex)
            {
                //Any Application Exception
                LogTextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                LogTextBlock.Text = log.ToString();
            }
        }
        private void Btn_ShowStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Get Selected company in combo
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);
                if (SelectedCompany == null)
                {
                    //Company must not be null
                    throw new ApplicationException("Selecione uma empresa!");
                }             
                ListOfStocks = _stockService.GetStocksFormated(SelectedCompany); //list Resulkt
                //Method to generate GridView
                GenerateGrid(ListOfStocks, SelectedCompany);
            }
            catch (ApplicationException ex)
            {
                LogTextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }

                LogTextBlock.Text = log.ToString();
            }
        }
        private void btnBalanceAll_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Stock> stocksToCalc;
            stocksToCalc = _stockService.GetStocks();

            foreach (Stock item in stocksToCalc)
            {
                try
                {
                    item.SetBalance();
                    _stockService.Update(item);
                }
                catch (Exception ex)
                {
                    LogTextBlock.Text = "Erro ao Tentar atualizar item"
                        + item.Product.GroupP
                        + ",  "
                        + item.Company.Name
                        + "."
                        + "\n"
                        + ex.Message;
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region --== CRUD Companies, Products and Stock==--
        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            switch (CmbSwitch.SelectedItem.ToString())
            {
                //Selects the type of Searching service to use according to the user selection
                //And populates the controls with the returned data.
                case "Empresa":
                    SelectedCompany = _companyService.FindByName(TxtSelection.Text.ToUpper());
                    TxtCoId.Text = SelectedCompany.Id.ToString();
                    TxtCoName.Text = SelectedCompany.Name;
                    txtCompanyMaxRevenues.Text = SelectedCompany.MaxRevenues.ToString("C2");
                    txtCompanyBalance.Text = SelectedCompany.Balance.ToString("C2");
                    InitializeComponent();
                    break;
                case "Produto":
                    SelectedProduct = _productService.FindByGroup(TxtSelection.Text.ToUpper());
                    TxtProdId.Text = SelectedProduct.Id.ToString();
                    TxtProdGroupP.Text = SelectedProduct.GroupP;
                    InitializeComponent();
                    break;
                case "Estoque":
                    string[] pars = TxtSelection.Text.Split(',');
                    SelectedCompany = _companyService.FindByName(pars[0]);
                    SelectedStock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, pars[1]);
                    TxtStkId.Text = SelectedStock.ToString();
                    TxtStkQtyPurchased.Text = SelectedStock.QtyPurchased.ToString();
                    TxtStkQtySold.Text = SelectedStock.QtySold.ToString();
                    TxtStkAmountPurchased.Text = SelectedStock.AmountPurchased.ToString("C2");
                    TxtStkAmountSold.Text = SelectedStock.AmountSold.ToString("C2");
                    CmbStkCompany.SelectedItem = SelectedStock.Company.Name;
                    CmbStkProduct.SelectedItem = SelectedStock.Product.GroupP;
                    DPkrStkLastInput.DisplayDate = SelectedStock.LastInput.Date;
                    DPkrStkLastSale.DisplayDate = SelectedStock.LastSales.Date;
                    InitializeComponent();
                    break;
                default:
                    break;
            }
        }

        //Crud Company
        private void Btn_CreateComp_Click(object sender, RoutedEventArgs e)
        {
            //Method for Create a New Company Record
            try
            {
                if (TxtCoName == null)
                {
                    //Validation required field
                    throw new RequiredFieldException("Favor preencher o nome da empresa para cadastrar");
                }
                //Calling Method Create from service layer and Returning to the log 
                log.AppendLine(_companyService.Create(TxtCoName.Text, Convert.ToDouble(txtCompanyMaxRevenues.Text)));

                TxtBlkLogCRUD.Text = log.ToString();
            }
            catch (Exception ex)
            {
                TxtBlkLogCRUD.Text = "";
                log.Clear();
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                LogTextBlock.Text = log.ToString();
            }
        }
        private void Btn_ReadComp_Click(object sender, RoutedEventArgs e)
        {
            //Method for listing Companies
            //Calling GetCompanies from service layer
            ListCompanies = _companyService.GetCompanies();
            //Rearranjing the list
            var listC = from c in ListCompanies select new { Nome = c.Name, Codigo = c.Id };
            //The following five lines is to display the companies on grid
            GrdView.AutoGenerateColumns = true;
            TxtBCompany.Text = "Lista de Empresas";
            GrdView.ItemsSource = listC.ToList();
            InitializeComponent();
            tbiDataView.IsSelected = true;

        }
        private void Btn_UpdateComp_Click(object sender, RoutedEventArgs e)
        {
            //Method to update a company 
            Company toUpdate = new Company();
            if (TxtCoId.Text == null && TxtCoName == null)
            {
                //validation for update
                throw new RequiredFieldException("Favor preencher o nome ou ID da empresa para Editar");
            }
            else
            {
                //calling method to find company
                toUpdate = _companyService.FindToUdate(Convert.ToInt32(TxtCoId.Text));
            }
            if (toUpdate == null || toUpdate.Id.ToString() != TxtCoId.Text)
            {
                //validation of the return 
                throw new NotFoundException("Nenhuma empresa localizada");
            }
            //Updating temporary model for further update in db context
            toUpdate.Name = TxtCoName.Text;
            toUpdate.Id = Convert.ToInt32(TxtCoId.Text);
            toUpdate.MaxRevenues = Convert.ToDouble(txtCompanyMaxRevenues.Text);
            toUpdate.SetBalance(CalculateCompanyBalance(_stockService.GetStocksByCompany(toUpdate), toUpdate));
            //Calling update method in service layer and returning result to log
            log.AppendLine(_companyService.Update(toUpdate));
            TxtBlkLogCRUD.Text = log.ToString();
        }

        //Products CRUD
        private void Btn_CreateProd_Click(object sender, RoutedEventArgs e)
        {
            //Method for create a new Product follows the same logic of the create company
            try
            {
                if (TxtProdGroupP == null)
                {
                    throw new RequiredFieldException("Favor preencher o nome do produto para cadastrar");
                }

                log.AppendLine(_productService.Create(TxtProdGroupP.Text));

                TxtBlkLogCRUD.Text = log.ToString();
            }
            catch (Exception ex)
            {
                TxtBlkLogCRUD.Text = "";
                log.Clear();
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                LogTextBlock.Text = log.ToString();
            }
        }
        private void Btn_ReadProd_Click(object sender, RoutedEventArgs e)
        {
            //Method to get all products and displying on grid
            ListOfProducts = _productService.GetProducts();
            var listP = from p in ListOfProducts select new { Nome = p.GroupP, Codigo = p.Id };
            GrdView.AutoGenerateColumns = true;
            TxtBCompany.Text = "Lista de Produtos";
            GrdView.ItemsSource = listP.ToList();
            InitializeComponent();
            tbiDataView.IsSelected = true;
        }
        private void Btn_UpdateProd_Click(object sender, RoutedEventArgs e)
        {
            //Method to update product follows the same logic of the update company
            Product toUpdate = new Product();
            if (TxtProdId.Text == null && TxtProdGroupP == null)
            {
                throw new RequiredFieldException("Favor preencher o nome ou ID do Produto para Editar");
            }
            else
            {
                toUpdate = _productService.FindToUdate(TxtProdGroupP.Text, null);
            }
            if (toUpdate == null || toUpdate.Id.ToString() != TxtProdId.Text)
            {
                throw new NotFoundException("Nenhum Produto localizado");
            }
            toUpdate.GroupP = TxtProdGroupP.Text;
            toUpdate.Id = Convert.ToInt32(TxtCoId.Text);
            log.AppendLine(_productService.Update(toUpdate));
            TxtBlkLogCRUD.Text = log.ToString();
        }

        //Crud Stock
        private void Btn_CreateStock_Click(object sender, RoutedEventArgs e)
        {
            //Method to create new Stock control, follows same logic as before.
            try
            {
                SelectedProduct = _productService.FindByGroup(CmbStkProduct.SelectedItem.ToString());
                SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
                if (SelectedProduct == null)
                {
                    throw new NotFoundException("Produto não encontrado!");
                }
                if (SelectedCompany == null)
                {
                    throw new NotFoundException("Empresa não encontrada!");
                }
                DateTime lstin = (DateTime)DPkrStkLastInput.SelectedDate;
                DateTime lstout = (DateTime)DPkrStkLastSale.SelectedDate;
                log.AppendLine(_stockService.Create(SelectedProduct,
                    Convert.ToInt32(TxtStkQtyPurchased.Text),
                    Convert.ToInt32(TxtStkQtySold.Text),
                    Convert.ToDouble(TxtStkAmountPurchased.Text),
                    Convert.ToDouble(TxtStkAmountSold.Text),
                    lstin.Date, lstout.Date,
                    SelectedCompany));

                TxtBlkLogCRUD.Text = log.ToString();
            }
            catch (MyApplicationException ex)
            {
                LogTextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                LogTextBlock.Text = log.ToString();
            }
        }
        private void Btn_ReadStock_Click(object sender, RoutedEventArgs e)
        {
            //Method to List all Stocks By Company
            if (CmbStkCompany != null)
            {
                SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
                if (SelectedCompany==null)
                {
                    throw new NotFoundException("Empresa não localizada");
                }
                ListOfStocks = _stockService.GetStocksByCompany(SelectedCompany);

                GrdView.AutoGenerateColumns = true;
                TxtBCompany.Text = "Lista de Estoques";
                GrdView.ItemsSource = ListOfStocks.ToList();
                InitializeComponent();
                tbiDataView.IsSelected = true;
            }
            else
            {
                throw new RequiredFieldException("Informa a empresa para filtrar os estoques");
            }
        }
        private void btnBalanceCalc_Click(object sender, RoutedEventArgs e)
        {
            SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
            if (SelectedCompany == null)
            {
                //Company must not be null
                throw new ApplicationException("Selecione uma empresa!");
            }
            ListOfStocks = _stockService.CalculateBalance(SelectedCompany);
            GenerateGrid(ListOfStocks, SelectedCompany);   
        }
        //Method to create an manual stock entry
        private void btnEntryStock_Click(object sender, RoutedEventArgs e)
        {
            SelectedProduct = _productService.FindByGroup(CmbStkProduct.SelectedItem.ToString());
            SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
            SelectedStock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, SelectedProduct.GroupP);
            SelectedStock.MovimentInput(
                Convert.ToInt32(TxtStkQtyPurchased.Text),
                Convert.ToDouble(TxtStkAmountPurchased.Text),
                DateTime.Now.Date);
            try
            {
                _stockService.Update(SelectedStock);
                TxtBlkLogCRUD.Text = "Atualizado com sucesso";
                CmbStkCompany.SelectedIndex = -1;
                CmbStkProduct.SelectedIndex = -1;
                TxtStkAmountPurchased.Text = string.Empty;
                TxtStkAmountSold.Text = string.Empty;
                TxtStkQtyPurchased.Text = string.Empty;
                TxtStkQtySold.Text = string.Empty;
            }
            catch (ApplicationException ex)
            {
                TxtBlkLogCRUD.Text = "Erro ao Atualizar" + ex.Message;
                throw new ApplicationException("Erro na Atualização" + ex.Message);
            }
        }
        //Method to create an manual stock sale
        private void btnSaleStock_Click(object sender, RoutedEventArgs e)
        {
            SelectedProduct = _productService.FindByGroup(CmbStkProduct.SelectedItem.ToString());
            SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
            SelectedStock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, SelectedProduct.GroupP);
            SelectedStock.MovimentSale(
                Convert.ToInt32(TxtStkQtySold.Text),
                Convert.ToDouble(TxtStkAmountSold.Text),
                DateTime.Now.Date);
            try
            {
                _stockService.Update(SelectedStock);
                TxtBlkLogCRUD.Text = "Atualizado com sucesso";
                CmbStkCompany.SelectedIndex = -1;
                CmbStkProduct.SelectedIndex = -1;
                TxtStkAmountPurchased.Text = string.Empty;
                TxtStkAmountSold.Text = string.Empty;
                TxtStkQtyPurchased.Text = string.Empty;
                TxtStkQtySold.Text = string.Empty;
            }
            catch (ApplicationException ex)
            {
                TxtBlkLogCRUD.Text = "Erro ao Atualizar" + ex.Message;
                throw new ApplicationException("Erro na Atualização" + ex.Message);
            }


        }
        #endregion

        #region --== NF Control ==--
        private void btnSearchNF_Click(object sender, RoutedEventArgs e)
        {
            //Method for types of querries switching by a combo
            string selection = cmbSearchNF.SelectedItem.ToString();
            switch (selection)
            {
                case "NumNF":
                    NF = _controlNFService.FindByNumber(Convert.ToInt32(txtNumber.Text));
                    anyId = NF.Id;
                    txtNumber.Text = NF.NFNumber.ToString();
                    txtValue.Text = NF.Value.ToString("C2");
                    dpkExpiration.DisplayDate = NF.Expiration.Date;
                    txtOperation.Text = NF.Operation.ToString();
                    cmbTypeNF.SelectedIndex = (int)NF.OperationType.GetTypeCode();
                    cmbNFCompany.SelectedItem = NF.Company.Name;
                    cmbDestinatary.SelectedItem = NF.Destinatary.Name;
                    InitializeComponent();
                    break;
                case "Empresa":
                    Notes = _controlNFService.FindByCompany(cmbNFCompany.SelectedItem.ToString());
                    dtgDataView.AutoGenerateColumns = true;
                    dtgDataView.ItemsSource = Notes.ToList();
                    tbiDataCrud.IsSelected = true;
                    InitializeComponent();
                    break;
                case "Destinatario":
                    Notes = _controlNFService.FindByDestination(cmbDestinatary.SelectedItem.ToString());
                    dtgDataView.AutoGenerateColumns = true;
                    dtgDataView.ItemsSource = Notes.ToList();
                    tbiDataCrud.IsSelected = true;
                    InitializeComponent();
                    break;
                case "Tipo":
                    //Grouping by type and put it on grid pré formated.
                    InitializeComponent();
                    //Observable type is for use in GridView as Grouping See Style on Window XAML
                    ObservableCollection<NFControl> group = new ObservableCollection<NFControl>();
                    //Grouped Query
                    group = _controlNFService.GetObservableNFs();
                    dtgNFData.AutoGenerateColumns = false;
                    //Instance of a collection View
                    ListCollectionView collection = new ListCollectionView(group);
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("OperationType"));
                    dtgNFData.ItemsSource = collection;
                    break;
                case "Experiam Hoje":
                    Notes = _controlNFService.GetByDate();
                    dtgDataView.AutoGenerateColumns = true;
                    dtgDataView.ItemsSource = Notes.ToList();
                    tbiDataCrud.IsSelected = true;
                    InitializeComponent();
                    break;
                case "Expiram Semana":
                    Notes = _controlNFService.GetByWeek();
                    dtgDataView.AutoGenerateColumns = true;
                    dtgDataView.ItemsSource = Notes.ToList();
                    tbiDataCrud.IsSelected = true;
                    InitializeComponent();
                    break;
                default:
                    break;
            }
        }
        private void cmbSearchNF_DropDownOpened(object sender, EventArgs e)
        {
            //Method to reactivate controls when dropdown opens
            txtNumber.IsEnabled = true;
            txtOperation.IsEnabled = true;
            txtValue.IsEnabled = true;
            dpkExpiration.IsEnabled = true;
            cmbTypeNF.IsEnabled = true;
            cmbNFCompany.IsEnabled = true;
            cmbDestinatary.IsEnabled = true;
        }
        private void btnSaveNFControl_Click(object sender, RoutedEventArgs e)
        {
            //Method to Save the new NF on control
            NF.NFNumber = Convert.ToInt32(txtNumber.Text);
            NF.Value = Convert.ToDouble(txtValue.Text);
            NF.Expiration = (DateTime)dpkExpiration.SelectedDate;
            NF.Operation = Convert.ToInt32(txtOperation.Text);
            NF.OperationType = (NFType)cmbTypeNF.SelectedItem;
            NF.Company = _companyService.FindByName(cmbNFCompany.SelectedItem.ToString());
            NF.Destinatary = _personService.FindByName(cmbDestinatary.SelectedItem.ToString());
            NF.GeneratorProposals = txtOriginProps.Text;
            TxtBlkLogNF.Text = _controlNFService.Crete(NF);

        }
        private void btnCreateNewNF_Click(object sender, RoutedEventArgs e)
        {
            //Method to activate controls when click on create new.
            txtNumber.IsEnabled = true;
            txtOperation.IsEnabled = true;
            txtValue.IsEnabled = true;
            dpkExpiration.IsEnabled = true;
            cmbTypeNF.IsEnabled = true;
            CmbCompany.IsEnabled = true;
            cmbDestinatary.IsEnabled = true;

        }
        private void btnEditNF_Click(object sender, RoutedEventArgs e)
        {
            //method to update/edit nf record
            NFControl nfToUpdate = new NFControl();
            nfToUpdate.Id = anyId;
            nfToUpdate.NFNumber = Convert.ToInt32(txtNumber.Text);
            nfToUpdate.Value = Convert.ToDouble(txtValue.Text);
            nfToUpdate.Expiration = (DateTime)dpkExpiration.SelectedDate;
            nfToUpdate.Operation = Convert.ToInt32(txtOperation.Text);
            nfToUpdate.OperationType = (NFType)cmbTypeNF.SelectedItem;
            nfToUpdate.Company = _companyService.FindByName(cmbNFCompany.SelectedItem.ToString());
            nfToUpdate.Destinatary = _personService.FindByName(cmbDestinatary.SelectedItem.ToString());
            nfToUpdate.GeneratorProposals = txtOriginProps.Text;
            TxtBlkLogNF.Text = _controlNFService.Update(nfToUpdate);
        }
        private void btnDeleteNF_Click(object sender, RoutedEventArgs e)
        {
            //method to delete nf
            if (NF != null)
            {
                _controlNFService.Delete(NF);
            }
        }
        #endregion

        #region --== CRUD Auxiliary dor NF Control ==--
        private void rbtCity_Click(object sender, RoutedEventArgs e)
        {
            //Method to alternates function of buttons
            if (rbtCity.IsChecked == true)
            {
                btnCreate.Content = "Criar Cidade";
                btnEdit.Content = "Editar Cidade";
                btnDelete.Content = "Deletar Cidade";
                btnSearch.Content = "Busca Cidade";
            }
        }
        private void rbtPerson_Click(object sender, RoutedEventArgs e)
        {
            //Method to alternates function of buttons
            if (rbtPerson.IsChecked == true)
            {
                btnCreate.Content = "Criar Pessoa";
                btnEdit.Content = "Editar Pessoa";
                btnDelete.Content = "Deletar Pessa";
                btnSearch.Content = "Busca Pessoa";
            }
        }
        private void rbtCompany_Click(object sender, RoutedEventArgs e)
        {
            //Method to alternates function of buttons
            if (rbtCompany.IsChecked == true)
            {
                btnCreate.Content = "Criar Empresa";
                btnEdit.Content = "Editar Empresa";
                btnDelete.Content = "Deletar Empresa";
                btnSearch.Content = "Busca Empresa";
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //Method to search according to combo selection and function selection
            if (rbtCity.IsChecked == true)
            {
                //City search method
                if (txtIteration.Text.Trim() == string.Empty)
                {
                    dtgDataView.AutoGenerateColumns = true;
                    dtgDataView.ItemsSource = _cityService.GetCities().ToList();
                }
                else
                {
                    City = _cityService.FindByName(txtIteration.Text.Trim());
                    lblCityId.Content = City.Id;
                    txtCity.Text = City.CityName;
                    cmbCityState.SelectedItem = City.State;
                }
            }
            else if (rbtPerson.IsChecked == true)
            {
                //Person search method
                if (txtIteration.Text.Trim() == string.Empty)
                {
                    dtgDataView.AutoGenerateColumns = true;
                    dtgDataView.ItemsSource = _personService.GetPeople().ToList();
                }
                else
                {
                    Person = _personService.FindByName(txtIteration.Text.Trim());
                    lblPersonId.Content = Person.Id;
                    txtPersonName.Text = Person.Name;
                    txtPersonDoc.Text = Person.Doc;
                    cmbCities.SelectedItem = Person.City;
                    cmbCityState.SelectedItem = Person.State;
                    cmbPersonType.SelectedItem = Person.Type;
                    cmbPersonCategory.SelectedItem = Person.Category;
                }
            }
            else if (rbtCompany.IsChecked == true)
            {
                //Company search method
                if (txtIteration.Text.Trim() == string.Empty)
                {
                    dtgDataView.AutoGenerateColumns = true;
                    dtgDataView.ItemsSource = _companyService.GetCompanies().ToList();
                }
                else
                {
                    SelectedCompany = null;
                    SelectedCompany = _companyService.FindByName(txtIteration.Text.Trim());
                    lblCompanyID.Content = SelectedCompany.Id;
                    txtCompanyNF.Text = SelectedCompany.Name;
                    txtMaxRevenuesNF.Text = SelectedCompany.MaxRevenues.ToString("C2");
                    txtCompanyBalanceNF.Text = SelectedCompany.Balance.ToString("C2");

                }
            }
            return;
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //Method to create new records according to function selection
            if (rbtCity.IsChecked == true)
            {
                //Create Citt
                City c = new City(txtCity.Text, (State)cmbCityState.SelectedItem);
                MessageBox.Show(_cityService.Create(c));
            }
            else if (rbtPerson.IsChecked == true)
            {
                //Create Person
                City city = _cityService.FindByName(cmbCities.SelectedItem.ToString());
                Person person = new Person(
                    txtPersonName.Text,
                    txtPersonDoc.Text,
                    city,
                    (State)cmbState.SelectedItem,
                    (PersonType)cmbPersonType.SelectedItem,
                    (PersonCategory)cmbPersonCategory.SelectedItem);
                MessageBox.Show(_personService.Create(person));
            }
            else if (rbtCompany.IsChecked == true)
            {
                //Create Company
                Company newCompany = new Company();
                newCompany.Name = txtCompanyNF.Text;
                newCompany.MaxRevenues = Convert.ToDouble(txtMaxRevenuesNF.Text);
                newCompany.SetBalance(0.0d);
                
                MessageBox.Show(_companyService.Create(newCompany));
            }
            return;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //Method to Edit 
            if (rbtCity.IsChecked == true)
            {
                //Edit City
                if (lblCityId.Content == null)
                {
                    MessageBox.Show("Antes de editar precisa localizar");
                    return;
                }
                City.CityName = txtCity.Text;
                City.State = (State)cmbCityState.SelectedItem;
                MessageBox.Show(_cityService.Update(City));
            }
            else if (rbtPerson.IsChecked == true)
            {
                //Edit Person
                if (lblPersonId.Content == null)
                {
                    MessageBox.Show("Antes de editar precisa localizar");
                    return;
                }
                Person.Name = txtPersonName.Text;
                Person.Doc = txtPersonDoc.Text;
                Person.Category = (PersonCategory)cmbPersonCategory.SelectedItem;
                Person.Type = (PersonType)cmbPersonType.SelectedItem;
                Person.City = _cityService.FindByName(cmbCities.SelectedItem.ToString());
                Person.State = (State)cmbState.SelectedItem;
                MessageBox.Show(_personService.Update(Person));
            }
            else if (rbtCompany.IsChecked == true)
            {
                //Edit Company
                if (lblCompanyID.Content == null)
                {
                    MessageBox.Show("Antes de editar precisa localizar");
                    return;
                }
                SelectedCompany.Name = txtCompanyNF.Text;
                SelectedCompany.MaxRevenues = Convert.ToDouble(txtMaxRevenuesNF.Text);
                SelectedCompany.SetBalance(
                    CalculateCompanyBalance(
                        _stockService.GetStocksByCompany(SelectedCompany),
                        SelectedCompany));
                MessageBox.Show(_companyService.Update(SelectedCompany));
            }
            return;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Delete Method for NF Control
            if (rbtCity.IsChecked == true)
            {
                //Delete City
                if (lblCityId.Content == null)
                {
                    MessageBox.Show("Antes de Deletar precisa localizar");
                    return;
                }
                MessageBoxResult result = MessageBox.Show("Realmente deseja excluir? " 
                    + City.CityName, 
                    "Confirmation", 
                    MessageBoxButton.YesNoCancel);
                if (result==MessageBoxResult.Yes)
                {
                    MessageBox.Show(_cityService.Delete(City));

                }
                else
                {
                    return;
                }
            }
            else if (rbtPerson.IsChecked == true)
            {
                //Delete Person

                if (lblPersonId.Content == null)
                {
                    MessageBox.Show("Antes de Deletar precisa localizar");
                    return;
                }
                MessageBoxResult result = MessageBox.Show("Realmente deseja excluir? "
                   + Person.Name,
                   "Confirmation",
                   MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(_personService.Delete(Person));

                }
                else
                {
                    return;
                }
            }
            else if (rbtCompany.IsChecked == true)
            {
                //Delete Company
                MessageBox.Show("Empresas não podem ser apagadas");
                return;
            }
            return;
        }


        #endregion

        #region --== Local Methods ==--
        private void GenerateGrid(IEnumerable<Object> gridContent, Company c)
        {
            //Changes view to the tab of dataview
            tbiDataView.IsSelected = true;
            //Activates the auto generate columns for the grid manages himself
            GrdView.AutoGenerateColumns = true;
            //attributes the data to grid
            TxtBCompany.Text = c.Name; //Label on top
            //attach list to grid for result
            GrdView.ItemsSource = gridContent.ToList();
            InitializeComponent();
        }
        private double CalculateCompanyBalance(IEnumerable<Stock> list, Company c)
        {
            double sum=0.0d;
            foreach (Stock item in list)
            {
                sum += item.AmountSold;
            }
           
            MessageBox.Show(_companyService.Update(c), 
                "Resultado", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
            return sum;
        }
        #endregion
    }
}
