﻿using System.ComponentModel.DataAnnotations;


namespace ListaTareas.Models.Todo
{
    public class Todo
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public bool IsDone { get; set; } = false;
        public string? UserId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
