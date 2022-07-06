#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace gameSwapCSharp.Models;

public class Response
{
    [Key]
    public int ResponseId {get;set;}

    [Required(ErrorMessage = "You must include a message")]
    public string ResponseContent {get;set;}

    [Required]
    [Range(1, Int32.MaxValue, ErrorMessage = "Proposed Price Must Be Greater Than 0")]
    public int ProposedPrice {get;set;}

    [Required]
    public int MessageId {get;set;}
    [Required]
    public int UserId {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}