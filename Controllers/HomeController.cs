using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using gameSwapCSharp.Models;

namespace gameSwapCSharp.Controllers;

public class HomeController : Controller
{
    private MyContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _config;
    public HomeController(ILogger<HomeController> logger, MyContext context, IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _config = config;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("useractions")]
    public IActionResult UserActions()
    {
        if (HttpContext.Session.GetInt32("user") != null)
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    [HttpPost("user/register")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            if(_context.Users.Any(u => u.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");
                return View("UserActions");
            }

            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("user", newUser.UserId);
            HttpContext.Session.SetString("username", newUser.Username);
            return RedirectToAction("Dashboard");
        }
        else
        {
            return View("UserActions");
        }
    }

    [HttpPost("user/login")]
    public IActionResult Login(LogUser loginUser)
    {
        if (ModelState.IsValid)
        {
            User userInDb = _context.Users.FirstOrDefault(u => u.Email == loginUser.LogEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("LogEmail", "Invalid Login Attempt");
                return View("UserActions");
            }
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);
            if(result == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid Login Attempt");
                return View("UserActions");
            }
            else
            {
                HttpContext.Session.SetInt32("user", userInDb.UserId);
                HttpContext.Session.SetString("username", userInDb.Username);
                return RedirectToAction("Dashboard");
            }
        }
        else
        {
            return View("UserActions");
        }
    }

