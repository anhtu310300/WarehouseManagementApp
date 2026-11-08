using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace warehouse_api.Domain
{
    [Table("warehouse_db_empTable")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("empId")]
        public int EmpId { get; set; }

        [Column("empName")]
        public string EmpName { get; set; } = string.Empty;

        [Column("warehouseAssignedId")]
        public int WarehouseAssignedId { get; set; }

        [Column("mobileNo")]
        public string MobileNo { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = string.Empty;

        [Column("userName")]
        public string UserName { get; set; } = string.Empty;

        [Column("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("isActive")]
        public bool IsActive { get; set; }
    }
}
