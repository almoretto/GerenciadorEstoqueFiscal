#region --== Dependency declaration ==--
using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Services.Exceptions;
using System.Windows;
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
            Company company = new Company();
            if (c == null)
            {
                throw new RequiredFieldException("Informe uma empresa para localizar");
            }
            company = _context.Companies
                 .Where(co => co.Id == c.Id)
                 .SingleOrDefault();
            if (company == null)
            {
                throw new NotFoundException("Entidade não encontrada");
            }
            return company;

        }
        public Company FindByName(string name)
        {
            try
            {
                Company company = new Company();
                if (name == "" || name == null)
                {
                    throw new RequiredFieldException("Campo necessario para busca");
                }
                company = _context.Companies.Where(c => c.Name == name).SingleOrDefault();
                return company;
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Empresa Não encontrada" + ex.Message);
            }

        }

        #region --== CRUD ==--
        public string Create(string name)
        {
            try
            {
                Company c = new Company(name);
                _context.Companies.Add(c);
                _context.SaveChanges();
                var test = _context.Companies.Where(t => t.Name == c.Name).FirstOrDefault();
                string response = "Id: " + test.Id.ToString() + " Nome: " + test.Name.ToUpper();

                return response;
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

        }
        public Company FindToUdate(int id)
        {
            Company co = new Company();

            if (id != 0)
            {
                co = _context.Companies.Find(id);
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
                if (co == null)
                {
                    throw new DbComcurrancyException("Entity could not be null or empty!");
                }
                _context.Companies.Update(co);
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
