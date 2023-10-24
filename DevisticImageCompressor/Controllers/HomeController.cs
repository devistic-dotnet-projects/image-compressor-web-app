using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevisticImageCompressor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                try
                {
                    // Define temporary file paths for uploaded and compressed images
                    string uploadTempPath = Server.MapPath("~/App_Data/Temp_Data/images");
                    string compressedTempPath = Server.MapPath("~/App_Data/Temp_Data/compressed");

                    // Ensure the directories exist, create them if necessary
                    if (!Directory.Exists(uploadTempPath))
                    {
                        Directory.CreateDirectory(uploadTempPath);
                    }
                    if (!Directory.Exists(compressedTempPath))
                    {
                        Directory.CreateDirectory(compressedTempPath);
                    }

                    // Generate a unique filename for the uploaded image
                    string uploadedFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    string uploadedFilePath = Path.Combine(uploadTempPath, uploadedFileName);

                    // Save the uploaded image to the temporary folder
                    image.SaveAs(uploadedFilePath);

                    // Measure the size of the uploaded file in megabytes
                    double originalFileSizeMB = (double)new FileInfo(uploadedFilePath).Length / (1024 * 1024);

                    // Compress the uploaded image
                    string compressedFileName = "compressed_" + uploadedFileName;
                    string compressedFilePath = Path.Combine(compressedTempPath, compressedFileName);

                    using (var sourceStream = System.IO.File.OpenRead(uploadedFilePath))
                    {
                        ImageCompressUtility.Compressimage(sourceStream, compressedFilePath, compressedFileName);
                    }

                    // Measure the size of the compressed file in megabytes
                    double compressedFileSizeMB = (double)new FileInfo(compressedFilePath).Length / (1024 * 1024);

                    // Read the compressed image as bytes
                    byte[] compressedImageBytes = System.IO.File.ReadAllBytes(compressedFilePath);

                    // Convert the bytes to base64
                    string base64CompressedImage = Convert.ToBase64String(compressedImageBytes);

                    // Store the base64 encoded image in session
                    ViewBag.Image = base64CompressedImage;

                    // Delete temporary files
                    System.IO.File.Delete(uploadedFilePath);

                    //return File(compressedFilePath, "image/jpeg"); // Modify content type as needed
                    System.IO.File.Delete(compressedFilePath);

                    // Compare file sizes in megabytes
                    ViewBag.OriginalFileSizeMB = originalFileSizeMB.ToString("#,##.00");
                    ViewBag.CompressedFileSizeMB = compressedFileSizeMB.ToString("#,##.00");
                }
                catch (Exception)
                {
                    // Handle any exceptions here
                    throw;
                }
            }
            return View();
        }

    }
}