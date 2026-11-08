using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse_api.Domain
{
    [Table("warehouse_db_bookingTable")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("bookingId")]
        public int BookingId { get; set; }

        [Column("customerId")]
        public int CustomerId { get; set; }

        [Column("warehouseId")]
        public int WarehouseId { get; set; }

        [Column("spaceId")]
        public int? SpaceId { get; set; }

        [Column("bookingFromDate", TypeName = "date")]
        public DateTime BookingFromDate { get; set; }

        [Column("bookingTillDate", TypeName = "date")]
        public DateTime BookingTillDate { get; set; }

        [Column("bookingDate", TypeName = "datetime")]
        public DateTime BookingDate { get; set; }

        [Column("sqftAreaNeeded")]
        public int SqftAreaNeeded { get; set; }

        [Column("totalAmount")]
        public double TotalAmount { get; set; }

        [Column("bookingDoneBy")]
        public int BookingDoneBy { get; set; }
    }
}