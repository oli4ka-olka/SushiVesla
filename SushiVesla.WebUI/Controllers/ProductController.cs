using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.WebUI.Models;

namespace SushiVesla.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 5;
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private ICommentRepository commentRepository;
        private IUserRepository userRepository;

        public ProductController(IProductRepository prodRepository, ICategoryRepository catRepository, ICommentRepository commRepository, IUserRepository usrRepository)
        {
            productRepository = prodRepository;
            categoryRepository = catRepository;
            commentRepository = commRepository;
            userRepository = usrRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ViewBag.SelectedMenuItem = "Menu";

            int categoryId = category == null ? 0 : categoryRepository.Categories.FirstOrDefault(c => c.Name == category).CategoryID;

            ProductListViewModel viewModel = new ProductListViewModel
            {
                Products = productRepository.Products
                .Where(p => category == null || p.Category == categoryId)
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                    productRepository.Products.Count() :
                    productRepository.Products.Where(e => e.Category == categoryId).Count()
                },
                CurrentCategory = category
            };

            return View(viewModel);
        }

        public ViewResult Product(int id)
        {


            ProductViewModel viewModel = new ProductViewModel
            {
                Product = productRepository.Products.FirstOrDefault(p => p.ProductID == id),
                Comments = GetCommentsForProduct(id)
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveComment(string text, int productId)
        {
            if (text != "")
            {
                Comment comment = new Comment
                {
                    UserID = userRepository.Users.FirstOrDefault(u => u.Login == User.Identity.Name).UserID,
                    ProductID = productId,
                    Time = DateTime.Now,
                    CommentText = text
                };

                commentRepository.SaveComment(comment);
            }

            return RedirectToAction("Product", new { id = productId });
        }

        private List<CommentViewModel> GetCommentsForProduct(int productId)
        {
            IEnumerable<Comment> commentsForProduct = commentRepository.Comments
                            .Where(c => c.ProductID == productId)
                            .OrderBy(c => c.CommentID);

            List<CommentViewModel> comments = new List<CommentViewModel>();

            foreach (var comment in commentsForProduct)
            {
                comments.Add(new CommentViewModel
                {
                    CommentID = comment.CommentID,
                    CommentText = comment.CommentText,
                    ProductID = comment.ProductID,
                    Time = comment.Time,
                    User = userRepository.Users.FirstOrDefault(u => u.UserID == comment.UserID)
                });
            }

            return comments;
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}
