using System;
using CarInventory.Data;
using CarInventory.Models;
using CarInverntory.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

class Program
{

  public static string DisplayMenu()
  {
    string menu = @"
    1: Display all cars (async EF query).
    2: Add a car (user input, async EF save).
    3: Update a car (input ID, edit, async EF update).
    4: Delete a car (input ID, async EF delete).
    5: Import from JSON (async file read, Dapper insert).
    6: Export to JSON (async EF query, file write).
    7: Exit.";
    return menu;
  }
  static void Main(string[] args)
  {
    IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    DataContextDapper dataContextDapper = new DataContextDapper(config);
    DataContextEF dataContextEF = new(config);

    string carsJson = File.ReadAllText("cars_data.json");

    while (true)
    {
      Console.WriteLine(DisplayMenu());

      string? userInput = Console.ReadLine();

      switch (userInput?.ToLower())
      {

        case "1":
          //  1: Display all cars (async EF query).

          // string sqlAll = "SELECT * FROM InventorySchema.Cars";
          Console.WriteLine("Displaying all cars....\n");
          // This uses dapper
          // IEnumerable<Car> outputData = dataContextDapper.LoadData<Car>(sqlAll);
          // foreach (Car car in outputData)
          // {
          //   Console.WriteLine(car.toString());
          // }

          // This uses EF
          List<Car> carList = dataContextEF.Cars.ToList();
          foreach (Car car in carList)
          {
            Console.WriteLine(car.toString());
          }
          Console.WriteLine("\nEnd of cars.");
          break;
        case "2":
          //  2: Add a car (user input, async EF save).
          Console.WriteLine("Enter the Make of the car: ");
          string? make = Console.ReadLine();
          Console.WriteLine("Enter the Model of the car: ");
          string? model = Console.ReadLine();

          Console.WriteLine("Enter the Year of the car: ");
          int year = Convert.ToInt32(Console.ReadLine());

          Console.WriteLine("Enter the Horsepower of the car: ");
          int horsepower = Convert.ToInt32(Console.ReadLine());

          Console.WriteLine("Enter if the car is electric (yes/no): ");
          string? electricInput = Console.ReadLine()?.Trim().ToLower();
          bool isElectric = electricInput == "yes" || electricInput == "true";

          Console.WriteLine("Enter the Price of the car: ");
          decimal price = Convert.ToDecimal(Console.ReadLine());

          Console.WriteLine("Enter the Color of the car: ");
          string? color = Console.ReadLine();

          DateTime dateAdded = DateTime.Now;

          Car newCar = new Car()
          {
            Make = make,
            Model = model,
            Year = year,
            Horsepower = horsepower,
            IsElectric = isElectric,
            Price = price,
            Color = color,
            DateAdded = dateAdded
          };

          dataContextEF.Cars.Add(newCar);
          dataContextEF.SaveChanges();
          Console.WriteLine("Car added successfully.");
          break;
        case "3":
          //  3: Update a car (input ID, edit, async EF update).
          Console.WriteLine("Enter the ID of the car you want to update: ");
          int carId = Convert.ToInt32(Console.ReadLine());
          Car carToUpdate = dataContextEF.Cars.Find(carId);
          if (carToUpdate == null)
          {
            Console.WriteLine("Car not found.");
            break;
          }
          else
          {
            Console.WriteLine("Enter the Make of the car: ");
            string? makeUpdate = Console.ReadLine();
            Console.WriteLine("Enter the Model of the car: ");
            string? modelUpdate = Console.ReadLine();

            Console.WriteLine("Enter the Year of the car: ");
            int yearUpdate = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the Horsepower of the car: ");
            int horsepowerUpdate = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter if the car is electric (yes/no): ");
            string? electricInputUpdate = Console.ReadLine()?.Trim().ToLower();
            bool isElectricUpdate = electricInputUpdate == "yes" || electricInputUpdate == "true";

            Console.WriteLine("Enter the Price of the car: ");
            decimal priceUpdate = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Enter the Color of the car: ");
            string? colorUpdate = Console.ReadLine();

            DateTime dateAddedUpdate = DateTime.Now;

            carToUpdate.Make = makeUpdate;
            carToUpdate.Model = modelUpdate;
            carToUpdate.Year = yearUpdate;
            carToUpdate.Horsepower = horsepowerUpdate;
            carToUpdate.IsElectric = isElectricUpdate;
            carToUpdate.Price = priceUpdate;
            carToUpdate.Color = colorUpdate;
            carToUpdate.DateAdded = dateAddedUpdate;

            dataContextEF.SaveChanges();
            Console.WriteLine("Car updated successfully.");
          }

          break;
        case "4":
          // 4: Delete a car (input ID, async EF delete).
          Console.WriteLine("Enter the ID of the car you want to delete: ");
          int idToDelete = Convert.ToInt32(Console.ReadLine());
          Car? carToDelete = dataContextEF.Cars.Find(idToDelete);
          if (carToDelete == null)
          {
            Console.WriteLine("Car not found");
            break;
          }
          else
          {
            dataContextEF.Cars.Remove(carToDelete);
            dataContextEF.SaveChanges();
            Console.WriteLine("Car deleted successfully.");
          }
          break;
        case "5":
          // 5: Import from JSON (async file read, EF insert).
          Console.WriteLine("Write the name of the file to read: ");
          string? fileName = Console.ReadLine();
          string json = File.ReadAllText(fileName);
          IEnumerable<Car>? cars = JsonConvert.DeserializeObject<IEnumerable<Car>>(json);
          if (cars != null)
          {
            foreach (Car car in cars)
            {
              dataContextEF.Cars.Add(car);
              dataContextEF.SaveChanges();
            }
          }
          Console.WriteLine("Cars imported successfully.");
          break;
        case "6":
          // 6: Export to JSON (async EF query, file write).
          Console.WriteLine("Write the name of the file to write: ");
          string? fileNameToExport = Console.ReadLine();
          List<Car> carsToExport = dataContextEF.Cars.ToList();
          string jsonToExport = JsonConvert.SerializeObject(carsToExport);
          File.WriteAllText(fileNameToExport, jsonToExport);
          Console.WriteLine("Cars exported successfully.");
          break;
        case "7":
          // 7: Exit.";
          Console.WriteLine("Exiting program...");
          return;
        default:
          break;

      }
    }



  }
}