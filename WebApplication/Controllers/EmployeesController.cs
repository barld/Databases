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
    public class EmployeesController : Controller
    {
        private WebApplicationContext db = new WebApplicationContext();
        Context context = new Context();

        // GET: Employees
        public ActionResult Index()
        {
            return View(context.Employees.GetAll());
        }

        // GET: Employees/Details/5
        [Route("Employees/Details/{BSN}")]
        public ActionResult Details(int? BSN)
        {
            if (BSN == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // is always safe because there is a null check
            // it is just to trick te compiler
            int bSN = BSN ?? 0;
            Employee employee = context.Employees.FindByBSN(bSN);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEmployee cEmployee)
        {
            if (ModelState.IsValid)
            {
                var employee = cEmployee.toEmployee();

                context.Employees.Add(employee);
                return RedirectToAction("Index");
            }
            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));
            return View(cEmployee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee =  new CreateEmployee(context.Employees.FindByBSN(id ?? 0));
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateEmployee employee)
        {
            if (ModelState.IsValid)
            {
                context.Employees.Update(employee.toEmployee());
                return RedirectToAction("Index");
            }
            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? BSN)
        {
            if (BSN == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = context.Employees.FindByBSN(BSN ?? 0);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int BSN)
        {
            context.Employees.DeleteByBSN(BSN);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
