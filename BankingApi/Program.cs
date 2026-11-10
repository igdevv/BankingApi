using BankingApi.Data;
using BankingApi.Dtos.Accounts;
using BankingApi.Dtos.Transfers;
using BankingApi.Exceptions;
using BankingApi.Services;
using BankingApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TransferService>();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<IValidator<WithdrawRequest>, WithdrawRequestValidator>();
builder.Services.AddScoped<IValidator<DepositRequest>, DepositRequestValidator>();
builder.Services.AddScoped<IValidator<CreateTransferRequest>, CreateTransferRequestValidator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
