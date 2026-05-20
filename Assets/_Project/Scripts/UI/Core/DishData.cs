using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DataQuality
{
    Official,
    Estimated,
    ManualDemo,
    Pending
}

public enum SpiceLevel
{
    None,
    Mild,
    Medium,
    Hot
}

public enum DishTemperature
{
    Cold,
    Warm,
    Hot,
    Frozen,
    RoomTemperature
}

public enum ARModelStatus
{
    PendingTripo,
    Placeholder,
    InProgress,
    Ready
}

public class DishData
{
    public string Id { get; }
    public string Name { get; }
    public string MenuSection { get; }
    public string Category { get; }
    public string SubCategory { get; }
    public string Description { get; }
    public string Story { get; }

    public string Portion { get; }
    public float PortionAmount { get; }
    public string PortionUnit { get; }
    public string Calories { get; }
    public int CaloriesValue { get; }

    public bool HasAR { get; }
    public bool Recommended { get; }
    public string Icon { get; }

    public string ImageKey { get; }
    public string ImageResourcePath { get; }
    public string OfficialImageReference { get; }
    public string ModelPrefabPath { get; }
    public string TripoPrompt { get; }

    public IReadOnlyList<string> Ingredients { get; }
    public IReadOnlyList<string> PreparationNotes { get; }
    public IReadOnlyList<string> NutritionFacts { get; }
    public IReadOnlyList<string> Warnings { get; }
    public IReadOnlyList<string> Allergens { get; }
    public IReadOnlyList<string> Tags { get; }
    public IReadOnlyList<string> Pairings { get; }

    public NutritionProfile Nutrition { get; }
    public ARDishMetadata ARMetadata { get; }

    public bool IsAlcoholic { get; }
    public bool IsKidsMenu { get; }
    public bool IsShareable { get; }
    public bool IsVegetarianFriendly { get; }
    public bool ContainsSeafood { get; }
    public bool ContainsPork { get; }
    public bool ContainsBeef { get; }
    public bool ContainsChicken { get; }
    public bool ContainsDairy { get; }
    public bool ContainsGluten { get; }
    public bool ContainsNuts { get; }

    public SpiceLevel SpiceLevel { get; }
    public DishTemperature Temperature { get; }
    public DataQuality BaseDataQuality { get; }
    public DataQuality NutritionDataQuality { get; }

    public DishData(
        string id,
        string name,
        string menuSection,
        string category,
        string subCategory,
        string description,
        string story,
        string portion,
        float portionAmount,
        string portionUnit,
        string calories,
        int caloriesValue,
        bool hasAR,
        bool recommended,
        string icon,
        string imageKey,
        string imageResourcePath,
        string officialImageReference,
        string modelPrefabPath,
        string tripoPrompt,
        IEnumerable<string> ingredients,
        IEnumerable<string> preparationNotes,
        IEnumerable<string> nutritionFacts,
        IEnumerable<string> warnings,
        IEnumerable<string> allergens,
        IEnumerable<string> tags,
        IEnumerable<string> pairings,
        NutritionProfile nutrition,
        ARDishMetadata arMetadata,
        bool isAlcoholic,
        bool isKidsMenu,
        bool isShareable,
        bool isVegetarianFriendly,
        bool containsSeafood,
        bool containsPork,
        bool containsBeef,
        bool containsChicken,
        bool containsDairy,
        bool containsGluten,
        bool containsNuts,
        SpiceLevel spiceLevel,
        DishTemperature temperature,
        DataQuality baseDataQuality,
        DataQuality nutritionDataQuality)
    {
        Id = id;
        Name = name;
        MenuSection = menuSection;
        Category = category;
        SubCategory = subCategory;
        Description = description;
        Story = story;

        Portion = portion;
        PortionAmount = portionAmount;
        PortionUnit = portionUnit;
        Calories = calories;
        CaloriesValue = caloriesValue;

        HasAR = hasAR;
        Recommended = recommended;
        Icon = icon;

        ImageKey = imageKey;
        ImageResourcePath = imageResourcePath;
        OfficialImageReference = officialImageReference;
        ModelPrefabPath = modelPrefabPath;
        TripoPrompt = tripoPrompt;

        Ingredients = new List<string>(ingredients ?? Array.Empty<string>());
        PreparationNotes = new List<string>(preparationNotes ?? Array.Empty<string>());
        NutritionFacts = new List<string>(nutritionFacts ?? Array.Empty<string>());
        Warnings = new List<string>(warnings ?? Array.Empty<string>());
        Allergens = new List<string>(allergens ?? Array.Empty<string>());
        Tags = new List<string>(tags ?? Array.Empty<string>());
        Pairings = new List<string>(pairings ?? Array.Empty<string>());

        Nutrition = nutrition;
        ARMetadata = arMetadata;

        IsAlcoholic = isAlcoholic;
        IsKidsMenu = isKidsMenu;
        IsShareable = isShareable;
        IsVegetarianFriendly = isVegetarianFriendly;
        ContainsSeafood = containsSeafood;
        ContainsPork = containsPork;
        ContainsBeef = containsBeef;
        ContainsChicken = containsChicken;
        ContainsDairy = containsDairy;
        ContainsGluten = containsGluten;
        ContainsNuts = containsNuts;

        SpiceLevel = spiceLevel;
        Temperature = temperature;
        BaseDataQuality = baseDataQuality;
        NutritionDataQuality = nutritionDataQuality;
    }

