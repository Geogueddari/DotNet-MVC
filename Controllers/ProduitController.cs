using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Controllers
{
    public class ProduitController : Controller
    {
      
            private readonly ApplicationDbContext _db;
            private readonly IWebHostEnvironment _webHostEnvironment;
            public ProduitController(ApplicationDbContext db, IWebHostEnvironment webHost)
            {
                _db = db;
                _webHostEnvironment = webHost;
            }
            public IActionResult Index()
            {
                 List<Produit> ProduitList = _db.Produits.ToList();
                return View(ProduitList);
            }
            [Authorize]
            public IActionResult Create()
            {
                ProduitVM produitVM = new()
                {
                    categorieListe = _db.Categories.ToList().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                    produit = new Produit()
                };
                return View(produitVM);
            }
            [HttpPost]
            public IActionResult Create(ProduitVM obj, IFormFile file)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);//donner un nom aléatoire
                    string filePath = Path.Combine(wwwRootPath, @"Images");
                    using (var fileStrem = new FileStream(Path.Combine(filePath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStrem);
                    }
                    obj.produit.imageURL = @"Images\" + fileName;
                }
                if (ModelState.IsValid)
                {
                    _db.Produits.Add(obj.produit);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else return View();
            }
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Récupérer le produit à modifier
            Produit obj = _db.Produits.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            // Préparer le ViewModel
            ProduitVM produitVM = new()
            {
                produit = obj,
                categorieListe = _db.Categories.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name, // Nom de la catégorie
                    Value = u.Id.ToString(), // ID de la catégorie
                    Selected = (u.Id == obj.categorieId) // Sélectionne la catégorie actuelle
                })
            };

            return View(produitVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProduitVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Gérer l'image
                if (file != null && file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.produit.imageURL = @"Images/" + fileName;
                }
                else
                {
                    // Préserver l'image existante
                    var existingProduct = _db.Produits.AsNoTracking().FirstOrDefault(p => p.Id == obj.produit.Id);
                    obj.produit.imageURL = existingProduct?.imageURL;
                }

                // Sauvegarder le produit
                _db.Produits.Update(obj.produit);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            else 
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Vous pouvez utiliser un logger pour plus de visibilité
                }
            }

            return View(obj);
        }

        [Authorize]
        public IActionResult Delete(int? id)
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                Produit obj = _db.Produits.Find(id);
                if (obj == null)
                {
                    return NotFound();
                }
                return View(obj);
            }
            [HttpPost, ActionName("Delete")]
            public IActionResult DeletePost(int? id)
            {
                Produit obj = _db.Produits.Find(id);
                if (obj == null)
                    return NotFound();
                _db.Produits.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
}
