using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeDataModels;
using WebApplication.Models;
using rdbm;

namespace WebApplication.Controllers
{
    public class HeadQuatersController : Controller
    {

        Context context = new Context();
       
        // GET: HeadQuaters
        public ActionResult Index()
        {
            return View(context.HeadQuaters.GetAll());
        }

        // GET: HeadQuaters/Details/5
        [Route("HeadQuaters/Details/{buildingName}")]
        public ActionResult Details(string buildingName)
        {
            if (buildingName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadQuater headQuater = context.HeadQuaters.FindByBuildingname(buildingName);
            if (headQuater == null)
            {
                return HttpNotFound();
            }
            return View(headQuater);
        }

        // GET: HeadQuaters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HeadQuaters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateHeadQuarters cHeadquarter)
        {
            if (ModelState.IsValid)
            {
                var Headquarter = cHeadquarter.ToHeadquarter();
                context.HeadQuaters.Add(Headquarter);
                return RedirectToAction("Index");
            }

            return View(cHeadquarter);
        }

        // GET: HeadQuaters/Edit/5
        [Route("HeadQuaters/Edit/{buildingName}")]
        public ActionResult Edit(string buildingName)
        {
            if (buildingName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var headquarter = new CreateHeadQuarters(context.HeadQuaters.FindByBuildingname(buildingName));
            if (headquarter == null)
            {
                return HttpNotFound();
            }
            return View(headquarter);
        }

        // POST: HeadQuaters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateHeadQuarters cHeadquarters)
        {
            if (ModelState.IsValid)
            {
                context.HeadQuaters.Update(cHeadquarters.ToHeadquarter());
                return RedirectToAction("Index");
            }
            return View(cHeadquarters);
        }

        // GET: HeadQuaters/Delete/5
        public ActionResult Delete(string buildingName)
        {
            if (buildingName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadQuater headQuater = context.HeadQuaters.FindByBuildingname(buildingName);
            if (headQuater == null)
            {
                return HttpNotFound();
            }
            return View(headQuater);
        }

        // POST: HeadQuaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string buildingName)
        {
            context.HeadQuaters.DeleteByBuildingname(buildingName);
      
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
