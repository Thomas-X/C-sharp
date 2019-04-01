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
    public class RecipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipes
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Recipes.ToList());
        }

        [AllowAnonymous]
        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            // pass all ingredients to view
            var mdl = new CreateRecipeViewModel()
            {
                IngredientTypes = db.IngredientTypes.ToList(),
                IngredientUnits = db.IngredientUnits.ToList()
            };
            return View(mdl);
        }

        public List<Ingredient> GetIngredients()
        {
            var ingredients = new List<Ingredient>();
            var createIngredientSwitch = false;
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

                if (currentIngredient.IngredientUnit != null && currentIngredient.IngredientType != null && currentIngredient.Value > 0)
                {
                    ingredients.Add(currentIngredient);
                    currentIngredient = null;
                }
            }

            return ingredients;
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,People,Description")]
            Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                var vals = Request.Form.AllKeys;
                var value1 = Request.Form["type-0"];
                recipe.Ingredients = GetIngredients();
                recipe.User = db.Users.Find(User.Identity.GetUserId());
                db.Recipes.Add(recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            if (recipe.User.Id != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,People,Description")]
            Recipe recipe)
        {
            if (recipe.User.Id != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Recipe recipe = db.Recipes.Find(id);
            if (recipe.User.Id != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            if (recipe.User.Id != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            db.Recipes.Remove(recipe);
            db.SaveChanges();
            return RedirectToAction("Index");
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