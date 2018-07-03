using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ContactSuggestion.Models
{
    public class VideoResult : ActionResult
    {
        /// <summary> 
        /// The below method will respond with the Video file 
        /// </summary> 
        /// <param name="context"></param> 
        public override void ExecuteResult(ControllerContext context)
        {
            //The File Path 
            var videoFilePath = HostingEnvironment.MapPath("~/Images/TAI.mp4");
            //The header information 
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=Win8.mp4");
            var file = new FileInfo(videoFilePath);
            //Check the file exist,  it will be written into the response 
            if (file.Exists)
            {
                var stream = file.OpenRead();
                var bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                context.HttpContext.Response.BinaryWrite(bytesinfile);
            }
        }
    }
}