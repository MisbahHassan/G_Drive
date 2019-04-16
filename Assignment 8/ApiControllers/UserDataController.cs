using Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Microsoft.WindowsAPICodePack.Shell;
using DAL;
using PDFHelper;
//using System.Web.Mvc;

namespace Assignment_8.ApiControllers
{
    public class UserDataController : ApiController
    {
        public static object PdfWriter { get; private set; }

        [HttpPost]
        public bool CreateNewFolder()
        {
            try
            {
                FolderDTO obj = new FolderDTO();
                var pfdID = HttpContext.Current.Request["ParentFolderID"];
                var FolderName = HttpContext.Current.Request["FolderName"];
                var login = HttpContext.Current.Request["Login"];
                UserDTO user = BAL.UserBO.getUserByLogin(login);
                obj.CreatedBy = user.ID;
                obj.ParentFolderID = Convert.ToInt32(pfdID);
                obj.Name = FolderName;
                obj.CreatedOn = DateTime.Now;
                int result = BAL.FolderBO.CreateNewFolder(obj);
                if (result > 0)
                {
                    return true;
                }
                
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return false;
        }
        [HttpPost]
        public bool DeleteFile()
        {
            try
            {
                var id = Convert.ToInt32(HttpContext.Current.Request["FileID"]);
                int result = BAL.FileBO.DeleteFile(id);
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return false;
        }
        [HttpPost]
        public bool DeleteFolder()
        {
            try
            {
                var id = Convert.ToInt32(HttpContext.Current.Request["FolderID"]);
                int result = BAL.FolderBO.DeleteFolder(id);
                if (result > 0)
                {
                    return true;
                }
               
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return false;
        }
        [HttpPost]
        public void uploadFile()
        {
            try
            {
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    foreach (var fileName in HttpContext.Current.Request.Files.AllKeys)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[fileName];
                        if (file != null)
                        {
                            FileDTO fileDTO = new FileDTO();
                            fileDTO.Name = file.FileName;
                            fileDTO.FileExt = Path.GetExtension(file.FileName);
                            fileDTO.UploadedOn = DateTime.Now;
                            fileDTO.ContentType = file.ContentType;

                            String uLogin = HttpContext.Current.Request["UserLogin"].ToString();
                            UserDTO user = BAL.UserBO.getUserByLogin(uLogin);
                            fileDTO.CreatedBy = user.ID;

                            var pfdID = HttpContext.Current.Request["FolderID"];
                            int pfid = Convert.ToInt32(pfdID);
                            fileDTO.ParentFolderID = pfid;
                            //var fSize = HttpContext.Current.Request["FileSize"];
                            //int fileSize = Convert.ToInt32(fSize);
                            fileDTO.FileSizeInKB = file.ContentLength / 1024; //file.ContentLength is the file size in bytes

                            fileDTO.fileUniqueName = Guid.NewGuid().ToString();

                            var rootpath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
                            var fileSavePath = System.IO.Path.Combine(rootpath, fileDTO.Name + fileDTO.FileExt);
                            file.SaveAs(fileSavePath);

                            int result = BAL.FileBO.saveFileInDB(fileDTO);
                        }

                    }
                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
        [HttpGet]
        public Object DownloadFile(int id)
        {
            try
            {
                var rootPath = System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles");
                var fileDTO = BAL.FileBO.GetFileByFileID(id);
                if (fileDTO != null)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    var fileFullPath = System.IO.Path.Combine(rootPath, fileDTO.Name + fileDTO.FileExt);
                    byte[] file = System.IO.File.ReadAllBytes(fileFullPath);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(file);

                    response.Content = new ByteArrayContent(file);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(fileDTO.ContentType);
                    response.Content.Headers.ContentDisposition.FileName = fileDTO.Name;
                    return response;
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    return response;
                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
            return null;
        }
        [HttpGet]
        public Object GetThumbnail(int id)
        {
            try
            {
                var rootPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");

                // Find file from DB using id
                var FileDTO = BAL.FileBO.GetFileByFileID(id);
                var fileFullPath = Path.Combine(rootPath, FileDTO.Name + FileDTO.FileExt);

                ShellFile shellFile = ShellFile.FromFilePath(fileFullPath);
                Bitmap shellThumb = shellFile.Thumbnail.MediumBitmap;

                if (FileDTO != null)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                    byte[] file = ImageToBytes(shellThumb); // Calling private ImageToBytes

                    MemoryStream ms = new MemoryStream(file);

                    response.Content = new ByteArrayContent(file);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");

                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(FileDTO.ContentType);
                    response.Content.Headers.ContentDisposition.FileName = FileDTO.Name;
                    return response;
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    return response;
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            // Physical path of root folder
            return null;

        }
        private byte[] ImageToBytes(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        [HttpPost]
        public String generatePdf()
        {
            var pfdID = HttpContext.Current.Request["id"];
            int pfid = Convert.ToInt32(pfdID);
            String pPath = HttpContext.Current.Server.MapPath("~/docs");
            String pdfName = PdfGenerator.GeneratePDF(pPath, pfid);
            return pdfName;
        }
        
    }
} 
