using AppShoping.Data;
using AppShoping.Entities;
using AppShoping.Repositories;
using AppShoping.Repositories.Extensions;

List<string> shopMenu = new() { "1 - Produkty do zakupu", "2 - Dodaj produkt", "3 - Dodaj produkt bio",
    "4 - Usuń produkt z listy zakupów", "5 - Wyjście z programu"};

var foodRepository = new SqlRepository<Food>(new ShopAppDbContext());

const string  AuditDataPath = "Audit.txt";
foodRepository.ItemAdded += OnWriteToFile;
foodRepository.ProductDeleted += OnProductDeleted;

void OnProductDeleted(object? sender, Food? e)
{
    using (var write = File.AppendText(AuditDataPath))
    { 
        write.WriteLine($"{DateTime.Now} usunięto : {e?.ProductName} z  {e?.GetType().Name}");
    }
}

void OnWriteToFile(object? sender, Food? e)
{
    using (var write = File.AppendText(AuditDataPath))
    { 
        write.WriteLine($"{DateTime.Now} dodano : {e?.ProductName} z {e?.GetType().Name}");
    }
}

void DisplayMenu()
{
    Console.Clear();
    Console.WriteLine("Wykaz zakupów do zrobienia\n");
    foreach (var i in shopMenu) Console.WriteLine(i);
    Console.WriteLine("\n------------- Wybierz opcję [1- 5] ---------------\n");
}

try
{
    RepositoryExtensions.ImportFoodFiles(foodRepository);
}

catch (Exception e)
{
    Console.WriteLine($"{e.Message}");
}
while (true)
{
    DisplayMenu();
    var choise = int.TryParse(Console.ReadLine(), out int result);

    if (choise && (result > 0 || result < 6))
    {
                
        switch (result)
        {
            case 1:

                RepositoryExtensions.WriteAllToConsole(foodRepository);
                Console.ReadKey();
                break;

            case 2:
                Console.WriteLine("Podaj nazwę produktu");
                var name = Console.ReadLine();
                foodRepository.Add(new Food { ProductName = name });
                foodRepository.Save();
                break;

            case 3:
                Console.WriteLine("Podaj nazwę produktu");
                var bioName = Console.ReadLine();
                foodRepository.Add(new BioFood { ProductName = bioName + " Bio" });
                foodRepository.Save();
                break;

            case 4:
                RepositoryExtensions.WriteAllToConsole(foodRepository);
                Console.WriteLine("Podaj ID produktu do usunięcia");
                var choiseId = int.TryParse(Console.ReadLine(), out result);
                try
                {   
                    foodRepository.Remove(foodRepository.GetById(result));
                }
                catch (Exception e)
                {
                    Console.WriteLine("UWAGA !!! =====> " + e.Message);
                    Console.ReadKey();
                }
                foodRepository.Save();
                break;

            case 5:
                try
                {
                    RepositoryExtensions.ExportFoodListToJsonFiles(foodRepository);
                }

                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    Console.ReadKey();
                }
                Environment.Exit(0);
                break;

            default:

                Alert();
                break;
        }
    }

    else
    {
        Alert();
    }
}


static void Alert()
{
    Console.WriteLine("Niewlasciwy wybór w Menu");
    Console.ReadLine();
}