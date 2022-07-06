#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace gameSwapCSharp.Models;

public class LogUser
{
    [Required(ErrorMessage = "You must provide an email")]
    [EmailAddress]
    public string LogEmail {get;set;}

    [Required(ErrorMessage = "You must provide a password")]
    [DataType(DataType.Password)]
    public string LogPassword {get;set;}
}