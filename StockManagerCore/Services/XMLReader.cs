using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using StockManagerCore.Models;

namespace StockManagerCore.Services
{
    class XMLReader
    {

        private List<InputXML> Inputs { get; set; } = new List<InputXML>();
        // private  List<SoldProduct> Sales { get; set; } = new List<SoldProduct>();
        [NotMapped]
        public string Path { get; set; }
        public bool Duzens { get; set; }
        public bool  Sales { get; set; }

        public XMLReader(string path, bool duzens, bool sales)
        {
            Path = path;
            Duzens = duzens;
            Sales = sales;
        }

        public string GetInputItens()
        {
            int item = 0;
            XElement xml = XElement.Load(Path);

            foreach (XElement x in xml.Elements())
            {
                InputXML p = new InputXML()
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
                item++;
            }
            GenerateGroups();
            return "Inputs Added :" + item.ToString();
        }
        private void GenerateGroups()
        {
            for (int i = 0; i < Inputs.Count ; i++)
            {
                if (Inputs[i].XProd.Contains("Anel"))
                {
                    Inputs[i].Group = "Anel";
                }
                if (Inputs[i].XProd.Contains("Argola"))
                {
                    Inputs[i].Group = "Argola";
                }
                if (Inputs[i].XProd.Contains("Bracelete"))
                {
                    Inputs[i].Group = "Bracelete";
                }
                if (Inputs[i].XProd.Contains("Brinco"))
                {
                    Inputs[i].Group = "Brinco";
                }
                if (Inputs[i].XProd.Contains("Choker"))
                {
                    Inputs[i].Group = "Choker";
                }
                if (Inputs[i].XProd.Contains("Colar"))
                {
                    Inputs[i].Group = "Colar";
                }
                if (Inputs[i].XProd.Contains("Corrente"))
                {
                    Inputs[i].Group = "Corrente";
                }
                if (Inputs[i].XProd.Contains("Pingente"))
                {
                    Inputs[i].Group = "Pingente";
                }
                if (Inputs[i].XProd.Contains("Pulseira"))
                {
                    Inputs[i].Group = "Pulseira";
                }
                if (Inputs[i].XProd.Contains("Tornozeleira"))
                {
                    Inputs[i].Group = "Tornozeleira";
                }
                if (Inputs[i].XProd.Contains("Variados"))
                {
                    Inputs[i].Group = "Variados";
                }
                if (Inputs[i].XProd.Contains("Broche"))
                {
                    Inputs[i].Group = "Broche";
                }
                if (Inputs[i].XProd.Contains("Partes e"))
                {
                    Inputs[i].Group = "Broche";
                }
            }
        }
        private void ProcessData()
        {


        }
    }
}
