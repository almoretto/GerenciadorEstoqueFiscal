#region --== Dependency declaration ==--
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
#endregion

namespace StockManagerCore.Services
{
    class FileReader
    {
        /// <summary>
        /// Class destinated to process the files for import data to system model and Database.
        /// This class has two ways import input records or sale records.
        /// in both the treatment occurrs on the same method but in separate decisions because of the layout
        /// the input file is close received, and the sales is created by the client.
        /// </summary>
        #region --== Local variables declarations ==--
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
        #endregion

        #region --== Class properties ==--
        public List<InputNFe> Inputs { get; set; } = new List<InputNFe>();
        public List<string[]> FileNfe { get; set; }
        public string Path { get; set; }
        public bool IsSales { get; set; }
        #endregion

        #region --== Constructors ==--
        public FileReader(string path, bool isSales)
        {
            Path = path;
            IsSales = isSales;
        }
        #endregion

        #region --== Functional methods ==--

        //Method to read the file and get the data to inside.
        public string GetInputItens()
        {

            FileNfe = new List<string[]>();
            int count = 0;
            //Using method to open file and read content on the provided path
            using (StreamReader sr = File.OpenText(Path)) //reduced form
            {
                //Loop until not the end
                while (!sr.EndOfStream)
                {
                    if (!IsSales) //If the boolean Sales seted as False - we treat Incomes.
                    {
                        //Here the method split the line by the tabulator character | in an string vector and store in an list of vectors.

                        string[] line = sr.ReadLine().ToUpper().Split("|");

                        if (count != 0) //the method disregards the header line.
                        {
                            //the decision will save only lines with these initial collumns to cleam the final matrix.
                            if (line[0] == "B" || line[0] == "H" || line[0] == "I" || line[0] == "N10h" || line[0] == "O07" || line[0] == "W02")
                            {
                                FileNfe.Add(line); //Matrix Created
                            }
                        }
                        count++;
                    }
                    else //If the Boolean was true then it is Sales so treated here.
                    {
                        //the decision splits each line by the tabulator ; in a string vector 
                        //and add each line to an list creating a matrix

                        string[] line = sr.ReadLine().ToUpper().Split(";");

                        if (count != 0) //the method disregards the header line.
                        {
                            FileNfe.Add(line);//Matrix Created
                        }
                        count++;
                    }
                }
                //On the end of the matrix creation the process continues.
                if (!IsSales) //Method for inputs bool = false
                {
                    ProcessInputFile(); //calling input file process method
                }
                else //method for sales bool = true
                {
                    ProcessOutputFile(); //calling output file process method.
                }
            }

            return "Inputs Added : " + Inputs.Count; // returns the processed inputs at the end.
        }

