using Microsoft.AspNetCore.Mvc;
using warehouse_api.Common.Responses;
using warehouse_api.DTOs.Dashboards;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Controllers;

[ApiController]
[Route("api/dashboards")]
public class DashboardsController : ControllerBase
{
    private readonly IDashboardService
        _dashboardService;

    public DashboardsController(
        IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("warehouses/{warehouseId:int}")]
    public async Task<IActionResult> GetWarehouseDashboard(
        int warehouseId,
        [FromQuery] DateTime? date)
    {
        var selectedDate =
            (date ?? DateTime.Today).Date;

        var dashboard =
            await _dashboardService
                .GetWarehouseDashboardAsync(
                    warehouseId,
                    selectedDate);

        if (dashboard is null)
        {
            return NotFound(
                new ApiResponse<object?>
                {
                    Data = null,
                    ErrorMessages = new List<string>
                    {
                        "Warehouse was not found."
                    },
                    IsSuccess = false
                });
        }

        return Ok(
            new ApiResponse<WarehouseDashboardResponseDto>
            {
                Data = dashboard,
                Message = "Warehouse dashboard retrieved successfully.",
                IsSuccess = true
            });
    }

    [HttpGet("owner")]
    public async Task<IActionResult> GetOwnerDashboard()
    {
        var dashboard =
            await _dashboardService
                .GetOwnerDashboardAsync();

        return Ok(
            new ApiResponse<OwnerDashboardResponseDto>
            {
                Data = dashboard,
                Message = "Owner dashboard retrieved successfully.",
                IsSuccess = true
            });
    }
}
