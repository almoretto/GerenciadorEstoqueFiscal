using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    //Custom Exception Handler
    class DbRelationalException : ApplicationException { public DbRelationalException(string message) : base(message) { } }
}
