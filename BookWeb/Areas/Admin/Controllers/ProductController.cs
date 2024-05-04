using Book.DataAceess.Repositories.Interfaces;
using Book.Models.ViewModel;
using Book.Utilites;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController(IUnitOfWork unit, IWebHostEnvironment webHost) : Controller
    {
        public IActionResult Index()
        {
            var products = unit.Product.GetAll(includeProperty: "Category").ToList();
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                CategoryList = unit.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id is null or 0)
            {
                return View(productVM); //Create
            }
            else
            {
                //update
                productVM.Product = unit.Product.Get(x => x.Id == id, includeProperties: "ProductImages");
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    unit.Product.Add(productVM.Product);
                }
                else
                {
                    unit.Product.Update(productVM.Product);
                }

                unit.Save();
                string webRootPath = webHost.WebRootPath;
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string productPath = @"images\products\product-" + productVM.Product.Id;
                            string finalPath = Path.Combine(webRootPath, productPath);
                            if (!Directory.Exists(finalPath))
                                Directory.CreateDirectory(finalPath);
                            using var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create);
                            file.CopyTo(fileStream);
                            ProductImage productImage = new()
                            {
                                ImageUrl = @"\" + productPath + @"\" + fileName,
                                ProductId = productVM.Product.Id,
                            };
                            productVM.Product.ProductImages ??= new List<ProductImage>();
                            productVM.Product.ProductImages.Add(productImage);
                        }
                    }

                    unit.Product.Update(productVM.Product);
                    unit.Save();
                }
                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            productVM.CategoryList = unit.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(productVM);
        }

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = unit.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                        Path.Combine(webHost.WebRootPath,
                            imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                unit.ProductImage.Remove(imageToBeDeleted);
                unit.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }
        #region Api Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = unit.Product.GetAll(includeProperty: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = unit.Product.Get(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(webHost.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(finalPath);
            }

            unit.Product.Remove(product);
            unit.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
