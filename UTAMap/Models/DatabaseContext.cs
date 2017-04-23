using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UTAMap.Models
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext() : base("DefaultConnection")
        {
        }
    }
}