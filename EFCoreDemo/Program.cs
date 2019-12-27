using EFCoreDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var dbContext = new AppDbContext())
            {
                //dbContext.Companys.Add(new Company
                //{
                //    Name = "星城科技",
                //    Address = "湖南长沙雨花区"
                //});
                //dbContext.SaveChanges();

                //Console.WriteLine("All Company in database:");
                //foreach (var company in dbContext.Companys)
                //{
                //    Console.WriteLine("{0}-{1}", company.Name, company.Address);
                //}

                Console.WriteLine("From SQL:");
                var nameText = "星城科技";
                FormattableString querySql = $@"SELECT * FROM companys WHERE Name = {nameText}";
                // EFCore 3.0
                //var comps = dbContext.Set<Company>().FromSqlInterpolated(querySql);
                // EFCore 2.x
                foreach (var company in dbContext.Set<Company>().FromSql(querySql).OrderBy(a => a.Id).ToList())
                {
                    Console.WriteLine("{0}-{1}", company.Name, company.Address);
                }

                Console.ReadKey();
            }
        }
    }
}
