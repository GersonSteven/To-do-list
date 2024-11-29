using System.ComponentModel.DataAnnotations;


namespace ListaTareas.Models.Todo
{
    public class Todo
    {   
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public bool IsDone { get; set; }
        public string? UserId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
