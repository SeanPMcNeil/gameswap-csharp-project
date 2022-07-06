#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace gameSwapCSharp.Models;
public class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Swap> Swaps { get; set; }
}