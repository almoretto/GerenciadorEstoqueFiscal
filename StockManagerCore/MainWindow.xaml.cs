using System;
using System.IO;
using System.Windows;
using StockManagerCore.Services;
using System.Text;

namespace StockManagerCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filename; 
        XMLReader xmlReader;
        bool sales, duzens;
        StringBuilder log = new StringBuilder();
        public MainWindow()
        {
            InitializeComponent();
           // CreateDbIfNotExists();

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
        private void ProcessInputs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sales = (bool)InputOutput.IsChecked;
                duzens = (bool)NFUn.IsChecked;
                log.Clear();
                //Instance the service
                xmlReader = new XMLReader(filename, duzens, sales);
                //Reading inputs and returning Log
                log.AppendLine(xmlReader.GetInputItens());
                Log_TextBlock.Text = log.ToString();
            }
            catch (IOException ex)
            {
                Log_TextBlock.Text = "";
                log.AppendLine(ex.Message);
                Log_TextBlock.Text = log.ToString();
            }
           

        }

        private void ProcessSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                log.Clear();
                //Instance the service
                xmlReader = new XMLReader(filename, );
                //Reading inputs and returning Log
                log.AppendLine(xmlReader.GetInputItens());
                Log_TextBlock.Text = log.ToString();
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
