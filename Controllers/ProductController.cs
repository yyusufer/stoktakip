// Controllers/ProductController.cs
using Microsoft.AspNetCore.Mvc;
using Takip.Models;
using Takip.Repositories;

namespace Takip.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _repository;

        public ProductController(ProductRepository repository)
        {
            _repository = repository;
        }

        // Tüm ürünleri listele
        public IActionResult Index()
        {
            var products = _repository.GetAllProducts();
            return View(products);
        }

        // Yeni ürün formu
        public IActionResult Create()
        {
            return View();
        }

        // Ürün ekle (POST)
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                _repository.AddProduct(product);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(product);
            }
        }

        // Ürün sil (POST)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Silme işlemi başarısız: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}