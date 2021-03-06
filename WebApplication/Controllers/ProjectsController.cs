﻿using System;
using System.Collections.Generic;
using System.Data;
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
    public class ProjectsController : Controller
    {
        private IContext context = new Context();

        // GET: Projects
        public ActionResult Index()
        {
            return View(context.Projects.GetAll());
        }

        public ActionResult CanNotPay()
        {
            return View("Index", context.Projects.GetAll().Where(p => !p.CanPayRent));
        }


        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = context.Projects.FindByProjectId(id ?? 0);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                context.Projects.Add(project);
                return RedirectToAction("Index");
            }

            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = context.Projects.FindByProjectId(id??0);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                context.Projects.Update(project);
                return RedirectToAction("Index");
            }
            ViewBag.HeadQuaterList = new SelectList(context.HeadQuaters.GetAll().Select(hq => hq.BuildingName));
            return View(project);
        }


        public ActionResult Manage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = context.Projects.FindByProjectId(id ?? 0);
            if (project == null)
            {
                return HttpNotFound();
            }
            var data = context.Projects.GetManageDataProject(id ?? 0);
            var pData = new ProjectManagment(data)
            {
                Budget = project.Budget,
                BuildingName = project.BuildingName,
                Hours = project.Hours,
                Name = project.Name,
                ProjectID = project.ProjectID
            };
            return View(pData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ProjectManagment pm)
        {
            if (ModelState.IsValid)
            {
                context.Projects.UpdateState(pm);
                return RedirectToAction("Index");
            }
            return Manage(pm.ProjectID);
        }


        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = context.Projects.FindByProjectId(id ?? 0);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            context.Projects.DeleteById(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
