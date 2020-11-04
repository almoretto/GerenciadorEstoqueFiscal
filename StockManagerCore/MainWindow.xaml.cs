
#region --== Dependency Declaration ==--
using System;
using System.Windows;
using StockManagerCore.Services;
using StockManagerCore.Data;
using StockManagerCore.Models;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
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
        #endregion

        #region --== Local Variables ==--
        string filename;
        FileReader nfeReader;
        bool sales;
        DateTime dateInitial = new DateTime();
        DateTime DateFinal = new DateTime();
        int qteTot = 0;
        int qty = 0;
        double amount = 0.0;
        double totAmount = 0.0;
        #endregion

        #region --== Models instantitation and support Lists ==--
        private Company SelectedCompany { get; set; } = new Company();
        public IEnumerable<Company> ListCompanies { get; set; }
        private List<InputProduct> ListInputProduct { get; set; } = new List<InputProduct>();
        private List<SoldProduct> ListOfSales { get; set; } = new List<SoldProduct>();
        private IEnumerable<Stock> ListStocks { get; set; } = new List<Stock>();
        private IEnumerable<Product> Products { get; set; }
        private Product Prod { get; set; } = new Product();
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

            Products = _productService.GetProducts();
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

                        ListInputProduct.Add(new InputProduct
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
                            SelectedCompany));
                    }
                    // _context.InputProducts.AddRange(ListInputProduct);
                    //  _context.SaveChanges();

                }
                else
                {
                    Log_TextBlock.Text = "";
                    log.AppendLine("Não pode processar venda como Compra!");
                    Log_TextBlock.Text = log.ToString();
                    throw new Exception("Não pode processar venda como Compra!");
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
                SelectedCompany = (from c in ListCompanies where c.Name == (string)CMB_Company.SelectedItem select c).FirstOrDefault();

                if (filename.EndsWith("TXT") || filename.EndsWith("txt"))
                {
                    throw new Exception("Não pode processar arquivo de entrada como venda!");
                }
                if (SelectedCompany == null)
                {
                    throw new Exception("Selecione uma empresa!");
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
                        p = GetProduct(item.Group, Products);

                        if (p == null)
                        {
                            throw new Exception("Produto Não encontrado!");
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
                    //  _context.SoldProducts.AddRange(ListOfSales);
                    //  _context.SaveChanges();

                }
                else
                {
                    Log_TextBlock.Text = "";
                    log.AppendLine("Não pode processar entrada como venda!");
                    Log_TextBlock.Text = log.ToString();
                    throw new Exception("Não pode processar entrada como venda!");
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
                SelectedCompany = (from c in ListCompanies where c.Name == (string)CMB_Company.SelectedItem select c).FirstOrDefault();

                if (SelectedCompany == null)
                {
                    throw new ApplicationException("Empresa deve ser selecionada!");
                }

                if (Rdn_In.IsChecked == true)
                {
                    dateInitial = DateTime.ParseExact(Txt_DateInitial.Text, "dd/MM/yyyy", provider);

                    GetInputs(dateInitial);

                    var groupProducts = ListInputProduct
                        .Where(co => co.Company.Id == SelectedCompany.Id)
                        .GroupBy(p => p.XProd);

                    GetStocks(SelectedCompany);

                    //moving through grouping
                    foreach (IGrouping<string, InputProduct> group in groupProducts)
                    {
                        Stock stock = new Stock();
                        stock = ListStocks.Where(s => s.Product.Group == group.Key).FirstOrDefault();

                        log.Append("Produto: " + group.Key);
                        txt_Console.Text = log.ToString();

                        foreach (InputProduct item in group)
                        {
                            qty += item.QCom;
                            amount += item.Vtotal;
                        }

                        stock.MovimentInput(qty, amount, dateInitial);

                        try
                        {
                            //  _context.Stocks.Update(stock);
                            //  _context.SaveChanges();
                        }
                        catch (DbUpdateException ex)
                        {
                            Log_TextBlock.Text = "";
                            log.AppendLine(ex.Message);
                            if (ex.InnerException != null)
                            {
                                log.AppendLine(ex.InnerException.Message);
                            }
                            Log_TextBlock.Text = log.ToString();
                        }

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
                    DateFinal = DateTime.ParseExact(Txt_DateFinal.Text, "dd/MM/yyyy", provider);

                    GetSales(dateInitial, DateFinal);
                    var groupOfSales = ListOfSales
                        .Where(co => co.Company.Id == SelectedCompany.Id)
                        .GroupBy(p => p.Product.Group);

                    GetStocks(SelectedCompany);

                    //moving through grouping
                    foreach (IGrouping<string, SoldProduct> group in groupOfSales)
                    {
                        Stock stock = new Stock();
                        stock = ListStocks.Where(s => s.Product.Group == group.Key).FirstOrDefault();

                        log.Append("Produto: " + group.Key);
                        txt_Console.Text = log.ToString();

                        foreach (SoldProduct item in group)
                        {
                            qty += item.QCom;
                            amount += item.Vtotal;
                        }

                        stock.MovimentSale(qty, amount, dateInitial);

                        try
                        {
                            //  _context.Stocks.Update(stock);
                            //  _context.SaveChanges();
                        }
                        catch (DbUpdateException ex)
                        {
                            Log_TextBlock.Text = "";
                            log.AppendLine(ex.Message);
                            if (ex.InnerException != null)
                            {
                                log.AppendLine(ex.InnerException.Message);
                            }
                            Log_TextBlock.Text = log.ToString();
                        }

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
                log.AppendLine("Lista Entradas: " + ListInputProduct.Count);
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
        private void GetStocks(Company c)
        {
            ListStocks.Clear();
            /* ListStocks = _context.Stocks
                    .Where(s => s.Company.Id == c.Id)
                    .Include(p => p.Product)
                    .Include(co => co.Company)
                    .ToList();*/
        }
        private void GetInputs(DateTime d)
        {
            ListInputProduct.Clear();

            /* ListInputProduct = _context.InputProducts
                        .Where(i => i.DhEmi.Year == d.Year
                        && i.DhEmi.Month == d.Month
                        && i.DhEmi.Day == d.Day)
                        .Include(i => i.Product)
                        .Include(i => i.Company)
                        .ToList();*/
        }
        private void GetSales(DateTime di, DateTime df)
        {
            ListOfSales.Clear();
            /* ListOfSales = _context.SoldProducts
                 .Where(s => s.DhEmi.Year >= di.Year
                        && s.DhEmi.Month >= di.Month
                        && s.DhEmi.Day >= di.Day
                        && s.DhEmi.Year <= df.Year
                        && s.DhEmi.Month <= df.Month
                        && s.DhEmi.Day <= df.Day)
                 .Include(s => s.Product)
                 .Include(s => s.Company)
                 .ToList();*/
        }
    }
}
