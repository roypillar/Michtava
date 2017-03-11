using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Models;
using FileHandler;

namespace Frontend.Controllers
{
    public class TextViewController : Controller
    {

        // GET: TextView
        public ActionResult Index()
        {
            ViewBag.Title = "";

            return View();
        }

        // GET: TextView/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TextView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextView/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TextView/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TextView/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TextView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TextView/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
