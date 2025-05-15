using Microsoft.AspNetCore.Mvc;
using Takip.Models;
using Takip.Repositories;

namespace Takip.Controllers
{
    public class SaleController : Controller
    {
        private readonly SaleRepository _saleRepo;
        private readonly ProductRepository _productRepo;
        private readonly PersonnelRepository _personnelRepo;

        public SaleController(SaleRepository saleRepo, ProductRepository productRepo, PersonnelRepository personnelRepo)
        {
            _saleRepo = saleRepo;
            _productRepo = productRepo;
            _personnelRepo = personnelRepo;
        }

        public IActionResult Index()
        {
            var sales = _saleRepo.GetAllSales();
            var products = _productRepo.GetAllProducts();
            var personnel = _personnelRepo.GetAllPersonnel();

            var saleViews = sales.Select(s => new
            {
                ProductName = products.First(p => p.Id == s.ProductId).Name,
                PersonnelName = personnel.First(p => p.Id == s.PersonnelId).Name,
                s.Quantity,
                s.Price,
                s.Time
            }).ToList();

            return View(saleViews);
        }

        public IActionResult Create()
        {
            ViewBag.Products = _productRepo.GetAllProducts();
            ViewBag.Personnel = _personnelRepo.GetAllPersonnel();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int productId, int personnelId, int quantity)
        {
            var product = _productRepo.GetAllProducts().First(p => p.Id == productId);
            var totalPrice = product.Price * quantity;

            var sale = new Sale
            {
                ProductId = productId,
                PersonnelId = personnelId,
                Quantity = quantity,
                Price = totalPrice,
                Time = DateTime.Now
            };

            _saleRepo.AddSale(sale);
            return RedirectToAction("Index");
        }
    }
}
