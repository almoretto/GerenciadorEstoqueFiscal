using StockManagerCore.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace StockManagerCore.UserInterface
{
	/// <summary>
	/// Lógica interna para WdnStockGrid.xaml
	/// </summary>
	public partial class WdnStockGrid : Window
	{
		private List<DispStockCompany> _gridStockDisplay { get; set; } = new List<DispStockCompany>();
		private Company _companyToDisplay { get; set; } = new Company();
		public WdnStockGrid( List<DispStockCompany> gridContent, Company company )
		{
			_gridStockDisplay = gridContent;
			_companyToDisplay = company;

			InitializeComponent();
		}

		private void WdnStockGrid_Loaded( object sender, RoutedEventArgs e )
		{
			GenerateGrid( _gridStockDisplay, _companyToDisplay );
		}
		
		private void btnClear_Click( object sender, RoutedEventArgs e )
		{
			GrdStock.Items.Clear();
		}
		private void btnExit_Click( object sender, RoutedEventArgs e )
		{
			btnClear_Click( btnClear, new RoutedEventArgs()); //Calling the Clear button
			this.Close();
		}
		
		//Methods
		private void GenerateGrid( List<DispStockCompany> gridContent, Company c )
		{
			int tq = 0;
			double tv = 0.0;
			foreach ( var item in gridContent )
			{
				tq += item.QteSaldo;
				tv += double.Parse( item.ValorSaldo.Replace( "R$", "" ).Trim(), CultureInfo.CurrentCulture );
			}

			//Activates the auto generate columns for the grid manages himself
			GrdStock.AutoGenerateColumns = true;
			//attributes the data to grid
			TxtBCompany.Text += ": " + c.Name; //Label on top
											   //attach list to grid for result
			lblTotalQtyInStock.Content = tq.ToString( "N0", CultureInfo.CurrentCulture );
			lblTotalAmtInStock.Content = tv.ToString( "C2" );
			GrdStock.ItemsSource = gridContent.ToList();
			//Changes view to the tab of dataview and makes visible

			InitializeComponent();
		}

	
	}
}
