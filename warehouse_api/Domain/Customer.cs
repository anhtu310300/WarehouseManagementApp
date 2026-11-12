using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse_api.Domain
{
    [Table("warehouse_db_customerMaster")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("customerId")]
        public int CustomerId { get; set; }

        [Column("custName")]
        public string? CustName { get; set; }

        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Column("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("gstNo")]
        public string GstNo { get; set; } = string.Empty;
    }
}
