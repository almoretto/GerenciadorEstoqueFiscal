using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagerCore.Models
{
    public struct DispStockCompany
    {
        public string Produto { get; set; }
        public int QteCompra { get; set; }
        public String ValorMedio { get; set; } //Formated
        public int QteVendida { get; set; }
        public int QteSaldo { get; set; }
        public String ValorSaldo { get; set; } //Formated
        public String DataSaldo { get; set; }// Formated
        public String ValorCompra { get; set; } //Formated
        public String ValorVenda { get; set; } //Formated
        public String UltimaSaída { get; set; }
        public String UltimaEntrada { get; set; }
    }
}
