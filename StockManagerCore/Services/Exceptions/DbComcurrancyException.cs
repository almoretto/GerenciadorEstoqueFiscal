using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    //Custom Exception Handler
    class DbComcurrancyException : ApplicationException { public DbComcurrancyException(string message) : base(message) { } }
}
