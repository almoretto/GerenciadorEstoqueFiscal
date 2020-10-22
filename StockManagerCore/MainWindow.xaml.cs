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
        XMLReader xmlReader;
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
                dlg.DefaultExt = ".xml";
                dlg.Filter = "XML Document (.XML)|*.xml";

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
        public List<InputProduct> ListInputProduct { get; set; }
        public IQueryable<Product> Products { get; set; }

        private void ProcessInputs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Products = from p in _context.Products select p;
                sales = (bool)InputOutput.IsChecked;
                log.Clear();
                if (!sales)
                {
                    //Instance the service
                    xmlReader = new XMLReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(xmlReader.GetInputItens());
                    Log_TextBlock.Text = log.ToString();
                    foreach (InputXML item in xmlReader.Inputs)
                    {
                        int idProd = (from p in Products
                                      where p.Group == item.Group
                                      select p.Id).SingleOrDefault();
                        ListInputProduct.Add(new InputProduct
                            (item.NItem,
                            item.XProd,
                            item.QCom,
                            item.VUnCom,
                            item.UCom,
                            item.Vtotal,
                            item.VUnTrib,
                            item.VTotTrib,
                            idProd,
                            item.DhEmi));

                    }
                    foreach (InputProduct item in ListInputProduct)
                    {
                        _context.InputProducts.Add(item);
                        _context.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                Log_TextBlock.Text = log.ToString();
            }


        }
        public List<SoldProduct> ListOfSales { get; set; }


        private void ProcessSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Products = from p in _context.Products select p;
                sales = (bool)InputOutput.IsChecked;
                log.Clear();

                if (sales)
                {
                    //Instance the service
                    xmlReader = new XMLReader(filename, sales);
                    //Reading inputs and returning Log
                    log.AppendLine(xmlReader.GetInputItens());
                    Log_TextBlock.Text = log.ToString();
                    foreach (InputXML item in xmlReader.Inputs)
                    {
                        int idProd = (from p in Products
                                      where p.Group == item.Group
                                      select p.Id).SingleOrDefault();
                        ListOfSales.Add(new SoldProduct
                            (item.NItem,
                            item.XProd,
                            item.QCom,
                            item.VUnCom,
                            item.UCom,
                            item.Vtotal,
                            item.VUnTrib,
                            item.VTotTrib,
                            idProd,
                            item.DhEmi));

                    }
                    foreach (SoldProduct item in ListOfSales)
                    {
                        _context.SoldProducts.Add(item);
                        _context.SaveChanges();
                    }

                }
                else
                {
                    throw new Exception(Log_TextBlock.Text = "Não pode processar entrada como venda");
                }

            }
            catch (IOException ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                Log_TextBlock.Text = log.ToString();
            }
        }


    }
}
