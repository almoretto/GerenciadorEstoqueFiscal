using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Models;
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

        #endregion

    }
}
