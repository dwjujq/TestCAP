namespace Message.API.Dtos
{
    public record InventoryDto
    {
        public Guid ProductId { get; set; } 

        public int Stock { get; set; }

    }
}
