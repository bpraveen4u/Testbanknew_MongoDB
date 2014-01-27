using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestBank.Entity.Model;
using TestBank.Web.Models;

namespace TestBank.Web.Controllers
{   
    public class OptionsController : Controller
    {
        private TestBankWebContext context = new TestBankWebContext();

        //
        // GET: /Options/

        public ViewResult Index()
        {
            return View(context.Options.Include(option => option.Options).ToList());
        }

        //
        // GET: /Options/Details/5

        public ViewResult Details(string id)
        {
            Option option = context.Options.Single(x => x.Id == id);
            return View(option);
        }

        //
        // GET: /Options/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Options/Create

        [HttpPost]
        public ActionResult Create(Option option)
        {
            if (ModelState.IsValid)
            {
                context.Options.Add(option);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(option);
        }
        
        //
        // GET: /Options/Edit/5
 
        public ActionResult Edit(string id)
        {
            Option option = context.Options.Single(x => x.Id == id);
            return View(option);
        }

        //
        // POST: /Options/Edit/5

        [HttpPost]
        public ActionResult Edit(Option option)
        {
            if (ModelState.IsValid)
            {
                context.Entry(option).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(option);
        }

        //
        // GET: /Options/Delete/5
 
        public ActionResult Delete(string id)
        {
            Option option = context.Options.Single(x => x.Id == id);
            return View(option);
        }

        //
        // POST: /Options/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Option option = context.Options.Single(x => x.Id == id);
            context.Options.Remove(option);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}