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
        
        //Constructor and dependency Injection to DbContext
        private readonly StockDBContext _context;
        public ProductService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        
        //Querry to fetch all products from database 
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }
        
        //Querry to Find a producta By Entity
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
        
        //Querry to find product by Name
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
        
        //Method to create a new product on database
        public string Create(string name)
        {
            Product p = new Product(name);
            _context.Products.Add(p);
            _context.SaveChanges();
            var test = _context.Products.Where(t => t.GroupP == p.GroupP).FirstOrDefault();
            string response = "Id: " + test.Id.ToString() + " Produto: " + test.GroupP.ToUpper();

            return response;
        }
        
        //Querry to find a specific product to edit
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
        
        //Method to update an edited product
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
       
        #endregion

        #endregion
    }
}
