using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class TextAddingController : Controller
    {
        // GET: TextAdding
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SubmitText()
        {
            HttpPostedFileBase myfile = Request.Files[0];

            if (myfile != null && myfile.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(myfile.FileName);
                
                if (fileName != null)
                {
                    var path = Path.Combine(Server.MapPath("Desktop"), fileName);
                    myfile.SaveAs(path);
                }
            }

            return View();
        }

        // GET: TextAdding/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TextAdding/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextAdding/Create
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

        // GET: TextAdding/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TextAdding/Edit/5
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

        // GET: TextAdding/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TextAdding/Delete/5
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
