using BookStore.Data.Repositories;
using BookStore.Data.Static;
using BookStore.Data.ViewModels;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BookStore.Controllers
{
    //[Authorize(Roles = UserRoles.Admin)]
    public class SubCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var subCategories = _unitOfWork.SubCategories.GetAllSubCategories();
            return View(subCategories);
        }

        [AllowAnonymous]
        public IActionResult ListBySubcategory(int id)
        {
            var categories = _unitOfWork.Categories.GetAll().OrderBy(x => x.Id).ToList();
            List<SubCategory> subCategories;
            subCategories = _unitOfWork.SubCategories.GetAll().Where(x => x.CategoryId == id).OrderBy(x => x.Id).ToList();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = id;

            return View(subCategories);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            var subCategoryDetails = _unitOfWork.SubCategories.GetSubCategoryById(id);
            if (subCategoryDetails == null) return View("NotFound");
            return View(subCategoryDetails);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var dropDown = _unitOfWork.SubCategories.GetDropDownValues();
            ViewBag.Categories = new SelectList(dropDown.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(SubCategoryVM subCategoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View(subCategoryVM);

            }

            _unitOfWork.SubCategories.AddNewSubCategory(subCategoryVM);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subCategoryDetails = _unitOfWork.SubCategories.GetSubCategoryById(id);
            if (subCategoryDetails == null) return View("NotFound");
            var response = new SubCategoryVM();
            response.Id = subCategoryDetails.Id;
            response.Name = subCategoryDetails.Name;
            response.CategoryId = subCategoryDetails.CategoryId;


            var dropDown = _unitOfWork.SubCategories.GetDropDownValues();
            ViewBag.Categories = new SelectList(dropDown.Categories, "Id", "Name");
            return View(response);
        }

        [HttpPost]
        public IActionResult Edit(int id, SubCategoryVM subCategoryVM)
        {
            if (id != subCategoryVM.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                //var dropDown = _unitOfWork.SubCategories.GetDropDownValues();
                //ViewBag.Categories = new SelectList(dropDown.Categories, "Id", "Name");
                return View(subCategoryVM);
            }
            _unitOfWork.SubCategories.UpdateSubCategory(subCategoryVM);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var subCategoryDetails = _unitOfWork.SubCategories.GetSubCategoryById(id);
            if (subCategoryDetails == null) return View("NotFound");
            return View(subCategoryDetails);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {

            var subCategoryDetails = _unitOfWork.SubCategories.GetSubCategoryById(id);
            if (subCategoryDetails == null) return View("NotFound");
            _unitOfWork.SubCategories.Delete(id);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
