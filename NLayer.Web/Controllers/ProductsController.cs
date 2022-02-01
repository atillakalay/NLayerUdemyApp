using Microsoft.AspNetCore.Mvc;
using NLayer.Service.Service;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductsWithCategory());
        }
    }
}
