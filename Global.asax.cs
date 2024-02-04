using ClinicaMVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ClinicaMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationDbContext db = new ApplicationDbContext();

            //Crear los roles
            CrearSuperUsuario(db);
            CrearAdmin(db);
            CrearRoles(db);
            AsignarPermiso(db);
            db.Dispose();
        }

        private void CrearSuperUsuario(ApplicationDbContext db)
        {
            var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = usermanager.FindByName("SuperAdmin");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    Email = "superadmin@gmail.com",
                    PasswordHash = new PasswordHasher().HashPassword("123")
                };
                usermanager.Create(user);
            }
        }

        private void CrearAdmin(ApplicationDbContext db)
        {
            var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = usermanager.FindByName("ADM");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "ADM",
                    Email = "Admin@hotmail.com",
                    PasswordHash = new PasswordHasher().HashPassword("123")
                };
                usermanager.Create(user);
            }
        }

        private void CrearRoles(ApplicationDbContext db)
        {
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (!rolemanager.RoleExists("SuperAdmin"))
            {
                rolemanager.Create(new IdentityRole("SuperAdmin"));
            }
            if (!rolemanager.RoleExists("Administrador"))
            {
                rolemanager.Create(new IdentityRole("Administrador"));
            }
            if (!rolemanager.RoleExists("Medico"))
            {
                rolemanager.Create(new IdentityRole("Medico"));
            }
            if (!rolemanager.RoleExists("Paciente"))
            {
                rolemanager.Create(new IdentityRole("Paciente"));
            }
        }

        private void AsignarRolesAUsuario(UserManager<ApplicationUser> userManager, ApplicationUser user, List<string> roles)
        {
            foreach (var role in roles)
            {
                if (!userManager.IsInRole(user.Id, role))
                {
                    userManager.AddToRole(user.Id, role);
                }
            }
        }


        private void AsignarPermiso(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // SuperAdmin
            var superadmin = userManager.FindByName("SuperAdmin");
            if (superadmin != null)
            {
                AsignarRolesAUsuario(userManager, superadmin, new List<string> { "SuperAdmin" });
            }

            // Admin
            var admin = userManager.FindByName("ADM");
            if (admin != null)
            {
                AsignarRolesAUsuario(userManager, admin, new List<string> { "Administrador" });
            }

            // Medico
            var medico = userManager.FindByEmail("DrManotas@hotmail.com");
            if (medico != null)
            {
                AsignarRolesAUsuario(userManager, medico, new List<string> { "Medico" });
            }

            // Paciente
            var paciente = userManager.FindByEmail("plutarco@hotmail.com");
            if (paciente != null)
            {
                AsignarRolesAUsuario(userManager, paciente, new List<string> { "Paciente" });
            }

        }
    }
}
