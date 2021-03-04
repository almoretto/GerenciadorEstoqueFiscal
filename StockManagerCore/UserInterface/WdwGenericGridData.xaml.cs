using StockManagerCore.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace StockManagerCore.UserInterface
{
    /// <summary>
    /// Lógica interna para WdwGenericGridData.xaml
    /// </summary>
    public partial class WdwGenericGridData : Window
    {

        #region --== Instances of Context, Provider and StringBuilder ==--
        /*Declatarions for dependency injection of services classes.
         * Each one of the services are responsible for the model thats concern to
         * Each method in the single service is designed to a single purpose.
         * trying to obey SOLID principles.
         */
        private readonly List<object> _inputObject;

        private readonly Company _company = new Company();
        private List<DispStockCompany> DispStocks { get; set; } = new List<DispStockCompany>();
        private string GridTitle { get; set; }

        //declaration of culture info for less verbose code
        CultureInfo provider = CultureInfo.InvariantCulture;
        //Declaration of the log string builder. This log is merely for information and its not stored in any place for a while.
        StringBuilder log = new StringBuilder();
        #endregion
        public WdwGenericGridData(List<object> inputObject, string title)
        {
            _inputObject = inputObject;

            GridTitle = title;

            InitializeComponent();

            txtBGridLabel.Visibility = Visibility.Hidden;
            lblTotalQty.Visibility = Visibility.Hidden;
            lblTotalAmt.Visibility = Visibility.Hidden;
            lblGridTittle.Content += GridTitle;

            PreparingView();

        }
        public WdwGenericGridData(List<DispStockCompany> stocks, string title, Company comp)
        {
            DispStocks = stocks;
            _company = comp;
            GridTitle = title;

            InitializeComponent();

            txtBGridLabel.Visibility = Visibility.Hidden;
            lblTotalQty.Visibility = Visibility.Hidden;
            lblTotalAmt.Visibility = Visibility.Hidden;
            lblGridTittle.Content += GridTitle;

            PreparingView();

        }
        #region --== Methods ==--
        private void PreparingView()
        {
            switch (GridTitle)
            {
                case "Produtos":
                    txtBGridLabel.Visibility = Visibility.Visible;
                    txtBGridLabel.Text = "Listagem de Produtos";
                    GenerateGrid(_inputObject);

                    break;

                case "Estoques":
                    txtBGridLabel.Visibility = Visibility.Visible;
                    txtBGridLabel.Text = "Estoque Empresa: " + _company.Name;
                    GenerateGridStock(DispStocks);

                    break;

                case "Empresas":
                    txtBGridLabel.Visibility = Visibility.Visible;
                    txtBGridLabel.Text = "Listagem de Empresas";
                    GenerateGrid(_inputObject);

                    break;

                default:
                    txtBGridLabel.Visibility = Visibility.Hidden;
                    txtBGridLabel.Text = GridTitle;
                    GenerateGrid(_inputObject);

                    break;
            }
        }



        private void GenerateGridStock(List<DispStockCompany> gridContent)
        {
            //Preparing data for Grid
            int tq = 0;
            double tv = 0.0;
            foreach (var item in gridContent)
            {
                tq += item.QteSaldo;
                tv += double.Parse(item.ValorSaldo.Replace("R$", "").Trim(), CultureInfo.CurrentCulture);
            }

           
            //Activates the auto generate columns for the grid manages himself
            GrdGenericGridView.AutoGenerateColumns = true;
            GrdGenericGridView.ItemsSource = gridContent.ToList();
            //attach list to grid for result
            lblTotalQty.Content += tq.ToString("N0", CultureInfo.CurrentCulture);
            lblTotalAmt.Content += tv.ToString("C2");
            InitializeComponent();

        }
        private void GenerateGrid(List<object> gridContent)
        {
            //Preparing Grid
            //Activates the auto generate columns for the grid manages himself
            GrdGenericGridView.AutoGenerateColumns = true;
            GrdGenericGridView.ItemsSource = gridContent.ToList();
            //attach list to grid for result
            InitializeComponent();

        }

        #endregion
    }
}