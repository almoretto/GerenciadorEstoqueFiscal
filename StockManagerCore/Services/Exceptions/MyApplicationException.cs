using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    //Custom Exception Handler
    class MyApplicationException : ApplicationException { public MyApplicationException(string message) : base(message) { } }
}
