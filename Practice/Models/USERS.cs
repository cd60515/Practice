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
        [Key]
        public string ID { get; set; }
        public string NAME { get; set; }
        public string PWD { get; set; }
    }

    public class DIARY
    {
        [Key]
        public string USER_ID { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DIARY_ID { get; set; }
        public DateTime DIARY_DATE { get; set; }
        public string DIARY_TEXT { get; set; }
    }
}
