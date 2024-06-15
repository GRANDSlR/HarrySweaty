using HarryManual.DataAccess;
using HarryManual.DataAccess.HarryCarrier;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HarryManual
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("DefaultConnection") {}

        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Person_Quote> Person_Quotes { get; set; }
        public DbSet<Person_Film> Person_Films { get; set; }

    }
}