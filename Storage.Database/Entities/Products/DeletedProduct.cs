using System;
using System.Collections.Generic;
using Storage.Database.Entities.Interfaces;
using Storage.Database.Entities.ProductInfos;

namespace Storage.Database.Entities.Products
{
    public class DeletedProduct : BaseProduct, ITrackable
    {
        public DateTime CreatedAt { get; set; }

        public List<DeletedProductInfo> DeletedProductInfos { get; set; }
    }
}