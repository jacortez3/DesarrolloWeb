﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaMVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ClinicaMVC.Filtros;

namespace ClinicaMVC.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrador")]
    [FiltroAutorizacion]
    public class pacientesController : Controller
    {
        private ProyectoVeris_MVC_Entities db = new ProyectoVeris_MVC_Entities();

        // GET: pacientes
        public ActionResult Index()
        {
            var pacientes = db.pacientes.Include(p => p.AspNetUsers);
            return View(pacientes.ToList());
        }

        // GET: pacientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pacientes pacientes = db.pacientes.Find(id);
            if (pacientes == null)
            {
                return HttpNotFound();
            }
            return View(pacientes);
        }

        // GET: pacientes/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: pacientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPaciente,IdUsuario,Nombre,Cedula,Edad,Genero,Estatura,Peso,Foto")] pacientes pacientes)
        {
            if (ModelState.IsValid)
            {
                db.pacientes.Add(pacientes);
                db.SaveChanges();
                // Asignar el rol "Paciente" al usuario
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                userManager.AddToRole(pacientes.IdUsuario, "Paciente");
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", pacientes.IdUsuario);
            return View(pacientes);
        }

        // GET: pacientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pacientes pacientes = db.pacientes.Find(id);
            if (pacientes == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", pacientes.IdUsuario);
            return View(pacientes);
        }

        // POST: pacientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPaciente,IdUsuario,Nombre,Cedula,Edad,Genero,Estatura,Peso,Foto")] pacientes pacientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pacientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", pacientes.IdUsuario);
            return View(pacientes);
        }

        // GET: pacientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pacientes pacientes = db.pacientes.Find(id);
            if (pacientes == null)
            {
                return HttpNotFound();
            }
            return View(pacientes);
        }

        // POST: pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pacientes pacientes = db.pacientes.Find(id);
            db.pacientes.Remove(pacientes);
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
