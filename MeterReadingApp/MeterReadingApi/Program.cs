using MeterReadingApi.Repositories;
using MeterReadingApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IAccountsRepository, AccountsRepository>();
builder.Services.AddSingleton<IMeterReadingsRepository, MeterReadingsRepository>();
builder.Services.AddTransient<IMeterReadingsService, MeterReadingsService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
