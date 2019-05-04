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
    public class JuniorDeveloperController : Controller
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
        // GET: JuniorDeveloper
        public ActionResult Index()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 5))
            {
                return View();
            }
            else
            {
                return View("../Home/index");
            }
        }
        public ActionResult JuniorDevProfile()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 5))
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
        public ActionResult JuniorDevProfile(user user)
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

                    user.user_Type = 5;

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
        //GET:JuniorDeveloper/ALL Requests
        public ActionResult Allrequest()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 5))
            {
                int id = Convert.ToInt32(Session["ID"]);
                var list = db.JD_ReQ(id).ToList();
              //  var list = db.TL_ReQ(id).ToList();
                return View(list);

            }
            return RedirectToAction("../Home/Index");
        }
        // GET: JuniorDeveloper/Deleterequest
        public ActionResult Deleterequest(int id)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["usertype"]) == 5))
                {
                    jdreq = db.JDRequests.Find(id);

                    if (jdreq == null)
                    {
                        return HttpNotFound();
                    }
                    return View(jdreq);
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
        //POST:JuniorDeveloper/DeleteRequest
        [HttpPost, ActionName("Deleterequest")]
        public ActionResult AcceptDeleterequest(int Id)
        {
            try
            {
                jdreq = db.JDRequests.Find(Id);

                db.JDRequests.Remove(jdreq);

                ViewBag.Error = "RequestDeleted";

                db.SaveChanges();

                return RedirectToAction("Allrequest");
            }

            catch
            {
                return View();
            }
        }
        //GET : JuniorDeveloper/ApproveRequest
        public ActionResult ApproveRequest(int Id)
        {
            if (Id != 0)
            {
                if ((Convert.ToInt32(Session["UserType"]) == 5))
                {
                    jdreq = db.JDRequests.Find(Id);

                    if (jdreq == null)
                    {
                        return HttpNotFound();
                    }
                    return View(jdreq);
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
        public ActionResult ApprovedRequest(JDRequest Jdreq)
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
                UpdateStatment = new SqlCommand("Update JDRequests set Rj_status = '" + 1 + "' Where  proj_ID = '" + jdreq.proj_ID + "' ", Con);
                UpdateStatment.ExecuteNonQuery();

                return RedirectToAction("Allrequest");
            }

            return View();
        }
        //GET:JuniorDeveloper/Allprojects
        public ActionResult Detailsproject()
        {
            if ((Convert.ToInt32(Session["usertype"]) == 5))
            {
                int id = Convert.ToInt32(Session["ID"]);
                return View(db.JDRequests.Where(model => model.User_ID == id).ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

    }
}
