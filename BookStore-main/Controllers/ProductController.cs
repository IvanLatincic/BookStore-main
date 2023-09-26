using BookStore.Data.Repositories;
using BookStore.Data.ViewModels;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;
        }

        [AllowAnonymous]
        public IActionResult Index(string searchString, string minPrice, string maxPrice, int pg =1)
        {
            var products = _unitOfWork.Products.GetAllProducts();
            ViewBag.User = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) || x.Author.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                var min = int.Parse(minPrice);
                products = products.Where(x => x.Price >= min);
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                var max = int.Parse(maxPrice);
                products = products.Where(x => x.Price <= max);
            }

            const int pageSize = 10;
            if (pg < 1) pg = 1;

            int count = products.Count();

            var pager = new Pager(count, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;
            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();


            ViewBag.Pager = pager;


            return View(data);
        }

        [AllowAnonymous]
        public IActionResult ListBySubCategories(int id, string searchString, string minPrice, string maxPrice, int page = 1)
        {
            ViewBag.User = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = _unitOfWork.Products.GetAllProducts().Where(x => x.SubCategoryId == id);


            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) || x.Author.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                var min = int.Parse(minPrice);
                products = products.Where(x => x.Price >= min);
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                var max = int.Parse(maxPrice);
                products = products.Where(x => x.Price <= max);
            }

            const int pageSize = 10;
            if (page < 1) page = 1;

            int count = products.Count();

            var pager = new Pager(count, page, pageSize);

            int recSkip = (page - 1) * pageSize;
            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult ListOnlyCurrentUsersProducts()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var p = _unitOfWork.Products.GetAllProducts().Where(x => x.UserId == userId).ToList();
            return View(p);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            ViewBag.User = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var productDetails = _unitOfWork.Products.GetProductById(id);
            if (productDetails == null) return View("NotFound");
            return View(productDetails);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var dropDown = _unitOfWork.Products.GetDropDownValues();
            ViewBag.Categories = new SelectList(dropDown.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (!User.Identity.IsAuthenticated)
            {
                //var dropDown = _unitOfWork.Products.GetDropDownValues();

                //ViewBag.Categories = new SelectList(dropDown.Categories, "Id", "Name");

                return View(productVM);

            }

            //if (productVM.ProductPicture != null)
            //{
            //    var productname = $"{Guid.NewGuid()} - {productVM.ProductPicture.FileName}";
            //    var src = "/images" + productname;
            //    var path = Path.Combine(_webHost.WebRootPath, src);
            //    using (var fileStream = new FileStream(path, FileMode.Create))
            //    {
            //        productVM.ProductPicture.CopyTo(fileStream);
            //    }
            //    productVM.ImageUrl = src;
            //}

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _unitOfWork.Products.AddNewProduct(productVM, userId);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetSubCategory(int categoryId)
        {
            var subCategory = _unitOfWork.SubCategories.GetAllSubCategories().Where(x => x.CategoryId == categoryId);
            return Json(new SelectList(subCategory, "Id", "Name"));
        }

        public IActionResult Edit(int id)
        {
            var productDetails = _unitOfWork.Products.GetProductById(id);
            if (productDetails == null) return View("NotFound");
            var response = new ProductVM();
            response.Id = productDetails.Id;
            response.Name = productDetails.Name;
            response.Author = productDetails.Author;
            response.CategoryId = productDetails.CategoryId;
            response.SubCategoryId = productDetails.SubCategoryId;
            response.Language = productDetails.Language;
            response.State = productDetails.State;
            response.Description = productDetails.Description;
            response.Price = productDetails.Price;

            var dropDown = _unitOfWork.Products.GetDropDownValues();
            ViewBag.Categories = new SelectList(dropDown.Categories, "Id", "Name");
            ViewBag.SubCategories = new SelectList(dropDown.SubCategories, "Id", "Name");
            return View(response);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductVM productVM)
        {
            if (id != productVM.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var dropDown = _unitOfWork.Products.GetDropDownValues();
                ViewBag.Categories = new SelectList(dropDown.Categories, "Id", "Name");
                ViewBag.SubCategories = new SelectList(dropDown.SubCategories, "Id", "Name");
                return View(productVM);
            }
            _unitOfWork.Products.UpdateProduct(productVM);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var productDetails = _unitOfWork.Products.GetProductById(id);
            if (productDetails == null) return View("NotFound");
            return View(productDetails);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var productDetails = _unitOfWork.Products.GetProductById(id);
            if (productDetails == null) return View("NotFound");
            _unitOfWork.Products.Delete(id);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
