using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse_api.Domain
{
    [Table("warehouse_db_warehousMaster")]
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("warehouseId")]
        public int WarehouseId { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("address1")]
        public string Address1 { get; set; } = string.Empty;

        [Column("address2")]
        public string Address2 { get; set; } = string.Empty;

        [Column("city")]
        public string City { get; set; } = string.Empty;

        [Column("totalArea")]
        public int TotalArea { get; set; }

        [Column("isColdStorageAvailable")]
        public bool? IsColdStorageAvailable { get; set; }
    }
}