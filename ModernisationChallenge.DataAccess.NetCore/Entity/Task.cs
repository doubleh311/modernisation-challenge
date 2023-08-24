using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernisationChallenge.DataAccess
{
    [Table("Tasks")]
    public class Task : BaseEntity
    {
        [Required]
        public bool? Completed { get; set; } = false;

        [Required]
        public string? Details { get; set; }
    }
}
