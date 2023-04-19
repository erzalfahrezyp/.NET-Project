using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.SyncDataService.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
    opts.JsonSerializerOptions.PropertyNamingPolicy = null);
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// service db
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IProductRepo, ProductRepo>();

builder.Services.AddHttpClient<IOrderDataClient,HttpOrderDataClient>();

// auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// seeding data
PrepDb.PrepPopulation(app);

app.Run();
