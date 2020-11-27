using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    //Custom Exception Handler
    class RequiredFieldException : Exception { public RequiredFieldException(string message) : base(message) { } }
}
