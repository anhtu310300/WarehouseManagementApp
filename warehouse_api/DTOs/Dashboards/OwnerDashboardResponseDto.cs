namespace warehouse_api.DTOs.Dashboards
{
    public class OwnerDashboardResponseDto
    {
        public int TotalWarehouses { get; set; }

        public int TotalSpaces { get; set; }

        public int ActiveSpaces { get; set; }

        public int InactiveSpaces { get; set; }

        public int TotalCustomers { get; set; }

        public int TotalEmployees { get; set; }

        public int ActiveEmployees { get; set; }

        public int TotalBookings { get; set; }

        public int TotalPayments { get; set; }

        public int CompletedPayments { get; set; }

        public double TotalBookingAmount { get; set; }

        public double TotalPaidAmount { get; set; }

        public double RemainingAmount { get; set; }
    }
}
