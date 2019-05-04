using projectManagementSys.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.UI.WebControls;

namespace projectManagementSys.Controllers
{
    public class AdminController : Controller
    {
        projectMSysEntities db = new projectMSysEntities();

        user user;
        project project;
       // Request request;
        public ActionResult Index()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 1))
            {
                return View(db.users.Find(Session["ID"]));
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // GET: users/Details/
        public ActionResult Detailsuser()
        {

            if ((Convert.ToInt32(Session["usertype"]) == 1))
            {
                return View(db.users.ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // GET: Admin/Create
        public ActionResult Adduser()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 1))
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Adduser(user user)
        {

            try
            {
                if (user.File != null)
                {
                    byte[] data = new byte[user.File.ContentLength];

                    user.File.InputStream.Read(data, 0, user.File.ContentLength);

                    user.image = data;

                }

                if (ModelState.IsValid)
                {
                    if (!db.users.Any(model => model.Email == user.Email))
                    {

                        db.users.Add(user);

                        db.SaveChanges();

                        return RedirectToAction("Detailsuser");
                    }

                    ViewBag.Message = "User With This Email Already Exist";

                    return View(user);
                }

                return View(user);
            }
            catch
            {
                return View(user);
            }
        }
        // GET: Admin/Edit/5
        public ActionResult Edituser(int Id=0)
        {
            if (Id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 1))
                {
                    user = db.users.Find(Id);

                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);
                }
               else
                {
                    return RedirectToAction("../Home/index");
                }

            }
            else
            {
                return RedirectToAction("../Admin/index");
            }
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edituser(user user)
        {
            try
            {
                if (user.File != null)
                {
                    byte[] data = new byte[user.File.ContentLength];

                    user.File.InputStream.Read(data, 0, user.File.ContentLength);

                    user.image = data;

                }

                if (ModelState.IsValid)
                {


                    db.Entry(user).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("AllUser");
                }
                return View(user);

            }
            catch
            {
                return View(user);
            }
        }            
        // GET: Admin/Delete/5
        public ActionResult Deleteuser(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 1))
                {
                    user = db.users.Find(id);

                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);
                }
                else
                {
                    return RedirectToAction("../Admin/index");
                }
            }
            else
            {
                return RedirectToAction("../Admin/index");
            }
        }

        [HttpPost, ActionName("DeleteUser")]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                user = db.users.Find(id);

                db.users.Remove(user);

                ViewBag.Error = "ImageDeleted";

                db.SaveChanges();

                return RedirectToAction("Detailsuser");
            }

            catch
            {
                return View();
            }
        }
        public ActionResult AdminProfile()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 1))
            {
                int UserID = Convert.ToInt32(Session["ID"]);

                user = db.users.Find(UserID);

                return View(user);
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }
        [HttpPost]
        public ActionResult AdminProfile(user user)
        {
            try
            {
                if (user.File != null)
                {
                    byte[] data = new byte[user.File.ContentLength];

                    user.File.InputStream.Read(data, 0, user.File.ContentLength);

                    user.image = data;

                }

                if (ModelState.IsValid)
                {

                    user.user_Type = 1;

                    db.Entry(user).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View(user);

            }
            catch
            {
                return View(user);
            }
        }
        public ActionResult Addproject()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 1))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Addproject(project project)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["ID"]);
                var writer = db.users.Where(model => model.Id == id).FirstOrDefault();
                project.user_id = writer.Id;
                project.Post_Status = 0;
                db.projects.Add(project);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            return View();
 
        }



        public ActionResult Editproject(int Id = 0)
        {
            if (Id != 0)
            {
                // if ((Convert.ToInt32(Session["projectType"]) == 1))
                // {
                project = db.projects.Find(Id);

             //   if (project == null)
              //  {
               //return HttpNotFound();
               // }
                return View(project);
            }
            else
            {
                return RedirectToAction("../Admin/index");
            }
            /*
            }
            else
            {
                return RedirectToAction("../Admin/index");
            } */
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Editproject(project project)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(project).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Detailsproject");
                }
                return View(project);

            }
            catch
            {
                return View(project);
            }
        }

        public ActionResult Deleteproject(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 1))
                {
                    project = db.projects.Find(id);

                    if (project == null)
                    {
                        return HttpNotFound();
                    }
                    return View(project);
                }
                else
                {
                    return RedirectToAction("../Home/Index");
                }
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }

        }

        [HttpPost, ActionName("Deleteproject")]
        public ActionResult DeleteProject(int id)
        {
            try
            {
                project = db.projects.Find(id);

                db.projects.Remove(project);

                ViewBag.Error = "ImageDeleted";

                db.SaveChanges();

                return RedirectToAction("Detailsproject");
            }

            catch
            {
                return View();
            }
        }
        public ActionResult Detailsproject()
        {

            if ((Convert.ToInt32(Session["usertype"]) == 1))
            {
            //    var allprojects = db.projects.ToList();
                return View(db.projects.ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }


    }
}
