using HarryManual.DataAccess;
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
    }
}