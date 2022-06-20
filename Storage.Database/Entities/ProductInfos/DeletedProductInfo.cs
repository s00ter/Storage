using System;
using Storage.Database.Entities.Interfaces;
using Storage.Database.Entities.Products;

namespace Storage.Database.Entities.ProductInfos
{
    public class DeletedProductInfo : BaseProductInfo, ITrackable
    {
        public int DeletedProductId { get; set; }
        public DeletedProduct DeletedProduct { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}