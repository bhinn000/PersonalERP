using PersonalERP;
using Microsoft.EntityFrameworkCore;
using PersonalERP.Interface;
using PersonalERP.Repository;
using PersonalERP.Service;
using PersonalERP.Services;
using PersonalERP.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IArtPieceRepo, ArtPieceRepo>();
builder.Services.AddScoped<IArtPieceService , ArtPieceService>();
builder.Services.AddScoped<ICustomerRepo,CustomerRepo>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICraftsOrderRepo, CraftsOrderRepo>();
builder.Services.AddScoped<ICraftsOrderService, CraftsOrderService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
    

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
