using Backend.Repositories;
using MongoDB.Driver;
var builder = WebApplication.CreateBuilder(args);

/* AddCors: 
 * Selvom frontend og backend kører på localhost, er de to forskellige origins, fordi portene er forskellige.
 * Browseren har derfor en sikkerhedsregel, der som udgangspunkt forhindrer frontend i at læse svar fra backend, medmindre backend giver tilladelse gennem CORS.
 * Her definerer vi sikkerhedspolitik: Vi tillader requests fra frontend på port 7030, Tillader HTTP-headers, Tillader alle metoder (GET, POST, PUT, DELETE osv.)
 */
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7030")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();