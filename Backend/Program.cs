using Backend.Repositories;
using MongoDB.Driver;
var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories 
builder.Services.AddScoped<UserAccountRepository>();
builder.Services.AddScoped<UserRepository>();

// MongoDB
var connectionString = builder.Configuration["MongoDB:ConnectionString"];
var mongoClient = new MongoClient(connectionString);
builder.Services.AddSingleton<IMongoDatabase>(mongoClient.GetDatabase("CleanSweeperDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();