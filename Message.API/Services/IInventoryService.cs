using Message.API.Dtos;

namespace Message.API.Services
{
    public interface IInventoryService
    {
        Task<bool> Create(InventoryDto inventoryDto);

        Task DeductInventory(OrderDto orderDto);
    }
}
