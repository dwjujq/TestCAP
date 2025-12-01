using DotNetCore.CAP;
using Message.API.Dtos;
using Message.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Message.API.Services
{
    public class InventoryService:IInventoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public InventoryService(AppDbContext dbContext,IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> Create(InventoryDto inventoryDto)
        {
            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = inventoryDto.ProductId,
                Stock = inventoryDto.Stock
            };
            _dbContext.Inventory.Add(inventory);
            var flag = await _dbContext.SaveChangesAsync();
            return flag == 1;
        }

        public async Task DeductInventory(OrderDto orderDto)
        {
            try
            {
                using var scope= _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetService<AppDbContext>();
                // （扣减库存）
                var inventory = await context.Inventory
                    .FirstOrDefaultAsync(i => i.ProductId == orderDto.ProductId);
                inventory.Stock -= orderDto.Quantity;
                throw new Exception("werwerwer");
                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
        }
    }
}
