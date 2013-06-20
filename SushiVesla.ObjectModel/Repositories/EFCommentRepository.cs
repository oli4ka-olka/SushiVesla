using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.ObjectModel.Repositories
{
    public class EFCommentRepository : ICommentRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Comment> Comments
        {
            get { return context.Comments; }
        }

        public void SaveComment(Comment comment)
        {
            if (comment.CommentID == 0)
            {
                context.Comments.Add(comment);
            }
            else
            {
                var comm = context.Comments.FirstOrDefault(c => c.CommentID == comment.CommentID);
                comm.CommentText = comment.CommentText;
                comm.ProductID = comment.ProductID;
                comm.UserID = comment.UserID;
                comm.Time = comment.Time;
            }
            context.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            context.Comments.Remove(comment);
            context.SaveChanges();
        }
    }
}
