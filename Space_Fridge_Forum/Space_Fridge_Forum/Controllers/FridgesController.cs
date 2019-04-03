using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Space_Fridge_Forum.Models;
using Space_Fridge_Forum.Models.ViewModels;

namespace Space_Fridge_Forum.Controllers
{
    [Authorize]
    public class FridgesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fridges
        public ActionResult Index()
        {
            var fridges = db.Fridges.Include(f => f.User);
            var filterFridges = fridges.ToList().Where(x => x.UserId == User.Identity.GetUserId());
            return View(filterFridges);
        }
        // GET: Fridges/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fridge fridge = db.Fridges.Find(id);
            if (fridge == null)
            {
                return HttpNotFound();
            }
            if (fridge.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            var mdl = new EditFridgeViewModel()
            {
                HexColor = fridge.HexColor,
                Ingredients = fridge.Ingredients,
                IngredientTypes = db.IngredientTypes.ToList(),
                IngredientUnits = db.IngredientUnits.ToList(),
                Name = fridge.Name,
                User = fridge.User,
                UserId = fridge.UserId
            };
            return View(mdl);

        }
        
        // GET: Fridges/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fridge fridge = db.Fridges.Find(id);
            if (fridge == null)
            {
                return HttpNotFound();
            }
            if (fridge.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", fridge.UserId);
            var mdl = new EditFridgeViewModel()
            {
                HexColor = fridge.HexColor,
                Ingredients = fridge.Ingredients,
                IngredientTypes = db.IngredientTypes.ToList(),
                IngredientUnits = db.IngredientUnits.ToList(),
                Name = fridge.Name,
                User = fridge.User,
                UserId = fridge.UserId
            };
            return View(mdl);
        }

        //TODO DRY (RecipesController.GetIngredients)
        public List<Ingredient> GetIngredients()
        {
            var ingredients = new List<Ingredient>();
            Ingredient currentIngredient = null;
            foreach (var formAllKey in Request.Form.AllKeys)
            {
                if (currentIngredient == null)
                {
                    currentIngredient = new Ingredient();
                }

                if (currentIngredient.Value == 0 && formAllKey.Contains("ingredientvalue"))
                {
                    int.TryParse(Request.Form[formAllKey], out var val);
                    if (val == 0)
                    {
                        currentIngredient = null;
                        continue;
                    }

                    currentIngredient.Value = val;
                }

                if (currentIngredient.IngredientType == null && formAllKey.Contains("type"))
                {
                    int.TryParse(Request.Form[formAllKey], out var val);
                    currentIngredient.IngredientType = db.IngredientTypes.First(x => x.Id == val);
                }

                if (currentIngredient.IngredientUnit == null && formAllKey.Contains("unit"))
                {
                    int.TryParse(Request.Form[formAllKey], out var val);
                    currentIngredient.IngredientUnit = db.IngredientUnits.First(x => x.Id == val);
                }

                if (currentIngredient.IngredientUnit != null && currentIngredient.IngredientType != null &&
                    currentIngredient.Value > 0)
                {
                    ingredients.Add(currentIngredient);
                    currentIngredient = null;
                }
            }

            return ingredients;
        }

        // POST: Fridges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public ActionResult Edit_Post(string id)
        {
            var fridge = db.Fridges.Where(x => x.UserId == id).Include(x => x.User).Include(x => x.Ingredients).ToList()[0];
            if (fridge.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                fridge.HexColor = Request.Form["HexColor"];
                fridge.Name = Request.Form["Name"];
                db.Entry(fridge).State = EntityState.Modified;
                db.SaveChanges();

                fridge.Ingredients = new List<Ingredient>();
                db.Entry(fridge).State = EntityState.Modified;
                db.SaveChanges();

                fridge.Ingredients = GetIngredients();
                db.Entry(fridge).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", fridge.UserId);
            return View(fridge);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
