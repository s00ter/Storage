namespace Storage.Database.Entities
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Action { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }

        public Product Product { get; set; }
    }
}
