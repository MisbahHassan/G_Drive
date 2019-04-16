using Assignment_8.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using BAL;

namespace Assignment_8.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(String login,String password)
        {
            Session["Login"] = login;
            var obj = BAL.UserBO.ValidateUser(login, password);
            if (obj != null)
            {
                Session["UserName"] = obj.Name;
                Session["UserID"] = obj.ID;

                List<FolderDTO> list = FolderBO.GetAllFoldersByID(obj.ID, 0);
                return View("Home", list);
            }
            else
            {
                ViewBag.MSG = "Invalid Login/Password";
               // Response.Write("<script>alert('Wrong Email or Password')</script>");
                return Redirect("~/Home/Login");
            }

        }
        public ActionResult DisplayFolder(int fid)
        {
            if (Session["Login"] != null)
            {
                Session["FolderId"] = fid;
                int createdByID = Convert.ToInt16(Session["UserID"]);
                FolderDTO fd = FolderBO.GetFolderByID(fid);
                TempData["FolderName"] = fd.Name;
                Session["pFolderId"] = fd.ParentFolderID;

                List<FolderDTO> list_1 = FolderBO.GetAllFoldersByID(createdByID, fid);
                Session["ListOfFolders"] = list_1;

                List<FileDTO> list_2 = FileBO.GetAllFilesByID(createdByID, fid);
                Session["ListOfFiles"] = list_2;
                return View("Details");
            }
            else
            {
                Response.Write("<script>alert('Login First')</script>");
                return Redirect("~/Home/Login");
            }

        }
        public ActionResult BackMove(int fid)
        {
            if (Session["Login"] != null)
            {
                Session["FolderId"] = fid;
                int createdByID = Convert.ToInt16(Session["UserID"]);
                FolderDTO fd = FolderBO.GetFolderByID(fid);
                TempData["FolderName"] = fd.Name;
                Session["pFolderId"] = fd.ParentFolderID;
                if (fd.ID == 0)
                {
                    List<FolderDTO> list = FolderBO.GetAllFoldersByID(createdByID, 0);
                    return View("Home", list);
                }
                else
                {
                    List<FolderDTO> list_1 = FolderBO.GetAllFoldersByID(createdByID, fid);
                    Session["ListOfFolders"] = list_1;

                    List<FileDTO> list_2 = FileBO.GetAllFilesByID(createdByID, fid);
                    Session["ListOfFiles"] = list_2;
                    return View("Details");
                }
            }
            else
            {
                Response.Write("<script>alert('Login First')</script>");
                return Redirect("~/Home/Login");
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            SessionManager.ClearSession();
            return RedirectToAction("Login","Home");
        }

    }
}