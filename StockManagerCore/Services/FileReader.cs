using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace StockManagerCore.Services
{
    class FileReader
    {
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
        public List<InputNFe> Inputs { get; set; } = new List<InputNFe>();
        public List<string[]> FileNfe { get; set; }
        public string Path { get; set; }
        public bool Sales { get; set; }

        public FileReader(string path, bool sales)
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
                    if (!Sales) //Incomings
                    {
                        string[] line = sr.ReadLine().Split("|");
                        if (count != 0)
                        {
                            //save only lines with these initial collumns
                            if (line[0] == "B" || line[0] == "H" || line[0] == "I" || line[0] == "N10h" || line[0] == "O07" || line[0] == "W02")
                            {
                                FileNfe.Add(line);
                            }
                        }
                        count++;
                    }
                    else //Sales
                    {
                        string[] line = sr.ReadLine().Split(";");
                        if (count != 0)
                        {
                            FileNfe.Add(line);
                        }
                        count++;
                    }
                }
                if (!Sales)
                {
                    ProcessInputFile();
                }
                else
                {
                    ProcessOutputFile();
                }
            }

            return "Inputs Added : " + count;
        }
      
        private void GenerateGroups()
        {
            //get names of the groups from name of product
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].XProd.Contains("ANEL"))
                {
                    Inputs[i].Group = "ANEL";
                }
                else if (Inputs[i].XProd.Contains("ARGOLA"))
                {
                    Inputs[i].Group = "ARGOLA";
                }
                else if (Inputs[i].XProd.Contains("BRACELETE"))
                {
                    Inputs[i].Group = "BRACELETE";
                }
                else if (Inputs[i].XProd.Contains("BRINCO"))
                {
                    Inputs[i].Group = "BRINCO";
                }
                else if (Inputs[i].XProd.Contains("CHOCKER") || Inputs[i].XProd.Contains("GARGANTILHA"))
                {
                    Inputs[i].Group = "CHOCKER";
                }
                else if (Inputs[i].XProd.Contains("COLAR") || Inputs[i].XProd.Contains("CORDAO"))
                {
                    Inputs[i].Group = "COLAR";
                }
                else if (Inputs[i].XProd.Contains("CORRENTE"))
                {
                    Inputs[i].Group = "CORRENTE";
                }
                else if (Inputs[i].XProd.Contains("PINGENTE"))
                {
                    Inputs[i].Group = "PINGENTE";
                }
                else if (Inputs[i].XProd.Contains("PULSEIRA"))
                {
                    Inputs[i].Group = "PULSEIRA";
                }
                else if (Inputs[i].XProd.Contains("TORNOZELEIRA"))
                {
                    Inputs[i].Group = "TORNOZELEIRA";
                }
                else if (Inputs[i].XProd.Contains("VARIADOS"))
                {
                    Inputs[i].Group = "VARIADOS";
                }
                else if (Inputs[i].XProd.Contains("BROCHE"))
                {
                    Inputs[i].Group = "BROCHE";
                }
                else if (Inputs[i].XProd.Contains("PARTES E"))
                {
                    Inputs[i].Group = "PEÇAS";
                }
                else if (Inputs[i].XProd.Contains("CONJUNTO") || Inputs[i].XProd.Contains("KIT"))
                {
                    Inputs[i].Group = "CONJUNTO";
                }
                else
                {
                    Inputs[i].Group = "VARIADOS";
                }
            }
        }

        //Incomings, treat each line collumn 0
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
                qCom = Convert.ToInt32(Math.Round(double.Parse(line[8], CultureInfo.InvariantCulture)));
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
        //Sales
        private void ProcessInputFile()
        {
            int count = 0;

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
        }
        private void ProcessOutputFile()
        {
            foreach (string[] line in FileNfe)
            {
                InputNFe sl = new InputNFe();
                sl.NItem = line[0];
                sl.XProd = line[1].ToUpper();
                sl.QCom = int.Parse(line[2]);
                sl.VUnCom = double.Parse(line[4]);
                sl.Vtotal = double.Parse(line[3]);
                sl.DhEmi = DateTime.ParseExact(line[5], "dd/MM/yyyy", provider);
                sl.Group = "";
                sl.UCom = "";
                sl.VTotTrib = 0.0;
                sl.VUnTrib = 0.0;

                Inputs.Add(sl);
            }

            GenerateGroups();
        }

    }
}
