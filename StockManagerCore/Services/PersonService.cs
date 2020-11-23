using System.Collections.Generic;
using System.Linq;
using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Services.Exceptions;


namespace StockManagerCore.Services
{
    public class PersonService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public PersonService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods==--
        public IEnumerable<Person> GetPeople()
        {
            return _context.People.OrderBy(c => c.Name);
        }
        public Person FindByName(string name)
        {
            Person p = new Person();
            p = _context.People.Where(p => p.Name == name).FirstOrDefault();
            return p;
        }
        #region --== Crud ==--
        public string Create(Person p)
        {
            try
            {
                _context.People.Add(p);
                _context.SaveChanges();
                return "Person added: "
                    + _context.People
                    .Where(ps => ps.Name == p.Name)
                    .FirstOrDefault()
                    + "Sussessfull!";
            }
            catch (DbComcurrancyException ex)
            {

                throw new DbComcurrancyException(ex.Message);
            }
        }
        public string Update(Person p)
        {
            try
            {
                _context.People.Update(p);
                _context.SaveChanges();
                return "Pessoa: " + p.Name + "Atualizada com sucesso!";
            }
            catch (DbComcurrancyException ex)
            {

                throw new DbComcurrancyException("Não pode atualizar \n" + ex.Message);
            }
        }
        public string Delete(Person p)
        {
            try
            {
                _context.People.Remove(p);
                _context.SaveChanges();
                return "Deletado com Sucesso!";
            }
            catch (DbComcurrancyException ex)
            {

                throw new DbComcurrancyException("Não pode atualizar \n" + ex.Message);
            }
           
        }
        #endregion
        #endregion
    }
}
