#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace gameSwapCSharp.Models;

public class Swap
{
    [Key]
    public int SwapId {get;set;}

    [Required]
    public int FinalPrice {get;set;}

    [Required]
    public string TrackingInfo {get;set;}

    [Required]
    public int BuyerId {get;set;}
    public User? Buyer {get;set;}

    [Required]
    public int SellerId {get;set;}
    public User? Seller {get;set;}

    public int GameId {get;set;}
    public Game? Game {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public byte GameReceived {get;set;} = 0;
}