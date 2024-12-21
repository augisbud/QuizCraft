using System.Text.Json.Serialization;

namespace QuizCraft.Domain.Data.Constants;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    GeneralKnowledge,
    Science,
    History,
    Geography,
    Sports,
    Entertainment,
    Technology,
    Mathematics,
    Literature,
    Music,
    Art,
    Movies,
    Politics,
    Language,
    Religion,
    FoodAndDrink,
    Nature,
    Health,
    Business,
    Travel
}