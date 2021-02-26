#region --== Dependency declaration ==--
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Services.Exceptions;
#endregion

namespace StockManagerCore.Services
{
    public class StockService
    {
        #region --== Constructor for dependency injection ==--
        //Constructor of the service class and dbcontext dependency injection
        private readonly StockDBContext _context;
        public StockService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        //Querry all stocks selecting by an specific company
        public IEnumerable<Stock> GetStocksByCompany(Company company)
        {
            if (company == null)
            {
                throw new RequiredFieldException("Empresa é obrigatoria para retornar lista de estoques");
            }
            try
            {
                //Entity Framework Querry including related product and company entities.
                return _context.Stocks
                    .Include(s => s.Product)
                    .Include(s => s.Company)
                    .Where(s => s.Company.Id == company.Id);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        //Querry Stock filtering by company and product
        public Stock GetStockByCompanyAndGroup(Company co, string grp)
        {
            Stock stock = new Stock();
            if (co == null || grp == null || grp == "")
            {
                throw new RequiredFieldException("Empresa e Produto são obrigatórios para localizar um estoque");
            }
            //EF query filtering company and product and including related company and product entities.
            stock = (_context.Stocks
                .Where(s => s.Company.Id == co.Id))
                .Where(s => s.Product.GroupP == grp)
                .Include(s => s.Product)
                .Include(s => s.Company)
                .FirstOrDefault();
            if (stock == null)
            {
                throw new NotFoundException("Estoque não encontrado com os parâmetros");
            }
            return stock;
        }

        //Querry all Stocks from database
        public IEnumerable<Stock> GetStocks()
        {
            IEnumerable<Stock> stocks = _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Company)
                .OrderBy(s => s.Product.GroupP);

            _context.Database.CloseConnection();

            return stocks;

        }

        //Querry all stocks and returns a formated list. Includin product name and company name.
        public IEnumerable<object> GetStocksFormated(Company company)
        {
            var query = from s in _context.Stocks
                        join c in _context.Companies on s.Company.Id equals c.Id
                        join p in _context.Products on s.Product.Id equals p.Id
                        where s.Company.Id == company.Id
                        select new
                        {
                            Produto = s.Product.GroupP,
                            ValorMed = s.AmountPurchased / s.QtyPurchased,
                            QteComprada = s.QtyPurchased,
                            QteVendida = s.QtySold,
                            QteSaldo = s.ProdQtyBalance,
                            ValorSaldo = s.ProdQtyBalance * (s.AmountPurchased / s.QtyPurchased),
                            DataSaldo = s.BalanceDate.ToString("dd/MM/yyyy"),
                            ValorCompra = s.AmountPurchased.ToString("C2"),
                            ValorVenda = s.AmountSold.ToString("C2"),
                            UltSaídaSys = s.LastSales.ToString("dd/MM/yyyy"),
                            UltEntradaSys = s.LastInput.ToString("dd/MM/yyyy")
                        };
            return query.ToList();
        }
        public IEnumerable<object> CalculateBalance(Company company)
        {
            try
            {
                IEnumerable<Stock> stocks;
                stocks = GetStocksByCompany(company);
                foreach (Stock item in stocks)
                {
                    item.SetBalance();
                }
                _context.UpdateRange(stocks);
                _context.SaveChanges();
            }
            catch (DbComcurrancyException ex)
            {
                MessageBox.Show("Erro ao tentar fazer update!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new DbUpdateConcurrencyException(ex.Message);
            }
            return GetStocksFormated(company).ToList();
        }


        #region --== Crud ==--

        //Method to Create new Stock
        public string Create(Product product, int? qtyPurchased, int? qtySold, double? amountPurchased,
            double? amountSold, DateTime lstImput, DateTime? lstSale, Company company)
        {
            Stock stk = new Stock();
            string result;
            try
            {
                if (product != null
                    && qtyPurchased.HasValue
                    && qtySold.HasValue
                    && amountPurchased.HasValue
                    && amountSold.HasValue
                    && lstImput != null
                    && lstSale != null
                    && company != null)
                {
                    stk = new Stock(product, qtyPurchased.Value, qtySold.Value, amountPurchased.Value, amountSold.Value,
                   lstImput, lstSale.Value, company);
                    _context.Stocks.Add(stk);
                    _context.SaveChanges();
                }
                else
                {
                    throw new RequiredFieldException("Todos os campos deste registro são obrigatórios");
                }
                var test = _context.Stocks.Where(s => s.Company == stk.Company && s.Product == stk.Product).FirstOrDefault();
                if (test == null)
                {
                    throw new NotFoundException("Estoque adicionado não foi localizado tente outra vez! \n Clique em visualizar para mostrar todos");
                }
                result = test.Id.ToString() + " Empresa: " + test.Company.Name + " Produto: " + test.Product.GroupP;
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Erro ao tetar criar novo!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new DbUpdateException(ex.Message);
            }
            return "Criado com sucesso estoque id: " + result;
        }

        //Method to Find a Strock of a specific product to edit.
        public Stock FindToUpdate(Company co, Product prod)
        {
            Stock stk = new Stock();
            if (co != null && prod != null)
            {
                stk = _context.Stocks
                    .Where(s => s.Company.Id == co.Id && s.Product.Id == prod.Id)
                    .FirstOrDefault();
                if (stk == null)
                {
                    throw new NotFoundException("Estoque com empresa :"
                        + co.Name
                        + " e Produto: "
                        + prod.GroupP
                        + "Não encontrado");
                }
                return stk;
            }
            throw new NotFoundException("Insuficient Data to find entity!");
        }

        //Method to Update/edit a stock on database
        public void Update(Stock stk)
        {
            try
            {
                _context.Stocks.Update(stk);
                _context.SaveChanges();
                _context.Database.CloseConnection();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                MessageBox.Show("Erro ao tentar fazer update!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }
        public void UpdateRange(IEnumerable<Stock> stks)
        {
            try
            {
                _context.Stocks.UpdateRange(stks);
                _context.SaveChanges();
                _context.Database.CloseConnection();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                MessageBox.Show("Erro ao tentar fazer update!\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new DbUpdateConcurrencyException(ex.Message);
            }

        }
        #endregion
        #endregion
    }
}
