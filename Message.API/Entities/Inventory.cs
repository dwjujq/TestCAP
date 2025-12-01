namespace Message.API.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; } 

        public int Stock { get; set; }

    }
}
