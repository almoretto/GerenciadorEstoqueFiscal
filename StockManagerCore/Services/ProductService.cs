#region --== Dependency Declaration ==--
using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Services.Exceptions;
using System.Windows;
#endregion

namespace StockManagerCore.Services
{
    public class ProductService
    {
        #region --== Constructor for dependency injection ==--
        private readonly StockDBContext _context;
        public ProductService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }
        public Product Find(Product p)
        {
            Product product = new Product();
            if (p==null)
            {
                throw new RequiredFieldException("Campo requerido para pesquisa");
            }
            product= _context.Products
                .Where(pr => pr.Id == p.Id)
                .SingleOrDefault();
            if (product == null)
            {
                throw new NotFoundException("Entidade não encontrada");
            }
            return product;
        }
        public Product FindByGroup(string gr)
        {
            Product product = new Product();
            if (gr==null || gr=="")
            {
                throw new RequiredFieldException("Campo requerido para busca");
            }
            product = _context.Products.Where(p => p.GroupP == gr).FirstOrDefault();
            if (product==null)
            {
                throw new NotFoundException("Entidade não encontrada");
            }
            return product;
        }
       
        #region --== CRUD ==--
        public string Create(string name)
        {
            Product p = new Product(name);
            _context.Products.Add(p);
            _context.SaveChanges();
            var test = _context.Products.Where(t => t.GroupP == p.GroupP).FirstOrDefault();
            string response = "Id: " + test.Id.ToString() + " Produto: " + test.GroupP.ToUpper();

            return response;
        }
        public Product FindToUdate(string name, int? id)
        {
            Product prd = new Product();

            if (id.HasValue)
            {
                prd = _context.Products.Find(id);
                if (prd == null)
                {
                    throw new NotFoundException("Id :" + id.ToString() + "Não encontrado");
                }
                return prd;
            }
            else if (name != "")
            {
                prd = _context.Products.Where(c => c.GroupP == name).FirstOrDefault();
                if (prd == null)
                {
                    throw new NotFoundException("Id :" + id.ToString() + "Não encontrado");
                }
                return prd;
            }
            throw new NotFoundException("Insuficient Data to find entity!");
        }
        public string Update(Product p)
        {
            try
            {
                if (p == null)
                {
                    throw new DbComcurrancyException("Entity could not be null or empty!");
                }
                _context.Products.Update(p);
                _context.SaveChanges();
            }
            catch (DbComcurrancyException ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += "\n" + ex.InnerException;
                }
                throw new DbComcurrancyException("Não foi possivel atualizar veja mensagem: \n" + msg);
            }
            return "Update realizado com sucesso!";
        }
        public string Delete(Product p)
        {
            MessageBoxResult result = MessageBox.Show("O registro de Produto: "
                + p.GroupP
                + ".\n Será excluído continuar?",
                "Confirmation",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Products.Remove(p);
                    _context.SaveChanges();
                }
                catch (DbComcurrancyException ex)
                {
                    string msg = ex.Message;
                    if (ex.InnerException != null)
                    {
                        msg += "\n" + ex.InnerException;
                    }
                    throw new DbRelationalException("Não foi possivel excluir por violação de relacionamento veja mensagem: \n" + msg);
                }
                return "Excluido com sucesso!";
            }
            else
            {
                return "Operação Cancelada!";
            }
        }
        #endregion

        #endregion
    }
}
