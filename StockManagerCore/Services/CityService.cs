using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Services.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services
{
    public class CityService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public CityService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods ==--
        
        //Querry to fetch all cities
        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.CityName);
        }
       
        //Querry to find a city by Name
        public City FindByName(string name)
        {
            return _context.Cities.Where(c => c.CityName == name).FirstOrDefault();
        }
       
        //Methods to Create New, Update or Delete Cities
        public string Create(City c)
        {
            try
            {
                _context.Cities.Add(c);
                _context.SaveChanges();
                return "City: "
                    + _context.Cities
                    .Where(c => c.CityName == c.CityName)
                    .FirstOrDefault()
                + " susscessfull Added";
            }
            catch (DbComcurrancyException ex)
            {
                throw new DbComcurrancyException("Não foi possível criar: \n" + ex.Message);
            }
        }
        public string Update(City c)
        {
            try
            {
                _context.Cities.Update(c);
                _context.SaveChanges();
                return "City: " + c.CityName + "Atualizada com sucesso!";
            }
            catch (DbComcurrancyException ex)
            {
                throw new DbComcurrancyException("Não pode atualizar\n" + ex.Message);
            }
        }
        public string Delete(City c)
        {
            try
            {
                _context.Cities.Remove(c);
                _context.SaveChanges();
                return "Deletado com Sucesso!";
            }
            catch (DbComcurrancyException ex)
            {
                throw new DbComcurrancyException("Não foi possível deletar!\n" + ex.Message);
            }


        }
        #endregion
    }
}
