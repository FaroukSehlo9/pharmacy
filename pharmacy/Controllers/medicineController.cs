using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmacy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace pharmacy.Controllers
{
    public class medicineController : Controller
    {
        pharmacyContext db;
        public medicineController(pharmacyContext db)
        {
            this.db = db;
        }

        //action for home (catalog) ======================================================================
        public IActionResult home()
        {
            List<catalog> c = db.catalogs.ToList();

            return View(c);
        }

        //action for show medecine in one catalog ======================================================================

        public IActionResult med_in_cat(int id)
        {
            List<medicine> m = db.medicines.Where(n => n.cat_id == id).Include(n => n.cat).Include(n => n.user).OrderByDescending(n => n.med_time).ToList();
            return View(m);
        }

        //action for show all medecine ======================================================================
        public IActionResult displaymed()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {
                List<medicine> m = db.medicines.Include(n => n.cat).Include(n => n.user).OrderByDescending(n => n.med_time).ToList();
                return View(m);
            }
        }


        //action for show user's medecine ======================================================================
        public IActionResult display_my_med()
        {

            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {
                int? id = HttpContext.Session.GetInt32("id");
                List<medicine> m = db.medicines.Include(n => n.cat).Include(n => n.user).Where(n => n.user_id == id).OrderByDescending(n => n.med_time).ToList();
                return View(m);
            }
        }


        //action for add new medecine ======================================================================
        public IActionResult create()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {

                //catalog
                List<catalog> ct = db.catalogs.ToList();
                ViewBag.cat = new SelectList(ct, "cat_id", "cat_name");
                return View();
            }
        }

        [HttpPost]
        public IActionResult create(medicine m, IFormFile img)
        {


            //img
            string path = $"wwwroot/medicin_photo/{img.FileName}";
            FileStream mp = new FileStream(path, FileMode.Create);
            img.CopyTo(mp);

            //save in db
            m.med_img = $"/medicin_photo/{img.FileName}";

            m.user_id = HttpContext.Session.GetInt32("id");
            m.med_time = DateTime.Now;

            db.medicines.Add(m);
            db.SaveChanges();
            return RedirectToAction("displaymed");
        }


        //action for edit medecine ======================================================================
        public IActionResult editmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {
                List<catalog> ct = db.catalogs.ToList();
                ViewBag.cat = new SelectList(ct, "cat_id", "cat_name");
                medicine? m = db.medicines.Where(n => n.med_id == id).SingleOrDefault();
                HttpContext.Session.SetInt32("med_id", id);
                return View(m);
            }
        }
        [HttpPost]
        public IActionResult editmed(medicine m, IFormFile img)
        {
           

            //img
            string path = $"wwwroot/medicin_photo/{img.FileName}";
            FileStream mp = new FileStream(path, FileMode.Create);
            img.CopyTo(mp);

            //save in db
            m.med_img = $"/medicin_photo/{img.FileName}";

            m.user_id = HttpContext.Session.GetInt32("id");
            m.med_time = DateTime.Now;
            int? id = HttpContext.Session.GetInt32("id");

            m.med_id  =(int)HttpContext.Session.GetInt32("med_id");   
             db.Entry(m).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("display_my_med");
        }



        //action for delete medecine ======================================================================

        public IActionResult deletemed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("sign_in", "user");
            }
            else
            {
                HttpContext.Session.SetInt32("med_id", id);

                medicine ? m = db.medicines.Where(n => n.med_id == id).SingleOrDefault();
                db.medicines.Remove(m);
                comment? c = db.comments.Where(n => n.med_id == id).FirstOrDefault();
                db.comments.Remove(c);
                db.SaveChanges();
                return RedirectToAction("display_my_med");
            }

        }




    }
}
