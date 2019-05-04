using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectManagementSys.Models;

namespace projectManagementSys.Controllers
{
    public class HomeController : Controller
    {
        projectMSysEntities db = new projectMSysEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {

            if (Session["UserType"] != null)
            {
                if (Session["UserType"].Equals(1))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (Session["UserType"].Equals(2))
                {
                    return RedirectToAction("Index", "Customer");
                }
                else if (Session["UserType"].Equals(3))
                {
                    return RedirectToAction("Index", "ProjectManager");
                }
                else if (Session["UserType"].Equals(4))
                {
                    return RedirectToAction("Index", "TeamLeader");
                }
                else if (Session["UserType"].Equals(5))
                {
                    return RedirectToAction("Index", "JuniorDevelopment");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(user user)
        {
            db = new projectMSysEntities();
            var validation = db.users.Where(model => model.Email == user.Email && model.password == user.password).FirstOrDefault();
            if (validation != null)
            {
                Session["ID"] = validation.Id;
                Session["FName"] = validation.FName.ToString();
                Session["LName"] = validation.LName.ToString();
                Session["usertype"] = validation.user_Type;

                if (validation.user_Type.Equals(1))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (validation.user_Type.Equals(2))
                {
                    return RedirectToAction("Index", "Customer");
                }
                else if (validation.user_Type.Equals(3))
                {
                    return RedirectToAction("Index", "ProjectManager");
                }
                else if (validation.user_Type.Equals(4))
                {
                    return RedirectToAction("Index", "Teamleader");
                }
                else if(validation.user_Type.Equals(5))
                {
                    return RedirectToAction("Index", "JuniorDeveloper");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ViewBag.Login = "Incorrect Email or password";
                return View(user);
            }

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return View("Index");
        }
        public ActionResult Register()
        {
            user user = new user() ;
            return PartialView("Register", user);
        }

        [HttpPost]
        public ActionResult Register(user user)
        {
           // projectMSysEntities db = new projectMSysEntities();
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
                    var validation = db.users.Where(model => model.Email == user.Email && model.password == user.password).FirstOrDefault();
                    if (validation != null)
                    {
                        Session["UserID"] = validation.Id;
                        Session["UserFirstName"] = validation.FName.ToString();
                        Session["UserLastName"] = validation.LName.ToString();
                        Session["UserType"] = validation.user_Type;
                        if (validation.user_Type.Equals(1))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (validation.user_Type.Equals(2))
                        {
                            return RedirectToAction("Index", "Customer");
                        }
                        else if (validation.user_Type.Equals(3))
                        {
                            return RedirectToAction("Index", "ProjectManager");
                        }
                        else if (validation.user_Type.Equals(4))
                        {
                            return RedirectToAction("Index", "TeamLeader");
                        }
                        else if (validation.user_Type.Equals(5))
                        {
                            return RedirectToAction("Index", "JuniorDeveloper");
                        }
                        else
                        {
                            return View("Index","Home");
                        }
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.Message = "This Email is Already Exist";

                return View(user);
            }
            return View(user);
        }
        public ActionResult MyProfile()
        {
            if (Session["usertype"].Equals(2))
            {
                return RedirectToAction("CustomerProfile", "Customer");
            }
            else if (Session["usertype"].Equals(3))
            {
                return RedirectToAction("ProjectManagerProfile", "ProjectManager");
            }
            else if (Session["usertype"].Equals(4))
            {
                return RedirectToAction("TeamLeaderProfile", "TeamLeader");
            }
            else if (Session["usertype"].Equals(5))
            {
                return RedirectToAction("JuniorDevProfile", "JuniorDeveloper");
            }
            else
            {
                return View();
            }
        }
    }
}