using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pilates.Models;
using Microsoft.EntityFrameworkCore;

namespace Pilates.Controllers;

public class MoveController : Controller
{
    private MyContext db;
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public MoveController(MyContext context)
    {

        db = context;
    }

    [HttpGet("/moves")]
    public IActionResult Move()
    {
        int? userId = HttpContext.Session.GetInt32("UUID");
        if (userId == null)
        {
            return RedirectToAction("Index");
        }


        User? userInDb = db.Users.FirstOrDefault(u => u.UserId == userId);
        if (userInDb == null)
        {
            return RedirectToAction("Move", "Move");
        }
                ViewModel viewModel = new ViewModel
        {
            User = userInDb,
            AllUsers = db.Users.ToList(),

    AllMoves = db.Move.ToList()
    };         
            // AllMoves = db.Move.ToList(),
            // Move = new Move(),

        return View("~/Views/move/move.cshtml", viewModel);
    }

        [HttpGet("/moves/new")]
    public IActionResult NewMove()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        User user = db.Users.FirstOrDefault(u => u.UserId == userId);

        // ViewBag.UserId = userId;

        return View("AddMove", new Pilates.Models.Move());
    }

    [HttpPost("/moves/create")]
    public IActionResult CreateMove(Move newMove)
    {
        int userId = (int)HttpContext.Session.GetInt32("UUID");
        // if (userId == null)
        // {
        //     return RedirectToAction("Move", "Move");
        // }

        newMove.UserId = userId;
        if (ModelState.IsValid)
        {
            db.Add(newMove);
            db.SaveChanges();
            return RedirectToAction("Move", "Move");
        }
        else
        {
            return View("AddMove");
        }
}
    [HttpGet("/moves/{MoveId}")]
    public IActionResult OneMove(int MoveId)
    {
        ViewModel viewModel = new ViewModel
        {

            Move = db.Move.FirstOrDefault(a => a.MoveId == MoveId),
            AllMoves = db.Move.ToList()
        };
        return View("OneMove", viewModel);
    }

        [HttpPost("/moves/{moveId}/delete")]
    public IActionResult Delete(int moveId)
    {
        Move? move = db.Move.FirstOrDefault(move => move.MoveId == moveId);

        if(move != null)
        {
            db.Move.Remove(move);
            db.SaveChanges();
        }

        return RedirectToAction("Move", "Move");
    }

    [HttpGet("/moves/edit/{moveId}")]
    public IActionResult Edit(int moveId)
    {        
        Move? move = db.Move.FirstOrDefault(move => move.MoveId == moveId);

        if (move == null || move.UserId != HttpContext.Session.GetInt32("UUID"))
        {
            return RedirectToAction("Move", "Move");
        }

        return View("~/Views/move/Edit.cshtml", move);
    }

    [HttpPost("/moves/{moveId}/update")]
    public IActionResult Update(Move editedMove, int moveId)
    {
        if (!ModelState.IsValid)
        {
            return Edit(moveId);
        }

        Move? dbMove = db.Move.FirstOrDefault(move => move.MoveId == moveId);

        if (dbMove == null)
        {
            return RedirectToAction("Move", "Move");
        }

        dbMove.Title = editedMove.Title;
        dbMove.Image = editedMove.Image;

        dbMove.Bool = editedMove.Bool;

        dbMove.UpdatedAt = DateTime.Now;

        db.Move.Update(dbMove);
        db.SaveChanges();

        return RedirectToAction("OneMove", new { moveId = moveId });
    }

}