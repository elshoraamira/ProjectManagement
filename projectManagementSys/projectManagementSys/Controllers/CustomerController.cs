using projectManagementSys.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.Data.SqlClient;



namespace projectManagementSys.Controllers
{
    public class CustomerController : Controller
    {
        projectMSysEntities db = new projectMSysEntities();
        user user;
        project project;
       // PM_REQ_Result pmrequest;
        Request request ;
        private SqlConnection Con;
        private SqlCommand UpdateStatment;
        private SqlCommand Update;

        // GET: Customer
        public ActionResult Index()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 2))
            {
               // return RedirectToAction("Index");
                return View();
            }
            else
            {
                return View("../Home/Index");
            }
        }

        // GET: Customer/CustomerProfile
        public ActionResult CustomerProfile()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 2))
            {
                int UserID = Convert.ToInt32(Session["ID"]);

                user = db.users.Find(UserID);

                return View(user);
            }
            else
            {
                return View("../Home/Index");
            }
        }

        [HttpPost]
        public ActionResult CustomerProfile(user user)
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

                    user.user_Type = 2;

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
        // GET: Customer/Addproject
        public ActionResult Addproject()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 2))
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // POST: Customer/Addproject
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


                return RedirectToAction("DetailsProject");
            }
            return View();
        }
        //GET : Customer/AllProjects
        public ActionResult Detailsproject()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 2))
            {
                int id = Convert.ToInt32(Session["ID"]);
                return View(db.projects.Where(model => model.user.Id == id).ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // GET: Customer/Editproject
        public ActionResult Editproject(int id = 0)
        {
            if (id != 0)
            {
                // if ((Convert.ToInt32(Session["projectType"]) == 1))
                // {
                project = db.projects.Find(id);

                //   if (project == null)
                //  {
                //return HttpNotFound();
                // }
                return View(project);
            }
            else
            {
                return RedirectToAction("../Customer/index");
            }
        }

        // POST: Customer/Editproject
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

        // GET: Customer/Deleteproject
        public ActionResult Deleteproject(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 2))
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

        // POST: Customer/Deleteproject
        [HttpPost, ActionName("Deleteproject")]
        public ActionResult DeleteProject(int id)
        {
            try
            {
                project = db.projects.Find(id);
                if ( project.PostID == request.Project_ID)
                {
                    foreach (var req in project.Requests.ToList())
                    {
                        db.Requests.Remove(req);
                        db.SaveChanges();
                        db.projects.Remove(project);
                        db.SaveChanges();
                    }


                    return RedirectToAction("Detailsproject");
                }
                else
                {
                    return View();
                }
            }

            catch
            {
                return View();
            }
        }
        //GET:Customer/ALL Requests
        public ActionResult Allrequest()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 2))
            {
                int id = Convert.ToInt32(Session["ID"]);
                var list = db.PM_REQ(id)/*.Where(x=>x.PM_ID!=null)*/.ToList();
                return View(list);

            }
            return RedirectToAction("../Home/Index");
        }
        // GET: Customer/Deleterequest
        public ActionResult Deleterequest(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 2))
                {
                    request = db.Requests.Find(id);

                    if (request == null)
                    {
                        return HttpNotFound();
                    }
                    return View(request);
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
        //POST:Customer/DeleteRequest
        [HttpPost, ActionName("Deleterequest")]
        public ActionResult AcceptDeleterequest(int Id)
        {
            try
            {
                request = db.Requests.Find(Id);

                db.Requests.Remove(request);

                ViewBag.Error = "RequestDeleted";

                db.SaveChanges();

                return RedirectToAction("Allrequest");
            }

            catch
            {
                return View();
            }
        }
        //GET : Customer/ApproveRequest
        public ActionResult ApproveRequest(int /*RequestID*/Id )
        {
            if (/*RequestID*/Id != 0)
            {
                if ((Convert.ToInt32(Session["UserType"]) == 2))
                {
                    request = db.Requests.Find(/*RequestID*/Id);

                    if (request == null)
                    {
                        return HttpNotFound();
                    }
                    return View(request);
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
        [HttpPost, ActionName("ApproveRequest")]
        public ActionResult ApprovedRequest(ApprovedProject approve, Request request)
        {
            if (ModelState.IsValid)
            {

                int id = Convert.ToInt32(Session["ID"]);
                var writer = db.users.Where(model => model.Id == id).FirstOrDefault();
                approve.Cust_id = writer.Id;
                approve.project_id = request.Project_ID;
                approve.ProjectMang_id = request.PM_ID;

                db.ApprovedProjects.Add(approve);

                db.SaveChanges();

                Con = new SqlConnection(@"Data source=DESKTOP-OJT395D;initial catalog=projectMSys;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework");
                Con.Open();

                UpdateStatment = new SqlCommand("Update Requests set R_status = '" + 1 + "' Where  Project_ID = '" + request.Project_ID + "' ", Con);
                Update = new SqlCommand("Update projects set Post_Status = '" + 1 +"' ", Con);

                UpdateStatment.ExecuteNonQuery();

                return RedirectToAction("Allrequest");
            }

            return View();


        }
    }
}
