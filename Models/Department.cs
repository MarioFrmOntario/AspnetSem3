using System;
using System.ComponentModel.DataAnnotations;

namespace Aspnet_Project.Models{
        public class Department
    {
        [Key]
        public int Id { get; set; } = 0;

        [Required, StringLength(300)]
        public string Name { get; set; } = String.Empty;

        [StringLength(1000)]
        public string? Description { get; set; } = String.Empty;

        public virtual ICollection<Product>? Products { get; set; }
    }
}