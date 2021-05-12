using StockManagerCore.Models;
using StockManagerCore.Services;
using StockManagerCore.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using StockManagerCore.UserInterface;

namespace StockManagerCore.UserInterface
{
    /// <summary>
    /// Lógica interna para WdnStockCrud.xaml
    /// </summary>
    public partial class WdnStockCrud : Window
    {
        #region --== Instances of Context, Provider and StringBuilder ==--
        /*Declatarions for dependency injection of services classes.
         * Each one of the services are responsible for the model thats concern to
         * Each method in the single service is designed to a single purpose.
         * trying to obey SOLID principles.
         */
        private readonly ProductService _productService;
        private readonly CompanyService _companyService;
        private readonly StockService _stockService;

        //declaration of culture info for less verbose code
        //CultureInfo provider = CultureInfo.CurrentCulture;

        //Declaration of the log string builder. This log is merely for information and its not stored in any place for a while.
        StringBuilder log = new StringBuilder();
        #endregion

       
        #region --== Models instantitation and support Lists ==--
        /*Private declaration of the models extructure for local use only and in order to facilitate
         the management of data*/
        private Company SelectedCompany { get; set; } = new Company();
        private Product SelectedProduct { get; set; } = new Product();
        private Stock SelectedStock { get; set; } = new Stock();
        private IEnumerable<Company> ListCompanies { get; set; }
        private List<DispStockCompany> ListOfStocksStruct { get; set; }
        private IEnumerable<Product> ListOfProducts { get; set; }
        private IEnumerable<object> GridList { get; set; }
        public WdwGenericGridData companyGrid { get; set; }
        #endregion

        public WdnStockCrud(ProductService productService, CompanyService companyService,
            StockService stockService)
        {
            _productService = productService;
            _companyService = companyService;
            _stockService = stockService;

            InitializeComponent();

            //these is for populate de comboboxes.
            ListCompanies = _companyService.GetCompanies();
            ListOfProducts = _productService.GetProducts();

            foreach (Company c in ListCompanies)
            {
                CmbStkCompany.Items.Add(c.Name);
            }
            foreach (Product product in ListOfProducts)
            {
                CmbStkProduct.Items.Add(product.GroupP);
            }

            InitializeComponent();

        }

        #region --== CRUD Companies, Products and Stock==--
        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            switch (CmbSwitch.SelectedItem.ToString())
            {
                //Selects the type of Searching service to use according to the user selection
                //And populates the controls with the returned data.
                case "Empresa":
                    SelectedCompany = _companyService.FindByName(TxtSelection.Text.ToUpper());
                    TxtCoId.Text = SelectedCompany.Id.ToString();
                    TxtCoName.Text = SelectedCompany.Name;
                    txtCompanyMaxRevenues.Text = SelectedCompany.MaxRevenues.ToString("C2");
                    txtCompanyBalance.Text = SelectedCompany.Balance.ToString("C2");
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
            //Method for Create a New Company Record
            try
            {
                if (TxtCoName == null)
                {
                    //Validation required field
                    throw new RequiredFieldException("Favor preencher o nome da empresa para cadastrar");
                }
                //Calling Method Create from service layer and Returning to the log 
                log.AppendLine(_companyService.Create(TxtCoName.Text, Convert.ToDouble(txtCompanyMaxRevenues.Text)));

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
                TxtBlkLogCRUD.Text = log.ToString();
            }
        }
        private void Btn_ReadComp_Click(object sender, RoutedEventArgs e)
        {
            //Geting the formated List of Companies
            GridList = _companyService.GetObjCompanies();

            // Creating Window
            companyGrid = new WdwGenericGridData(GridList.ToList(), "Empresas");
            // Open Window
            companyGrid.ShowDialog();

        }
        private void Btn_UpdateComp_Click(object sender, RoutedEventArgs e)
        {
            //Method to update a company 
            Company toUpdate = new Company();
            if (TxtCoId.Text == null && TxtCoName == null)
            {
                //validation for update
                throw new RequiredFieldException("Favor preencher o nome ou ID da empresa para Editar");
            }
            else
            {
                //calling method to find company
                toUpdate = _companyService.FindToUdate(Convert.ToInt32(TxtCoId.Text));
            }
            if (toUpdate == null || toUpdate.Id.ToString() != TxtCoId.Text)
            {
                //validation of the return 
                throw new NotFoundException("Nenhuma empresa localizada");
            }
            //Updating temporary model for further update in db context
            toUpdate.Name = TxtCoName.Text;
            toUpdate.Id = Convert.ToInt32(TxtCoId.Text);
            toUpdate.MaxRevenues = Convert.ToDouble(txtCompanyMaxRevenues.Text);
            toUpdate.SetBalance(_companyService.CalculateCompanyBalance(_stockService.GetStocksByCompany(toUpdate), toUpdate));
            //Calling update method in service layer and returning result to log
            log.AppendLine(_companyService.Update(toUpdate));
            TxtBlkLogCRUD.Text = log.ToString();
        }

