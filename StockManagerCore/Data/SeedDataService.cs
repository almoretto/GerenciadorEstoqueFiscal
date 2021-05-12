#region --== Dependency declaration ==--
using System;
using System.Collections.Generic;
using System.Linq;
using StockManagerCore.Models;
using StockManagerCore.Models.Enums;
#endregion

namespace StockManagerCore.Data
{
    class SeedDataService
    {
        #region --== Constructor for dependency injection ==--
        private StockDBContext _Context;
        //This Constructor is for Injection of Dependency
        public SeedDataService(StockDBContext context) { _Context = context; }
        #endregion

        #region --== Properties ==--
        private List<Product> Ps { get; set; } = new List<Product>();
        private List<Company> Cs { get; set; } = new List<Company>();
        

        #endregion

        //Seeding data region
        public void Seed()
        {
            #region --== Seeding Products groups ==--
            if (!_Context.Products.Any())
            {
                Ps.Add(new Product("ANEL"));
                Ps.Add(new Product("ARGOLA"));
                Ps.Add(new Product("BRACELETE"));
                Ps.Add(new Product("BRINCO"));
                Ps.Add(new Product("CHOKER"));
                Ps.Add(new Product("COLAR"));
                Ps.Add(new Product("CORRENTE"));
                Ps.Add(new Product("PINGENTE"));
                Ps.Add(new Product("PULSEIRA"));
                Ps.Add(new Product("TORNOZELEIRA"));
                Ps.Add(new Product("PEÇAS"));
                Ps.Add(new Product("VARIADOS"));
                Ps.Add(new Product("BROCHE"));
                Ps.Add(new Product("CONJUNTO"));
                Ps.Add(new Product("ACESSORIO"));

                _Context.Products.AddRange(Ps); //Adding to DBSet
                _Context.SaveChanges(); //Saving changes persisting;
            }
            #endregion

            #region --== Seeding Companies ==--
            if (!_Context.Companies.Any())
            {
                Cs.Add(new Company("ATACADAO", 1600000.00));
                Cs.Add(new Company("JR", 1600000.00));
                Cs.Add(new Company("FABRICACAO", 1600000.00));
                Cs.Add(new Company("ATACADAO MCP", 1600000.00));
                _Context.Companies.AddRange(Cs); //Add to dbset
                _Context.SaveChanges(); //Persist
            }
            #endregion

            #region --== Seeding Stocks ==--
            if (!_Context.Stocks.Any())
            {
                #region --== Seeding Stocks Company Atacadao ==--
                var com = _Context.Companies.Where(c => c.Name == "ATACADAO").FirstOrDefault();
                var prd = _Context.Products.Where(p => p.GroupP == "ANEL").FirstOrDefault();
                Stock s1 = new Stock(prd, 142019, 106319, 76819.44, 406753.01, new DateTime(2019, 12, 31), new DateTime(2019,12,31), com);
                prd = _Context.Products.Where(p => p.GroupP == "ARGOLA").FirstOrDefault();
                Stock s2 = new Stock(prd, 34773, 28596, 26462.60, 107119.72, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRACELETE").FirstOrDefault();
                Stock s3 = new Stock(prd, 21239, 10263, 33939.80, 74116.11, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRINCO").FirstOrDefault();
                Stock s4 = new Stock(prd, 208071, 156291, 149007.39, 495604.81, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP == "CHOKER").FirstOrDefault();
                Stock s5 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "COLAR").FirstOrDefault();
                Stock s6 = new Stock(prd, 66801, 50189, 75709.71, 321378.54, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP == "CORRENTE").FirstOrDefault();
                Stock s7 = new Stock(prd, 27218, 26379, 29582.35, 92188.25, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP == "PINGENTE").FirstOrDefault();
                Stock s8 = new Stock(prd, 30177, 21240, 11661.92, 38930.58, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP == "PULSEIRA").FirstOrDefault();
                Stock s9 = new Stock(prd, 83160, 37040, 76877.02, 180068.76, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP == "TORNOZELEIRA").FirstOrDefault();
                Stock s10 = new Stock(prd, 4115, 2221, 9490.42, 7638.90, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP== "PEÇAS").FirstOrDefault();
                Stock s11 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP== "VARIADOS").FirstOrDefault();
                Stock s12 = new Stock(prd, 7399, 6838, 3278.92, 11014.06, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP== "BROCHE").FirstOrDefault();
                Stock s13 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP== "CONJUNTO").FirstOrDefault();
                Stock s27 = new Stock(prd, 2771, 1945, 6694.93, 9741.07, new DateTime(2019, 12, 31), new DateTime(2019, 12, 31), com);
                prd = _Context.Products.Where(p => p.GroupP== "ACESSORIO").FirstOrDefault();
                Stock s57 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                #endregion
                #region --== Dbset and commit Company Atacadao ==--
                _Context.Stocks.AddRange(s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s27);

                _Context.SaveChanges();
                #endregion

                #region --== Seeding Stocks Company JR ==--
                com = _Context.Companies.Where(c => c.Name == "JR").FirstOrDefault();
                prd = _Context.Products.Where(p => p.GroupP == "ANEL").FirstOrDefault();
                Stock s14 = new Stock(prd, 180892, 0, 91078.69, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "ARGOLA").FirstOrDefault();
                Stock s15 = new Stock(prd, 6913, 0, 1337.70, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRACELETE").FirstOrDefault();
                Stock s16 = new Stock(prd, 4659, 0, 3116.88, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRINCO").FirstOrDefault();
                Stock s17 = new Stock(prd, 238034, 0, 126968.50, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CHOKER").FirstOrDefault();
                Stock s18 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "COLAR").FirstOrDefault();
                Stock s19 = new Stock(prd, 9264, 0, 6786.61, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CORRENTE").FirstOrDefault();
                Stock s20 = new Stock(prd, 20464, 0, 19476.01, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PINGENTE").FirstOrDefault();
                Stock s21 = new Stock(prd, 16209, 0, 8971.30, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PULSEIRA").FirstOrDefault();
                Stock s22 = new Stock(prd, 31514, 0, 22208.20, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "TORNOZELEIRA").FirstOrDefault();
                Stock s23 = new Stock(prd, 5000, 0, 1364.09, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PEÇAS").FirstOrDefault();
                Stock s24 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "VARIADOS").FirstOrDefault();
                Stock s25 = new Stock(prd, 3518, 0, 5295.52, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BROCHE").FirstOrDefault();
                Stock s26 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CONJUNTO").FirstOrDefault();
                Stock s28 = new Stock(prd, 5898, 0, 6786.61, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "ACESSORIO").FirstOrDefault();
                Stock s58 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                #endregion
                #region --== Dbset and commit Company 2 ==--
                _Context.Stocks.AddRange(s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24, s25, s26, s28);

                _Context.SaveChanges();
                #endregion

                #region --== Seeding Stocks Company FABRICACAO ==--
                com = _Context.Companies.Where(c => c.Name == "FABRICACAO").FirstOrDefault();
                prd = _Context.Products.Where(p => p.GroupP == "ANEL").FirstOrDefault();
                Stock s29 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "ARGOLA").FirstOrDefault();
                Stock s30 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRACELETE").FirstOrDefault();
                Stock s31 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRINCO").FirstOrDefault();
                Stock s32 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CHOKER").FirstOrDefault();
                Stock s33 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "COLAR").FirstOrDefault();
                Stock s34 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CORRENTE").FirstOrDefault();
                Stock s35 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PINGENTE").FirstOrDefault();
                Stock s36 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PULSEIRA").FirstOrDefault();
                Stock s37 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "TORNOZELEIRA").FirstOrDefault();
                Stock s38 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PEÇAS").FirstOrDefault();
                Stock s39 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "VARIADOS").FirstOrDefault();
                Stock s40 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BROCHE").FirstOrDefault();
                Stock s41 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CONJUNTO").FirstOrDefault();
                Stock s42 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "ACESSORIO").FirstOrDefault();
                Stock s59 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                #endregion
                #region --== Dbset and commit Company 3 ==--
                _Context.Stocks.AddRange(s29, s30, s31, s32, s33, s34, s35, s36, s37, s38, s39, s40, s41, s42);

                _Context.SaveChanges();
                #endregion

                #region --== Seeding Stocks Company ATACADAO MCP ==--
                com = _Context.Companies.Where(c => c.Name == "ATACADAO MCP").FirstOrDefault();
                prd = _Context.Products.Where(p => p.GroupP == "ANEL").FirstOrDefault();
                Stock s43 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "ARGOLA").FirstOrDefault();
                Stock s44 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRACELETE").FirstOrDefault();
                Stock s45 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BRINCO").FirstOrDefault();
                Stock s46 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CHOKER").FirstOrDefault();
                Stock s47 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "COLAR").FirstOrDefault();
                Stock s48 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CORRENTE").FirstOrDefault();
                Stock s49 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PINGENTE").FirstOrDefault();
                Stock s50 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PULSEIRA").FirstOrDefault();
                Stock s51 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "TORNOZELEIRA").FirstOrDefault();
                Stock s52 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "PEÇAS").FirstOrDefault();
                Stock s53 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "VARIADOS").FirstOrDefault();
                Stock s54 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "BROCHE").FirstOrDefault();
                Stock s55 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "CONJUNTO").FirstOrDefault();
                Stock s56 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                prd = _Context.Products.Where(p => p.GroupP == "ACESSORIO").FirstOrDefault();
                Stock s60 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
                #endregion
                #region --== Dbset and commit Company 4 ==--
                _Context.Stocks.AddRange(s43, s44, s45, s46, s47, s48, s49, s50, s51, s52, s53, s54, s55, s56);

                _Context.SaveChanges();
                #endregion
            }
            #endregion

            //#region --== Seeding Cities ==--
            //if (!_Context.Cities.Any())
            //{
            //    Cty.Add(new City("CURITIBA", State.PR));
            //    Cty.Add(new City("BIRIGUI", State.SP));
            //    Cty.Add(new City("MANHAÇU", State.MG));
            //    Cty.Add(new City("LONDRINA", State.PR));
            //    Cty.Add(new City("SAO LUIZ", State.MA));
            //    Cty.Add(new City("SAO PAULO", State.SP));
            //    Cty.Add(new City("PARANAVAI", State.PR));
            //    Cty.Add(new City("GOIANIA", State.GO));
            //    Cty.Add(new City("MANAUS", State.AM));
            //    Cty.Add(new City("SALVADOR", State.BA));
            //    Cty.Add(new City("ERECHIM", State.RS));
            //    Cty.Add(new City("BELEM", State.PA));
            //    Cty.Add(new City("JABOATAO DOS GUARARAPES", State.PE));
            //    Cty.Add(new City("CAMPO GRANDE", State.MS));
            //    Cty.Add(new City("CUIABA", State.MT));
            //    Cty.Add(new City("FORTALEZA", State.CE));
            //    Cty.Add(new City("TEREZINA", State.PI));
            //    Cty.Add(new City("SOROCABA", State.SP));
            //    Cty.Add(new City("TRES ARROIOS", State.RS));
            //    Cty.Add(new City("LAURO DE FREITAS", State.BA));
            //    Cty.Add(new City("ARAGUAINA", State.TO));
            //    Cty.Add(new City("GURUPI", State.TO));
            //    Cty.Add(new City("ADAMANTINA", State.SP));

            //    _Context.Cities.AddRange(Cty); //Adding to DBSet
            //    _Context.SaveChanges();
            //}
            //#endregion

            //#region ---== Seeding People ==--
            //if (!_Context.People.Any())
            //{
            //    Pep.Add(new Person("ADILSON",
            //        "",
            //        _Context.Cities
            //        .Where(c => c.CityName == "ADAMANTINA")
            //        .FirstOrDefault(),
            //        State.SP,
            //        PersonType.PF,
            //        PersonCategory.Representante));
            //    Pep.Add(new Person("ALBERTO",
            //        "",
            //        _Context.Cities
            //        .Where(c => c.CityName == "CUIABA")
            //        .FirstOrDefault(),
            //        State.MT,
            //        PersonType.PF,
            //        PersonCategory.Representante));
            //    Pep.Add(new Person("CYNTHIA",
            //        "",
            //        _Context.Cities
            //        .Where(c => c.CityName == "TEREZINA")
            //        .FirstOrDefault(),
            //        State.PI,
            //        PersonType.PF,
            //        PersonCategory.Representante));

            //    _Context.People.AddRange(Pep);
            //    _Context.SaveChanges();
            //}
            //else
            //{
            //    return;//Seed Realized
            //}
            //#endregion
        }
    }
}

