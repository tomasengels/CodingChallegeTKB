using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodingChallegeTKB.Models;

namespace CodingChallegeTKB.Controllers
{
    public class DebtorsController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: Debtors
        public ActionResult Index()
        {
            return View(db.Debtors.ToList());
        }

        // GET: Debtors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debtor debtor = db.Debtors.Find(id);
            if (debtor == null)
            {
                return HttpNotFound();
            }
            return View(debtor);
        }

        // GET: Debtors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Debtors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Number,Name,Telephone,Mobile,Email,IsClosed")] Debtor debtor)
        {
            if (ModelState.IsValid)
            {
                db.Debtors.Add(debtor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(debtor);
        }

        // GET: Debtors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debtor debtor = db.Debtors.Find(id);
            if (debtor == null)
            {
                return HttpNotFound();
            }
            return View(debtor);
        }

        // POST: Debtors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Number,Name,Telephone,Mobile,Email,IsClosed")] Debtor debtor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(debtor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(debtor);
        }

        // GET: Debtors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debtor debtor = db.Debtors.Find(id);
            if (debtor == null)
            {
                return HttpNotFound();
            }
            return View(debtor);
        }

        // POST: Debtors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Debtor debtor = db.Debtors.Find(id);
            db.Debtors.Remove(debtor);
            db.SaveChanges();
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

        public void ExportXML()
        {
            var data = db.Debtors.ToList();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename = export.xml");
            Response.ContentType = "text/xml";

            var serializer = new System.Xml.Serialization.XmlSerializer(data.GetType());
            serializer.Serialize(Response.OutputStream, data);
        }
    }
}
