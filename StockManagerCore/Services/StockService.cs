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
        private readonly StockDBContext _context;
        public StockService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        public IEnumerable<Stock> GetStocksByCompany(Company company)
        {
            if (company==null)
            {
                throw new RequiredFieldException("Empresa é obrigatoria para retornar lista de estoques");
            }
            try
            {
                return _context.Stocks
                    .Include(s => s.Product)
                    .Include(s => s.Company)
                    .Where(s => s.Company.Id == company.Id);
            }
            catch(NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }
        public Stock GetStockByCompanyAndGroup(Company co, string grp)
        {
            Stock stock = new Stock();
            if (co == null || grp == null || grp == "")
            {
                throw new RequiredFieldException("Empresa e Produto são obrigatórios para localizar um estoque");
            }
            stock = (_context.Stocks
                .Where(s => s.Company.Id == co.Id))
                .Where(s => s.Product.GroupP == grp)
                .Include(s => s.Product)
                .Include(s => s.Company)
                .FirstOrDefault();
            if (stock==null)
            {
                throw new NotFoundException("Estoque não encontrado com os parâmetros");
            }
            return stock;
        }
        public IEnumerable<Stock> GetStocks()
        {
            return _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Company)
                .OrderBy(s => s.Product.GroupP);
        }
        public IEnumerable<object> GetStocksFormated(Company company)
        {
            var query = from s in _context.Stocks
                        join c in _context.Companies on s.Company.Id equals c.Id
                        join p in _context.Products on s.Product.Id equals p.Id
                        where s.Company.Id == company.Id
                        select new
                        {
                            Produto = s.Product.GroupP,
                            QteComprada = s.QtyPurchased,
                            QteVendida = s.QtySold,
                            QteSaldo = (s.QtyPurchased - s.QtySold),
                            ValorCompra = s.AmountPurchased.ToString("C2"),
                            ValorVenda = s.AmountSold.ToString("C2"),
                            ValorSaldo = (s.AmountPurchased - s.AmountSold).ToString("C2"),
                            UltimaSaída = s.LastSales.ToString("dd/MM/yyyy"),
                            UltimaEntrada = s.LastInput.ToString("dd/MM/yyyy")
                        };
            return query.ToList();
        }
        public void Update(Stock stk)
        {
            try
            {
                _context.Stocks.Update(stk);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }
        #region --== Crud ==--
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

                throw new DbUpdateException(ex.Message);
            }
            return "Criado com sucesso estoque id: " + result;
        }
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
        
        #endregion
        #endregion
    }
}
