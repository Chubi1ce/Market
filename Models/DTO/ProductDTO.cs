namespace Market.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int ProductGroupId { get; set; }
        public string? Description { get; set; }
    }
}
