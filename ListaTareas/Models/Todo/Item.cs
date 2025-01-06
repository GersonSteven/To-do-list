using System.ComponentModel.DataAnnotations;


namespace ListaTareas.Models.Todo
{
    public class Item
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Titulo.")]
        public string Title { get; set; }
        public bool IsDone { get; set; } = false;
        public string? UserId { get; set; }
        [Required]
        [DataType(DataType.DateTime), Display(Name = "Fecha y hora de finalizacion.")]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
