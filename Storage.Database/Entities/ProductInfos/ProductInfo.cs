using System.ComponentModel.DataAnnotations.Schema;
using Storage.Database.Entities.Products;

namespace Storage.Database.Entities.ProductInfos
{
    public class ProductInfo : BaseProductInfo
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
