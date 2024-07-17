using Application.Product;
using Data.EF.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViewModel.System;
using ViewModel.Users;
using WebApp.Models;

namespace WebApp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductServicecs _productService;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        public HomeController(ILogger<HomeController> logger,
            IProductServicecs productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 20)
        {
            var latesProduct = await _productService.GetLatestProducts(SystemContants.NumberLatesProduct);
            var allProduct = await _productService.GetPagingProducts(pageIndex,pageSize); 
            var homeVM = new HomeViewModel
            {
                LatestProducts = latesProduct,
                AllProduct = allProduct
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}