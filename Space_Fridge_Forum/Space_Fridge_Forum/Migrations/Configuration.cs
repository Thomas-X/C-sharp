using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Space_Fridge_Forum.Models;

namespace Space_Fridge_Forum.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Space_Fridge_Forum.Models.ApplicationDbContext>
    {
        // g
        // ml
        private string[][] IngredientTypes =
            {
            new[] {"g", "fusilli" },
            new[] {"g", "lasagnabladen"},
            new[] {"g", "macaroni"},
            new[] {"g", "spaghetti"},
            new[] {"g", "tortellini"},
            new[] {"g", "aardappelen"},
            new[] {"g", "bloemkool"},
            new[] {"g", "broccoli"},
            new[] {"g", "boerenkool"},
            new[] {"g", "bruine bonen"},
            new[] {"g", "champignons"},
            new[] {"g", "courgette"},
            new[] {"g", "komkommer"},
            new[] {"g", "paprika"},
            new[] {"g", "prei"},
            new[] {"g", "spinazie"},
            new[] {"g", "spruitjes"},
            new[] {"g", "taugé"},
            new[] {"g", "tomaat"},
            new[] {"g", "ui"},
            new[] {"g", "witlof"},
            new[] {"g", "witte bonen"},
            new[] {"g", "wortel"},
            new[] {"g", "zuurkool"},
            new[] {"g", "aardbei"},
            new[] {"g", "ananas"},
            new[] {"g", "appel"},
            new[] {"g", "avocado"},
            new[] {"g", "banaan"},
            new[] {"g", "blauwe bessen"},
            new[] {"g", "citroen"},
            new[] {"g", "kiwi"},
            new[] {"g", "mandarijn"},
            new[] {"g", "meloen"},
            new[] {"g", "olijf"},
            new[] {"g", "peer"},
            new[] {"g", "perzik"},
            new[] {"g", "pruim"},
            new[] {"g", "boter"},
            new[] {"g", "boterham"},
            new[] {"g", "bouillon"},
            new[] {"g", "couscous"},
            new[] {"ml", "creme fraiche"},
            new[] {"g", "eieren"},
            new[] {"g", "kaas"},
            new[] {"ml", "ketchup"},
            new[] {"ml", "ketjap"},
            new[] {"g", "knoflook"},
            new[] {"ml", "mayonaise"},
            new[] {"ml", "melk"},
            new[] {"ml", "olijfolie"},
            new[] {"g", "peper"},
            new[] {"g", "pinda"},
            new[] {"g", "rijst"},
            new[] {"g", "sojasaus"},
            new[] {"g", "suiker"},
            new[] {"ml", "tomatensaus"},
            new[] {"ml", "water"},
            new[] {"g", "zout"},
            new[] {"g", "forel"},
            new[] {"g", "garnalen"},
            new[] {"g", "makreel"},
            new[] {"g", "sliptong"},
            new[] {"g", "tonijn"},
            new[] {"g", "zalm"},
            new[] {"g", "braadworst"},
            new[] {"g", "karbonade"},
            new[] {"g", "knakworst"},
            new[] {"g", "slavink"},
            new[] {"g", "spek"},
            new[] {"g", "varkensgehakt"},
            new[] {"g", "biefstuk"},
            new[] {"g", "kogelbiefstuk"},
            new[] {"g", "hamblokjes"},
            new[] {"g", "hamburger"},
            new[] {"g", "rundergehakt"},
            new[] {"g", "tartaar"},
            new[] {"g", "tournedos"},
            new[] {"g", "kalkoenfilet"},
            new[] {"g", "kipfilet"},
            new[] {"g", "kippenpoot"},
            new[] {"g", "kipschnitzel"},
            new[] {"g", "lamskoteletten"},
            new[] { "g", "shoarma"},
        };

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Space_Fridge_Forum.Models.ApplicationDbContext context)
        {
            Dictionary<string, IngredientUnit> ingredientUnits = new Dictionary<string, IngredientUnit>()
            {
                // Weight
                {"g", new IngredientUnit() {Unit = "g"}},
                // Fluid
                {"ml", new IngredientUnit() {Unit = "ml"}},
            };
            
            var ingredientTypeMaker = new Func<string[], IngredientType>((type) =>
                new IngredientType() {Type = type[1], IngredientUnit = ingredientUnits[type[0]]});

            var _ingredientTypes = new List<IngredientType>();

            foreach (var ingredientType in IngredientTypes)
            {
                _ingredientTypes.Add(ingredientTypeMaker(ingredientType));
            }

            context.IngredientTypes.AddOrUpdate(x => x.Id, _ingredientTypes.ToArray());

            context.SaveChanges();

            // https://i.pinimg.com/564x/46/66/9d/46669d8dfef675c0b6bcf7bd66ab448f.jpg
        }
    }
}