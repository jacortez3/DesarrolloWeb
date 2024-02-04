using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaMVC.Models;
using ClinicaMVC.Filtros;

namespace ClinicaMVC.Controllers
{
    [Authorize(Roles = "Medico, SuperAdmin")]
    [FiltroAutorizacion]
    public class recetasController : Controller
    {
        private ProyectoVeris_MVC_Entities db = new ProyectoVeris_MVC_Entities();

        // GET: recetas
        public ActionResult Index()
        {
            var recetas = db.recetas.Include(r => r.consultas).Include(r => r.medicamentos);
            return View(recetas.ToList());
        }

        // GET: recetas/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recetas recetas = db.recetas.Find(id);
            if (recetas == null)
            {
                return HttpNotFound();
            }
            return View(recetas);
        }

        // GET: recetas/Create

        public ActionResult Create()
        {
            ViewBag.IdConsulta = new SelectList(db.consultas, "IdConsulta", "Diagnostico");
            ViewBag.IdMedicamento = new SelectList(db.medicamentos, "IdMedicamento", "Nombre");
            return View();
        }

        // POST: recetas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "IdReceta,IdConsulta,IdMedicamento,Cantidad")] recetas recetas)
        {
            if (ModelState.IsValid)
            {
                db.recetas.Add(recetas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdConsulta = new SelectList(db.consultas, "IdConsulta", "Diagnostico", recetas.IdConsulta);
            ViewBag.IdMedicamento = new SelectList(db.medicamentos, "IdMedicamento", "Nombre", recetas.IdMedicamento);
            return View(recetas);
        }

        // GET: recetas/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recetas recetas = db.recetas.Find(id);
            if (recetas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdConsulta = new SelectList(db.consultas, "IdConsulta", "Diagnostico", recetas.IdConsulta);
            ViewBag.IdMedicamento = new SelectList(db.medicamentos, "IdMedicamento", "Nombre", recetas.IdMedicamento);
            return View(recetas);
        }

        // POST: recetas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "IdReceta,IdConsulta,IdMedicamento,Cantidad")] recetas recetas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recetas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdConsulta = new SelectList(db.consultas, "IdConsulta", "Diagnostico", recetas.IdConsulta);
            ViewBag.IdMedicamento = new SelectList(db.medicamentos, "IdMedicamento", "Nombre", recetas.IdMedicamento);
            return View(recetas);
        }

        // GET: recetas/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recetas recetas = db.recetas.Find(id);
            if (recetas == null)
            {
                return HttpNotFound();
            }
            return View(recetas);
        }

        // POST: recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            recetas recetas = db.recetas.Find(id);
            db.recetas.Remove(recetas);
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
    }
}
