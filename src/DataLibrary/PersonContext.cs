using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary
{
    public class PersonContext : DbContext
    {
        private readonly string _dbName;

        public DbSet<Person> Person { get; set; }

        public PersonContext() : this(dbName: @"data\data.db")
        {

        }

        public PersonContext(string dbName)
        {
            this._dbName = dbName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite($"Data Source={_dbName}");
                options.EnableSensitiveDataLogging(true);
            }
        }

    }

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }
    }
}
