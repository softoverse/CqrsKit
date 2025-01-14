using System.ComponentModel.DataAnnotations;

namespace Softoverse.CqrsKit.TestConsole.Models;

public class Student
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required, Range(0, 200)]
    public int? Age { get; set; }

    [Required]
    public string Gender { get; set; }
}