using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    [Table("Stock")]
    public class Stock
    {
       [Key] 
       public int Id { get; set; }  

       public string Symbol  { get; set; } = string.Empty;

       public string  CompanyName { get; set; } = string.Empty;
       
       [Column(TypeName = "decimal(18,2)")]
       public decimal Purchase { get; set;}
       
       [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }    

        public string Industry { get; set; }  = string.Empty;
        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set;} = new List<Comment>();
        public List<Portfolio> Portfolio { get; set;} = new List<Portfolio>();


    }
}