using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Models;
using StockManagerCore.Data;
using StockManagerCore.Services.Exceptions;
using System.Collections.ObjectModel;
using System;

namespace StockManagerCore.Services
{
    public class ControlNFService
    {
        #region --== Dependency Injection and constructor ==--
        private readonly StockDBContext _context;
        public ControlNFService(StockDBContext context) { _context = context; }
        #endregion

        #region --== Methods==--
        
        //Querry to fetch all Records on NFControls
        public IEnumerable<NFControl> GetControls()
        {
            return _context.NFControls.OrderBy(c => c.Expiration);
        }
        
        //Querry returning an Observable Collection to Create a Grouped GridView 
        public ObservableCollection<NFControl> GetObservableNFs()
        {
             return (ObservableCollection<NFControl>)_context.NFControls.OrderBy(c => c.Expiration);
        }
        
        //Querry to Find NfControl record by Number
        public NFControl FindByNumber(int number)
        {
            return _context.NFControls.Where(n => n.NFNumber == number).FirstOrDefault();
        }

        //Querry to Find all NfControl record by company
        public IEnumerable<NFControl>FindByCompany(string name)
        {
            return _context.NFControls
                .Where(n => n.Company.Name == name)
                .OrderBy(n => n.Expiration);
        }

        //Querry to Find all NfControl record by destination
        public IEnumerable<NFControl> FindByDestination(string destinatary)
        {
            return _context.NFControls
                .Where(n => n.Destinatary.Name == destinatary)
                .OrderBy(n => n.Expiration);
        }

        //Querry to Find all NfControl record by Date Now
        public IEnumerable<NFControl> GetByDate()
        {
            return _context.NFControls.Where(d => d.Expiration.Date == DateTime.Now.Date);
        }

        //Querry to Find all NfControl record by date now + 7 days
        public IEnumerable<NFControl> GetByWeek()
        {
            return _context.NFControls
                .Where(d => d.Expiration.Date >= DateTime.Now.Date 
                && d.Expiration.Date <= DateTime.Now.AddDays(7).Date);
        }
        #region --== Crud ==--
       
        //Methods to Create new, Edit and Delete a NFControl Record
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
                    return result + ": Created!";
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
        public string Update(NFControl NF)
        {
            string result;
            try
            {
                if (NF == null)
                {
                    throw new RequiredFieldException("Required Entity");
                }
                else
                {
                    _context.NFControls.Update(NF);
                    _context.SaveChanges();
                    result = _context.NFControls
                         .Where(n => n.NFNumber == NF.NFNumber)
                         .FirstOrDefault()
                         .ToString()+": Updated!";
                    return result;
                }
            }
            catch (DbComcurrancyException ex)
            {
                throw new DbComcurrancyException(ex.Message);
            }
            catch (RequiredFieldException ex)
            {
                throw new RequiredFieldException(ex.Message);
            }
        }
        public string Delete(NFControl NF)
        {
            string result;
            try
            {
                if (NF == null)
                {
                    throw new RequiredFieldException("Required Entity");
                }
                else
                {
                    _context.NFControls.Remove(NF);
                    _context.SaveChanges();
                    result = _context.NFControls
                         .Where(n => n.NFNumber == NF.NFNumber)
                         .FirstOrDefault()
                         .ToString() + ": Deleted!";
                    return result;
                }
            }
            catch (DbComcurrancyException ex)
            {
                throw new DbComcurrancyException(ex.Message);
            }
            catch (RequiredFieldException ex)
            {
                throw new RequiredFieldException(ex.Message);
            }
        }
        #endregion
        #endregion
    }
}
