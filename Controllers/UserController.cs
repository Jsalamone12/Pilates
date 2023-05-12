using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pilates.Models;
using Microsoft.AspNetCore.Identity;

namespace Pilates.Controllers;

public class UserController : Controller
{
    private MyContext db;

    public UserController(MyContext context)
    {

        db = context;
    }

[HttpGet("")]
public IActionResult Index()
{
    if (HttpContext.Session.GetInt32("UUID") != null)
        {
            return RedirectToAction("Move", "Move");
        }
    return View("Index");
}


    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        PasswordHasher<User> hashed = new PasswordHasher<User>();
        newUser.Password = hashed.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);

        return RedirectToAction("Move", "Move");
    }

    [HttpPost("/login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if (!ModelState.IsValid)
        {

        }

        User? userInDb = db.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);

        if (userInDb == null)
        {

            ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
            return View("Index");
        }

        PasswordHasher<LoginUser> hashed = new PasswordHasher<LoginUser>();

        var result = hashed.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);                                    // Result can be compared to 0 for failure        
        if (result == 0)
        {
            ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
            return View("Index");
        }

        HttpContext.Session.SetInt32("UUID", userInDb.UserId);
        HttpContext.Session.SetString("Name", userInDb.Name);

        return RedirectToAction("Move", "Move");
    }

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "User");
    }

}