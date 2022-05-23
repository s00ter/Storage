using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Storage
{
    class AppContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public AppContext() : base("DefaultConnection") { }
    }
}
