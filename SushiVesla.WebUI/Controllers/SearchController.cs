using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.WebUI.Models;

namespace SushiVesla.WebUI.Controllers
{
    public class SearchController : Controller
    {
        public int PageSize = 5;
        private IProductRepository productRepository;

        public SearchController(IProductRepository prodRepository)
        {
            productRepository = prodRepository;
        }

        public ActionResult Index(int page = 1, string textForSearch = "")
        {
            ViewBag.SelectedMenuItem = "Home";

            SearchResultsViewModel viewModel;

            if (textForSearch == "")
            {
                viewModel = new SearchResultsViewModel();
            }
            else
            {
                viewModel = new SearchResultsViewModel
                {
                    Products = productRepository.Products
                    .Where(p => p.Name.ToLower().Contains(textForSearch.ToLower()))
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = productRepository.Products.Where(p => p.Name.ToLower().Contains(textForSearch.ToLower())).Count()
                    },
                    TextForSearch = textForSearch
                };
            }
            return View(viewModel);
        }
    }
}
