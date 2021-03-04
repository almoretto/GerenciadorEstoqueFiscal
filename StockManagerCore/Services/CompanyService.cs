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

        //Querry to fetch all Companies Records
        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companies.OrderBy(c => c.Name);
        }

        //Querry to find a specific company
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

        //Querry to find a company by name
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

        //Querry to Find a specific company by Id to update.
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

        public IEnumerable<object> GetObjCompanies()
        {
            var query = from c in _context.Companies
                        join s in _context.Stocks on c.Id equals s.Company.Id
                        select new
                        {
                            Nome = c.Name,
                            Codigo = c.Id,
                            MaxFaturamento = c.MaxRevenues.ToString("C2"),
                            SaldoFaturavel = c.Balance.ToString("C2")
                        };
            return query.ToList();
        }

        public double CalculateCompanyBalance(IEnumerable<Stock> list, Company c)
        {
            double sum = 0.0d;
            try
            {
               
                foreach (Stock item in list)
                {
                    sum += item.AmountSold;
                }
                _context.Companies.Update(c);
                _context.SaveChanges();
                MessageBox.Show("Saldo Atualizado! "+ sum.ToString("C2"),
                    "Resultado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (DbComcurrancyException ex)
            {
                MessageBox.Show("Saldo não atualizado!\n " + ex.Message,
                   "Erro",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                throw new DbComcurrancyException(ex.Message);
            }
          
            return sum;
        }

        #region --== CRUD ==--

        //Methods to Create New and Update records of companies.
        public string Create(string name, double maxR)
        {
            try
            {
                Company c = new Company(name, maxR);
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
        public string Create(Company c)
        {
            try
            {
                _context.Companies.Add(c);
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
            return "Adicionado com sucesso";
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
