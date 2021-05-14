#region --== Classes Dependencies using area ==--
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using StockManagerCore.Models;
using StockManagerCore.Services;
#endregion

namespace StockManagerCore.UserInterface
{
    /// <summary>
    /// Lógica interna para WdnProcessFiles.xaml
    /// </summary>
    public partial class WdnProcessFiles : Window
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

        //declaration of culture info for less verbose code
        CultureInfo provider = CultureInfo.InvariantCulture;
        //Declaration of the log string builder. This log is merely for information and its not stored in any place for a while.
        StringBuilder log = new StringBuilder();
        #endregion

        #region --== Local Variables ==--
        int importCount = 0;
        string lastfile;
        string filename;
        FileReader nfeReader;
        bool isSales;
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
        private InputProduct InputProduct { get; set; } = new InputProduct();
        private List<SoldProduct> ListOfSales { get; set; } = new List<SoldProduct>();
        private IEnumerable<Company> ListCompanies { get; set; }
        private List<DispStockCompany> ListOfStocksStruct { get; set; }


        #endregion
        public WdnProcessFiles(InputService inputService, SaleService saleService, ProductService productService,
            CompanyService companyService, StockService stockService)
        {
            //Constructor of the form MainWindow here we call the dependency injection

            _inputService = inputService;
            _saleService = saleService;
            _productService = productService;
            _companyService = companyService;
            _stockService = stockService;

            InitializeComponent();
            //these is for populate de comboboxes.
            ListCompanies = _companyService.GetCompanies();
            btnBalanceAll.IsEnabled = false;
            btnProcessFile.IsEnabled = false;
            btnProcessSales.IsEnabled = false;
            BtnCalculate.IsEnabled = false;

            foreach (Company c in ListCompanies)
            {
                CmbCompany.Items.Add(c.Name);
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
            importCount++;
            //This Method is responsible to care of process the import files in input records.
            try
            {
                //Clean log
                LogTextBlock.Text = string.Empty;
                LogTextBlock.Text = "Iniciando importação em: " + importCount.ToString();
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
                if (lastfile == filename)
                {
                    throw new ApplicationException("Nota já Inserida");
                }
                //Here just inform that imports is occurring
                LogTextBlock.Text = "\nImportando: " + filename;
                //Sets the bool variable to False to indicate to the service class where to process this file, 
                //because this file is input record.
                isSales = false;
                log.Clear();
                if (!isSales) //If Sales = False or !(not) True
                {
                    btnProcessFile.IsEnabled = false; //Disable the ProcessFile Button
                    //Instance the service that process the file. 
                    //Calling the constructor for reading the file passing the File Path and 
                    //the Bool variable to indicate where to process
                    nfeReader = new FileReader(filename, isSales);
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

                        //callcing an method to padronize the names of the products as groups.
                        item.AlternateNames();

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

                MessageBoxResult result = MessageBox.Show("Upload Terminado, Inserir nova? " +
                                                        "\n Ao pressionar não o sistema irá limpar tudo!" +
                                                        "\n Inseridos: " + LogTextBlock.Text + " registros",
                                                            "Controle de Estoque",
                                                            MessageBoxButton.YesNo,
                                                            MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    LogTextBlock.Text = String.Empty;
                    lastfile = filename;
                    ClearFile();
                    importCount = 0;
                    btnProcessFile.IsEnabled = true;
                }
                else
                {
                    ClearForm();
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
                if (lastfile == filename)
                {
                    throw new ApplicationException("Nota já Inserida");
                }
                //Here just inform that imports is occurring
                LogTextBlock.Text = "Importando: " + filename;
                //Sets the bool variable to TRUE to indicate to the service class where to process this file, 
                //because this file is Sales record.
                isSales = true;
                log.Clear();

                if (isSales)
                {
                    btnProcessSales.IsEnabled = false;
                    //Instance the service
                    nfeReader = new FileReader(filename, isSales);
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

                MessageBoxResult result = MessageBox.Show("Upload Terminado, Inserir nova? " +
                                                        "\n Ao pressionar não o sistema irá limpar tudo!" +
                                                        "\n Inseridos: " + LogTextBlock.Text + " registros",
                                                            "Controle de Estoque",
                                                            MessageBoxButton.YesNo,
                                                            MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    LogTextBlock.Text = String.Empty;
                    lastfile = filename;
                    ClearFile();
                    importCount = 0;
                    btnProcessSales.IsEnabled = true;

                }
                else
                {
                    ClearForm();
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
                    BtnCalculate.IsEnabled = false;
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
                    BtnCalculate.IsEnabled = false;
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
                MessageBoxResult result = MessageBox.Show("Processamento Terminado, efetuar novo? " +
                                                                "\n Ao pressionar não o sistema irá limpar tudo!",
                                                                    "Controle de Estoque",
                                                                    MessageBoxButton.YesNo,
                                                                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    LogTextBlock.Text = String.Empty;
                    lastfile = filename;
                    ClearFile();
                    importCount = 0;
                    BtnCalculate.IsEnabled = true;
                    TxtConsole.Text = string.Empty;
                }
                else
                {
                    ClearForm();
                }
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
                ListOfStocksStruct = _stockService.GetStocksStructured(SelectedCompany); //list Resulkt
                                                                                         //Method to generate GridView
                WdnStockGrid viewStockGrid = new WdnStockGrid( ListOfStocksStruct , SelectedCompany );
                viewStockGrid.ShowDialog();
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
        private void BtnBalanceAll_Click(object sender, RoutedEventArgs e)
        {
            List<Stock> stocksToCalc;
            stocksToCalc = _stockService.GetStocks();

            foreach (Stock item in stocksToCalc)
            {
                try
                {
                    item.SetBalance();

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
            _stockService.UpdateRange(stocksToCalc);
            MessageBox.Show("Calculo Efetuado em:" + DateTime.Now.ToString("dd/MMM/yyyy"),
                "Atualização de Saldo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ClearFile();
            ClearForm();
            this.Close();
        }
        #endregion


        #region --== Local Methods ==--

        private void ClearFile()
        {
            filename = string.Empty;
            FileNameTextBox.Text = string.Empty;
        }

        private void ClearForm()
        {
            LogTextBlock.Text = String.Empty;
            TxtConsole.Text = String.Empty;
            lastfile = filename;
            ClearFile();
            importCount = 0;
            BtnCalculate.IsEnabled = true;
            btnProcessSales.IsEnabled = true;
            btnProcessFile.IsEnabled = true;
            btnBalanceAll.IsEnabled = true;
        }
        #endregion

        
    }
}
