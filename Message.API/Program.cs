using Consul;
using Message.API;
using Message.API.Extensions;
using Message.API.Message;
using Message.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;

var dbConnection = configuration.GetConnectionString("InventoryContext");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection)));

builder.Services.AddMCodeCap();

builder.Services.AddScoped<IMessageTracker, MySqlMessageTracker>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

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
