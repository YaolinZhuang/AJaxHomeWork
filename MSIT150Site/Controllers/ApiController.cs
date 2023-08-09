using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MSIT150Site.Models;
using NuGet.Protocol.Core.Types;

namespace MSIT150Site.Controllers
{

    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        private readonly IWebHostEnvironment _host;

        public ApiController(DemoContext context, IWebHostEnvironment host)
            {
                _context = context;
                _host = host;
             }
            public IActionResult Index()
        {
            Thread.Sleep(3000);//睡3s
            return Content("Yoyoyo!! Ajax!");
        }
        public IActionResult getDemo(AddUserViewModel user)
        {
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "Guest";
            }
            return Content($"你好{user.name},你今年{user.age}歲嗎?");
        }
        public IActionResult Register(Members member,IFormFile file)
        {
            //return Content($"{_host.ContentRootPath}");  //C:\shared\Ajax\MSIT150Site\
            //return Content($"{_host.WebRootPath}");  //C:\shared\Ajax\MSIT150Site\wwwroot
            string filePath = Path.Combine(_host.WebRootPath, "image", file.FileName); //C:\shared\Ajax\MSIT150Site\wwwroot\uploads\walk.gif

            //上傳檔案到uploads資料夾中
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            byte[]?imgByte= null;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            //return Content($"上傳檔案到{filePath}");

            member.FileName = file.FileName;
            member.FileData = imgByte;
            _context.Members.Add(member);
            _context.SaveChanges();
            return Content("新增成功!!");
        }
    }
}
