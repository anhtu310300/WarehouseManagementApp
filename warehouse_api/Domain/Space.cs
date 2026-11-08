using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse_api.Domain
{
    [Table("warehouse_db_spaceMaster")]
    public class Space
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpaceId { get; set; }
        
        [Column("warehouseId")]
        public int WarehouseId { get; set; }

        [Column("spaceName")]
        public string SpaceName { get; set; } = string.Empty;

        [Column("areaSqft")]
        public int AreaSqft { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }
    }
}
