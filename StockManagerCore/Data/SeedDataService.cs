using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using StockManagerCore.Models;


namespace StockManagerCore.Data
{
    class SeedDataService
    {
        private StockDBContext _Context;
        private string dboMyTable;
        public SeedDataService(StockDBContext context) //This Constructor is for Injection of Dependency
        {
            _Context = context;
        }
        public void Seed()
        {
            if (_Context.Products.Any() || _Context.Companies.Any() || _Context.Stocks.Any())
            {
                return; //DB Has been populated
            }
            //Seeding Products groups
            Product p1 = new Product(1, "ANEL");
            Product p2 = new Product(2, "ARGOLA");
            Product p3 = new Product(3, "BRACELETE");
            Product p4 = new Product(4, "BRINCO");
            Product p5 = new Product(5, "CHOCER");
            Product p6 = new Product(6, "COLAR");
            Product p7 = new Product(7, "CORRENTE");
            Product p8 = new Product(8, "PINGENTE");
            Product p9 = new Product(9, "PULSEIRA");
            Product p10 = new Product(10, "TORNOZELEIRA");
            Product p11 = new Product(11, "PEAÇAS");
            Product p12 = new Product(12, "VARIADOS");
            Product p13 = new Product(13, "BROCHE");

            //Seeding Companies
            Company c1 = new Company(1, "ATACADAO");
            Company c2 = new Company(2, "JR");

            //Seeding Stocks Company Id=1
            Stock s1 = new Stock(1, p1, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s2 = new Stock(2, p2, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s3 = new Stock(3, p3, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s4 = new Stock(4, p4, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s5 = new Stock(5, p5, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s6 = new Stock(6, p6, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s7 = new Stock(7, p7, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s8 = new Stock(8, p8, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s9 = new Stock(9, p9, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s10 = new Stock(10, p10, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s11 = new Stock(11, p11, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s12 = new Stock(12, p12, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            Stock s13 = new Stock(13, p13, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c1);
            //Seeding Stocks Company id=2
            Stock s14 = new Stock(14, p1, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s15 = new Stock(15, p2, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s16 = new Stock(16, p3, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s17 = new Stock(17, p4, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s18 = new Stock(18, p5, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s19 = new Stock(19, p6, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s20 = new Stock(20, p7, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s21 = new Stock(21, p8, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s22 = new Stock(22, p9, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s23 = new Stock(23, p10, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s24 = new Stock(24, p11, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s25 = new Stock(25, p12, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);
            Stock s26 = new Stock(26, p13, 0, 0, 0.0, 0.0, new DateTime(2020, 01, 01), c2);

            //Sending |T-SQL


            _Context.Products.AddRange(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);

            _Context.Companies.AddRange(c1, c2);

            _Context.Stocks.AddRange(s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13,
                                    s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24, s25, s26
            );

            _Context.SaveChanges();
        }

        public void SetInsertON(string table)
        {
            using (var connection = new SqlConnection("server=tcp:192.168.100.2,1433;Initial Catalog =StockKManagerDB;User Id=sa;Password=$3nh@2018;"))
            {
                connection.Open();
                var query = "SET IDENTITY_INSERT " + table + " ON;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@identityColumnValue", 3);
                    command.ExecuteNonQuery();
                }

            }
        }
        public void SetInsertOff(string table)
        {
            using (var connection = new SqlConnection("server=tcp:192.168.100.2,1433;Initial Catalog =StockKManagerDB;User Id=sa;Password=$3nh@2018;"))
            {
                connection.Open();
                var query = "SET IDENTITY_INSERT " + table + " OFF;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

            }

        }
    }
}

