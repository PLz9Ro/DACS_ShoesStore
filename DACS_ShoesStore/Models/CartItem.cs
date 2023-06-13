using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DACS_ShoesStore.Models
{
    public class CartItem
    {
        public Product Product {get;set;}
        [Key]
        public int Quantity {get;set;}
        public int ProductId { get; set; }
        
        [Required]
        public int Total { get; set; }


    }

}