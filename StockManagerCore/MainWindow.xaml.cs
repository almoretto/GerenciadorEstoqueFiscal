
#region --== Dependency Declaration ==--
using System;
using System.Windows;
using StockManagerCore.Services;
using StockManagerCore.Services.Exceptions;
using StockManagerCore.Models;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
#endregion

namespace StockManagerCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region --== Instances of Context, Provider and StringBuilder ==--
        private readonly InputService _inputService;
        private readonly SaleService _saleService;
        private readonly ProductService _productService;
        private readonly CompanyService _companyService;
        private readonly StockService _stockService;
        CultureInfo provider = CultureInfo.InvariantCulture;
        StringBuilder log = new StringBuilder();
        DataGridTextColumn c = new DataGridTextColumn();
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
        #endregion

        #region --== Models instantitation and support Lists ==--
        private Company SelectedCompany { get; set; } = new Company();
        private Product SelectedProduct { get; set; } = new Product();
        private Stock SelectedStock { get; set; } = new Stock();
        private InputProduct InputProduct { get; set; } = new InputProduct();
        private List<SoldProduct> ListOfSales { get; set; } = new List<SoldProduct>();
        public IEnumerable<Company> ListCompanies { get; set; }
        public IEnumerable<object> ListOfStocks { get; set; }
        public IEnumerable<Product> ListOfProducts { get; set; }
        #endregion

        public MainWindow(InputService inputService, SaleService saleService, ProductService productService,
            CompanyService companyService, StockService stockService)
        {
            _inputService = inputService;
            _saleService = saleService;
            _productService = productService;
            _companyService = companyService;
            _stockService = stockService;

            InitializeComponent();

            ListCompanies = _companyService.GetCompanies();
            ListOfProducts = _productService.GetProducts();
            foreach (Company c in ListCompanies)
            {
                CmbCompany.Items.Add(c.Name);
                CmbStkCompany.Items.Add(c.Name);
            }
            foreach (Product product in ListOfProducts)
            {
                CmbStkProduct.Items.Add(product.GroupP);
            }
            InitializeComponent();
          
        }

        #region --== Functions of Tb_Functions TAB ==--
        private void BtnFileOpen_Click(object sender, RoutedEventArgs e)
        {
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
                LogTextBlock.Text = "";
                log.AppendLine(ex.Message);
                LogTextBlock.Text = log.ToString();
            }
        }
        private void ProcessInputs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogTextBlock.Text = string.Empty;
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);
               
                if (filename.EndsWith("csv") || filename.EndsWith("CSV"))
                {
                    throw new ApplicationException("Não pode processar arquivo de venda como entrada!");
                }
                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Selecione uma empresa!");
                }
                LogTextBlock.Text = "Importando: " + filename;
                sales = false;
                log.Clear();
                if (!sales)
                {
                    //Instance the service
                    nfeReader = new FileReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(nfeReader.GetInputItens());
                    LogTextBlock.Text = log.ToString();
                    foreach (InputNFe item in nfeReader.Inputs)
                    {
                        Product p = new Product();
                        p = _productService.FindByGroup(item.Group);

                        item.AlternateNames();

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

                        _inputService.InsertInputs(InputProduct);
                    }
                }
                else //Exception
                {
                    LogTextBlock.Text = "";
                    log.AppendLine("Não pode processar venda como Compra!");
                    LogTextBlock.Text = log.ToString();
                    throw new ApplicationException("Não pode processar venda como Compra!");
                }

            }
            catch (Exception ex)
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
        private void ProcessSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogTextBlock.Text = string.Empty;
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);

                if (filename.EndsWith("TXT") || filename.EndsWith("txt"))
                {
                    throw new ApplicationException("Não pode processar arquivo de entrada como venda!");
                }
                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Selecione uma empresa!");
                }

                LogTextBlock.Text = "Importando: " + filename;
                sales = true;
                log.Clear();

                if (sales)
                {
                    //Instance the service
                    nfeReader = new FileReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(nfeReader.GetInputItens());
                    LogTextBlock.Text = log.ToString();
                    foreach (InputNFe item in nfeReader.Inputs)
                    {
                        Product p = new Product();
                        p = _productService.FindByGroup(item.Group);

                        if (p == null)
                        {
                            throw new ApplicationException(" Pelo menos um produto da Nota não foi encontrado! \n Importação abortada!");
                        }
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
                    _saleService.InsertMultiSales(ListOfSales);
                }
                else
                {
                    LogTextBlock.Text = "";
                    log.AppendLine("Não pode processar entrada como venda!");
                    LogTextBlock.Text = log.ToString();
                    throw new ApplicationException("Não pode processar entrada como venda!");
                }

            }
            catch (Exception ex)
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
        private void Btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = 0;
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);

                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Empresa deve ser selecionada!");
                }

                if (RdnIn.IsChecked == true)
                {
                    dateInitial = DateTime.ParseExact(TxtDateInitial.Text, "dd/MM/yyyy", provider);

                    IEnumerable<InputProduct> listInputProduct = _inputService.GetInputsByDateAndCompany(dateInitial, SelectedCompany);

                    var groupProducts = listInputProduct.GroupBy(p => p.XProd);

                    //moving through grouping
                    foreach (IGrouping<string, InputProduct> group in groupProducts)
                    {
                        Stock stock = new Stock();
                        stock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, group.Key);

                        log.Append("Produto: " + group.Key);
                        TxtConsole.Text = log.ToString();

                        foreach (InputProduct item in group)
                        {
                            qty += item.QCom;
                            amount += item.Vtotal;
                            count++;
                        }

                        stock.MovimentInput(qty, amount, dateInitial);
                        _stockService.Update(stock);

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
                if (RdnOut.IsChecked == true && SelectedCompany != null)//Continue from here 30/10
                {
                    dateInitial = DateTime.ParseExact(TxtDateInitial.Text, "dd/MM/yyyy", provider);
                    dateFinal = DateTime.ParseExact(TxtDateFinal.Text, "dd/MM/yyyy", provider);

                    IEnumerable<SoldProduct> salesByDateAndCompany = _saleService.GetSalesByDateAndCompany(dateInitial, dateFinal, SelectedCompany);

                    var groupOfSales = salesByDateAndCompany.GroupBy(p => p.Product.GroupP);

                    //moving through grouping
                    foreach (IGrouping<string, SoldProduct> group in groupOfSales)
                    {
                        Stock stock = new Stock();
                        stock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, group.Key);

                        log.Append("Produto: " + group.Key);
                        TxtConsole.Text = log.ToString();

                        foreach (SoldProduct item in group)
                        {
                            qty += item.QCom;
                            amount += item.Vtotal;
                        }

                        stock.MovimentSale(qty, amount, dateInitial);

                        stock.MovimentInput(qty, amount, dateInitial);
                        _stockService.Update(stock);

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
                log.AppendLine("Lista Entradas: " + count.ToString());
                log.AppendLine("Lista Saídas: " + ListOfSales.Count);
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
        private void Btn_ShowStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedCompany = _companyService.FindByName((string)CmbCompany.SelectedItem);

                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Selecione uma empresa!");
                }
                tbiDataView.IsSelected = true;
                GrdView.AutoGenerateColumns = true;
                TxtBCompany.Text = CmbCompany.SelectedItem.ToString();
                ListOfStocks = _stockService.GetStocksFormated(SelectedCompany);

                GrdView.ItemsSource = ListOfStocks.ToList();
                InitializeComponent();
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
        #endregion

        #region --== CRUD Companies, Products and Stock==--
        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            switch (CmbSwitch.SelectedItem.ToString())
            {
                case "Empresa":
                    SelectedCompany = _companyService.FindByName(TxtSelection.Text.ToUpper());
                    TxtCoId.Text = SelectedCompany.Id.ToString();
                    TxtCoName.Text = SelectedCompany.Name;
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
            try
            {
                if (TxtCoName == null)
                {
                    throw new RequiredFieldException("Favor preencher o nome da empresa para cadastrar");
                }

                log.AppendLine(_companyService.Create(TxtCoName.Text));

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
            ListCompanies = _companyService.GetCompanies();
            var listC = from c in ListCompanies select new { Nome = c.Name, Codigo = c.Id };
            GrdView.AutoGenerateColumns = true;
            TxtBCompany.Text = "Lista de Empresas";
            GrdView.ItemsSource = listC.ToList();
            InitializeComponent();
            tbiDataView.IsSelected = true;

        }
        private void Btn_UpdateComp_Click(object sender, RoutedEventArgs e)
        {
            Company toUpdate = new Company();
            if (TxtCoId.Text == null && TxtCoName == null)
            {
                throw new RequiredFieldException("Favor preencher o nome ou ID da empresa para Editar");
            }
            else
            {
                toUpdate = _companyService.FindToUdate(Convert.ToInt32(TxtCoId.Text));
            }
            if (toUpdate == null || toUpdate.Id.ToString() != TxtCoId.Text)
            {
                throw new NotFoundException("Nenhuma empresa localizada");
            }
            toUpdate.Name = TxtCoName.Text;
            toUpdate.Id = Convert.ToInt32(TxtCoId.Text);
            log.AppendLine(_companyService.Update(toUpdate));
            TxtBlkLogCRUD.Text = log.ToString();
        }

        //Products CRUD
        private void Btn_CreateProd_Click(object sender, RoutedEventArgs e)
        {
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
            if (CmbStkCompany != null)
            {
                SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedIndex.ToString());
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
        #endregion


    }
}