    public string GetIngredientsText()
    {
        return BuildBulletText(Ingredients, "Ingredientes no especificados.");
    }

    public string GetPreparationText()
    {
        return BuildBulletText(PreparationNotes, "Preparación no especificada.");
    }

    public string GetNutritionText()
    {
        List<string> details = new List<string>();

        details.Add($"Calorías: {Calories}");
        details.Add($"Porción: {Portion}");

        if (Nutrition != null)
        {
            details.Add($"Proteína: {Nutrition.ProteinGrams:0.#} g");
            details.Add($"Carbohidratos: {Nutrition.CarbohydratesGrams:0.#} g");
            details.Add($"Azúcares: {Nutrition.SugarsGrams:0.#} g");
            details.Add($"Grasas totales: {Nutrition.TotalFatGrams:0.#} g");
            details.Add($"Grasas saturadas: {Nutrition.SaturatedFatGrams:0.#} g");
            details.Add($"Fibra: {Nutrition.FiberGrams:0.#} g");
            details.Add($"Sodio: {Nutrition.SodiumMilligrams:0} mg");
            details.Add($"Calidad nutrimental: {Nutrition.Quality}");
        }

        foreach (string fact in NutritionFacts)
        {
            details.Add(fact);
        }

        return BuildBulletText(details, "Información nutrimental no disponible.");
    }

    public string GetWarningsText()
    {
        return BuildBulletText(Warnings, "Sin advertencias específicas registradas.");
    }

    public string GetAllergensText()
    {
        return BuildBulletText(Allergens, "Alérgenos no especificados.");
    }

    public string GetTagsText()
    {
        return string.Join(", ", Tags);
    }

    public string GetPairingsText()
    {
        return BuildBulletText(Pairings, "Sin maridaje sugerido.");
    }

    private static string BuildBulletText(IEnumerable<string> values, string fallback)
    {
        List<string> safeValues = values?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToList() ?? new List<string>();

        if (safeValues.Count == 0)
        {
            return fallback;
        }

        return "• " + string.Join("\n• ", safeValues);
    }
}

[Serializable]
public class NutritionProfile
{
    public float ProteinGrams { get; }
    public float CarbohydratesGrams { get; }
    public float SugarsGrams { get; }
    public float TotalFatGrams { get; }
    public float SaturatedFatGrams { get; }
    public float FiberGrams { get; }
    public float SodiumMilligrams { get; }
    public DataQuality Quality { get; }

    public NutritionProfile(
        float proteinGrams,
        float carbohydratesGrams,
        float sugarsGrams,
        float totalFatGrams,
        float saturatedFatGrams,
        float fiberGrams,
        float sodiumMilligrams,
        DataQuality quality)
    {
        ProteinGrams = proteinGrams;
        CarbohydratesGrams = carbohydratesGrams;
        SugarsGrams = sugarsGrams;
        TotalFatGrams = totalFatGrams;
        SaturatedFatGrams = saturatedFatGrams;
        FiberGrams = fiberGrams;
        SodiumMilligrams = sodiumMilligrams;
        Quality = quality;
    }
}

[Serializable]
public class ARDishMetadata
{
    public ARModelStatus ModelStatus { get; }
    public Vector3 InitialLocalPosition { get; }
    public Vector3 InitialLocalEulerAngles { get; }
    public Vector3 InitialLocalScale { get; }
    public float MinScale { get; }
    public float MaxScale { get; }
    public string ModelingComplexity { get; }
    public string ModelGenerationNotes { get; }

    public ARDishMetadata(
        ARModelStatus modelStatus,
        Vector3 initialLocalPosition,
        Vector3 initialLocalEulerAngles,
        Vector3 initialLocalScale,
        float minScale,
        float maxScale,
        string modelingComplexity,
        string modelGenerationNotes)
    {
        ModelStatus = modelStatus;
        InitialLocalPosition = initialLocalPosition;
        InitialLocalEulerAngles = initialLocalEulerAngles;
        InitialLocalScale = initialLocalScale;
        MinScale = minScale;
        MaxScale = maxScale;
        ModelingComplexity = modelingComplexity;
        ModelGenerationNotes = modelGenerationNotes;
    }
}