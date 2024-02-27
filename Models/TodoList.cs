using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    [Serializable]
    public class TodoList
    {
        [Required]
        public int Id { get; set; }
        public string? Label { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? CreationDate { get; set; }
        public string? FinishDate { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }

    }
}

