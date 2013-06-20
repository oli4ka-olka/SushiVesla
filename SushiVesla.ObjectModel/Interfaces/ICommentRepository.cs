using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.ObjectModel.Interfaces
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }
        void SaveComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
