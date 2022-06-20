using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Storage.Database.Entities.ProductInfos;

namespace Storage.Database.Entities.Products
{
    public class Product : BaseProduct
    {
        [InverseProperty(nameof(ProductInfo.Product))]
        public List<ProductInfo> ProductInfos { get; set; }
    }
}