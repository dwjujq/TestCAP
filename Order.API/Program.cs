using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Order.API;
using Order.API.Extensions;
using Order.API.Message;
using Order.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;

var orderDbConnection = configuration.GetConnectionString("OrderContext");
builder.Services.AddDbContext<AppDbContext>(options=>options.UseMySql(orderDbConnection,ServerVersion.AutoDetect(orderDbConnection)));

builder.Services.AddMCodeCap();

builder.Services.AddScoped<IMessageTracker, MySqlMessageTracker>();
builder.Services.AddScoped<IOrderService,OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
