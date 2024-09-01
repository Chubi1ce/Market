namespace Market.Models
{
    public class Storage:BaseModel
    {
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
