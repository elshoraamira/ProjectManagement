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

namespace projectManagementSys.Controllers
{
    public class TeamLeaderController : Controller
    {
        projectMSysEntities db = new projectMSysEntities();
        user user;
        // project project;
        // PM_PROJ_Result projectrequest;
        // Request request;
        submitJD_Result subJD;
        TLRequest tlrequest;
       JDRequest jdreq;
        private SqlConnection Con;
        private SqlCommand UpdateStatment;
        // GET: TeamLeader
        public ActionResult Index()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 4))
            {
                return View();
            }
            else
            {
                return View("../Home/index");
            }
        }
        public ActionResult TeamLeaderProfile()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 4))
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
        public ActionResult TeamLeaderProfile(user user)
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

                    user.user_Type = 4;

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

        //GET:TeamLeader/ALL Requests
        public ActionResult Allrequest()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 4))
            {
                int id = Convert.ToInt32(Session["ID"]);
                var list = db.TL_ReQ(id).ToList();
                return View(list);

            }
            return RedirectToAction("../Home/Index");
        }
        // GET: TeamLeader/Deleterequest
        public ActionResult Deleterequest(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 4))
                {
                    tlrequest = db.TLRequests.Find(id);

                    if (tlrequest == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tlrequest);
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
        //POST:TeamLeader/DeleteRequest
        [HttpPost, ActionName("Deleterequest")]
        public ActionResult AcceptDeleterequest(int Id)
        {
            try
            {
                tlrequest = db.TLRequests.Find(Id);

                db.TLRequests.Remove(tlrequest);

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
        public ActionResult ApproveRequest(int Id)
        {
            if (Id != 0)
            {
                if ((Convert.ToInt32(Session["UserType"]) == 4))
                {
                    tlrequest = db.TLRequests.Find(Id);

                    if (tlrequest == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tlrequest);
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
        public ActionResult ApprovedRequest(TLRequest tlrequest)
        {
            if (ModelState.IsValid)
            {

                int id = Convert.ToInt32(Session["ID"]);
                var writer = db.users.Where(model => model.Id == id).FirstOrDefault();
                //approve.Cust_id = writer.Id;
                //approve.project_id = request.Project_ID;
                //approve.ProjectMang_id = request.PM_ID;

                //db.ApprovedProjects.Add(approve);

               // db.SaveChanges();

                Con = new SqlConnection(@"Data source=DESKTOP-OJT395D;initial catalog=projectMSys;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework");
                Con.Open();

                UpdateStatment = new SqlCommand("Update TLRequests set R_Status = '" + 1 + "' Where  Project_ID = '" + tlrequest.Project_ID + "' ", Con);
                UpdateStatment.ExecuteNonQuery();

                return RedirectToAction("Allrequest");
            }

            return View();
        }
        //GET:TeamLeader/Allprojects
        public ActionResult Detailsproject()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 4))
            {
                int id = Convert.ToInt32(Session["ID"]);
                return View(db.TLRequests.Where(model => model.TL_ID == id).ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }
        //GET:TeamLeader/AllJDmembers
        public ActionResult JDMembers()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 4))
            {
                int id = Convert.ToInt32(Session["ID"]);
                var list = db.users.Where(model => model.user_Type == 5).ToList();
                return View(list);

            }
            else
            {
                return View("../Home/Index");
            }
        }
        public ActionResult SubmitProject(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 4))
                {
                    user = db.users.Find(id);
                    var PrjList =
                                    from prj in db.TLRequests
                                    join Pr in db.projects on prj.Project_ID equals Pr.PostID
                                    select new { Project_ID = prj.Project_ID, Project_name = Pr.Title }; //produces flat sequence
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
        public ActionResult SubmitProject(JDRequest jdreq ,TLRequest tlrequest , user user)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["ID"]);
                var writer = db.users.Where(model => model.user_Type == 5).FirstOrDefault();
                jdreq.User_ID = writer.Id;
                jdreq.proj_ID = tlrequest.Project_ID;
                jdreq.Rj_status = 0;
                db.JDRequests.Add(jdreq);
                db.SaveChanges();
                return RedirectToAction("JDMembers");
            }
            var PrjList =
                from prj in db.TLRequests
                join Pr in db.projects on prj.Project_ID equals Pr.PostID
                select new { Project_ID = prj.Project_ID, Project_name = Pr.Title }; //produces flat sequence
            ViewBag.Projects = new SelectList(PrjList, "Project_ID", "Project_name",tlrequest.Project_ID);
            return View();
        }
        //public ActionResult List()
        //{

        //    //projectMSysEntities db = new projectMSysEntities();
        //    //var gettlprojectlist = db.TLRequests.ToList();
        //    //SelectList list = new SelectList(gettlprojectlist, "project_ID", "project_ID");
        //    //ViewBag.list = list;
        //    //ViewBag.Projects = new SelectList(db.TLRequests, "Project_ID", "Project_ID");
        //    return View();
        //}
    }
}
