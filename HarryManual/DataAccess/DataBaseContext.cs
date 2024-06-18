using HarryManual.DataAccess;
using HarryManual.DataAccess.HarryCarrier;
using System.Data.Entity;

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
        public DbSet<CustomCategory> CustomCategories { get; set; }
        public DbSet<CustomCategory_Note> CustomCategory_Notes { get; set; }
        public DbSet<Notes> Notes { get; set; }

    }
}