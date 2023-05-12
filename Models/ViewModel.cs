#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Pilates.Models;

public class ViewModel
{
    public User? User { get; set; }
    public List<User> AllUsers { get; set; }

    public Move? Move { get; set; }
    public List<Move> AllMoves { get; set; }


}
