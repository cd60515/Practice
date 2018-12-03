using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Practice.Models;

namespace Practice.Service
{
    public class MyContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DIARY>()
                .HasKey(x => new { x.USER_ID,x.DIARY_ID });
        }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Practice.Models.USERS> USERS { get; set; }
        public DbSet<Practice.Models.DIARY> DIARY { get; set; }
    }
}
