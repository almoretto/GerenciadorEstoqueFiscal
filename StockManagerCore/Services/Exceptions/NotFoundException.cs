using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Services.Exceptions
{
    class NotFoundException : MyApplicationException { public NotFoundException(string message) : base(message) { } }
}
