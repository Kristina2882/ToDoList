using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace ToDoList.Models
{
    public class Item
    {

        public int ItemId { get; set; }
        [Required(ErrorMessage ="To-do's description should be added!")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must add your To-do to a category. Have you already created the category?")]
        public int CategoryId { get; set; } 
        public Category? Category { get; set; }
        
        public List<ItemTag>? JoinEntities { get; } 
        
        public bool IsDone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }   
    }
}
