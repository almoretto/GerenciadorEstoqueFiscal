using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    class DbRelationalException : ApplicationException { public DbRelationalException(string message) : base(message) { } }
}
