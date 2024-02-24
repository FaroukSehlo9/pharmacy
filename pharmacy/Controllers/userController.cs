using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmacy.Models;

namespace pharmacy.Controllers
{
    public class userController : Controller
    {
        pharmacyContext db;
        public userController(pharmacyContext db)
        {
            this.db = db;
        }

        //action for sign up ======================================================================
        public ActionResult sign_up()
        {
            return View();
        }
        [HttpPost]
        public ActionResult sign_up(user s, IFormFile img)
        {
            //uplod file
            string path = $"wwwroot/profile_photo/{img.FileName}";
            FileStream pp = new FileStream(path, FileMode.Create);
            img.CopyTo(pp);

            //save in db
            s.img = $"/profile_photo/{img.FileName}";
            db.users.Add(s);
            db.SaveChanges();

            return RedirectToAction("sign_in");
        }

        //action for sign in ======================================================================
        public ActionResult sign_in()
        {
            return View();
        }

        [HttpPost]
        public ActionResult sign_in(string email, string password)
        {
            user ? s = db.users.Where(n => n.email == email && n.password == password).FirstOrDefault();
            if (s != null)
            {

                HttpContext.Session.SetInt32("id", s.id);

                return RedirectToAction("home", "medicine");
            }
            else
            {

                ViewBag.status = "incorrect Email or Password";
                return View();
            }
        }

        //action for sign out ======================================================================
        public ActionResult sign_out()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("sign_in");
        }

        //action for profile ======================================================================
        public ActionResult profile()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return RedirectToAction("sign_in");
            }
            user ? s = db.users.Where(n => n.id == id).FirstOrDefault();
            return View(s);
        }

        // action for edit profile ======================================================================
        public ActionResult editProfile(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {
                user ? s = db.users.Where(n => n.id == id).SingleOrDefault();

                return View(s);
            }
        }
        [HttpPost]
        public ActionResult editProfile(user s, IFormFile img)
        {
            db.Entry(s).State = EntityState.Modified;


            //save in db

            int? id = HttpContext.Session.GetInt32("id");

            db.SaveChanges();

            return RedirectToAction("profile");
        }

        //action for edit profile photo ======================================================================
        public ActionResult editphoto(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {
                user ? s = db.users.Where(n => n.id == id).SingleOrDefault();

                return View(s);
            }
        }
        [HttpPost]
        public ActionResult editphoto(user s, IFormFile img)
        {
            db.Entry(s).State = EntityState.Modified;
            //uplod file
            string path = $"wwwroot/profile_photo/{img.FileName}";
            FileStream pp = new FileStream(path, FileMode.Create);
            img.CopyTo(pp);

            //save in db
            s.img = $"/profile_photo/{img.FileName}";
            int ? id = HttpContext.Session.GetInt32("id");
            

            db.SaveChanges();
            return RedirectToAction("profile");
        }

    }
    }
