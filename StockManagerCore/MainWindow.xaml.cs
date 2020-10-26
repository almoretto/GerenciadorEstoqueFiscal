using System;
using System.IO;
using System.Windows;
using StockManagerCore.Services;
using StockManagerCore.Data;
using StockManagerCore.Models;
using System.Text;
using System.Collections.Generic;
using System.Linq;

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
            catch (IOException ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                Log_TextBlock.Text = log.ToString();
            }
        }
        public List<InputProduct> ListInputProduct { get; set; } = new List<InputProduct>();
        public List<SoldProduct> ListOfSales { get; set; } = new List<SoldProduct>();
        public IQueryable<Product> Products { get; set; }
        public Product Prod { get; set; } = new Product();
        private void ProcessInputs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (filename.EndsWith("csv")||filename.EndsWith("CSV"))
                {
                    throw new Exception("Não pode processar arquivo de venda como entrada!");
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
                            item.DhEmi));

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
                if (ex.InnerException!=null)
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
                        ListOfSales.Add(new SoldProduct
                            (item.NItem,
                            item.XProd,
                            item.QCom,
                            item.VUnCom,
                            item.Vtotal,
                            item.DhEmi,
                            Prod));

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
            catch (IOException ex)
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
