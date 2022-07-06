#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace gameSwapCSharp.Models;

public class Message
{
    [Key]
    public int MessageId {get;set;}

    [Required(ErrorMessage = "You must include a message")]
    public string MessageContent {get;set;}

    [Required]
    [Range(1, Int32.MaxValue, ErrorMessage = "Proposed Price Must Be Greater Than 0")]
    public int ProposedPrice {get;set;}

    [Required]
    public int SenderId {get;set;}
    public User? Sender {get;set;}

    [Required]
    public int RecipientId {get;set;}
    public User? Recipient {get;set;}

    public int GameId {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public List<Response> Responses {get;set;} = new List<Response>();
}