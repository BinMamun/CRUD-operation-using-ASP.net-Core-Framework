using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Exam.Controllers
{
    public class BreedsController : Controller
    {

        readonly CatDbContext db;
        public BreedsController(CatDbContext db) { this.db = db; }
        public IActionResult Index()
        {
            return View(db.Breeds.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Breed b)
        {
            if (ModelState.IsValid)
            {
                db.Breeds.Add(b);
                db.SaveChanges();
                return PartialView("_CreatePartial", true);
            }
            return PartialView("_CreatePartial", false);
        }

        public ActionResult Edit(int id)
        {
            return View(db.Breeds.First(x => x.BreedId == id));
        }

        [HttpPost]

        public ActionResult Edit(Breed b)
        {
            if (ModelState.IsValid)
            {
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(b);
        }


        public ActionResult Delete(int id)
        {
            return View(db.Breeds.First(x => x.BreedId == id));
        }

        [HttpPost, ActionName("Delete")]

        public ActionResult ConfirmDelete(int id)
        {
            Breed b = new Breed { BreedId = id };

            if (!db.Cats.Any(x => x.BreedId == id))
            {
                db.Entry(b).State = EntityState.Deleted;
                db.SaveChanges();
                return PartialView("_DeleteMgs", true);
            }
            return PartialView("_DeleteMgs", false);

        }
    }
}
