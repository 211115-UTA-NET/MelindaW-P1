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

//Add the repository services
builder.Services.AddSingleton<ICustomerRepository>(spcr => new SqlCustomerRepository(connectionString));
builder.Services.AddSingleton<IStoreRepository>(spsr => new SqlStoreRepository(connectionString));

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
