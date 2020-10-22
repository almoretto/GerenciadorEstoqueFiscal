using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
using StockManagerCore.Models;

namespace StockManagerCore.Services
{
    class XMLReader
    {

        public List<InputXML> Inputs { get; set; } = new List<InputXML>();
        // private  List<SoldProduct> Sales { get; set; } = new List<SoldProduct>();
        CultureInfo provider = CultureInfo.InvariantCulture;
        DateTime dhEmi;
        string nItem;
        string xProd;
        int qCom;
        double vUnCom;
        string uCom;
        double vTotal;
        double vUnTrib;
        double vTotTrib;

        [NotMapped]
        public string Path { get; set; }
        [NotMapped]
        public bool Sales { get; set; }

        public XMLReader(string path, bool sales)
        {
            Path = path;
            Sales = sales;
        }

        public string GetInputItens()
        {
            int count = 0;
            using (StreamReader sr = File.OpenText(Path)) //forma reduzida
            {

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split('|');
                    if (line.Length != 0)
                    {
                        for (int i = 0; i < line.Length; i++)
                        {

                            /*
                            InputXML p = new InputXML();
                            dhEmi = DateTime.ParseExact(x.Element("dhEmi").Value, "yyyy-MM-dd", provider);
                            nItem = x.Attribute("nItem").Value;
                            xProd = x.Attribute("xProd").Value;
                            qCom = int.Parse(x.Attribute("qCom").Value);
                            vUnCom = double.Parse(x.Attribute("vUnCom").Value);
                            uCom = x.Attribute("uCom").Value;
                            vTotal = double.Parse(x.Attribute("vTotal").Value);
                            vUnTrib = double.Parse(x.Attribute("vUnTrib").Value);
                            vTotTrib = double.Parse(x.Attribute("vTotTrib").Value);

                            p.DhEmi = dhEmi;
                            p.NItem = nItem;
                            p.XProd = xProd;
                            p.QCom = qCom;
                            p.VUnCom = vUnCom;
                            p.UCom = uCom;
                            p.Vtotal = vTotal;
                            p.VUnTrib = vUnTrib;
                            p.VTotTrib = vTotTrib;
                            */
                            Inputs.Add(p);
                        }
                    }
                    count++;
                }
            }
            GenerateGroups();
            return "Inputs Added :" + count.ToString();
        }
        private void GenerateGroups()
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].XProd.Contains("Anel"))
                {
                    Inputs[i].Group = "Anel";
                }
                else if (Inputs[i].XProd.Contains("Argola"))
                {
                    Inputs[i].Group = "Argola";
                }
                else if (Inputs[i].XProd.Contains("Bracelete"))
                {
                    Inputs[i].Group = "Bracelete";
                }
                else if (Inputs[i].XProd.Contains("Brinco"))
                {
                    Inputs[i].Group = "Brinco";
                }
                else if (Inputs[i].XProd.Contains("Choker"))
                {
                    Inputs[i].Group = "Choker";
                }
                else if (Inputs[i].XProd.Contains("Colar") || Inputs[i].XProd.Contains("Gargantilha"))
                {
                    Inputs[i].Group = "Colar";
                }
                else if (Inputs[i].XProd.Contains("Corrente"))
                {
                    Inputs[i].Group = "Corrente";
                }
                else if (Inputs[i].XProd.Contains("Pingente"))
                {
                    Inputs[i].Group = "Pingente";
                }
                else if (Inputs[i].XProd.Contains("Pulseira"))
                {
                    Inputs[i].Group = "Pulseira";
                }
                else if (Inputs[i].XProd.Contains("Tornozeleira"))
                {
                    Inputs[i].Group = "Tornozeleira";
                }
                else if (Inputs[i].XProd.Contains("Variados"))
                {
                    Inputs[i].Group = "Variados";
                }
                else if (Inputs[i].XProd.Contains("Broche"))
                {
                    Inputs[i].Group = "Broche";
                }
                else if (Inputs[i].XProd.Contains("Partes e"))
                {
                    Inputs[i].Group = "Peças";
                }
                else if (Inputs[i].XProd.Contains("Conjunto"))
                {
                    Inputs[i].Group = "Conjunto";
                }
                else
                {
                    Inputs[i].Group = "Variados";
                }
            }
        }

    }
}
