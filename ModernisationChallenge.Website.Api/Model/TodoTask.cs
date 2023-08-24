using System.ComponentModel.DataAnnotations;

namespace ModernisationChallenge.Website.Api.Model
{
    public class TodoTask
    {
        public int? Id { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool? Completed { get; set; } = false;

        [Required]
        public string? Details { get; set; }
    }
}
