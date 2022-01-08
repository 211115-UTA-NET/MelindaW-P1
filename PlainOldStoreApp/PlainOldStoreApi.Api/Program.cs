using Microsoft.AspNetCore.Mvc.Formatters;
using PlainOldStoreApp.DataStorage;

string connectionString = await File.ReadAllTextAsync("C:/Users/melin/OneDrive/Desktop/RevGit/MelindaW-P0/waggonerm-posa-db.txt");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    var jsonFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().First();
    jsonFormatter.SerializerOptions.WriteIndented = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add the repository services to the container.
builder.Services.AddSingleton<ICustomerRepository>(spCr => new SqlCustomerRepository(connectionString));
builder.Services.AddSingleton<IStoreRepository>(spSr => new SqlStoreRepository(connectionString));
builder.Services.AddSingleton<IProductRepository>(spPr => new SqlProductRepository(connectionString));
builder.Services.AddSingleton<IOrderRepository>(spOr => new SqlOrderRepository(connectionString));

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
