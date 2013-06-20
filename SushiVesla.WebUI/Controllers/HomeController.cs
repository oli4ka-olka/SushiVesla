using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private INewsRepository newsRepsitory;

        public HomeController(INewsRepository nRepository)
        {
            newsRepsitory = nRepository;
        }

        public ActionResult Index()
        {
            ViewBag.SelectedMenuItem = "Home";
            return View(newsRepsitory.News.OrderByDescending(n => n.PublishTime));
        }

        public FileContentResult GetImage(int newsId)
        {
            News news = newsRepsitory.News.FirstOrDefault(n => n.NewsID == newsId);
            if (news != null)
            {
                return File(news.ImageData, news.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}
