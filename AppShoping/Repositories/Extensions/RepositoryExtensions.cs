using AppShoping.Entities;
using System.Text.Json;
namespace AppShoping.Repositories.Extensions;

public static class RepositoryExtensions
{
    public static void ExportFoodListToJsonFiles<T>(this IRepository<T> repository)
        where T : class, IEntity
    {
        var products = repository.GetAll().ToList();
        var json = JsonSerializer.Serialize(products);
        File.WriteAllText("product.json", json);
    }

    public static void ImportFoodFiles<T>(this IRepository<T> repository)
         where T : class, IEntity
    {
        var data = File.ReadAllText("product.json");
        var items = JsonSerializer.Deserialize<List<T>>(data);
        foreach (var item in items) repository.Add(item);
        repository.Save();
    }

    public static void WriteAllToConsole(this IReadRepository<IEntity> allFoods)

    {
        var items = allFoods.GetAll();
        int counter = items.Count();
        if (counter != 0)
        {
            foreach (var item in items) Console.WriteLine(item);
        }
        else
        {
            Console.WriteLine("Pusta lista zakupów  < Powrót do Menu - Wcisnij dowolny klawisz>");
            Console.ReadKey();
        }
    }
}