
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
                CMB_Company.Items.Add(c.Name);
                Cmb_StkCompany.Items.Add(c.Name);
            }
            foreach (Product product in ListOfProducts)
            {
                Cmb_StkProduct.Items.Add(product.GroupP);
            }
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
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                Log_TextBlock.Text = log.ToString();
            }
        }
        private void ProcessInputs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedCompany = _companyService.FindByName((string)CMB_Company.SelectedItem);

                if (filename.EndsWith("csv") || filename.EndsWith("CSV"))
                {
                    throw new ApplicationException("Não pode processar arquivo de venda como entrada!");
                }
                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Selecione uma empresa!");
                }

                sales = false;
                log.Clear();
                if (!sales)
                {
                    //Instance the service
                    nfeReader = new FileReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(nfeReader.GetInputItens());
                    Log_TextBlock.Text = log.ToString();
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
                    Log_TextBlock.Text = "";
                    log.AppendLine("Não pode processar venda como Compra!");
                    Log_TextBlock.Text = log.ToString();
                    throw new ApplicationException("Não pode processar venda como Compra!");
                }

            }
            catch (Exception ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }

                Log_TextBlock.Text = log.ToString();
            }
        }
        private void ProcessSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedCompany = _companyService.FindByName((string)CMB_Company.SelectedItem);

                if (filename.EndsWith("TXT") || filename.EndsWith("txt"))
                {
                    throw new ApplicationException("Não pode processar arquivo de entrada como venda!");
                }
                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Selecione uma empresa!");
                }

                sales = true;
                log.Clear();

                if (sales)
                {
                    //Instance the service
                    nfeReader = new FileReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(nfeReader.GetInputItens());
                    Log_TextBlock.Text = log.ToString();
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
                    Log_TextBlock.Text = "";
                    log.AppendLine("Não pode processar entrada como venda!");
                    Log_TextBlock.Text = log.ToString();
                    throw new ApplicationException("Não pode processar entrada como venda!");
                }

            }
            catch (Exception ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                Log_TextBlock.Text = log.ToString();
            }
        }
        private void Btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = 0;
                SelectedCompany = _companyService.FindByName((string)CMB_Company.SelectedItem);

                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Empresa deve ser selecionada!");
                }

                if (Rdn_In.IsChecked == true)
                {
                    dateInitial = DateTime.ParseExact(Txt_DateInitial.Text, "dd/MM/yyyy", provider);

                    IEnumerable<InputProduct> listInputProduct = _inputService.GetInputsByDateAndCompany(dateInitial, SelectedCompany);

                    var groupProducts = listInputProduct.GroupBy(p => p.XProd);

                    //moving through grouping
                    foreach (IGrouping<string, InputProduct> group in groupProducts)
                    {
                        Stock stock = new Stock();
                        stock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, group.Key);

                        log.Append("Produto: " + group.Key);
                        txt_Console.Text = log.ToString();

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
                        txt_Console.Text = log.ToString();
                        qteTot += qty;
                        totAmount += amount;
                        qty = 0;
                        amount = 0.0;
                    }

                    log.Append("QteTotal: " + qteTot.ToString());
                    log.AppendLine(" | ValorTotal: " + totAmount.ToString("C2", CultureInfo.CurrentCulture));
                    txt_Console.Text = log.ToString();
                }
                if (Rdn_Out.IsChecked == true && SelectedCompany != null)//Continue from here 30/10
                {
                    dateInitial = DateTime.ParseExact(Txt_DateInitial.Text, "dd/MM/yyyy", provider);
                    dateFinal = DateTime.ParseExact(Txt_DateFinal.Text, "dd/MM/yyyy", provider);

                    IEnumerable<SoldProduct> salesByDateAndCompany = _saleService.GetSalesByDateAndCompany(dateInitial, dateFinal, SelectedCompany);

                    var groupOfSales = salesByDateAndCompany.GroupBy(p => p.Product.GroupP);

                    //moving through grouping
                    foreach (IGrouping<string, SoldProduct> group in groupOfSales)
                    {
                        Stock stock = new Stock();
                        stock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, group.Key);

                        log.Append("Produto: " + group.Key);
                        txt_Console.Text = log.ToString();

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
                        txt_Console.Text = log.ToString();
                        qteTot += qty;
                        totAmount += amount;
                        qty = 0;
                        amount = 0.0;
                    }
                    log.Append("QteTotal: " + qteTot.ToString());
                    log.AppendLine(" | ValorTotal: " + totAmount.ToString("C2", CultureInfo.CurrentCulture));
                    txt_Console.Text = log.ToString();

                }
                log.AppendLine("Lista Entradas: " + count.ToString());
                log.AppendLine("Lista Saídas: " + ListOfSales.Count);
            }
            catch (ApplicationException ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                Log_TextBlock.Text = log.ToString();
            }
        }
        private void Btn_ShowStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedCompany = _companyService.FindByName((string)CMB_Company.SelectedItem);

                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Selecione uma empresa!");
                }
                Tb_DataView.IsSelected = true;
                Grd_View.AutoGenerateColumns = true;
                TxtB_Company.Text = CMB_Company.SelectedItem.ToString();
                ListOfStocks = _stockService.GetStocksFormated(SelectedCompany);

                Grd_View.ItemsSource = ListOfStocks.ToList();
                InitializeComponent();
            }
            catch (ApplicationException ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }

                Log_TextBlock.Text = log.ToString();
            }
        }
        #endregion

        #region --== CRUD Companies, Products and Stock==--
        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            switch (Cmb_Switch.SelectedItem.ToString())
            {
                case "Empresa":
                    SelectedCompany = _companyService.FindByName(Txt_Selection.Text.ToUpper());
                    Txt_CoId.Text = SelectedCompany.Id.ToString();
                    Txt_CoName.Text = SelectedCompany.Name;
                    InitializeComponent();
                    break;
                case "Produto":
                    SelectedProduct = _productService.FindByGroup(Txt_Selection.Text.ToUpper());
                    Txt_prodId.Text = SelectedProduct.Id.ToString();
                    Txt_ProdGroupP.Text = SelectedProduct.GroupP;
                    InitializeComponent();
                    break;
                case "Estoque":
                    string[] pars = Txt_Selection.Text.Split(',');
                    SelectedCompany = _companyService.FindByName(pars[0]);
                    SelectedStock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, pars[1]);
                    Txt_StkId.Text = SelectedStock.ToString();
                    Txt_StkQtyPurchased.Text = SelectedStock.QtyPurchased.ToString();
                    Txt_StkQtySold.Text = SelectedStock.QtySold.ToString();
                    Txt_StkAmountPurchased.Text = SelectedStock.AmountPurchased.ToString("C2");
                    Txt_StkAmountSold.Text = SelectedStock.AmountSold.ToString("C2");
                    Cmb_StkCompany.SelectedItem = SelectedStock.Company.Name;
                    Cmb_StkProduct.SelectedItem = SelectedStock.Product.GroupP;
                    Dpk_StkLastInput.DisplayDate = SelectedStock.LastInput.Date;
                    Dpk_StkLastSale.DisplayDate = SelectedStock.LastSales.Date;
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
                if (Txt_CoName == null)
                {
                    throw new RequiredFieldException("Favor preencher o nome da empresa para cadastrar");
                }

                log.AppendLine(_companyService.Create(Txt_CoName.Text));

                TxtBlk_LogCRUD.Text = log.ToString();
            }
            catch (Exception ex)
            {
                TxtBlk_LogCRUD.Text = "";
                log.Clear();
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                Log_TextBlock.Text = log.ToString();
            }
        }
        private void Btn_ReadComp_Click(object sender, RoutedEventArgs e)
        {
            ListCompanies = _companyService.GetCompanies();
            var listC = from c in ListCompanies select new { Nome = c.Name, Codigo = c.Id };
            Grd_View.AutoGenerateColumns = true;
            TxtB_Company.Text = "Lista de Empresas";
            Grd_View.ItemsSource = listC.ToList();
            InitializeComponent();
            Tb_DataView.IsSelected = true;

        }
        private void Btn_UpdateComp_Click(object sender, RoutedEventArgs e)
        {
            Company toUpdate = new Company();
            if (Txt_CoId.Text == null && Txt_CoName == null)
            {
                throw new RequiredFieldException("Favor preencher o nome ou ID da empresa para Editar");
            }
            else
            {
                toUpdate = _companyService.FindToUdate(Convert.ToInt32(Txt_CoId.Text));
            }
            if (toUpdate == null || toUpdate.Id.ToString() != Txt_CoId.Text)
            {
                throw new NotFoundException("Nenhuma empresa localizada");
            }
            toUpdate.Name = Txt_CoName.Text;
            toUpdate.Id = Convert.ToInt32(Txt_CoId.Text);
            log.AppendLine(_companyService.Update(toUpdate));
            TxtBlk_LogCRUD.Text = log.ToString();
        }
        private void Btn_DeleteComp_Click(object sender, RoutedEventArgs e)
        {
            Company toDelete = new Company();
            if (Txt_CoId.Text == null && Txt_CoName == null)
            {
                throw new RequiredFieldException("Favor preencher o nome ou ID da empresa para Deletar");
            }
            else if (Txt_CoName.Text == null && Txt_CoId != null)
            {
                toDelete = _companyService.FindToUdate(Convert.ToInt32(Txt_CoId.Text));
            }
            if (toDelete == null)
            {
                throw new NotFoundException("Nenhuma empresa localizada");
            }
            log.AppendLine(_companyService.Delete(toDelete));
            TxtBlk_LogCRUD.Text = log.ToString();
        }

        //Products CRUD
        private void Btn_CreateProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Txt_ProdGroupP == null)
                {
                    throw new RequiredFieldException("Favor preencher o nome do produto para cadastrar");
                }

                log.AppendLine(_productService.Create(Txt_ProdGroupP.Text));

                TxtBlk_LogCRUD.Text = log.ToString();
            }
            catch (Exception ex)
            {
                TxtBlk_LogCRUD.Text = "";
                log.Clear();
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                Log_TextBlock.Text = log.ToString();
            }
        }
        private void Btn_ReadProd_Click(object sender, RoutedEventArgs e)
        {
            ListOfProducts = _productService.GetProducts();
            var listP = from p in ListOfProducts select new { Nome = p.GroupP, Codigo = p.Id };
            Grd_View.AutoGenerateColumns = true;
            TxtB_Company.Text = "Lista de Produtos";
            Grd_View.ItemsSource = listP.ToList();
            InitializeComponent();
            Tb_DataView.IsSelected = true;
        }
        private void Btn_UpdateProd_Click(object sender, RoutedEventArgs e)
        {
            Product toUpdate = new Product();
            if (Txt_prodId.Text == null && Txt_ProdGroupP == null)
            {
                throw new RequiredFieldException("Favor preencher o nome ou ID do Produto para Editar");
            }
            else
            {
                toUpdate = _productService.FindToUdate(Txt_ProdGroupP.Text, null);
            }
            if (toUpdate == null || toUpdate.Id.ToString() != Txt_prodId.Text)
            {
                throw new NotFoundException("Nenhum Produto localizado");
            }
            toUpdate.GroupP = Txt_ProdGroupP.Text;
            toUpdate.Id = Convert.ToInt32(Txt_CoId.Text);
            log.AppendLine(_productService.Update(toUpdate));
            TxtBlk_LogCRUD.Text = log.ToString();
        }
        private void Btn_DeleteProd_Click(object sender, RoutedEventArgs e)
        {
            Product toDelete = new Product();
            if (Txt_prodId.Text == null && Txt_ProdGroupP == null)
            {
                throw new RequiredFieldException("Favor preencher o nome ou ID do Produto para Deletar");
            }
            else if (Txt_prodId.Text == null && Txt_ProdGroupP != null)
            {
                toDelete = _productService.FindToUdate(Txt_ProdGroupP.Text, null);
            }
            else if (Txt_ProdGroupP.Text == null && Txt_prodId != null)
            {
                toDelete = _productService.FindToUdate("", Convert.ToInt32(Txt_prodId.Text));
            }
            if (toDelete == null)
            {
                throw new NotFoundException("Nenhum Produto localizado");
            }
            log.AppendLine(_productService.Delete(toDelete));
            TxtBlk_LogCRUD.Text = log.ToString();
        }
        
        //Crud Stock
        private void Btn_CreateStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedProduct = _productService.FindByGroup(Cmb_StkProduct.SelectedItem.ToString());
                SelectedCompany = _companyService.FindByName(Cmb_StkCompany.SelectedItem.ToString());
                if (SelectedProduct == null)
                {
                    throw new NotFoundException("Produto não encontrado!");
                }
                if (SelectedCompany == null)
                {
                    throw new NotFoundException("Empresa não encontrada!");
                }
                DateTime lstin = (DateTime)Dpk_StkLastInput.SelectedDate;
                DateTime lstout = (DateTime)Dpk_StkLastSale.SelectedDate;
                log.AppendLine(_stockService.Create(SelectedProduct,
                    Convert.ToInt32(Txt_StkQtyPurchased.Text),
                    Convert.ToInt32(Txt_StkQtySold.Text),
                    Convert.ToDouble(Txt_StkAmountPurchased.Text),
                    Convert.ToDouble(Txt_StkAmountSold.Text),
                    lstin.Date, lstout.Date,
                    SelectedCompany));

                TxtBlk_LogCRUD.Text = log.ToString();
            }
            catch (MyApplicationException ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                Log_TextBlock.Text = log.ToString();
            }
        }
        private void Btn_ReadStock_Click(object sender, RoutedEventArgs e)
        {
            if (Cmb_StkCompany != null)
            {
                SelectedCompany = _companyService.FindByName(Cmb_StkCompany.SelectedIndex.ToString());
                ListOfStocks = _stockService.GetStocksByCompany(SelectedCompany);

                Grd_View.AutoGenerateColumns = true;
                TxtB_Company.Text = "Lista de Estoques";
                Grd_View.ItemsSource = ListOfStocks.ToList();
                InitializeComponent();
                Tb_DataView.IsSelected = true;
            }
            else
            {
                throw new RequiredFieldException("Informa a empresa para filtrar os estoques");
            }
        }
        private void Btn_DeleteStock_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedStock != null)
            {
                log.AppendLine(_stockService.Delete(SelectedStock));
                TxtBlk_LogCRUD.Text = log.ToString();
            }
        }

        #endregion


    }
}