        //Method to generate the name of the product as groupnames, getting in the description name of each product
        private void GenerateGroups()
        {
            //get names of the groups from name of product
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].XProd.Contains("ANEL") || Inputs[i].XProd.Contains("ANEIS") || Inputs[i].XProd.Contains("ANÉIS"))
                {
                    Inputs[i].Group = "ANEL";
                }
                else if (Inputs[i].XProd.Contains("ARGOLA") || Inputs[i].XProd.Contains("Trio de Argolas") || Inputs[i].XProd.Contains("ARGOLAS"))
                {
                    Inputs[i].Group = "ARGOLA";
                }
                else if (Inputs[i].XProd.Contains("BRACELETE") || Inputs[i].XProd.Contains("BRACELETES"))
                {
                    Inputs[i].Group = "BRACELETE";
                }
                else if (Inputs[i].XProd.Contains("BRINCO") || Inputs[i].XProd.Contains("BRINCOS"))
                {
                    Inputs[i].Group = "BRINCO";
                }
                else if (Inputs[i].XProd.Contains("CHOKER") || Inputs[i].XProd.Contains("GARGANTILHA") || Inputs[i].XProd.Contains("GARGANTILHAS"))
                {
                    Inputs[i].Group = "CHOKER";
                }
                else if (Inputs[i].XProd.Contains("COLAR") || Inputs[i].XProd.Contains("CORDAO")
                    || Inputs[i].XProd.Contains("CORDÃO") || Inputs[i].XProd.Contains("ESCAPULARIO")
                    || Inputs[i].XProd.Contains("ESCAPULÁRIO") || Inputs[i].XProd.Contains("COLARES")
                    || Inputs[i].XProd.Contains("CORDÕES") || Inputs[i].XProd.Contains("CORDOES")
                    || Inputs[i].XProd.Contains("ESCAPULARIOS") || Inputs[i].XProd.Contains("ESCAPULÁRIOS"))
                {
                    Inputs[i].Group = "COLAR";
                }
                else if (Inputs[i].XProd.Contains("CORRENTE") || Inputs[i].XProd.Contains("CORRENTES"))
                {
                    Inputs[i].Group = "CORRENTE";
                }
                else if (Inputs[i].XProd.Contains("PINGENTE") || Inputs[i].XProd.Contains("PINGENTES"))
                {
                    Inputs[i].Group = "PINGENTE";
                }
                else if (Inputs[i].XProd.Contains("PULSEIRA") || Inputs[i].XProd.Contains("PULSEIRAS"))
                {
                    Inputs[i].Group = "PULSEIRA";
                }
                else if (Inputs[i].XProd.Contains("TORNOZELEIRA") || Inputs[i].XProd.Contains("TORNOZELEIRAS"))
                {
                    Inputs[i].Group = "TORNOZELEIRA";
                }
                else if (Inputs[i].XProd.Contains("VARIADOS") || Inputs[i].XProd.Contains("VARIADO")
                    || Inputs[i].XProd.Contains("VARIADA") || Inputs[i].XProd.Contains("VARIADAS"))
                {
                    Inputs[i].Group = "VARIADOS";
                }
                else if (Inputs[i].XProd.Contains("BROCHE") || Inputs[i].XProd.Contains("BROCHES"))
                {
                    Inputs[i].Group = "BROCHE";
                }
                else if (Inputs[i].XProd.Contains("PARTES E") || Inputs[i].XProd.Contains("PECAS") || Inputs[i].XProd.Contains("PEÇAS"))
                {
                    Inputs[i].Group = "PEÇAS";
                }
                else if (Inputs[i].XProd.Contains("CONJUNTO") || Inputs[i].XProd.Contains("KIT") || Inputs[i].XProd.Contains("CONJUNTOS"))
                {
                    Inputs[i].Group = "CONJUNTO";
                }
                else if (Inputs[i].XProd.Contains("ACESSÓRIOS") || Inputs[i].XProd.Contains("ACESSORIOS")
               || Inputs[i].XProd.Contains("acessorios") || Inputs[i].XProd.Contains("acessórios"))
                {
                    Inputs[i].Group = "ACESSORIO";
                }
                else
                {
                    Inputs[i].Group = "VARIADOS";
                }
            }
        }

        //Process the lines of the Incomings, treat each line on collumn 0 to identificate each field data 
        //on the tabulated string.
        private void ProcessLines(string[] line)
        {
            if (line[0] == "B") //Lines that begins with B have the date on position 7 of the tabulated vector
            {
                DateTime date = new DateTime();
                date = DateTime.Parse(line[7], provider, DateTimeStyles.None);
                dhEmi = date.Date;

            }
            else if (line[0] == "H")//Lines that begins with H have the item number on position 1 of the tabulated vector
            {
                nItem = line[1];
            }
            else if (line[0] == "I")//Lines that begins with I have the Products xpecs on position 3, 7, 8 and 9  of the tabulated vector
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
            else if (line[0] == "N10h")//Lines that begins with N10h have the ICMS Taxes on position 7 of the tabulated vector
            {
                //ICMS
                vTotTrib = 0.0;
                vTotTrib = double.Parse(line[7], CultureInfo.InvariantCulture);

            }
            else if (line[0] == "O07")//Lines that begins with O07 have the IPI Tax on position 2 of the tabulated vector
            {
                //IPI
                vTotTrib += double.Parse(line[2], CultureInfo.InvariantCulture);
                vUnTrib = vTotTrib / qCom;
            }
            else if (line[0] == "W02") //Lines that begins with W02 have the Total amount on position 19 of the tabulated vector
            {
                totalNFe = double.Parse(line[19], CultureInfo.InvariantCulture);
            }
        }

        //Process the file of Inputs
        private void ProcessInputFile()
        {
            Inputs.Clear();
            int count = 0;

            for (int i = 0; i < FileNfe.Count; i++)
            {
                if (i == 0)
                {
                    ProcessLines(FileNfe[i]); //Call the Method to process the lines of the file
                }
                else
                {
                    string[] test = FileNfe[i];
                    if (test[0] == "H")// Verifys if the line is An item inicialization.
                    {
                        count++;
                        for (int j = 0; j < 4; j++)
                        {
                            ProcessLines(FileNfe[i + j]);//Process the products expecs of each item
                        }
                        InputNFe p = new InputNFe(); //Create a new temporary instance of the mirror model.
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
                        Inputs.Add(p); //Add the temporary to a list
                    }
                    else if (test[0] == "W02") // if this came to true the the file came to end and the method will get the total amount
                    {
                        ProcessLines(FileNfe[i]);
                    }

                }
            }
            foreach (InputNFe item in Inputs)
            {
                //Creates an total of Products purchased and total qty
                somaNFe += item.Vtotal;
                qteTotal += item.QCom;
            }
            if (totalNFe > somaNFe) //tests if the amount of the file is greater than the sum got above.
            {
                //Calculates the difference beetween total and the sum of products total.
                dif = totalNFe - somaNFe;
                //Divides the difference by the totoal qty
                difUn = dif / qteTotal;
                foreach (InputNFe item in Inputs)
                {
                    //Adds the unitary difference multiplied by qty purchased and sum to total of that product.
                    item.Vtotal += (difUn * item.QCom);
                    //Sum the unitary difference to the unitary value
                    item.VUnCom += difUn;
                }
            }

            GenerateGroups(); //Call the method to generate the name as groups.
        }

        //Process the file of sales
        private void ProcessOutputFile()
        {
            Inputs.Clear();
            foreach (string[] line in FileNfe)
            {
                //Instances the temporary mirror model
                InputNFe sl = new InputNFe();
                //attributes all values from the matrix line to the model
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
                //add the instance to the list.
                Inputs.Add(sl);
            }

            GenerateGroups();

        }
        #endregion
    }
}
