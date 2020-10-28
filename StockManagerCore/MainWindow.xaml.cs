using System;
using System.Windows;
using StockManagerCore.Services;
using StockManagerCore.Data;
using StockManagerCore.Models;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace StockManagerCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StockDBContext _context;
        string filename;
        FileReader nfeReader;
        bool sales;
        DateTime dateInitial = new DateTime();
        DateTime DateFinal = new DateTime();
        CultureInfo provider = CultureInfo.InvariantCulture;
        private Company selectedCompany { get; set; } = new Company();
        public IQueryable<Company> listCompanies { get; set; }
        private List<InputProduct> ListInputProduct { get; set; } = new List<InputProduct>();
        private List<SoldProduct> ListOfSales { get; set; } = new List<SoldProduct>();
        private List<Stock> ListStocks { get; set; } = new List<Stock>();
        private IQueryable<Product> Products { get; set; }
        private Product Prod { get; set; } = new Product();

        StringBuilder log = new StringBuilder();
        public MainWindow(StockDBContext context)
        {
            _context = context;
            InitializeComponent();
        }
        private void BtnFileOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listCompanies = from c in _context.Companies select c;
                CMB_Company.IsEnabled = true;
                foreach (Company c in listCompanies)
                {
                    CMB_Company.Items.Add(c.Name);
                }

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

                selectedCompany = (from c in listCompanies where c.Name == (string)CMB_Company.SelectedItem select c).FirstOrDefault();

                if (filename.EndsWith("csv") || filename.EndsWith("CSV"))
                {
                    throw new Exception("Não pode processar arquivo de venda como entrada!");
                }
                if (selectedCompany == null)
                {
                    throw new Exception("Selecione uma empresa!");
                }
                Products = from p in _context.Products select p;
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
                        Prod = (from p in Products
                                where p.Group == item.Group
                                select p).SingleOrDefault();
                        ListInputProduct.Add(new InputProduct
                            (item.NItem,
                            item.XProd,
                            item.QCom,
                            item.VUnCom,
                            item.UCom,
                            item.Vtotal,
                            item.VUnTrib,
                            item.VTotTrib,
                            Prod,
                            item.DhEmi,
                            selectedCompany));

                    }
                    _context.InputProducts.AddRange(ListInputProduct);
                    _context.SaveChanges();

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
                selectedCompany = (from c in listCompanies where c.Name == (string)CMB_Company.SelectedItem select c).FirstOrDefault();

                if (filename.EndsWith("TXT") || filename.EndsWith("txt"))
                {
                    throw new Exception("Não pode processar arquivo de entrada como venda!");
                }
                if (selectedCompany == null)
                {
                    throw new Exception("Selecione uma empresa!");
                }

                Products = from p in _context.Products select p;
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
                        Prod = (from p in Products
                                where p.Group == item.Group
                                select p).SingleOrDefault();
                        if (Prod == null)
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
                            Prod,
                            selectedCompany));

                    }
                    _context.SoldProducts.AddRange(ListOfSales);
                    _context.SaveChanges();

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

                ListInputProduct.Clear();
                ListOfSales.Clear();
                log.AppendLine("Lista Entradas: " + ListInputProduct.Count);
                log.AppendLine("Lista Saídas: " + ListOfSales.Count);
                Log_TextBlock.Text = log.ToString();
                if (Rdn_In.IsChecked == true)
                {
                    dateInitial = DateTime.ParseExact(Txt_DateInitial.Text, "dd/MM/yyyy", provider);
                    ListInputProduct = (from inp in _context.InputProducts
                                        where inp.DhEmi.Date == dateInitial.d
                                        select inp).ToList();

                    var grupos = from g in ListInputProduct
                                 group g by g.Product;
                    var qty = 0;
                    var amount = 0.0;
                    foreach (IGrouping<Product, InputProduct> group in grupos)
                    {
                        log.Append("Produto: " + group.Key.Group);
                        Log_TextBlock.Text = log.ToString();
                        foreach (InputProduct item in group)
                        {
                            qty += item.QCom;
                            amount += item.Vtotal;
                        }
                        log.Append("Qte: " + qty.ToString());
                        log.Append(" | Valor: " + amount.ToString("F2", CultureInfo.CurrentCulture));
                        Log_TextBlock.Text = log.ToString();
                    }
                }
                if (Rdn_Out.IsChecked == true)
                {
                    dateInitial = DateTime.ParseExact(Txt_DateInitial.Text, "dd/MM/yyyy", provider);
                    DateFinal = DateTime.ParseExact(Txt_DateFinal.Text, "dd/MM/yyyy", provider);
                    ListOfSales = (from sal in _context.SoldProducts
                                   where sal.DhEmi >= dateInitial && sal.DhEmi <= DateFinal
                                   select sal).ToList();
                }
                log.AppendLine("Lista Entradas: " + ListInputProduct.Count);
                log.AppendLine("Lista Saídas: " + ListOfSales.Count);


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
    }
}
