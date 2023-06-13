    using Microsoft.Build.Framework.XamlTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DACS_ShoesStore.Models
{
    [Table("Product")]
    public  class Product 
    {
        public Product()
        {
            this.ProductImage = new HashSet<ProductImage>();

            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [StringLength(50)]
        //Model DataAnnotations
        [Required(ErrorMessage = "Vui long nhap tieu de")]
        public string Title { get; set; }
        [StringLength(500)]
        public string SubTitle { get; set; }
        public decimal? Price { get; set; }

        [StringLength(200)]
        public string FeatureImage { get; set; }
  /*      public int ProductCateId { get; set; }*/

        [StringLength(500)]
        public string Des { get; set; }
        [StringLength(50)]
        public string Alias { get; set; }
        public Category Category { get; set; }
        public int? ViewCount { get; set; }

/*        public virtual Category categorys  { get; set; }
*/
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }

    }

}

   
