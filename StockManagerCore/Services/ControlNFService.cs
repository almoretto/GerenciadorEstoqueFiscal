using System;
using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Models;
using StockManagerCore.Models.Enums;
using StockManagerCore.Data;
using StockManagerCore.Services.Exceptions;
using System.Collections.ObjectModel;

namespace StockManagerCore.Services
{
    public class ControlNFService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public ControlNFService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods==--
        public IEnumerable<NFControl> GetControls()
        {
            return _context.NFControls.OrderBy(c => c.Expiration);
        }
        public ObservableCollection<NFControl> GetObservableNFs()
        {
             return (ObservableCollection<NFControl>)_context.NFControls.OrderBy(c => c.Expiration);
        }
        public NFControl FindByNumber(int number)
        {
            return _context.NFControls.Where(n => n.NFNumber == number).FirstOrDefault();
        }
        public IEnumerable<NFControl>FindByCompany(string name)
        {
            return _context.NFControls
                .Where(n => n.Company.Name == name)
                .OrderBy(n => n.Expiration);
        }
        public IEnumerable<NFControl> FindByDestination(string destinatary)
        {
            return _context.NFControls
                .Where(n => n.Destinatary.Name == destinatary)
                .OrderBy(n => n.Expiration);
        }

        public string Crete(NFControl NF)
        {
            string result;
            try
            {
                if (NF==null)
                {
                    throw new RequiredFieldException("Required Entity");
                }
                else
                {
                    _context.NFControls.Add(NF);
                    _context.SaveChanges();
                   result= _context.NFControls
                        .Where(n => n.NFNumber == NF.NFNumber)
                        .FirstOrDefault()
                        .ToString();
                    return result;
                }
            }
            catch (DbComcurrancyException ex)
            {
                throw new DbComcurrancyException(ex.Message);
            }
            catch(RequiredFieldException ex)
            {
                throw new RequiredFieldException(ex.Message);
            }
        }
        #endregion

    }
}
