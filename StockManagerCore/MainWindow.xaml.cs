
#region --== Dependency Declaration ==--
using System;
using System.Windows;
using StockManagerCore.Services;
using StockManagerCore.Models;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
using System.Data;
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
        private InputProduct InputProduct { get; set; } = new InputProduct();
        private List<SoldProduct> ListOfSales { get; set; } = new List<SoldProduct>();
        public IEnumerable<Company> ListCompanies { get; set; }
        public IQueryable<Stock> ListOfStocks { get; set; }
        private DataTable DataGridTable { get; set; } = new DataTable();
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
            foreach (Company c in ListCompanies)
            {
                CMB_Company.Items.Add(c.Name);
            }
        }
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
                        _stockService.UpdateStock(stock);

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
                        _stockService.UpdateStock(stock);

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
                TxtB_Company.Text = CMB_Company.SelectedItem.ToString();
                ListOfStocks = _stockService.GetStocksByCompany(SelectedCompany);
                 
                DataGridTable.Columns.Add("Produto");
                DataGridTable.Columns.Add("Qte Comprada");
                DataGridTable.Columns.Add("Qte Vendida");
                DataGridTable.Columns.Add("Total Comprado");
                DataGridTable.Columns.Add("Total Vendido");
                DataGridTable.Columns.Add("Qte Saldo");
                DataGridTable.Columns.Add("Valor Saldo");

                foreach (Stock item in ListOfStocks)
                {
                    DataGridTable.Rows.Add(item);
                }

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
    }
}
