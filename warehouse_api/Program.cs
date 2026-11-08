using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.Mappings;
using warehouse_api.Repositories;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services;
using warehouse_api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var allowedOrigins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()
    ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // auto assembly, no need manual adding each validators

var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection")
    ?? throw new InvalidOperationException(
        "The database connection string was not found.");

builder.Services.AddDbContext<WarehouseDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();

builder.Services.AddScoped<
    IAuthRepository,
    AuthRepository>();

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IWarehouseRepository,
    WarehouseRepository>();

builder.Services.AddScoped<
    IWarehouseService,
    WarehouseService>();

builder.Services.AddScoped<
    ISpaceRepository,
    SpaceRepository>();

builder.Services.AddScoped<
    ISpaceService,
    SpaceService>();

builder.Services.AddScoped<
    ICustomerRepository,
    CustomerRepository>();

builder.Services.AddScoped<
    ICustomerService,
    CustomerService>();

builder.Services.AddScoped<
    IBookingRepository,
    BookingRepository>();

builder.Services.AddScoped<
    IBookingService,
    BookingService>();

builder.Services.AddScoped<
    IPaymentRepository,
    PaymentRepository>();

builder.Services.AddScoped<
    IPaymentService,
    PaymentService>();

builder.Services.AddScoped<
    IDashboardService,
    DashboardService>();

builder.Services.AddScoped<
    IPasswordHasher<Employee>,
    PasswordHasher<Employee>>();

builder.Services.AddAutoMapper(
    configuration => { },
    typeof(EmployeeProfile)); // auto, no need manual implement each profile

var app = builder.Build();

var swaggerEnabled =
    app.Environment.IsDevelopment()
    || app.Configuration.GetValue<bool>("Swagger:Enabled");

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
