using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi
{
    public class PersonApiDbContext : DbContext
    {
        public PersonApiDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseNpgsql("User ID=postgres;Password=baris0307;Host=localhost;Port=5432;Database=postgres;Pooling=true;", x => x.MigrationsAssembly("PersonApi"));
            base.OnConfiguring(ob);
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
