using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Models;
using StockManagerCore.Models.Enums;
using StockManagerCore.Data;


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

        #endregion

    }
}
