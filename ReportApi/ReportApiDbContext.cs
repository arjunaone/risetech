using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportApi
{
    public class ReportApiDbContext : DbContext
    {
        public ReportApiDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseNpgsql("User ID=postgres;Password=;Host=localhost;Port=5432;Database=postgres;Pooling=true;", x => x.MigrationsAssembly("ReportApi"));
            base.OnConfiguring(ob);
        }

        public DbSet<Report> Reports { get; set; }
    }
}
