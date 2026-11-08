using Microsoft.EntityFrameworkCore;
using warehouse_api.Domain;

namespace warehouse_api.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(
        DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<Space> Spaces => Set<Space>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Payment> Payments => Set<Payment>();
}