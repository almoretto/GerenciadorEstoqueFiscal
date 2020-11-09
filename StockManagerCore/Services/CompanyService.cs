#region --== Dependency declaration ==--
using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Services.Exceptions;
#endregion


namespace StockManagerCore.Services
{
    public class CompanyService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public CompanyService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companies.OrderBy(c => c.Name);
        }
        public Company Find(Company c)
        {
            return _context.Companies
                .Where(co => co.Id == c.Id)
                .SingleOrDefault();
        }
        public Company FindByName(string name)
        {
            return _context.Companies.Where(c => c.Name == name).SingleOrDefault();
        }

        #region --== CRUD ==--
        public string Create(string name)
        {
            Company c = new Company(name);
            _context.Companies.Add(c);
            _context.SaveChanges();
            var test = _context.Companies.Where(t => t.Name == c.Name).FirstOrDefault();
            string response = "Id: " + test.Id.ToString() + " Nome: " + test.Name.ToUpper();

            return response;
        }
        public Company FindToUdate(string name, int? id)
        {
            Company co = new Company();

            if (id.HasValue)
            {
                co = _context.Companies.Find(id);
                if (co == null)
                {
                    throw new NotFoundException("Id :" + id.ToString() + "Não encontrado");
                }
                return co;
            }
            else if (name != "")
            {
                co = _context.Companies.Where(c => c.Name == name).FirstOrDefault();
                if (co == null)
                {
                    throw new NotFoundException("Id :" + id.ToString() + "Não encontrado");
                }
                return co;
            }
            throw new NotFoundException("Insuficient Data to find entity!");
        }
        public string Update(Company co)
        {
            try
            {
                _context.Update(co);
                _context.SaveChanges();
            }
            catch (DbComcurrancyException ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += "\n" + ex.InnerException;
                }
                throw new DbComcurrancyException("Não foi possivel atualizar veja mensagem \n" + msg);
            }
            return "Update realizado com sucesso!";
        }
        #endregion
        #endregion
    }
}