        //Products CRUD
        private void Btn_CreateProd_Click(object sender, RoutedEventArgs e)
        {
            //Method for create a new Product follows the same logic of the create company
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
                TxtBlkLogCRUD.Text = log.ToString();
            }
        }
        private void Btn_ReadProd_Click(object sender, RoutedEventArgs e)
        {

            //Geting the formated List of Companies
            GridList = _productService.GetObjProducts();

            // Creating Window
            companyGrid = new WdwGenericGridData(GridList.ToList(), "Produtos");
            // Open Window
            companyGrid.ShowDialog();

        }
        private void Btn_UpdateProd_Click(object sender, RoutedEventArgs e)
        {
            //Method to update product follows the same logic of the update company
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
            //Method to create new Stock control, follows same logic as before.
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
                TxtBlkLogCRUD.Text = "";
                log.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    log.AppendLine(ex.InnerException.Message);
                }
                TxtBlkLogCRUD.Text = log.ToString();
            }
        }
        private void Btn_ReadStock_Click(object sender, RoutedEventArgs e)
        {
            //Method to List all Stocks By Company
            if (CmbStkCompany != null)
            {
                SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
                if (SelectedCompany == null)
                {
                    throw new NotFoundException("Empresa não localizada");
                }

                //Geting the formated List of Companies
                if (chkFormated.IsChecked == true)
                {
                    ListOfStocksStruct = _stockService.GetStocksStructured(SelectedCompany);
                    companyGrid = new WdwGenericGridData(ListOfStocksStruct, "Estoques", SelectedCompany);
                }
                else
                {
                    GridList = _stockService.GetStocksByCompany(SelectedCompany);
                    companyGrid = new WdwGenericGridData(GridList.ToList(), "Estoque "+ SelectedCompany.Name);
                }

                // Creating Window

                // Open Window
                companyGrid.ShowDialog();


            }
            else
            {
                throw new RequiredFieldException("Informa a empresa para filtrar os estoques");
            }
        }
        private void BtnBalanceCalc_Click(object sender, RoutedEventArgs e)
        {
            SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
            if (SelectedCompany == null)
            {
                //Company must not be null
                throw new ApplicationException("Selecione uma empresa!");
            }
            ListOfStocksStruct = _stockService.CalculateBalance(SelectedCompany);
            GridList = (IEnumerable<object>)ListOfStocksStruct;
            // Creating Window
            WdwGenericGridData companyGrid = new WdwGenericGridData(GridList.ToList(), "Estoques Consolidado");
            // Open Window
            companyGrid.ShowDialog();
        }
        //Method to create an manual stock entry
        private void BtnEntryStock_Click(object sender, RoutedEventArgs e)
        {
            SelectedProduct = _productService.FindByGroup(CmbStkProduct.SelectedItem.ToString());
            SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
            SelectedStock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, SelectedProduct.GroupP);
            SelectedStock.MovimentInput(
                Convert.ToInt32(TxtStkQtyPurchased.Text),
                Convert.ToDouble(TxtStkAmountPurchased.Text),
                DateTime.Now.Date);
            try
            {
                _stockService.Update(SelectedStock);
                TxtBlkLogCRUD.Text = "Atualizado com sucesso";
                CmbStkCompany.SelectedIndex = -1;
                CmbStkProduct.SelectedIndex = -1;
                TxtStkAmountPurchased.Text = string.Empty;
                TxtStkAmountSold.Text = string.Empty;
                TxtStkQtyPurchased.Text = string.Empty;
                TxtStkQtySold.Text = string.Empty;
            }
            catch (ApplicationException ex)
            {
                TxtBlkLogCRUD.Text = "Erro ao Atualizar" + ex.Message;
                throw new ApplicationException("Erro na Atualização" + ex.Message);
            }
        }
        //Method to create an manual stock sale
        private void BtnSaleStock_Click(object sender, RoutedEventArgs e)
        {
            SelectedProduct = _productService.FindByGroup(CmbStkProduct.SelectedItem.ToString());
            SelectedCompany = _companyService.FindByName(CmbStkCompany.SelectedItem.ToString());
            SelectedStock = _stockService.GetStockByCompanyAndGroup(SelectedCompany, SelectedProduct.GroupP);
            SelectedStock.MovimentSale(
                Convert.ToInt32(TxtStkQtySold.Text),
                Convert.ToDouble(TxtStkAmountSold.Text),
                DateTime.Now.Date);
            try
            {
                _stockService.Update(SelectedStock);
                TxtBlkLogCRUD.Text = "Atualizado com sucesso";
                CmbStkCompany.SelectedIndex = -1;
                CmbStkProduct.SelectedIndex = -1;
                TxtStkAmountPurchased.Text = string.Empty;
                TxtStkAmountSold.Text = string.Empty;
                TxtStkQtyPurchased.Text = string.Empty;
                TxtStkQtySold.Text = string.Empty;
            }
            catch (ApplicationException ex)
            {
                TxtBlkLogCRUD.Text = "Erro ao Atualizar" + ex.Message;
                throw new ApplicationException("Erro na Atualização" + ex.Message);
            }


        }
        #endregion
    }
}
