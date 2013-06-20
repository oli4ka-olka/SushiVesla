using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.ObjectModel.Interfaces
{
    public interface INewsRepository
    {
        IQueryable<News> News { get; }
        void SaveNews(News news);
        void DeleteNews(News news);
    }
}
