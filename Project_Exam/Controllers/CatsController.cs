using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Exam.Models;
using Project_Exam.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Exam.Controllers
{

    public class CatsController : Controller
    {
        readonly CatDbContext db;
        readonly IWebHostEnvironment env;
        public CatsController(CatDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View(db.Cats.Include(x => x.Breed).ToList());
        }


        public IActionResult Create()
        {
            ViewBag.Breeds = db.Breeds.ToList();
            return View();
        }

        [HttpPost]

        public IActionResult Create(CatViewModel cr)
        {

            if (ModelState.IsValid)
            {
                Cat c = new Cat
                {
                    CatName = cr.CatName,
                    Dob = cr.Dob,
                    Gender = cr.Gender,
                    Available = cr.Available,
                    BreedId = cr.BreedId,
                    Picture = "1.jpg"
                };

                if (cr.Picture != null && cr.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    string filename = Guid.NewGuid() + Path.GetExtension(cr.Picture.FileName);
                    string filepath = Path.Combine(dir, filename);
                    FileStream fs = new FileStream(filepath, FileMode.Create);

                    cr.Picture.CopyTo(fs);
                    fs.Flush();
                    fs.Close();

                    c.Picture = filename;
                }


                db.Cats.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Breeds = db.Breeds.ToList();
            return View(cr);
        }


        public ActionResult Edit(int id)
        {

            var data = db.Cats.First(x => x.CatId == id);

            ViewBag.Breeds = db.Breeds.ToList();
            ViewBag.CurrentPic = data.Picture;
            return View(new CatViewModel
            {
                CatId = data.CatId,
                CatName = data.CatName,
                Dob = data.Dob,
                Gender = data.Gender,
                Available = data.Available,
                BreedId = data.BreedId
            });
        }
        [HttpPost]
        public IActionResult Edit(CatViewModel cr)
        {
            Cat c = db.Cats.First(x => x.CatId == cr.CatId);

            if (ModelState.IsValid)
            {

                c.CatName = cr.CatName;
                c.Dob = cr.Dob;
                c.Gender = cr.Gender;
                c.Available = cr.Available;
                c.BreedId = cr.BreedId;

                if (cr.Picture != null && cr.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    string filename = Guid.NewGuid() + Path.GetExtension(cr.Picture.FileName);
                    string filepath = Path.Combine(dir, filename);
                    FileStream fs = new FileStream(filepath, FileMode.Create);

                    cr.Picture.CopyTo(fs);
                    fs.Flush();
                    fs.Close();

                    c.Picture = filename;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Breeds = db.Breeds.ToList();
            ViewBag.CurrentPic = c.Picture;
            return View(cr);
        }

        public ActionResult Delete(int id)
        {
            Cat c = db.Cats.Include(x => x.Breed).First(x => x.CatId == id);
            ViewBag.Breeds = db.Breeds.ToList();
            ViewBag.CurrentPic = c.Picture;
            return View(c);
        }

        [HttpPost, ActionName("Delete")]

        public ActionResult ConfirmDelete(int id)
        {
            Cat c = new Cat { CatId = id };
            db.Entry(c).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
