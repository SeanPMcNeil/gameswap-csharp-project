#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace gameSwapCSharp.Models;

public class Game
{
    [Key]
    public int GameId {get;set;}

    [Required]
    public string Title {get;set;}
    [Required]
    public string Platform {get;set;}
    [Required]
    public string Quality {get;set;}
    [Required]
    [Range(1, Int32.MaxValue, ErrorMessage = "Price Must Be Greater Than 0")]
    public int Price {get;set;}

    [Required]
    public int UserId {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    
    public User? Owner {get;set;}

    public List<Message> Inquiries {get;set;} = new List<Message>();

    public Swap? Swap {get;set;}
}