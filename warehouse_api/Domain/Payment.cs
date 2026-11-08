using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse_api.Domain
{
    [Table("warehouse_db_paymentMaster")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("paymentId")]
        public int PaymentId { get; set; }

        [Column("bookingId")]
        public int BookingId { get; set; }

        [Column("paymendDate", TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [Column("paymentAmount")]
        public double PaymentAmount { get; set; }

        [Column("payementMode")]
        public string PaymentMode { get; set; } = string.Empty;

        [Column("paymentNaration")]
        public string PaymentNarration { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("receivedId")]
        public int ReceivedId { get; set; }
    }
}