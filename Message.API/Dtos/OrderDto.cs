namespace Message.API.Dtos
{
    public record OrderDto
    {
        public Guid Id { get; init; }

        public Guid ProductId { get; init; }

        public int Quantity { get; set; }
    }
}
