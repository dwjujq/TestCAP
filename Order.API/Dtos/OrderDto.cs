namespace Order.API.Dtos
{
    public class OrderDto
    {
        public Guid? Id { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
