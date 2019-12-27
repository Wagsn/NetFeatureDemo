using EFCoreDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MySqlConnector.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EFCoreDemo
{
    public class AppDbContext : DbContext
    {
        public DbSet<Company> Companys { get; set; }
        private IConfiguration configuration;

        public AppDbContext()
        {
            configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseMySql(configuration.GetConnectionString("Default"));
        }

        // 2.x
        //public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, _) => true, true) });
#pragma warning disable CS0618 // 类型或成员已过时
        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[]
        {
            new Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Information, true)
        });
#pragma warning restore CS0618 // 类型或成员已过时

        // 3.0
        //public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