    [HttpPost("user/addcoins")]
    public IActionResult AddCoins()
    {
        User loggedInUser = _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("user"));
        loggedInUser.Coins += 10;
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        ViewBag.LoggedUser = _context.Users.Include(g => g.OwnedGames).Include(s => s.OwnedSwaps).Include(b => b.BoughtSwaps).FirstOrDefault(a => a.UserId == (int)HttpContext.Session.GetInt32("user"));
        ViewBag.RelevantSwaps = _context.Swaps.Include(b => b.Buyer).Include(s => s.Seller).Include(g=>g.Game).Where(i => i.SellerId == HttpContext.Session.GetInt32("user") || i.BuyerId == HttpContext.Session.GetInt32("user"));
        ViewBag.gbApi = _config["GiantBomb:ApiKey"];
        return View();
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [HttpGet("browse")]
    public IActionResult Browse()
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        ViewBag.AllGames = _context.Games.Include(o => o.Owner).OrderBy(c => c.CreatedAt).ToList();
        ViewBag.AllSwaps = _context.Swaps.ToList();
        ViewBag.gbApi = _config["GiantBomb:ApiKey"];
        return View();
    }

    [HttpGet("game/new")]
    public IActionResult NewGame()
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        Console.WriteLine(HttpContext.Session.GetInt32("user"));
        return View();
    }

    [HttpPost("game/add")]
    public IActionResult AddGame(Game newGame)
    {
        newGame.UserId = (int)HttpContext.Session.GetInt32("user");
        if (ModelState.IsValid)
        {
            if (newGame.UserId != HttpContext.Session.GetInt32("user"))
            {
                ModelState.AddModelError("Title", "Invalid Listing Addition");
                return View("NewGame");
            }
            _context.Add(newGame);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        else
        {
            ModelState.AddModelError("Title", "Invalid Listing Addition");
            return View("NewGame");
        }
    }

    [HttpGet("game/{gameId}")]
    public IActionResult OneGame(int gameId)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        Game singleGame = _context.Games.Include(o => o.Owner).SingleOrDefault(g => g.GameId == gameId);
        ViewBag.gbApi = _config["GiantBomb:ApiKey"];
        return View(singleGame);
    }

    [HttpGet("game/edit/{gameId}")]
    public IActionResult EditGame(int gameId)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        Game singleGame = _context.Games.Include(o => o.Owner).SingleOrDefault(g => g.GameId == gameId);
        if (singleGame.UserId != HttpContext.Session.GetInt32("user"))
        {
            return RedirectToAction("Dashboard");
        }
        return View(singleGame);
    }

    [HttpPost("game/update/{gameId}")]
    public IActionResult UpdateGame(int gameId, Game updatedGame)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        Game oldGame = _context.Games.FirstOrDefault(g => g.GameId == gameId);
        if (oldGame.UserId != HttpContext.Session.GetInt32("user"))
        {
            return RedirectToAction("Dashboard");
        }
        if (ModelState.IsValid)
        {
            oldGame.Title = updatedGame.Title;
            oldGame.Platform = updatedGame.Platform;
            oldGame.Quality = updatedGame.Quality;
            oldGame.Price = updatedGame.Price;
            oldGame.UpdatedAt = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("OneGame", oldGame);
        }
        return View("EditGame", new{ gameId = updatedGame.GameId });
    }

    [HttpGet("game/delete/{gameId}")]
    public IActionResult DeleteGame(int gameId)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        Game gameToDelete = _context.Games.SingleOrDefault(g => g.GameId == gameId);
        if (gameToDelete.UserId != HttpContext.Session.GetInt32("user"))
        {
            return RedirectToAction("Dashboard");
        }
        _context.Remove(gameToDelete);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpGet("game/message/{gameId}")]
    public IActionResult Message(int gameId)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        ViewBag.SingleGame = _context.Games.Include(o => o.Owner).SingleOrDefault(g => g.GameId == gameId);
        return View();
    }

    [HttpPost("game/message/{gameId}/send")]
    public IActionResult SendMessage(int gameId, Message newMessage)
    {
        Game requestedGame = _context.Games.Include(o => o.Owner).FirstOrDefault(g => g.GameId == gameId);
        User buyer = _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("user"));
        if (buyer.Coins < newMessage.ProposedPrice)
        {
            ModelState.AddModelError("ProposedPrice", "Not Enough Coins, Please Obtain More");
            ViewBag.SingleGame = _context.Games.Include(o => o.Owner).SingleOrDefault(g => g.GameId == gameId);
            return View("Message");
        }
        newMessage.SenderId = buyer.UserId;
        newMessage.RecipientId = requestedGame.Owner.UserId;
        newMessage.GameId = requestedGame.GameId;
        if (ModelState.IsValid)
        {
            _context.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        else
        {
            ViewBag.SingleGame = _context.Games.Include(o => o.Owner).SingleOrDefault(g => g.GameId == gameId);
            return View("Message");
        }
    }

    [HttpGet("inbox")]
    public IActionResult Inbox()
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        ViewBag.Messages = _context.Messages.Include(r => r.Responses).Include(s => s.Sender).Include(r => r.Recipient).Where(i => i.RecipientId == HttpContext.Session.GetInt32("user") || i.SenderId == HttpContext.Session.GetInt32("user")).OrderByDescending(c => c.CreatedAt).ToList();
        List<Game> MessagedGames = new List<Game>();
        foreach (Message m in ViewBag.Messages)
        {
            Game oneGame = _context.Games.FirstOrDefault(g => g.GameId == m.GameId);
            MessagedGames.Add(oneGame);
        }
        ViewBag.RelevantGames = MessagedGames;
        return View();
    }

    [HttpGet("message/{messageId}")]
    public IActionResult OneMessage(int messageId)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        Message oneMessage = _context.Messages.Include(s => s.Sender).Include(r => r.Recipient).Include(r => r.Responses).FirstOrDefault(m => m.MessageId == messageId);
        ViewBag.oneGame = _context.Games.FirstOrDefault(g => g.GameId == oneMessage.GameId);
        List<Swap> allSwaps = _context.Swaps.Where(s => s.SellerId == oneMessage.RecipientId && s.BuyerId == oneMessage.SenderId && s.GameId == oneMessage.GameId).ToList();
        if (allSwaps.Count < 1)
        {
            ViewBag.AlreadyTraded = 0;
        }
        else
        {
            foreach (Swap s in allSwaps)
            {
                if (s.SellerId == oneMessage.RecipientId && s.BuyerId == oneMessage.SenderId && s.GameId == ViewBag.oneGame.GameId && s.GameReceived == 1)
                {
                    ViewBag.AlreadyTraded = 1;
                    break;
                }
                else if (s.SellerId == oneMessage.RecipientId && s.BuyerId == oneMessage.SenderId && s.GameId == ViewBag.oneGame.GameId && s.GameReceived == 2)
                {
                    ViewBag.AlreadyTraded = 2;
                    break;
                }
                else
                {
                    ViewBag.AlreadyTraded = 0;
                    break;
                }
            }
        }
        return View(oneMessage);
    }

    [HttpGet("message/{messageId}/reply")]
    public IActionResult WriteReply(int messageId)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        ViewBag.ParentMessage = _context.Messages.Include(s => s.Sender).FirstOrDefault(m => m.MessageId == messageId);
        return View();
    }

    [HttpPost("message/{messageId}/reply/send")]
    public IActionResult SendReply(int messageId, Response newResponse)
    {
        newResponse.MessageId = messageId;
        newResponse.UserId = (int)HttpContext.Session.GetInt32("user");
        if (ModelState.IsValid)
        {
            _context.Add(newResponse);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        else
        {
            ViewBag.ParentMessage = _context.Messages.Include(s => s.Sender).FirstOrDefault(m => m.MessageId == messageId);
            return View("WriteReply");
        }
    }

    [HttpGet("swap/{buyerId}/{sellerId}/{gameId}/{finalPrice}")]
    public IActionResult SwapInit (int buyerId, int sellerId, int gameId, int finalPrice)
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("UserActions");
        }
        Swap tempSwap = new Swap();
        tempSwap.BuyerId = buyerId;
        tempSwap.Buyer = _context.Users.FirstOrDefault(u => u.UserId == buyerId);
        tempSwap.SellerId = sellerId;
        tempSwap.Seller = _context.Users.FirstOrDefault(u => u.UserId == sellerId);
        tempSwap.GameId = gameId;
        tempSwap.Game = _context.Games.FirstOrDefault(g => g.GameId == gameId);
        tempSwap.FinalPrice = finalPrice;
        tempSwap.TrackingInfo = "";
        return View(tempSwap);
    }

    [HttpPost("swap/initialize")]
    public IActionResult AddSwap(Swap newSwap)
    {
        if (ModelState.IsValid)
        {
            User buyer = _context.Users.FirstOrDefault(u => u.UserId == newSwap.BuyerId);
            if (buyer.Coins < newSwap.FinalPrice)
            {
                ModelState.AddModelError("TrackingInfo", "Buyer does not have enough coins for swap");
                newSwap.Buyer = _context.Users.FirstOrDefault(u => u.UserId == newSwap.BuyerId);
                newSwap.Seller = _context.Users.FirstOrDefault(u => u.UserId == newSwap.SellerId);
                newSwap.Game = _context.Games.FirstOrDefault(g => g.GameId == newSwap.GameId);
                return View("SwapInit", newSwap);
            }
            newSwap.GameReceived = 2;
            buyer.Coins -= newSwap.FinalPrice;
            _context.Add(newSwap);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        else
        {
            newSwap.Buyer = _context.Users.FirstOrDefault(u => u.UserId == newSwap.BuyerId);
            newSwap.Seller = _context.Users.FirstOrDefault(u => u.UserId == newSwap.SellerId);
            newSwap.Game = _context.Games.FirstOrDefault(g => g.GameId == newSwap.GameId);
            return View("SwapInit", newSwap);
        }
    }

    [HttpGet("swap/view/{swapId}")]
    public IActionResult OneSwap(int swapId)
    {
        Swap oneSwap = _context.Swaps.Include(s => s.Seller).Include(b => b.Buyer).Include(g => g.Game).FirstOrDefault(s => s.SwapId == swapId);
        return View(oneSwap);
    }

    [HttpPost("swap/finalize/{swapId}")]
    public IActionResult EndSwap(int swapId)
    {
        Swap swapToFinalize = _context.Swaps.SingleOrDefault(s => s.SwapId == swapId);
        User seller = _context.Users.FirstOrDefault(u => u.UserId == swapToFinalize.SellerId);
        seller.Coins += swapToFinalize.FinalPrice;
        swapToFinalize.GameReceived = 1;
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

