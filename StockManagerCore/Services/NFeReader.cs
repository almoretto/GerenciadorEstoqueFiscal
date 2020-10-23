using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StockManagerCore.Services
{
    class NFeReader
    {
        public List<InputNFe> Inputs { get; set; } = new List<InputNFe>();
        public List<string[]> FileNfe { get; set; }
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
        double totalNFe;
        double somaNFe = 0.0;
        int qteTotal = 0;
        double dif, difUn;

        [NotMapped]
        public string Path { get; set; }
        [NotMapped]
        public bool Sales { get; set; }

        public NFeReader(string path, bool sales)
        {
            Path = path;
            Sales = sales;
        }

        public string GetInputItens()
        {
            FileNfe = new List<string[]>();
            int count = 0;
            using (StreamReader sr = File.OpenText(Path)) //forma reduzida
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split("|");
                    if (count != 0)
                    {
                        if (line[0] == "B" || line[0] == "H" || line[0] == "I" || line[0] == "N10h" || line[0] == "O07" || line[0] == "W02")
                        {
                            FileNfe.Add(line);
                        }
                    }
                    count++;
                }
            }
            count = 0;

            for (int i = 0; i < FileNfe.Count; i++)
            {
                if (i == 0)
                {
                    ProcessLines(FileNfe[i]);
                }
                else
                {
                    string[] test = FileNfe[i];
                    if (test[0] == "H")
                    {
                        count++;
                        for (int j = 0; j < 4; j++)
                        {
                                ProcessLines(FileNfe[i + j]);
                        }
                        InputNFe p = new InputNFe();
                        p.DhEmi = dhEmi;
                        p.NItem = nItem;
                        p.XProd = xProd;
                        p.QCom = qCom;
                        p.VUnCom = vUnCom;
                        p.UCom = uCom;
                        p.Vtotal = vTotal;
                        p.Vtotal = vTotal + vTotTrib;
                        p.VTotTrib = vTotTrib;
                        p.VUnTrib = vUnTrib + vUnCom;
                        Inputs.Add(p);
                    }
                    else if (test[0] == "W02")
                    {
                        ProcessLines(FileNfe[i]);
                    }
                }
            }
            foreach (InputNFe item in Inputs)
            {
                somaNFe += item.VTotTrib;
                qteTotal += item.QCom;
            }
            if (totalNFe > somaNFe)
            {
                dif = totalNFe - somaNFe;
                difUn = dif / qteTotal;
                foreach (InputNFe item in Inputs)
                {
                    item.VUnTrib += difUn;
                }
            }

            GenerateGroups();
            return "Inputs Added : " + count;
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
        private void ProcessLines(string[] line)
        {


            if (line[0] == "B")
            {
                dhEmi = DateTime.Parse(line[7], provider, DateTimeStyles.None);
            }
            else if (line[0] == "H")
            {
                nItem = line[1];
            }
            else if (line[0] == "I")
            {
                qCom = 0;
                vUnCom = 0.0;
                xProd = "";
                uCom = "";
                xProd = line[3];
                qCom = Convert.ToInt32(Math.Round(double.Parse(line[8],CultureInfo.InvariantCulture)));
                vUnCom = double.Parse(line[9], CultureInfo.InvariantCulture);
                uCom = line[7];
                vTotal = qCom * vUnCom;
            }
            else if (line[0] == "N10h")
            {
                //ICMS
                vTotTrib = 0.0;
                vTotTrib = double.Parse(line[7], CultureInfo.InvariantCulture);

            }
            else if (line[0] == "O07")
            {
                //IPI
                vTotTrib += double.Parse(line[2], CultureInfo.InvariantCulture);
                vUnTrib = vTotTrib / qCom;
            }
            else if (line[0] == "W02")
            {
                totalNFe = double.Parse(line[19], CultureInfo.InvariantCulture);
            }
        }
    }
}
