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
        public IActionResult getImageByte(int id = 1)
        {
            Members? members = _context.Members.Find(id);
            byte[]? img = members.FileData;
            return File(img, "image/jpeg");
        }
        //回傳城市的JSON資料

        public IActionResult Cities()
        {
            var cities = _context.Address.Select(c => c.City).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(cities);
        }

        //根據城市名稱，回傳城市的鄉鎮區JSON資料
        public IActionResult Districts(string city)
        {
            var districts = _context.Address.Where(a => a.City == city).Select(c => c.SiteId).Distinct();

            return Json(districts);
        }

        //根據鄉鎮區名稱，回傳鄉鎮區的路名JSON資料
        public IActionResult Roads(string siteId)
        {
            var roads = _context.Address.Where(a => a.SiteId == siteId).Select(c => c.Road).Distinct();

            return Json(roads);
        }
    }
}
