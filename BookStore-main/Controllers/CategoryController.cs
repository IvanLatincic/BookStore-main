using BookStore.Data.Repositories;
using BookStore.Data.Static;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BookStore.Controllers
{
    //[Authorize(Roles = UserRoles.Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Categories.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            _unitOfWork.Categories.Add(category);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var categoryDetails = _unitOfWork.Categories.GetById(id);
            if (categoryDetails == null) return View("NotFound");
            return View(categoryDetails);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categoryDetails = _unitOfWork.Categories.GetById(id);
            if (categoryDetails == null) return View("NotFound");
            return View(categoryDetails);
        }

        [HttpPost]
        public IActionResult Edit(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _unitOfWork.Categories.Update(id, category);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var categoryDetails = _unitOfWork.Categories.GetById(id);
            if (categoryDetails == null) return View("NotFound");
            return View(categoryDetails);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var categoryDetails = _unitOfWork.Categories.GetById(id);
            if (categoryDetails == null) return View("NotFound");
            _unitOfWork.Categories.Delete(id);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
