#region --== Dependency declaration ==--
using System;
using System.Collections.Generic;
using System.Linq;
using StockManagerCore.Models;
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

        public void Seed()
        {
            if (_Context.Products.Any() || _Context.Companies.Any() || _Context.Stocks.Any())
            {
                return; //DB Has been populated
            }
            #region --== Seeding Products groups ==--
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
            Ps.Add(new Product("PEAÇAS"));
            Ps.Add(new Product("VARIADOS"));
            Ps.Add(new Product("BROCHE"));
            Ps.Add(new Product("CONJUNTO"));

            _Context.Products.AddRange(Ps); //Adding to DBSet
            #endregion

            #region --== Seeding Companies ==--
            Cs.Add(new Company("ATACADAO"));
            Cs.Add(new Company("JR"));
            Cs.Add(new Company("FABRICACAO"));
            Cs.Add(new Company("ATACADAO MCP"));

            _Context.Companies.AddRange(Cs); //Add to dbset
            _Context.SaveChanges(); //Persist
            #endregion

            #region --== Seeding Stocks Company Id=1 ==--
            var prd = _Context.Products.Where(p => p.Id == 1).FirstOrDefault();
            var com = _Context.Companies.Where(c => c.Id == 1).FirstOrDefault();
            Stock s1 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 2).FirstOrDefault();
            Stock s2 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 3).FirstOrDefault();
            Stock s3 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 4).FirstOrDefault();
            Stock s4 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 5).FirstOrDefault();
            Stock s5 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 6).FirstOrDefault();
            Stock s6 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 7).FirstOrDefault();
            Stock s7 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 8).FirstOrDefault();
            Stock s8 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 9).FirstOrDefault();
            Stock s9 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 10).FirstOrDefault();
            Stock s10 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 11).FirstOrDefault();
            Stock s11 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 12).FirstOrDefault();
            Stock s12 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 13).FirstOrDefault();
            Stock s13 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 14).FirstOrDefault();
            Stock s27 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            #endregion
            #region --== Dbset and commit Company 1 ==--
            _Context.Stocks.AddRange(s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s27);

            _Context.SaveChanges();
            #endregion

            #region --== Seeding Stocks Company id=2 ==--
            prd = _Context.Products.Where(p => p.Id == 1).FirstOrDefault();
            com = _Context.Companies.Where(c => c.Id == 2).FirstOrDefault();
            Stock s14 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 2).FirstOrDefault();
            Stock s15 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 3).FirstOrDefault();
            Stock s16 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 4).FirstOrDefault();
            Stock s17 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 5).FirstOrDefault();
            Stock s18 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 6).FirstOrDefault();
            Stock s19 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 7).FirstOrDefault();
            Stock s20 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 8).FirstOrDefault();
            Stock s21 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 9).FirstOrDefault();
            Stock s22 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 10).FirstOrDefault();
            Stock s23 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 11).FirstOrDefault();
            Stock s24 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 12).FirstOrDefault();
            Stock s25 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 13).FirstOrDefault();
            Stock s26 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 14).FirstOrDefault();
            Stock s28 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            #endregion
            #region --== Dbset and commit Company 2 ==--
            _Context.Stocks.AddRange(s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24, s25, s26, s28);

            _Context.SaveChanges();
            #endregion

            #region --== Seeding Stocks Company id=3 ==--
            prd = _Context.Products.Where(p => p.Id == 1).FirstOrDefault();
            com = _Context.Companies.Where(c => c.Id == 3).FirstOrDefault();
            Stock s29 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 2).FirstOrDefault();
            Stock s30 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 3).FirstOrDefault();
            Stock s31 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 4).FirstOrDefault();
            Stock s32 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 5).FirstOrDefault();
            Stock s33 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 6).FirstOrDefault();
            Stock s34 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 7).FirstOrDefault();
            Stock s35 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 8).FirstOrDefault();
            Stock s36 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 9).FirstOrDefault();
            Stock s37 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 10).FirstOrDefault();
            Stock s38 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 11).FirstOrDefault();
            Stock s39 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 12).FirstOrDefault();
            Stock s40 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 13).FirstOrDefault();
            Stock s41 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 14).FirstOrDefault();
            Stock s42 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            #endregion
            #region --== Dbset and commit Company 3 ==--
            _Context.Stocks.AddRange(s29, s30, s31, s32, s33, s34, s35, s36, s37, s38, s39, s40, s41, s42);

            _Context.SaveChanges();
            #endregion

            #region --== Seeding Stocks Company id = 4 ==--
            prd = _Context.Products.Where(p => p.Id == 1).FirstOrDefault();
            com = _Context.Companies.Where(c => c.Id == 4).FirstOrDefault();
            Stock s43 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 2).FirstOrDefault();
            Stock s44 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 3).FirstOrDefault();
            Stock s45 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 4).FirstOrDefault();
            Stock s46 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 5).FirstOrDefault();
            Stock s47 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 6).FirstOrDefault();
            Stock s48 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 7).FirstOrDefault();
            Stock s49 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 8).FirstOrDefault();
            Stock s50 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 9).FirstOrDefault();
            Stock s51 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 10).FirstOrDefault();
            Stock s52 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 11).FirstOrDefault();
            Stock s53 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 12).FirstOrDefault();
            Stock s54 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 13).FirstOrDefault();
            Stock s55 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            prd = _Context.Products.Where(p => p.Id == 14).FirstOrDefault();
            Stock s56 = new Stock(prd, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), com);
            #endregion
            #region --== Dbset and commit Company 4 ==--
            _Context.Stocks.AddRange(s43, s44, s45, s46, s47, s48, s49, s50, s51, s52, s53, s54, s55, s56);

            _Context.SaveChanges();
            #endregion
        }
    }
}

