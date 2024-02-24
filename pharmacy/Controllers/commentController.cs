using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmacy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
namespace pharmacy.Controllers
{
    public class commentController : Controller
    {

        pharmacyContext db;
        public commentController(pharmacyContext db)
        {
            this.db = db;
        }

        //action for show comment ======================================================================
        public IActionResult sh_comment(int id)
        {
            List<comment> c = db.comments.Where(n => n.med_id == id).Include(n => n.user).ToList();
            return View(c);
        }


        //action for Add comment ======================================================================

        public IActionResult add_comment(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {
                HttpContext.Session.SetInt32("med_id", id);

                return View();
            }
        }
        [HttpPost]
        public IActionResult add_comment(comment c)
        {
            c.user_id = HttpContext.Session.GetInt32("id");
            c.med_id = HttpContext.Session.GetInt32("med_id");
            c.time = DateTime.Now;
            db.comments.Add(c);
            db.SaveChanges();
            return RedirectToAction("sh_comment", "comment", new { id = c.med_id });
        }

    }
}
