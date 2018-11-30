using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Practice.Service;

namespace Practice.Models
{
    public class USERS
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string NAME { get; set; }
        public string PWD { get; set; }
    }

    public class DIARY
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string USER_ID { get; set; }
        public string DIARY_ID { get; set; }
        public DateTime DIARY_DATE { get; set; }
        public string DIARY_TEXT { get; set; }
    }
    //public class MyContext : DbContext
    //{
    //    public MyContext(DbContextOptions<MyContext> options) : base(options) { }
    //    public DbSet<USERS> USERS { get; set; }
    //    public DbSet<USERS> DIARY { get; set; }
    //}
}
