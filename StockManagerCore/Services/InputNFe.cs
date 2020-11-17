using System;

namespace StockManagerCore.Services
{
    public class InputNFe
    {
        #region --== Field properties ==--
        public DateTime DhEmi { get; set; }
        public string NItem { get; set; }
        public string XProd { get; set; }
        public int QCom { get; set; }
        public double VUnCom  { get; set; }
        public string UCom { get; set; }
        public double  Vtotal { get; set; }
        public double VUnTrib { get; set; }
        public double VTotTrib { get; set; }
        public string Group { get; set; }
        #endregion
       
        //Methods
        public void AlternateNames()
        {
            //get names of the groups from name of product
            if (XProd.Contains("ANEL"))
            {
                XProd = "ANEL";
            }
            else if (XProd.Contains("ARGOLA") || XProd.Contains("Trio de Argolas"))
            {
                XProd = "ARGOLA";
            }
            else if (XProd.Contains("BRACELETE"))
            {
                XProd = "BRACELETE";
            }
            else if (XProd.Contains("BRINCO"))
            {
                XProd = "BRINCO";
            }
            else if (XProd.Contains("CHOKER") || XProd.Contains("GARGANTILHA") || XProd.Contains("GARGANTILHAS"))
            {
                XProd = "CHOKER";
            }
            else if (XProd.Contains("COLAR") || XProd.Contains("CORDAO"))
            {
                XProd = "COLAR";
            }
            else if (XProd.Contains("CORRENTE"))
            {
                XProd = "CORRENTE";
            }
            else if (XProd.Contains("PINGENTE"))
            {
                XProd = "PINGENTE";
            }
            else if (XProd.Contains("PULSEIRA"))
            {
                XProd = "PULSEIRA";
            }
            else if (XProd.Contains("TORNOZELEIRA"))
            {
                XProd = "TORNOZELEIRA";
            }
            else if (XProd.Contains("VARIADOS"))
            {
                XProd = "VARIADOS";
            }
            else if (XProd.Contains("BROCHE"))
            {
                XProd = "BROCHE";
            }
            else if (XProd.Contains("PARTES E"))
            {
                XProd = "PEÇAS";
            }
            else if (XProd.Contains("CONJUNTO") || XProd.Contains("KIT"))
            {
                XProd = "CONJUNTO";
            }
            else
            {
                XProd = "VARIADOS";
            }
        }
    }

}
