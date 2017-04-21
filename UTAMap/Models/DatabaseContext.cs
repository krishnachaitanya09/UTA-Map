using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UTAMap.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<POI> Buildings { get; set; }

        public DatabaseContext() : base("DefaultConnection")
        {
        }
    }
}