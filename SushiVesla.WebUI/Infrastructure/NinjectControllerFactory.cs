using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Ninject;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.ObjectModel.Repositories;
using SushiVesla.WebUI.Infrastructure.AuthorizationProviders;
using SushiVesla.WebUI.Infrastructure.Interfaces;

namespace SushiVesla.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
            ninjectKernel.Inject(Membership.Provider);
            ninjectKernel.Inject(Roles.Provider);
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            //Mock<IProductRepository> mockProducts = new Mock<IProductRepository>();
            //mockProducts.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product{ProductID = 1, Name = "Football", Price = 25, Category = 1, Description = "Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 Desc1 "},
            //    new Product{ProductID = 2, Name = "Surf board", Price = 175, Category = 2, Description = "Desc2"},
            //    new Product{ProductID = 3, Name = "Running shoes", Price = 95, Category = 3, Description = "Desc3"},
            //    new Product{ProductID = 3, Name = "Running shoes 1", Price = 95, Category = 3, Description = "Desc31"},
            //    new Product{ProductID = 3, Name = "Running shoes 2", Price = 95, Category = 3, Description = "Desc32"},
            //    new Product{ProductID = 3, Name = "Running shoes 3", Price = 95, Category = 3, Description = "Desc33"},
            //    new Product{ProductID = 3, Name = "Running shoes 4", Price = 95, Category = 3, Description = "Desc33"},
            //    new Product{ProductID = 3, Name = "Running shoes 5", Price = 95, Category = 4, Description = "Desc34"}
            //}.AsQueryable());
            //ninjectKernel.Bind<IProductRepository>().ToConstant(mockProducts.Object);
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();

            //Mock<ICategoryRepository> mockCategories = new Mock<ICategoryRepository>();
            //mockCategories.Setup(m => m.Categories).Returns(new List<Category>
            //{
            //    new Category {CategoryID  = 1, Name = "Soccer"},
            //    new Category {CategoryID  = 2, Name = "Basketball"},
            //    new Category {CategoryID  = 3, Name = "Football"},
            //    new Category {CategoryID  = 4, Name = "None products"}
            //}.AsQueryable());
            //ninjectKernel.Bind<ICategoryRepository>().ToConstant(mockCategories.Object);
            ninjectKernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();

            //Mock<IUserRepository> mockUsers = new Mock<IUserRepository>();
            //mockUsers.Setup(m => m.Users).Returns(new List<User>
            //{
            //    new User { UserID = 1, Login = "U", Password = "U123", Name = "User1", Surnaame = "SUser1", EmailAddress = "u1@mail.u", PhoneNumber = "1111111111", Role = UserRoles.User},
            //    new User { UserID = 2, Login = "SU", Password = "SU123", Name = "User2ServStaff", Surnaame = "SUser2", EmailAddress = "u2@mail.u", PhoneNumber = "2222222222", Role = UserRoles.ServiceStaff},
            //    new User { UserID = 3, Login = "AU", Password = "AU123", Name = "User3Admin", Surnaame = "SUser3", EmailAddress = "u3@mail.u", PhoneNumber = "3333333333", Role = UserRoles.Admin}
            //}.AsQueryable());
            //ninjectKernel.Bind<IUserRepository>().ToConstant(mockUsers.Object);
            ninjectKernel.Bind<IUserRepository>().To<EFUserRepository>();

            //Mock<ICommentRepository> mockComment = new Mock<ICommentRepository>();
            //mockComment.Setup(m => m.Comments).Returns(new List<Comment>
            //{
            //    new Comment { CommentID = 1, ProductID = 2, CommentText = "Comment 1", UserID = 1, Time = DateTime.Now },
            //    new Comment { CommentID = 2, ProductID = 2, CommentText = "Comment 2", UserID = 2, Time = DateTime.Now },
            //    new Comment { CommentID = 3, ProductID = 2, CommentText = "Comment 3", UserID = 3, Time = DateTime.Now },
            //}.AsQueryable());
            //ninjectKernel.Bind<ICommentRepository>().ToConstant(mockComment.Object);
            ninjectKernel.Bind<ICommentRepository>().To<EFCommentRepository>();

            //Mock<INewsRepository> mockNews = new Mock<INewsRepository>();
            //mockNews.Setup(n => n.News).Returns(new List<News>
            //{
            //    new News{ NewsID = 1, Title = "Title 1", PublishTime=DateTime.Now, Body = "Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body Body ", ShowOnMainPage = true },
            //    new News{ NewsID = 2, Title = "Title 2", PublishTime=DateTime.Now, Body = "Body Body Body Body Body Body Body Body Body Body Body Body Body", ShowOnMainPage = true },
            //    new News{ NewsID = 3, Title = "Title 3", PublishTime=DateTime.Now, Body = "Body Body Body Body Body Body Body Body Body Body Body", ShowOnMainPage = true },
            //    new News{ NewsID = 4, Title = "Title 4", PublishTime=DateTime.Now, Body = "Body Body Body Body Body Body Body Body Body", ShowOnMainPage = true },
            //    new News{ NewsID = 5, Title = "Title 5", PublishTime=DateTime.Now, Body = "Body Body Body Body Body Body Body", ShowOnMainPage = true },
            //    new News{ NewsID = 6, Title = "Title 6", PublishTime=DateTime.Now, Body = "Body Body Body Body Body", ShowOnMainPage = false },
            //    new News{ NewsID = 7, Title = "Title 7", PublishTime=DateTime.Now, Body = "Body Body Body", ShowOnMainPage = true }
            //}.AsQueryable());
            //ninjectKernel.Bind<INewsRepository>().ToConstant(mockNews.Object);
            ninjectKernel.Bind<INewsRepository>().To<EFNewsRepository>();

            //Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            //mockOrderRepository.Setup(m => m.Orders).Returns(new List<Order>
            //{
            //    new Order{OrderID = "110062013040511", SubmittedBy = 1, ProcessedBy = 0, Delivered = false, Paid = false, TheOrderIsExecuted = false, UserName = "User1", PhoneNumber = "111111111", City = "City1", Address = "Address1", Country = "Country1", State = "State1", PaymentMethod = "Cash", GiftWrap = false, RushOrder = false},
            //    new Order{OrderID = "110062013040512", SubmittedBy = 2, ProcessedBy = 0, Delivered = false, Paid = false, TheOrderIsExecuted = false, UserName = "User2", PhoneNumber = "2222222222", City = "City2", Address = "Address2", Country = "Country2", State = "State2", PaymentMethod = "Cash", GiftWrap = false, RushOrder = false},
            //    new Order{OrderID = "110062013040513", SubmittedBy = 3, ProcessedBy = 0, Delivered = false, Paid = false, TheOrderIsExecuted = false, UserName = "User3", PhoneNumber = "3333333333", City = "City3", Address = "Address3", Country = "Country3", State = "State3", PaymentMethod = "Cash", GiftWrap = false, RushOrder = false},
            //    new Order{OrderID = "110062013040514", SubmittedBy = 1, ProcessedBy = 0, Delivered = false, Paid = false, TheOrderIsExecuted = false, UserName = "User1", PhoneNumber = "111111111", City = "City1", Address = "Address1", Country = "Country1", State = "State1", PaymentMethod = "Cash", GiftWrap = false, RushOrder = false}
            //}.AsQueryable());
            //ninjectKernel.Bind<IOrderRepository>().ToConstant(mockOrderRepository.Object);
            ninjectKernel.Bind<IOrderRepository>().To<EFOrderRepository>();

            //Mock<IOrderProductRepository> mockOrderProductRepository = new Mock<IOrderProductRepository>();
            //mockOrderProductRepository.Setup(m => m.OrderProducts).Returns(new List<OrderProduct>
            //{
            //    new OrderProduct{ ID = 1, OrderID = "110062013040511", ProductID = 1, Price = 12, Quantity = 1},
            //    new OrderProduct{ ID = 2, OrderID = "110062013040511", ProductID = 2, Price = 24, Quantity = 2},
            //    new OrderProduct{ ID = 3, OrderID = "110062013040512", ProductID = 1, Price = 12, Quantity = 3},
            //    new OrderProduct{ ID = 4, OrderID = "110062013040513", ProductID = 1, Price = 24, Quantity = 4},
            //    new OrderProduct{ ID = 5, OrderID = "110062013040513", ProductID = 3, Price = 24, Quantity = 5},
            //    new OrderProduct{ ID = 6, OrderID = "110062013040514", ProductID = 1, Price = 12, Quantity = 6},
            //    new OrderProduct{ ID = 7, OrderID = "110062013040514", ProductID = 4, Price = 24, Quantity = 7},
            //}.AsQueryable());
            //ninjectKernel.Bind<IOrderProductRepository>().ToConstant(mockOrderProductRepository.Object);
            ninjectKernel.Bind<IOrderProductRepository>().To<EFOrderProductRepository>();

            //EmailSettings emailSettings = new EmailSettings
            //{
            //    WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            //};
            //ninjectKernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
            ninjectKernel.Bind<IOrderProcessor>().To<DBOrderProcessor>();

            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}