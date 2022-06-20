using System.Collections.Generic;

namespace Storage.Database.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Cost { get; set; }
        public string Coming { get; set; }
        public int Amount { get; set; }
        public string VendorCode { get; set; }
        public string Status { get; set; }
        public string Info { get; set; }
        public string Client { get; set; }
        public string Provider { get; set; }
        public string DimensionType { get; set; }
        public string ProductType { get; set; }


        public List<ProductInfo> ProductInfo { get; set; }
    }
}