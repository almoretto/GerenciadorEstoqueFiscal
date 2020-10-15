using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using StockManagerCore.Models;

namespace StockManagerCore.Services
{
    class XMLReader
    {
        
        private static List<InputProduct> Inputs { get; set; } = new List<InputProduct>();
        private static List<SoldProduct> Sales { get; set; } = new List<SoldProduct>();
        [NotMapped]
        public string Path { get; set; }

        public XMLReader(string path)
        {
            Path = path;
        }

        public static void GetInputItens(string path)
        {

            XElement xml = XElement.Load(path);

            foreach (XElement x in xml.Elements())
            {
                InputProduct p = new InputProduct()
                {
                    NItem = x.Attribute("nItem").Value,
                    XProd = x.Attribute("xProd").Value,
                    QCom = int.Parse(x.Attribute("qCom").Value),
                    VUnCom = double.Parse(x.Attribute("vUnCom").Value),
                    UCom = x.Attribute("uCom").Value,
                    Vtotal = double.Parse(x.Attribute("vTotal").Value),
                    VUnTrib = double.Parse(x.Attribute("vUnTrib").Value),
                    VTotTrib = double.Parse(x.Attribute("vTotTrib").Value)
                };
                Inputs.Add(p);
            }
        }
        public static void GetSoldItens(string path)
        {

            XElement xml = XElement.Load(path);

            foreach (XElement x in xml.Elements())
            {
                SoldProduct s = new SoldProduct()
                {
                    NItem = x.Attribute("nItem").Value,
                    XProd = x.Attribute("xProd").Value,
                    QCom = int.Parse(x.Attribute("qCom").Value),
                    VUnCom = double.Parse(x.Attribute("vUnCom").Value),
                    UCom = x.Attribute("uCom").Value,
                    Vtotal = double.Parse(x.Attribute("vTotal").Value),
                    VUnTrib = double.Parse(x.Attribute("vUnTrib").Value),
                    VTotTrib = double.Parse(x.Attribute("vTotTrib").Value)
                };
                Sales.Add(s);
            }
        }
    }
}
