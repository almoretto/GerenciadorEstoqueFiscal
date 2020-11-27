using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    //Custom Exception Handler
    class NotFoundException : MyApplicationException { public NotFoundException(string message) : base(message) { } }
}
