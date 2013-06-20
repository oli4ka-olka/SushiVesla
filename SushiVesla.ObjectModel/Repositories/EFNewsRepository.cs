using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.ObjectModel.Repositories
{
    public class EFNewsRepository:INewsRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<News> News
        {
            get { return context.News; }
        }

        public void SaveNews(News news)
        {
            if (news.NewsID == 0)
            {
                context.News.Add(news);
            }
            else
            {
                var newNews = context.News.FirstOrDefault(n => n.NewsID == news.NewsID);
                newNews.Title = news.Title;
                newNews.Body = news.Body;
                newNews.ImageMimeType = news.ImageMimeType;
                newNews.ImageData = news.ImageData;
            }
            context.SaveChanges();
        }

        public void DeleteNews(News news)
        {
            context.News.Remove(news);
            context.SaveChanges();
        }
    }
}
