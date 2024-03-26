namespace Stock.API.Models.Entities
{
    public class OrderStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
