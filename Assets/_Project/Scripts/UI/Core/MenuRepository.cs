using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MenuRepository
{
    public static readonly string[] Categories =
    {
        "Recomendados",
        "Desayunos",
        "Antipasti",
        "Zuppe",
        "Insalate",
        "Pizzas",
        "Pasta",
        "Pasta Ripiena",
        "Della Cucina",
        "Abbondanza",
        "Postres",
        "Bebidas",
        "Vinos",
        "Infantil"
    };

    public static List<DishData> GetDishes()
    {
        List<DishData> dishes = new List<DishData>();

        AddDesayunos(dishes);
        AddComidasBase(dishes);
        AddPizzas(dishes);
        AddPastas(dishes);
        AddDellaCucina(dishes);
        AddAbbondanza(dishes);
        AddPostres(dishes);
        AddBebidas(dishes);
        AddVinos(dishes);
        AddInfantil(dishes);

        return dishes;
    }

    public static DishData FindById(string dishId)
    {
        return GetDishes().FirstOrDefault(dish => dish.Id == dishId);
    }

    private static DishData D(
        string id,
        string name,
        string section,
        string category,
        string subCategory,
        string description,
        string portion,
        int calories,
        bool hasAR,
        bool recommended,
        string icon,
        string imageKey,
        string[] ingredients,
        string[] preparation,
        string[] tags,
        bool isAlcoholic = false,
        bool isKidsMenu = false,
        bool isShareable = false,
        bool vegetarian = false,
        bool seafood = false,
        bool pork = false,
        bool beef = false,
        bool chicken = false,
        bool dairy = true,
        bool gluten = true,
        bool nuts = false,
        SpiceLevel spiceLevel = SpiceLevel.None,
        DishTemperature temperature = DishTemperature.Hot,
        ARModelStatus modelStatus = ARModelStatus.Ready)
    {
        float portionAmount = ParseAmount(portion);
        string portionUnit = ParseUnit(portion);
        string modelPath = $"Assets/_Project/Prefabs/AR_Dishes/{id}.prefab";

        NutritionProfile nutrition = BuildEstimatedNutrition(
            category,
            calories,
            seafood,
            pork,
            beef,
            chicken,
            dairy,
            gluten,
            nuts,
            isAlcoholic,
            isKidsMenu
        );

        string[] allergens = BuildAllergens(seafood, pork, beef, chicken, dairy, gluten, nuts, isAlcoholic);
        string[] warnings = BuildWarnings(isAlcoholic, isKidsMenu, seafood, nuts, dairy, gluten);
        string[] nutritionFacts = BuildNutritionFacts(portion, calories, nutrition);
        string[] pairings = BuildPairings(category, isAlcoholic, isKidsMenu, spiceLevel);

        return new DishData(
            id: id,
            name: name,
            menuSection: section,
            category: category,
            subCategory: subCategory,
            description: description,
            story: BuildStory(name, category),
            portion: portion,
            portionAmount: portionAmount,
            portionUnit: portionUnit,
            calories: calories > 0 ? $"{calories} kcal" : "Variable / consultar menú",
            caloriesValue: calories,
            hasAR: true,
            recommended: recommended,
            icon: icon,
            imageKey: imageKey,
            imageResourcePath: $"Images/Menu/{imageKey}",
            officialImageReference: $"Página oficial Italianni’s / imagen: {imageKey}",
            modelPrefabPath: modelPath,
            tripoPrompt: string.Empty,
            ingredients: ingredients,
            preparationNotes: preparation,
            nutritionFacts: nutritionFacts,
            warnings: warnings,
            allergens: allergens,
            tags: tags,
            pairings: pairings,
            nutrition: nutrition,
            arMetadata: BuildARMetadata(category, ARModelStatus.Ready),
            isAlcoholic: isAlcoholic,
            isKidsMenu: isKidsMenu,
            isShareable: isShareable,
            isVegetarianFriendly: vegetarian,
            containsSeafood: seafood,
            containsPork: pork,
            containsBeef: beef,
            containsChicken: chicken,
            containsDairy: dairy,
            containsGluten: gluten,
            containsNuts: nuts,
            spiceLevel: spiceLevel,
            temperature: temperature,
            baseDataQuality: DataQuality.Official,
            nutritionDataQuality: DataQuality.Estimated
        );
    }

    private static void AddDesayunos(List<DishData> dishes)
    {
        const string s = "Desayunos";

        dishes.Add(D(
            "omelette_champinones",
            "Omelette de Champiñones",
            s,
            "Desayunos",
            "Omelettes",
            "Relleno de champiñones, queso panela y epazote, bañado en salsa de quesos, gratinado con Mozzarella y acompañado de pan de ajo con perejil.",
            "446 g",
            795,
            false,
            false,
            "🍳",
            "omelette_champinones_italiannis",
            new[] { "Huevo", "Champiñones", "Queso panela", "Epazote", "Salsa de quesos", "Queso Mozzarella", "Pan de ajo", "Perejil", "Mantequilla", "Sal", "Pimienta" },
            new[] { "Omelette relleno", "Gratinado", "Servido con pan de ajo" },
            new[] { "Desayuno", "Omelette", "Queso", "Champiñones" },
            vegetarian: true,
            dairy: true,
            gluten: true
        ));

        dishes.Add(D(
            "omelette_di_formaggio",
            "Omelette di Formaggio",
            s,
            "Desayunos",
            "Omelettes",
            "Relleno de combinación cremosa de quesos, espinacas, alcachofas y champiñones, servido sobre salsa cremosa de chipotle y pan toscano con mantequilla al ajo.",
            "558 g",
            993,
            false,
            false,
            "🍳",
            "omelette_di_formaggio_italiannis",
            new[] { "Huevo", "Quesos mixtos", "Espinacas", "Alcachofas", "Champiñones", "Chipotle", "Crema", "Pan toscano", "Mantequilla al ajo", "Sal", "Pimienta" },
            new[] { "Omelette relleno", "Servido sobre salsa cremosa", "Acompañado con pan toscano" },
            new[] { "Desayuno", "Omelette", "Queso", "Cremoso", "Picante suave" },
            vegetarian: true,
            spiceLevel: SpiceLevel.Mild
        ));

        dishes.Add(D(
            "huevos_al_forno",
            "Huevos al Forno",
            s,
            "Desayunos",
            "Huevos",
            "Huevos horneados y cubiertos con salsa alla Bolognese, gratinados y acompañados de pan de ajo della Casa.",
            "478 g",
            928,
            false,
            false,
            "🍳",
            "huevos_al_forno_italiannis",
            new[] { "Huevo", "Salsa alla Bolognese", "Carne de res", "Jitomate", "Queso Mozzarella", "Queso Parmesano", "Pan de ajo", "Hierbas italianas" },
            new[] { "Horneado", "Gratinado", "Servido caliente" },
            new[] { "Desayuno", "Huevos", "Bolognese", "Gratinado" },
            beef: true
        ));

        dishes.Add(D(
            "omelette_leggero",
            "Omelette Leggero",
            s,
            "Desayunos",
            "Omelettes",
            "Omelette de claras relleno de champiñones asados, servido sobre salsa Pomodoro y albahaca fresca.",
            "462 g",
            361,
            false,
            false,
            "🍳",
            "omelette_leggero_italiannis",
            new[] { "Claras de huevo", "Champiñones asados", "Salsa Pomodoro", "Albahaca fresca", "Aceite de oliva", "Sal", "Pimienta" },
            new[] { "Preparado con claras", "Servido con salsa Pomodoro", "Opción más ligera" },
            new[] { "Desayuno", "Ligero", "Claras", "Vegetariano" },
            vegetarian: true,
            dairy: false
        ));

        dishes.Add(D(
            "omelette_di_asparagi",
            "Omelette di Asparagi",
            s,
            "Desayunos",
            "Omelettes",
            "Omelette con espárragos, queso Cenizo de Cabra y salsa cremosa con un toque de chipotle, acompañado de pan toscano con mantequilla al ajo.",
            "412 g",
            715,
            false,
            false,
            "🍳",
            "omelette-di-asparagi_italiannis",
            new[] { "Huevo", "Espárragos", "Queso Cenizo de Cabra", "Chipotle", "Crema", "Pan toscano", "Mantequilla al ajo", "Sal", "Pimienta" },
            new[] { "Omelette relleno", "Servido con salsa cremosa", "Acompañado con pan" },
            new[] { "Desayuno", "Espárragos", "Queso de cabra", "Picante suave" },
            vegetarian: true,
            spiceLevel: SpiceLevel.Mild
        ));

        dishes.Add(D(
            "huevo_tocino_alla_italiana",
            "Huevo con Tocino alla Italiana",
            s,
            "Desayunos",
            "Huevos",
            "Huevos revueltos con calabazas y tocino con un toque de chipotle, gratinados con Mozzarella y rebanadas de pan toscano, servidos sobre salsa Alfredo y Pesto.",
            "422 g",
            848,
            false,
            false,
            "🥓",
            "huevos_alla_italiana-italiannis",
            new[] { "Huevo", "Tocino", "Calabaza", "Chipotle", "Queso Mozzarella", "Pan toscano", "Salsa Alfredo", "Pesto", "Albahaca", "Ajo" },
            new[] { "Huevos revueltos", "Gratinado", "Servido sobre salsa" },
            new[] { "Desayuno", "Tocino", "Queso", "Chipotle" },
            pork: true,
            spiceLevel: SpiceLevel.Mild
        ));

        dishes.Add(D(
            "enfrijoladas_cecina",
            "Enfrijoladas con Cecina",
            s,
            "Desayunos",
            "Mexicano",
            "Enfrijoladas rellenas de huevo revuelto, bañadas en salsa cremosa de frijol y acompañadas con cecina de res.",
            "549 g",
            1071,
            false,
            false,
            "🌯",
            "enfrijoladas_cecina_italiannis",
            new[] { "Tortilla", "Huevo revuelto", "Frijol", "Crema", "Cecina de res", "Queso", "Cebolla", "Aceite", "Sal" },
            new[] { "Rellenas de huevo", "Bañadas en salsa cremosa de frijol", "Servidas con cecina" },
            new[] { "Desayuno", "Enfrijoladas", "Cecina", "Mexicano" },
            beef: true
        ));

        dishes.Add(D(
            "enchiladas_suizas",
            "Enchiladas Suizas",
            s,
            "Desayunos",
            "Mexicano",
            "Enchiladas rellenas de pollo, queso o huevo revuelto, gratinadas con queso Gouda y acompañadas de frijoles refritos.",
            "768 g",
            1496,
            false,
            false,
            "🌯",
            "enchiladas_suizas_italiannis",
            new[] { "Tortilla", "Pollo", "Queso", "Huevo", "Salsa verde", "Queso Gouda", "Crema", "Frijoles refritos", "Cebolla" },
            new[] { "Rellenas", "Gratinadas", "Acompañadas de frijoles" },
            new[] { "Desayuno", "Enchiladas", "Gratinado" },
            chicken: true
        ));
    }

    private static void AddComidasBase(List<DishData> dishes)
    {
        const string s = "Comidas";

        dishes.Add(D(
            "insalata_pera_gorgonzola",
            "Insalata di Pera e Gorgonzola",
            s,
            "Insalate",
            "Ensaladas",
            "Mezcla de lechugas con aderezo dulce, tiras de pechuga asada a la parrilla, nuez, pera y queso Gorgonzola.",
            "507 g",
            500,
            false,
            true,
            "🥗",
            "insalata_di_pera_gorgonzola_italiannis",
            new[] { "Lechuga", "Espinaca", "Aderezo dulce", "Pechuga de pollo asada", "Nuez", "Pera", "Queso Gorgonzola", "Aceite de oliva", "Vinagre", "Sal", "Pimienta" },
            new[] { "Servida fría", "Pollo asado a la parrilla", "Aderezada al momento" },
            new[] { "Ensalada", "Pollo", "Ligero", "Nuez", "Gorgonzola" },
            chicken: true,
            nuts: true,
            temperature: DishTemperature.Cold
        ));

        dishes.Add(D(
            "insalata_cesare_gde",
            "Insalata Cesare Grande",
            s,
            "Insalate",
            "Ensaladas",
            "Lechuga romana con aderezo Cesare y croutones de pan Focaccia.",
            "340 g",
            710,
            false,
            false,
            "🥗",
            "insalata_cesare_italiannis",
            new[] { "Lechuga romana", "Aderezo Cesare", "Croutones de pan Focaccia", "Queso Parmesano", "Ajo", "Aceite", "Limón", "Pimienta" },
            new[] { "Servida fría", "Croutones agregados al final" },
            new[] { "Ensalada", "Cesare", "Clásico" },
            temperature: DishTemperature.Cold
        ));

        dishes.Add(D(
            "zuppa_minestrone",
            "Zuppa Minestrone",
            s,
            "Zuppe",
            "Sopas",
            "Sopa italiana de verduras con pasta, jitomate y hierbas.",
            "360 g",
            280,
            false,
            false,
            "🍲",
            "zuppa_minestrone_italiannis",
            new[] { "Caldo de verduras", "Jitomate", "Zanahoria", "Apio", "Calabaza", "Pasta corta", "Frijol", "Ajo", "Cebolla", "Albahaca", "Orégano" },
            new[] { "Servida caliente", "Cocción lenta de verduras" },
            new[] { "Sopa", "Verduras", "Vegetariano" },
            vegetarian: true
        ));

        dishes.Add(D(
            "antipasto_formaggio",
            "Formaggio Fondente",
            s,
            "Antipasti",
            "Entradas",
            "Queso fundido estilo italiano con pan tostado y hierbas.",
            "320 g",
            890,
            false,
            false,
            "🧀",
            "formaggio_fondente_italiannis",
            new[] { "Queso Mozzarella", "Queso Parmesano", "Queso Gorgonzola", "Hierbas italianas", "Pan tostado", "Aceite de oliva", "Ajo" },
            new[] { "Servido fundido", "Ideal para compartir" },
            new[] { "Entrada", "Queso", "Compartir" },
            vegetarian: true,
            isShareable: true
        ));
    }

    private static void AddPizzas(List<DishData> dishes)
    {
        const string s = "Comidas";

        dishes.Add(D(
            "pizza_peperoni",
            "Pizza Peperoni",
            s,
            "Pizzas",
            "Tradicional",
            "Abundante porción de Pepperoni, queso Mozzarella fresco y aros de cebolla morada.",
            "560 g",
            1330,
            true,
            true,
            "🍕",
            "pizza_peperoni_italiannis",
            new[] { "Masa de pizza", "Harina de trigo", "Agua", "Levadura", "Aceite de oliva", "Salsa Pomodoro", "Jitomate", "Queso Mozzarella", "Pepperoni", "Cebolla morada", "Orégano", "Sal", "Pimienta" },
            new[] { "Horneada", "Masa delgada tradicional", "Queso fundido", "Servida caliente" },
            new[] { "Pizza", "Pepperoni", "Clásico", "AR", "Popular" },
            pork: true
        ));

        dishes.Add(D(
            "pizza_cuatro_quesos",
            "Cuatro Quesos",
            s,
            "Pizzas",
            "Tradicional",
            "Pizza con Mozzarella, Parmesano, Gorgonzola y queso Cenizo de Cabra sobre salsa Pomodoro con cebollín.",
            "500 g",
            1030,
            true,
            true,
            "🧀",
            "pizza_cuatro_quesos_italiannis",
            new[] { "Masa de pizza", "Salsa Pomodoro", "Queso Mozzarella", "Queso Parmesano", "Queso Gorgonzola", "Queso Cenizo de Cabra", "Cebollín", "Aceite de oliva", "Orégano" },
            new[] { "Horneada", "Mezcla de quesos", "Servida caliente" },
            new[] { "Pizza", "Quesos", "Vegetariana", "AR" },
            vegetarian: true
        ));

        dishes.Add(D(
            "pizza_margherita",
            "Pizza Margherita Clásica",
            s,
            "Pizzas",
            "Tradicional",
            "Pizza con salsa Pomodoro, queso Mozzarella, orégano, jitomate y hojas frescas de albahaca.",
            "562 g",
            995,
            true,
            true,
            "🍕",
            "pizza_margherita_italiannis",
            new[] { "Masa de pizza", "Salsa Pomodoro", "Queso Mozzarella", "Jitomate", "Albahaca fresca", "Orégano", "Aceite de oliva", "Sal" },
            new[] { "Horneada", "Terminada con albahaca fresca" },
            new[] { "Pizza", "Margherita", "Vegetariana", "Clásico", "AR" },
            vegetarian: true
        ));

        dishes.Add(D(
            "pizza_fra_diavolo",
            "Pizza Fra Diavolo",
            s,
            "Pizzas",
            "Especialidad",
            "Camarones salteados en salsa de jitomate ligeramente picante con arúgula, espárragos, piñones tostados y queso Feta.",
            "665 g",
            1310,
            true,
            true,
            "🍕",
            "1-tradicional_fra_diavolo_italiannisjpg",
            new[] { "Masa de pizza", "Salsa de jitomate", "Camarones", "Arúgula", "Espárragos", "Piñones tostados", "Queso Feta", "Aceite de oliva", "Chile", "Ajo" },
            new[] { "Camarones salteados", "Horneada", "Terminada con arúgula" },
            new[] { "Pizza", "Mariscos", "Picante suave", "AR" },
            seafood: true,
            nuts: true,
            spiceLevel: SpiceLevel.Mild
        ));

        dishes.Add(D(
            "pizza_pollo_croccante",
            "Pizza Pollo Croccante",
            s,
            "Pizzas",
            "Especialidad",
            "Pizza con salsa Pomodoro, Mozzarella, Gorgonzola, espinaca, pechuga sazonada y salsa pesto de chipotle con Parmesano croccante.",
            "759 g",
            1520,
            true,
            true,
            "🍕",
            "pollo-croccante",
            new[] { "Masa de pizza", "Salsa Pomodoro", "Queso Mozzarella", "Queso Gorgonzola", "Espinaca", "Pechuga de pollo", "Pesto de chipotle", "Queso Parmesano", "Aceite de oliva", "Ajo" },
            new[] { "Pollo sazonado", "Horneada", "Terminada con Parmesano croccante" },
            new[] { "Pizza", "Pollo", "Chipotle", "AR" },
            chicken: true,
            spiceLevel: SpiceLevel.Mild
        ));

        dishes.Add(D(
            "pizza_bolognesa",
            "Pizza Bolognesa",
            s,
            "Pizzas",
            "Especialidad",
            "Pizza con salsa Bolognese preparada con carne de res y salsa de jitomate Italianni’s, champiñones y queso Parmesano.",
            "746 g",
            1300,
            true,
            true,
            "🍕",
            "bolognesa-essenza-italiannis",
            new[] { "Masa de pizza", "Salsa Bolognese", "Carne de res", "Salsa de jitomate", "Champiñones", "Queso Parmesano", "Queso Mozzarella", "Hierbas italianas" },
            new[] { "Horneada", "Salsa Bolognese de la casa", "Servida caliente" },
            new[] { "Pizza", "Bolognese", "Carne", "AR" },
            beef: true
        ));

        dishes.Add(D(
            "pizza_toscana",
            "Pizza Toscana",
            s,
            "Pizzas",
            "Especialidad",
            "Pizza con Prosciutto, arúgula, cebolla al vino tinto y queso Cenizo de Cabra.",
            "662 g",
            1246,
            true,
            false,
            "🍕",
            "pizza-toscana-italiannis",
            new[] { "Masa de pizza", "Prosciutto", "Arúgula", "Cebolla al vino tinto", "Queso Cenizo de Cabra", "Salsa Pomodoro", "Aceite de oliva" },
            new[] { "Horneada", "Terminada con arúgula", "Cebolla reducida al vino" },
            new[] { "Pizza", "Prosciutto", "Premium", "AR" },
            pork: true
        ));

        dishes.Add(D(
            "pizza_la_mia_creazione",
            "La Mia Creazione",
            s,
            "Pizzas",
            "Personalizable",
            "Pizza personalizada. Elige 3 ingredientes favoritos y crea tu pizza ideal.",
            "Personalizable",
            0,
            true,
            false,
            "🍕",
            "pizza_la_mia_creazione_italiannis_2",
            new[] { "Masa de pizza", "Salsa Pomodoro", "Queso Mozzarella", "Jamón", "Champiñón", "Prosciutto", "Aceituna negra", "Pepperoni", "Cebolla morada", "Pimiento rojo", "Salchicha rostizada", "Pimiento verde", "Alcachofa", "Piña", "Jitomate", "Arúgula" },
            new[] { "Personalizable", "Horneada", "Ingredientes variables" },
            new[] { "Pizza", "Personalizable", "AR" },
            pork: true
        ));
    }

    private static void AddPastas(List<DishData> dishes)
    {
        const string s = "Comidas";

        dishes.Add(D(
            "fettuccini_alfredo",
            "Fettuccini Alfredo",
            s,
            "Pasta",
            "Pasta larga",
            "Pasta con queso Parmesano y salsa Alfredo cremosa.",
            "521 g",
            1306,
            true,
            true,
            "🍝",
            "fettuccini_alfredo_italiannis",
            new[] { "Fettuccini", "Harina de trigo", "Huevo", "Crema", "Mantequilla", "Queso Parmesano", "Ajo", "Pimienta negra", "Sal", "Perejil" },
            new[] { "Pasta cocida al dente", "Salsa cremosa", "Servida caliente", "Terminada con Parmesano" },
            new[] { "Pasta", "Alfredo", "Cremosa", "Clásico", "AR" },
            vegetarian: true
        ));

        dishes.Add(D(
            "spaghetti_bolognese",
            "Spaghetti alla Bolognese",
            s,
            "Pasta",
            "Pasta larga",
            "Spaghetti preparado con carne de res y salsa de jitomate Italianni’s.",
            "Porción individual",
            720,
            true,
            true,
            "🍝",
            "spaghetti_alla_bolognese_italiannis",
            new[] { "Spaghetti", "Harina de trigo", "Huevo", "Carne de res", "Jitomate", "Pasta de tomate", "Cebolla", "Zanahoria", "Apio", "Ajo", "Aceite de oliva", "Albahaca", "Orégano", "Queso Parmesano" },
            new[] { "Pasta cocida al dente", "Salsa Bolognese cocinada lentamente", "Servida caliente" },
            new[] { "Pasta", "Bolognese", "Carne", "Clásico", "AR" },
            beef: true
        ));

        dishes.Add(D(
            "frutti_di_mare",
            "Frutti di Mare",
            s,
            "Pasta",
            "Mariscos",
            "Pasta Linguini con camarones, calamares, mejillones y salmón en salsa de vino blanco ligeramente picosa.",
            "670 g",
            1155,
            true,
            true,
            "🍝",
            "frutti_di_mare_italiannis",
            new[] { "Linguini", "Camarones", "Calamares", "Mejillones", "Salmón", "Vino blanco", "Ajo", "Aceite de oliva", "Jitomate", "Perejil", "Chile", "Sal", "Pimienta" },
            new[] { "Mariscos salteados", "Salsa de vino blanco", "Pasta al dente", "Servida caliente" },
            new[] { "Pasta", "Mariscos", "Picante suave", "AR" },
            seafood: true,
            spiceLevel: SpiceLevel.Mild
        ));

        dishes.Add(D(
            "linguini_fra_diavolo",
            "Linguini Fra Diavolo",
            s,
            "Pasta",
            "Mariscos",
            "Camarones salteados en salsa de jitomate ligeramente picante con espinacas, espárragos, piñones tostados y queso Feta.",
            "551 g",
            1004,
            true,
            true,
            "🍝",
            "linguini-fra-diavolo-italiannis",
            new[] { "Linguini", "Camarones", "Salsa de jitomate", "Espinacas", "Espárragos", "Piñones tostados", "Queso Feta", "Ajo", "Aceite de oliva", "Chile" },
            new[] { "Camarones salteados", "Pasta al dente", "Terminada con queso Feta" },
            new[] { "Pasta", "Camarón", "Picante suave", "AR" },
            seafood: true,
            nuts: true,
            spiceLevel: SpiceLevel.Mild
        ));

        dishes.Add(D(
            "lasagna_biancini",
            "Lasagna alla Biancini",
            s,
            "Pasta",
            "Horneada",
            "Lasagna con espinacas, champiñones, salsa alla Bolognese, Mozzarella y salsa Marinara.",
            "636 g",
            787,
            true,
            true,
            "🍽️",
            "1-lasagna-biancini_italiannis",
            new[] { "Láminas de lasagna", "Espinacas", "Champiñones", "Salsa Bolognese", "Salsa Marinara", "Queso Mozzarella", "Queso Parmesano", "Ricotta", "Ajo", "Hierbas italianas" },
            new[] { "Horneada", "Gratinada", "Servida caliente" },
            new[] { "Pasta", "Lasagna", "Horneado", "AR" },
            beef: true
        ));

        dishes.Add(D(
            "ravioli_romana",
            "Ravioli alla Romana",
            s,
            "Pasta Ripiena",
            "Ravioli",
            "Ravioles rellenos de queso Ricotta y espinacas, bañados con salsa alla Bolognese y salsa Alfredo, gratinados con Mozzarella y Parmesano.",
            "552 g",
            1240,
            true,
            false,
            "🥟",
            "ravioli_alla_romana_italiannis",
            new[] { "Ravioli", "Queso Ricotta", "Espinacas", "Salsa Bolognese", "Salsa Alfredo", "Queso Mozzarella", "Queso Parmesano", "Harina de trigo", "Huevo" },
            new[] { "Pasta rellena", "Gratinada", "Servida caliente" },
            new[] { "Pasta rellena", "Ravioli", "Queso", "AR" },
            beef: true
        ));

        dishes.Add(D(
            "risotto_funghi_pollo",
            "Risotto ai Funghi e Pollo",
            s,
            "Pasta",
            "Risotto",
            "Risotto con salsa al vino blanco, champiñones, pollo parrillado, jitomates deshidratados y queso Parmesano.",
            "1011 g",
            1133,
            true,
            false,
            "🍚",
            "risotto_ai_funghi_e_pollo_italiannis",
            new[] { "Arroz Arborio", "Vino blanco", "Champiñones", "Pollo parrillado", "Jitomates deshidratados", "Queso Parmesano", "Caldo", "Mantequilla", "Ajo", "Cebolla" },
            new[] { "Cocción cremosa", "Pollo a la parrilla", "Servido caliente" },
            new[] { "Risotto", "Pollo", "Cremoso", "AR" },
            chicken: true
        ));
    }

    private static void AddDellaCucina(List<DishData> dishes)
    {
        const string s = "Comidas";

        dishes.Add(D(
            "pollo_parmigiana",
            "Pollo alla Parmigiana",
            s,
            "Della Cucina",
            "Pollo",
            "Pechugas empanizadas a la romana con salsa Marinara, gratinadas con Mozzarella y Parmesano, acompañadas de Spaghetti.",
            "721 g",
            1407,
            true,
            true,
            "🍗",
            "pollo-alla-parmigiana",
            new[] { "Pechuga de pollo", "Pan molido", "Harina", "Huevo", "Salsa Marinara", "Queso Mozzarella", "Queso Parmesano", "Spaghetti", "Aceite", "Ajo", "Hierbas italianas" },
            new[] { "Pollo empanizado", "Gratinado", "Servido con pasta" },
            new[] { "Pollo", "Gratinado", "Clásico", "AR" },
            chicken: true
        ));

        dishes.Add(D(
            "salmone_limone",
            "Salmone al Limone",
            s,
            "Della Cucina",
            "Salmón",
            "Filete de salmón asado a la parrilla con salsa de limón, servido con pasta Fettuccini al burro.",
            "544 g",
            1336,
            true,
            true,
            "🐟",
            "salmone_al_limone_abb_italiannis",
            new[] { "Salmón", "Limón", "Mantequilla", "Fettuccini", "Aceite de oliva", "Ajo", "Perejil", "Sal", "Pimienta" },
            new[] { "Salmón a la parrilla", "Salsa de limón", "Servido con pasta" },
            new[] { "Salmón", "Pescado", "Premium", "AR" },
            seafood: true
        ));

        dishes.Add(D(
            "rib_eye_fiorentina",
            "Rib Eye alla Fiorentina",
            s,
            "Della Cucina",
            "Carne",
            "Rib Eye madurado y asado a la parrilla, acompañado de puré de papa al Parmesano y salsa a la pimienta.",
            "585 g",
            1373,
            true,
            true,
            "🥩",
            "rib_eye_alla_fiorentina_italiannis",
            new[] { "Rib Eye", "Papa", "Queso Parmesano", "Crema", "Mantequilla", "Pimienta", "Salsa de carne", "Sal", "Aceite de oliva" },
            new[] { "Carne a la parrilla", "Puré cremoso", "Servido caliente" },
            new[] { "Carne", "Rib Eye", "Premium", "AR" },
            beef: true
        ));

        dishes.Add(D(
            "pollo_picatta",
            "Pollo Picatta",
            s,
            "Della Cucina",
            "Pollo",
            "Pechugas de pollo ligeramente rebozadas con salsa de limón, vino blanco y alcaparras, jitomates y champiñones, servido con Capellini.",
            "818 g",
            1574,
            true,
            false,
            "🍗",
            "pollo_picatta_italiannis",
            new[] { "Pechuga de pollo", "Harina", "Limón", "Vino blanco", "Alcaparras", "Jitomate", "Champiñones", "Capellini", "Mantequilla", "Ajo" },
            new[] { "Pollo rebozado", "Salsa cítrica", "Servido con pasta" },
            new[] { "Pollo", "Limón", "Vino blanco", "AR" },
            chicken: true
        ));
    }

    private static void AddAbbondanza(List<DishData> dishes)
    {
        const string s = "Comidas";

        dishes.Add(D(
            "abb_fettuccini_alfredo",
            "Abbondanza Fettuccini Alfredo",
            s,
            "Abbondanza",
            "Pasta para compartir",
            "Fettuccini Alfredo en porción abundante con queso Parmesano.",
            "899 g",
            1790,
            true,
            false,
            "🍝",
            "fettuccini_alfredo_abb_italiannis",
            new[] { "Fettuccini", "Crema", "Mantequilla", "Queso Parmesano", "Ajo", "Pimienta negra", "Perejil" },
            new[] { "Porción abundante", "Para compartir", "Servido caliente" },
            new[] { "Abbondanza", "Pasta", "Compartir", "AR" },
            vegetarian: true,
            isShareable: true
        ));

        dishes.Add(D(
            "abb_spaghetti_bolognese",
            "Abbondanza Spaghetti alla Bolognese",
            s,
            "Abbondanza",
            "Pasta para compartir",
            "Spaghetti al dente preparado con carne de res y salsa de jitomate Italianni’s.",
            "930 g",
            1279,
            true,
            false,
            "🍝",
            "spaghetti_bolognese_abb_italiannis",
            new[] { "Spaghetti", "Carne de res", "Jitomate", "Pasta de tomate", "Cebolla", "Zanahoria", "Apio", "Ajo", "Queso Parmesano" },
            new[] { "Porción abundante", "Para compartir", "Salsa cocinada lentamente" },
            new[] { "Abbondanza", "Pasta", "Carne", "Compartir", "AR" },
            beef: true,
            isShareable: true
        ));

        dishes.Add(D(
            "abb_pollo_parmigiana",
            "Abbondanza Pollo alla Parmigiana",
            s,
            "Abbondanza",
            "Pollo para compartir",
            "Pechugas de pollo empanizadas a la romana con salsa Marinara, Mozzarella y Parmesano, acompañadas de Spaghetti.",
            "1437 g",
            2898,
            true,
            false,
            "🍗",
            "pollo_alla_parmigiana_abb_italiannis",
            new[] { "Pollo", "Pan molido", "Salsa Marinara", "Queso Mozzarella", "Queso Parmesano", "Spaghetti", "Aceite", "Hierbas italianas" },
            new[] { "Porción abundante", "Para compartir", "Gratinado" },
            new[] { "Abbondanza", "Pollo", "Compartir", "AR" },
            chicken: true,
            isShareable: true
        ));
    }

    private static void AddPostres(List<DishData> dishes)
    {
        const string s = "Postres";

        dishes.Add(D(
            "tiramisu",
            "Tiramisù",
            s,
            "Postres",
            "Dolci",
            "Pastel con galletas soletas cubiertas con café espresso, queso Mascarpone, licor de café, ron, hojuelas de chocolate y cocoa.",
            "324 g",
            1520,
            true,
            true,
            "🍰",
            "tiramisu_italiannis",
            new[] { "Galletas soletas", "Café espresso", "Queso Mascarpone", "Licor de café", "Ron", "Chocolate", "Cocoa", "Azúcar", "Crema" },
            new[] { "Postre frío", "Capas de soleta y crema", "Espolvoreado con cocoa" },
            new[] { "Postre", "Café", "Chocolate", "AR" },
            isAlcoholic: true,
            dairy: true,
            gluten: true,
            temperature: DishTemperature.Cold
        ));

        dishes.Add(D(
            "pastel_tartufo",
            "Pastel Tartufo",
            s,
            "Postres",
            "Dolci",
            "Rebanada de pastel de chocolate elaborado con capas de mousse de chocolate blanco y oscuro.",
            "355 g",
            1106,
            true,
            true,
            "🍫",
            "tartufo_italiannis",
            new[] { "Chocolate oscuro", "Chocolate blanco", "Mousse", "Crema", "Azúcar", "Harina", "Huevo", "Mantequilla", "Cocoa" },
            new[] { "Servido frío", "Capas de mousse", "Rebanada individual" },
            new[] { "Postre", "Chocolate", "AR" },
            dairy: true,
            gluten: true,
            temperature: DishTemperature.Cold
        ));

        dishes.Add(D(
            "panna_cotta_frambuesa",
            "Panna Cotta con Salsa de Frambuesa",
            s,
            "Postres",
            "Dolci",
            "Crema dulce con Amaretto sobre salsa de frambuesa, acompañada de fresas, moras y zarzamoras.",
            "219 g",
            630,
            true,
            false,
            "🍮",
            "panna-cotta_italiannis",
            new[] { "Crema", "Azúcar", "Grenetina", "Amaretto", "Frambuesa", "Fresa", "Mora", "Zarzamora", "Vainilla" },
            new[] { "Postre frío", "Textura cremosa", "Servido con frutos rojos" },
            new[] { "Postre", "Frambuesa", "Frío", "AR" },
            isAlcoholic: true,
            dairy: true,
            gluten: false,
            temperature: DishTemperature.Cold
        ));
    }

    private static void AddBebidas(List<DishData> dishes)
    {
        const string s = "Bebidas";

        dishes.Add(D(
            "soda_durazno",
            "Soda Italiana Durazno",
            s,
            "Bebidas",
            "Sodas Italianas",
            "Soda italiana con base de jarabe y agua mineralizada sabor durazno.",
            "218 mL",
            170,
            false,
            true,
            "🥤",
            "sodas-italianas-italiannis",
            new[] { "Jarabe sabor durazno", "Agua mineralizada", "Hielo" },
            new[] { "Servida fría", "Bebida gasificada", "Preparada al momento" },
            new[] { "Bebida", "Soda italiana", "Sin alcohol" },
            dairy: false,
            gluten: false,
            temperature: DishTemperature.Cold
        ));

        dishes.Add(D(
            "limonada",
            "Limonada",
            s,
            "Bebidas",
            "Refrescantes",
            "Limonada fresca.",
            "410 mL",
            100,
            false,
            true,
            "🍋",
            "bebidas_italiannis",
            new[] { "Limón", "Agua", "Azúcar", "Hielo" },
            new[] { "Servida fría", "Preparada al momento" },
            new[] { "Bebida", "Sin alcohol", "Refrescante" },
            dairy: false,
            gluten: false,
            temperature: DishTemperature.Cold
        ));

        dishes.Add(D(
            "aperol_spritz",
            "Aperol Spritz",
            s,
            "Bebidas",
            "Cocteles",
            "Mezcla de Aperol, Prosecco y soda con toque cítrico.",
            "400 mL",
            213,
            false,
            false,
            "🍹",
            "aperol_spritz_italiannis",
            new[] { "Aperol", "Prosecco", "Soda", "Naranja", "Hielo" },
            new[] { "Servido frío", "Coctel con alcohol", "Preparado al momento" },
            new[] { "Bebida", "Alcohol", "Coctel" },
            isAlcoholic: true,
            dairy: false,
            gluten: false,
            temperature: DishTemperature.Cold
        ));
    }

    private static void AddVinos(List<DishData> dishes)
    {
        const string s = "Vinos";

        dishes.Add(D(
            "vino_chianti_classico",
            "Chianti Classico, Valiano DOCG",
            s,
            "Vinos",
            "Tinto",
            "Vino tinto italiano de la región Toscana, Centro de Italia.",
            "Botella 750 mL / Copa 206 mL",
            0,
            false,
            false,
            "🍷",
            "vino_chianti_classico",
            new[] { "Vino tinto", "Uva Sangiovese", "Alcohol" },
            new[] { "Servir a temperatura recomendada para vino tinto", "Ideal para carnes, pastas rojas y pizzas intensas" },
            new[] { "Vino", "Tinto", "Italia", "Alcohol" },
            isAlcoholic: true,
            dairy: false,
            gluten: false,
            temperature: DishTemperature.RoomTemperature
        ));

        dishes.Add(D(
            "vino_prosecco_extra_dry",
            "Glera, Prosecco Extra Dry Maximilian I DOC",
            s,
            "Vinos",
            "Espumoso",
            "Prosecco italiano de la región Veneto, Norte de Italia.",
            "Botella 750 mL / Copa 206 mL",
            0,
            false,
            false,
            "🍾",
            "vino_prosecco_extra_dry",
            new[] { "Glera", "Prosecco", "Alcohol" },
            new[] { "Servir frío", "Ideal para aperitivos, postres ligeros y celebraciones" },
            new[] { "Vino", "Espumoso", "Italia", "Alcohol" },
            isAlcoholic: true,
            dairy: false,
            gluten: false,
            temperature: DishTemperature.Cold
        ));
    }

    private static void AddInfantil(List<DishData> dishes)
    {
        const string s = "Infantil";

        dishes.Add(D(
            "inf_spaghetti_bolognese",
            "Spaghetti alla Bolognese Infantil",
            s,
            "Infantil",
            "Pasta Bambino",
            "Spaghetti alla Bolognese en porción infantil.",
            "200 g",
            305,
            true,
            false,
            "🍝",
            "menu-infantil_mobile_italiannis",
            new[] { "Spaghetti", "Salsa Bolognese", "Carne de res", "Jitomate", "Queso Parmesano" },
            new[] { "Porción infantil", "Servido caliente" },
            new[] { "Infantil", "Pasta", "Bolognese", "AR" },
            isKidsMenu: true,
            beef: true
        ));

        dishes.Add(D(
            "inf_fettuccini_alfredo",
            "Fettuccini Alfredo Infantil",
            s,
            "Infantil",
            "Pasta Bambino",
            "Fettuccini Alfredo en porción infantil.",
            "200 g",
            520,
            true,
            false,
            "🍝",
            "menu-infantil_mobile_italiannis",
            new[] { "Fettuccini", "Salsa Alfredo", "Crema", "Mantequilla", "Queso Parmesano" },
            new[] { "Porción infantil", "Servido caliente" },
            new[] { "Infantil", "Pasta", "Alfredo", "AR" },
            isKidsMenu: true,
            vegetarian: true
        ));

        dishes.Add(D(
            "inf_pizza_pepperoni",
            "Pizza Pepperoni Infantil",
            s,
            "Infantil",
            "Pizza Bambino",
            "Pizza infantil de pepperoni.",
            "294 g / 15 cm",
            675,
            true,
            false,
            "🍕",
            "menu-infantil_mobile_italiannis",
            new[] { "Masa de pizza", "Salsa Pomodoro", "Queso Mozzarella", "Pepperoni" },
            new[] { "Porción infantil", "Horneada", "Servida caliente" },
            new[] { "Infantil", "Pizza", "Pepperoni", "AR" },
            isKidsMenu: true,
            pork: true
        ));
    }

    private static NutritionProfile BuildEstimatedNutrition(
        string category,
        int calories,
        bool seafood,
        bool pork,
        bool beef,
        bool chicken,
        bool dairy,
        bool gluten,
        bool nuts,
        bool alcoholic,
        bool kids)
    {
        if (calories <= 0)
        {
            return new NutritionProfile(0, 0, 0, 0, 0, 0, 0, DataQuality.Pending);
        }

        float proteinFactor = 0.10f;
        float carbFactor = 0.42f;
        float sugarFactor = 0.08f;
        float fatFactor = 0.38f;
        float satFactor = 0.13f;
        float fiber = 4f;
        float sodium = calories * 1.15f;

        if (category == "Pizzas")
        {
            proteinFactor = 0.13f;
            carbFactor = 0.43f;
            fatFactor = 0.36f;
            satFactor = 0.14f;
            sodium = calories * 1.25f;
            fiber = 5f;
        }
        else if (category == "Pasta" || category == "Pasta Ripiena" || category == "Abbondanza")
        {
            proteinFactor = 0.12f;
            carbFactor = 0.48f;
            fatFactor = 0.30f;
            satFactor = 0.10f;
            sodium = calories * 1.05f;
            fiber = 6f;
        }
        else if (category == "Postres")
        {
            proteinFactor = 0.06f;
            carbFactor = 0.52f;
            sugarFactor = 0.30f;
            fatFactor = 0.38f;
            satFactor = 0.18f;
            sodium = calories * 0.35f;
            fiber = 2f;
        }
        else if (category == "Bebidas" || category == "Vinos")
        {
            proteinFactor = 0.01f;
            carbFactor = alcoholic ? 0.25f : 0.80f;
            sugarFactor = alcoholic ? 0.12f : 0.65f;
            fatFactor = 0.01f;
            satFactor = 0.0f;
            sodium = calories * 0.08f;
            fiber = 0f;
        }
        else if (category == "Della Cucina")
        {
            proteinFactor = 0.22f;
            carbFactor = 0.22f;
            fatFactor = 0.42f;
            satFactor = 0.14f;
            sodium = calories * 1.10f;
            fiber = 3f;
        }

        if (beef || chicken || seafood)
        {
            proteinFactor += 0.05f;
            carbFactor -= 0.04f;
        }

        if (dairy)
        {
            satFactor += 0.03f;
        }

        if (nuts)
        {
            fatFactor += 0.04f;
            proteinFactor += 0.02f;
        }

        if (kids)
        {
            sodium *= 0.85f;
        }

        float protein = calories * proteinFactor / 4f;
        float carbs = calories * carbFactor / 4f;
        float sugars = calories * sugarFactor / 4f;
        float fat = calories * fatFactor / 9f;
        float satFat = calories * satFactor / 9f;

        return new NutritionProfile(
            proteinGrams: protein,
            carbohydratesGrams: carbs,
            sugarsGrams: sugars,
            totalFatGrams: fat,
            saturatedFatGrams: satFat,
            fiberGrams: fiber,
            sodiumMilligrams: sodium,
            quality: DataQuality.Estimated
        );
    }

    private static string[] BuildNutritionFacts(string portion, int calories, NutritionProfile nutrition)
    {
        if (calories <= 0 || nutrition == null)
        {
            return new[]
            {
                "Información nutrimental variable según preparación, porción o selección del cliente.",
                "Los valores detallados deben confirmarse con información oficial del restaurante."
            };
        }

        return new[]
        {
            $"Porción oficial registrada: {portion}",
            $"Energía oficial o de menú: {calories} kcal",
            $"Proteína estimada: {nutrition.ProteinGrams:0.#} g",
            $"Carbohidratos estimados: {nutrition.CarbohydratesGrams:0.#} g",
            $"Azúcares estimados: {nutrition.SugarsGrams:0.#} g",
            $"Grasas totales estimadas: {nutrition.TotalFatGrams:0.#} g",
            $"Grasas saturadas estimadas: {nutrition.SaturatedFatGrams:0.#} g",
            $"Fibra estimada: {nutrition.FiberGrams:0.#} g",
            $"Sodio estimado: {nutrition.SodiumMilligrams:0} mg"
        };
    }

    private static string[] BuildAllergens(bool seafood, bool pork, bool beef, bool chicken, bool dairy, bool gluten, bool nuts, bool alcoholic)
    {
        List<string> allergens = new List<string>();

        if (gluten)
        {
            allergens.Add("Gluten / trigo");
        }

        if (dairy)
        {
            allergens.Add("Lácteos");
        }

        if (seafood)
        {
            allergens.Add("Pescado o mariscos");
        }

        if (nuts)
        {
            allergens.Add("Nueces o frutos secos");
        }

        if (pork)
        {
            allergens.Add("Cerdo / embutidos");
        }

        if (beef)
        {
            allergens.Add("Res");
        }

        if (chicken)
        {
            allergens.Add("Pollo");
        }

        if (alcoholic)
        {
            allergens.Add("Alcohol");
        }

        allergens.Add("Puede existir contaminación cruzada en cocina.");

        return allergens.ToArray();
    }

    private static string[] BuildWarnings(bool alcoholic, bool kids, bool seafood, bool nuts, bool dairy, bool gluten)
    {
        List<string> warnings = new List<string>
        {
            "Información nutrimental detallada estimada cuando no aparece de forma explícita en el menú oficial.",
            "Consulta disponibilidad, ingredientes y restricciones directamente en el restaurante.",
            "Los alimentos pueden procesarse en ambientes con alérgenos."
        };

        if (alcoholic)
        {
            warnings.Add("Prohibida su venta a menores de edad.");
            warnings.Add("El abuso en el consumo de alcohol es nocivo para la salud.");
        }

        if (kids)
        {
            warnings.Add("Producto del menú infantil; porción menor a la del menú tradicional.");
        }

        if (seafood)
        {
            warnings.Add("Contiene pescado o mariscos.");
        }

        if (nuts)
        {
            warnings.Add("Contiene o puede contener frutos secos.");
        }

        if (dairy)
        {
            warnings.Add("Contiene lácteos.");
        }

        if (gluten)
        {
            warnings.Add("Contiene gluten o trigo.");
        }

        return warnings.ToArray();
    }

    private static string[] BuildPairings(string category, bool alcoholic, bool kids, SpiceLevel spiceLevel)
    {
        if (kids)
        {
            return new[] { "Agua natural", "Limonada", "Naranjada", "Soda italiana sin alcohol" };
        }

        if (alcoholic)
        {
            return new[] { "Consumir con alimentos", "Ideal para experiencia de mesa", "Evitar consumo excesivo" };
        }

        switch (category)
        {
            case "Pizzas":
                return spiceLevel == SpiceLevel.Mild
                    ? new[] { "Limonada", "Soda italiana", "Vino tinto ligero", "Agua mineral" }
                    : new[] { "Soda italiana", "Vino tinto", "Agua mineral" };

            case "Pasta":
            case "Pasta Ripiena":
                return new[] { "Vino blanco", "Vino tinto suave", "Limonada", "Agua mineral" };

            case "Della Cucina":
                return new[] { "Vino tinto", "Vino blanco", "Agua mineral", "Soda italiana" };

            case "Postres":
                return new[] { "Café espresso", "Cappuccino", "Vino espumoso", "Agua natural" };

            case "Bebidas":
                return new[] { "Pastas", "Pizzas", "Postres ligeros" };

            default:
                return new[] { "Agua natural", "Soda italiana", "Limonada" };
        }
    }

    private static string BuildStory(string name, string category)
    {
        return $"{name} forma parte de la experiencia de menú de Italianni’s. En la app se presenta como una opción de la categoría {category}, pensada para ayudar al cliente a conocer mejor el platillo antes de ordenar.";
    }

    private static ARDishMetadata BuildARMetadata(string category, ARModelStatus modelStatus)
    {
        Vector3 position = new Vector3(0f, 0.02f, 0f);
        Vector3 rotation = Vector3.zero;
        Vector3 scale = Vector3.one;
        string complexity = "Media";
        string notes = "Modelo 3D disponible en el catálogo AR.";

        switch (category)
        {
            case "Pizzas":
                complexity = "Baja-Media";
                notes = "Modelo circular de pizza listo para visor 3D y Realidad Aumentada.";
                break;

            case "Pasta":
            case "Pasta Ripiena":
            case "Abbondanza":
                complexity = "Media-Alta";
                notes = "Modelo de pasta listo para visor 3D y Realidad Aumentada.";
                break;

            case "Della Cucina":
                complexity = "Alta";
                notes = "Modelo de platillo fuerte listo para visor 3D y Realidad Aumentada.";
                break;

            case "Postres":
                complexity = "Media";
                notes = "Modelo de postre listo para visor 3D y Realidad Aumentada.";
                break;

            case "Bebidas":
            case "Vinos":
                complexity = "Media";
                notes = "Modelo de bebida listo para visor 3D y Realidad Aumentada.";
                break;

            case "Desayunos":
                complexity = "Media";
                notes = "Modelo de desayuno listo para visor 3D y Realidad Aumentada.";
                break;

            case "Antipasti":
            case "Zuppe":
            case "Insalate":
            case "Infantil":
            default:
                complexity = "Media";
                notes = "Modelo 3D listo para visor de producto y Realidad Aumentada.";
                break;
        }

        return new ARDishMetadata(
            ARModelStatus.Ready,
            position,
            rotation,
            scale,
            0.18f,
            1.60f,
            complexity,
            notes
        );
    }

    private static float ParseAmount(string portion)
    {
        if (string.IsNullOrWhiteSpace(portion))
        {
            return 0f;
        }

        string number = new string(portion.TakeWhile(character =>
            char.IsDigit(character) || character == '.').ToArray());

        return float.TryParse(number, out float value) ? value : 0f;
    }

    private static string ParseUnit(string portion)
    {
        if (string.IsNullOrWhiteSpace(portion))
        {
            return "N/A";
        }

        if (portion.Contains("mL"))
        {
            return "mL";
        }

        if (portion.Contains("L"))
        {
            return "L";
        }

        if (portion.Contains("cm"))
        {
            return "cm";
        }

        if (portion.Contains("g"))
        {
            return "g";
        }

        return "variable";
    }
}