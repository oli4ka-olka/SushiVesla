using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private INewsRepository newsRepository;
        private IUserRepository userRepository;
        IOrderProductRepository orderProductRepository;

        public AdminController(IProductRepository repository, ICategoryRepository catRepository, INewsRepository nRepository, IUserRepository usrRepository, IOrderProductRepository ordProductRepository)
        {
            productRepository = repository;
            categoryRepository = catRepository;
            newsRepository = nRepository;
            userRepository = usrRepository;
            orderProductRepository = ordProductRepository;
        }

        public ViewResult Index()
        {
            return View();
        }

        //=============================================== Products ====================================================

        public ViewResult Products()
        {
            return View(productRepository.Products.Where(x => !x.Deleted));
        }

        public ViewResult EditProduct(int productId)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            //ViewData["AllGenres"] = from genre in Data.GetGenres() select new SelectListItem { Text = genre.Name, Value = genre.Id.ToString() };
            ViewData["Categories"] = GetCategoriesList();
            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                productRepository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Products");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult CreateProduct()
        {
            ViewData["Categories"] = GetCategoriesList();
            return View("EditProduct", new Product());
        }

        public ActionResult DeleteProduct(int productId)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                int ordersCount = orderProductRepository.OrderProducts.Count(x => x.ProductID == productId);
                if (ordersCount > 0)
                {
                    product.Deleted = true;
                    productRepository.SaveProduct(product);
                }
                else
                {
                    productRepository.DeleteProduct(product);
                    TempData["message"] = string.Format("{0} was deleted", product.Name);
                }
            }
            return RedirectToAction("Products");
        }

        private IEnumerable<SelectListItem> GetCategoriesList()
        {
            return from category in categoryRepository.Categories where (!category.Deleted) select new SelectListItem { Text = category.Name, Value = SqlFunctions.StringConvert((double)category.CategoryID) };
        }

        //=============================================== Categories ====================================================

        public ViewResult Categories()
        {
            return View(categoryRepository.Categories);
        }

        public ViewResult EditCategory(int categoryId)
        {
            Category category = categoryRepository.Categories.FirstOrDefault(p => p.CategoryID == categoryId);
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    category.ImageMimeType = image.ContentType;
                    category.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(category.ImageData, 0, image.ContentLength);
                }
                categoryRepository.SaveCategory(category);
                TempData["message"] = string.Format("{0} has been saved", category.Name);
                return RedirectToAction("Categories");
            }
            else
            {
                return View(category);
            }
        }

        public ViewResult CreateCategory()
        {
            return View("EditCategory", new Category());
        }

        public ActionResult DeleteCategory(int categoryId)
        {
            Category category = categoryRepository.Categories.FirstOrDefault(p => p.CategoryID == categoryId);
            if (category != null)
            {
                int productsCountForCategory = productRepository.Products.Count(x => x.Category == category.CategoryID);
                if (productsCountForCategory > 0)
                {
                    //TempData["deleteCategoryError"] = string.Format("Category of \"{0}\" includes goods. It can not be removed!", category.Name);
                    category.Deleted = true;
                    categoryRepository.SaveCategory(category);
                }
                else
                {
                    categoryRepository.DeleteCategory(category);
                    TempData["message"] = string.Format("{0} was deleted", category.Name);
                }
            }
            return RedirectToAction("Categories");
        }

        //=============================================== News ====================================================

        public ViewResult News()
        {
            return View(newsRepository.News);
        }

        public ViewResult EditNews(int newsId)
        {
            News news = newsRepository.News.FirstOrDefault(p => p.NewsID == newsId);
            return View(news);
        }

        [HttpPost]
        public ActionResult EditNews(News news, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    news.ImageMimeType = image.ContentType;
                    news.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(news.ImageData, 0, image.ContentLength);
                }
                news.PublishTime = DateTime.Now;
                newsRepository.SaveNews(news);
                TempData["message"] = string.Format("{0} has been saved", news.Title);
                return RedirectToAction("News");
            }
            else
            {
                return View(news);
            }
        }

        public ViewResult CreateNews()
        {
            return View("EditNews", new News());
        }

        public ActionResult DeleteNews(int newsId)
        {
            News news = newsRepository.News.FirstOrDefault(p => p.NewsID == newsId);
            if (news != null)
            {
                newsRepository.DeleteNews(news);
                TempData["message"] = string.Format("{0} was deleted", news.Title);
            }
            return RedirectToAction("News");
        }

        //=============================================== Users ====================================================

        public ViewResult Users()
        {
            return View(userRepository.Users.Where(x => !x.Deleted));
        }

        public ViewResult EditUser(int userId)
        {
            User user = userRepository.Users.FirstOrDefault(p => p.UserID == userId);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.UserID == 0)
                {
                    if (userRepository.Users.Count(x => x.Login == user.Login) > 0)
                    {
                        ModelState.AddModelError("", string.Format("User with login \"{0}\" is already exists!", user.Login));
                        return View(user);
                    }
                }
                userRepository.SaveUser(user);
                TempData["message"] = string.Format("{0} has been saved", user.Login);
                return RedirectToAction("Users");
            }
            else
            {
                return View(user);
            }
        }

        public ViewResult CreateUser()
        {
            return View("EditUser", new User());
        }

        public ActionResult DeleteUser(int userId)
        {
            User user = userRepository.Users.FirstOrDefault(p => p.UserID == userId);
            if (user != null)
            {
                if (user.Login != "Admin")
                {
                    //userRepository.DeleteUser(user);
                    //TempData["message"] = string.Format("{0} was deleted", user.Login);
                    user.Deleted = true;
                    userRepository.SaveUser(user);
                }
            }
            return RedirectToAction("Users");
        }
    }
}
