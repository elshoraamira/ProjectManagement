using projectManagementSys.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Net;

namespace projectManagementSys.Controllers
{
    public class ProjectManagerController : Controller
    {
        projectMSysEntities db = new projectMSysEntities();
        user user;
        //project
        //  project postUpdate;

        // PM_REQ_Result pmrequest;
        //  Request request;
        project project;
       
       // PM_PROJ_Result projectrequest;

        private SqlConnection Con;
        private SqlCommand UpdateStatment;
        private SqlCommand Update;
        project pro;
        ApprovedProject approve;
        // GET: ProjectManager
        public ActionResult Index()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 3))
            {
                return View();
            }
            else
            {
                return View("../Home/index");
            }
        }
        public PartialViewResult Detailsproject()
        {

            if ((Convert.ToInt32(Session["usertype"]) == 3))
            {
                var allprojects = db.projects.Where(x=>x.Post_Status != 1).ToList();
                return PartialView(allprojects);
            }
            else
            {
                return PartialView("../Home/Index");
            }
        }

        public ActionResult Project()
        {
            int id = Convert.ToInt32(Session["ID"]);
           // var projects = db.projects.Include(p => p.user);
             return View(db.ApprovedProjects.Where(model => model.ProjectMang_id == id).ToList());
           // return View(projects.ToList());
        }

        public PartialViewResult Sechadual()
        {

            if ((Convert.ToInt32(Session["usertype"]) == 3))
            {
                var allprojects = db.projects.Where(x => x.Post_Status != 1).ToList();
                return PartialView(allprojects);
            }
            else
            {
                return PartialView("../Home/Index");
            }
        }

        public ActionResult DetailsTeam(int? id)
        {
            return View(db.Teams.Where(model => model.project.PostID == id).ToList());
        }

        public ActionResult Delete(int? id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 3))
                {
                    var x = db.ApprovedProjects.Where(y => y.project_id == id).Select(y=> y.ApprovedProjectID).SingleOrDefault();
                    approve = db.ApprovedProjects.Find(x);

                    if (approve == null)
                    {
                        return HttpNotFound();
                    }
                    return View(approve);
                }
                else
                {
                    return RedirectToAction("../ProjectManager/Project");
                }
            }
            else
            {
                return RedirectToAction("../ProjectManager/Project");
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var x = db.ApprovedProjects.Where(y => y.project_id == id).Select(y => y.ApprovedProjectID).SingleOrDefault();
                approve = db.ApprovedProjects.Find(x);
                db.ApprovedProjects.Remove(approve);

               // ViewBag.Error = "ImageDeleted";

                db.SaveChanges();


                Con = new SqlConnection(@"Data source=DESKTOP-OJT395D;initial catalog=projectMSys;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework");
                Con.Open();

                // UpdateStatment = new SqlCommand("Update Requests set R_status = '" + 0 + "' Where  Project_ID = '" + pro.Project_ID + "' ", Con);
                Update = new SqlCommand("Update projects set Post_Status = '" + 0 + "' Where  PostID = '" + approve.project_id +"'", Con);

                Update.ExecuteNonQuery();
                return RedirectToAction("Project");

            }

            catch
            {
                return View();
            }
        }


        

        // GET: Teams/Delete/5
        public ActionResult DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("DeleteTeam")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeamConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("DetailsTeam");
        }
        // GET: Customer/PMProfile
        public ActionResult ProjectManagerProfile()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 3))
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

        public ActionResult TeamLeader()
        {
            var users = db.users.Include(u => u.userType);
            return View(db.users.Where(model => model.user_Type == 4).ToList());

        }
        public ActionResult JuniorDeveloper()
        {
            var users = db.users.Include(u => u.userType);
            return View(db.users.Where(model => model.user_Type == 5).ToList());

        }

        [HttpPost]
        public ActionResult ProjectManagerProfile(user user)
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

                    user.user_Type = 3;

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
        public ActionResult SubmitProject(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 3))
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
                    return View("../Home/Index");
                }
            }
            else
            {
                return View("../Home/Index");
            }

        }

        [HttpPost, ActionName("SubmitProject")]
        public ActionResult SubmitProject(Request request, project project)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["ID"]);
                var writer = db.users.Where(model => model.Id == id).FirstOrDefault();
                request.PM_ID = writer.Id;
                request.Project_ID = project.PostID;
                request.R_status = 0;
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ProjectComplete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pro = db.projects.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            return View(pro);
        }
        [HttpPost, ActionName("ProjectComplete")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectComplete(project pro)
        {

            // project = db.projects.Find(id);

            //Convert.ToBoolean(Convert.ToInt32(myString));

            if (ModelState.IsValid)
            {

                Con = new SqlConnection(@"Data source=DESKTOP-OJT395D;initial catalog=projectMSys;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework");
                Con.Open();
                UpdateStatment = new SqlCommand("Update projects set Avilable = '" + 1 + "' Where  PostID = '" + pro.PostID + "' ", Con);

                UpdateStatment.ExecuteNonQuery();
                //bool val = Boolean.Parse("Complete");
                return RedirectToAction("Project");
            }

            return View();
        }

        
        public ActionResult SendRequestJD(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 3))
                {
                    user = db.users.Find(id);
                    var PrjList =
                                    from prj in db.ApprovedProjects
                                    join Pr in db.projects on prj.project_id equals Pr.PostID
                                    select new { Project_ID = prj.project_id, Project_name = Pr.Title }; //produces flat sequence
                    ViewBag.Projects = new SelectList(PrjList, "Project_ID", "Project_name");

                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View();
                }
                else
                {
                    return View("../Home/Index");
                }
            }
            else
            {
                return View("../Home/Index");
            }

        }

        [HttpPost]
        public ActionResult SendRequestJD(JDRequest jdreq, ApprovedProject approve, user user)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["ID"]);
                var writer = db.users.Where(model => model.user_Type == 5).FirstOrDefault();
                jdreq.User_ID = writer.Id;
                jdreq.proj_ID = approve.project_id;
                jdreq.Rj_status = 0;
                db.JDRequests.Add(jdreq);
                db.SaveChanges();
                return RedirectToAction("JuniorDeveloper");
            }
            var PrjList =
                from prj in db.ApprovedProjects
                join Pr in db.projects on prj.project_id equals Pr.PostID
                select new { Project_ID = prj.project_id, Project_name = Pr.Title }; //produces flat sequence
            ViewBag.Projects = new SelectList(PrjList, "Project_ID", "Project_name", approve.project_id);
            return View();
        }

        public ActionResult SendRequestTL(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 3))
                {
                    user = db.users.Find(id);
                    var PrjList =
                                    from prj in db.ApprovedProjects
                                    join Pr in db.projects on prj.project_id equals Pr.PostID
                                    select new { Project_ID = prj.project_id, Project_name = Pr.Title }; //produces flat sequence
                    ViewBag.Projects = new SelectList(PrjList, "Project_ID", "Project_name");

                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View();
                }
                else
                {
                    return View("../Home/Index");
                }
            }
            else
            {
                return View("../Home/Index");
            }

        }

        [HttpPost]
        public ActionResult SendRequestTL(TLRequest tLrequest, ApprovedProject approve, user user)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["ID"]);
                var writer = db.users.Where(model => model.user_Type == 4).FirstOrDefault();
                tLrequest.TL_ID = writer.Id;
                tLrequest.Project_ID = approve.project_id;
                tLrequest.R_Status = 0;
                db.TLRequests.Add(tLrequest);
                db.SaveChanges();
                return RedirectToAction("TeamLeader");
            }
            var PrjList =
                from prj in db.ApprovedProjects
                join Pr in db.projects on prj.project_id equals Pr.PostID
                select new { Project_ID = prj.project_id, Project_name = Pr.Title }; //produces flat sequence
            ViewBag.Projects = new SelectList(PrjList, "Project_ID", "Project_name", approve.project_id);
            return View();
        }
    }
}
