using warehouse_api.DTOs.Dashboards;

namespace warehouse_api.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<WarehouseDashboardResponseDto?>
            GetWarehouseDashboardAsync(
                int warehouseId,
                DateTime dashboardDate);

        Task<OwnerDashboardResponseDto>
            GetOwnerDashboardAsync();
    }
}
