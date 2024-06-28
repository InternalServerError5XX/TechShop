using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Domain.Entities
{
    public class BaseEntity
    {
        [Column(Order = 0)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
