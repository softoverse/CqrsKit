using System.ComponentModel.DataAnnotations;

namespace Softoverse.CqrsKit.WebApi.Models;

public class Student
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required, Range(0, 200)]
    public int? Age { get; set; }
    
    [Required]
    public int AgeCategory { get; set; }

    [Required]
    public string Gender { get; set; }
}

public enum AgeCategory
{
    ADT, // 13 and greater
    CHD, // 2 to under 12 years 
    INF // 0 to under 2 years
}