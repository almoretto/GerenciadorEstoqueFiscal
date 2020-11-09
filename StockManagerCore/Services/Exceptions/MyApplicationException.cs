using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    class MyApplicationException : ApplicationException { public MyApplicationException(string message) : base(message) { } }
}
