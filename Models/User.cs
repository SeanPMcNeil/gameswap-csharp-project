#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace gameSwapCSharp.Models;

public class User
{
    [Key]
    public int UserId {get;set;}
    // Other data you want to save
    [Required]
    [MinLength(2)]
    public string Username {get;set;}
    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [Required]
    public string Address {get;set;}
    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password {get;set;}

    [Required]
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PassConfirm {get;set;}

    public int Coins {get;set;} = 0;

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public List<Game> OwnedGames {get;set;} = new List<Game>();
    
    [InverseProperty("Sender")]
    public List<Message> SentMessages {get;set;} = new List<Message>();
    [InverseProperty("Recipient")]
    public List<Message> ReceivedMessages {get;set;} = new List<Message>();
    
    public List<Response> SentResponses {get;set;} = new List<Response>();

    [InverseProperty("Seller")]
    public List<Swap> OwnedSwaps {get;set;} = new List<Swap>();
    [InverseProperty("Buyer")]
    public List<Swap> BoughtSwaps {get;set;} = new List<Swap>();
}