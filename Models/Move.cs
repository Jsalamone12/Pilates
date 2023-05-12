#pragma warning disable CS8618 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pilates.Models;


public class Move 
{
    [Key]
    public int MoveId { get; set; }


    [Required(ErrorMessage = "is required.")]
    [Display(Name = "Image (URL format please")]

    public string Image { get; set; }

    [Required(ErrorMessage = "is required.")]

    public string Title { get; set; }


    [Required(ErrorMessage = "is required.")]

    public bool Bool { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
