namespace CarInventory.Models
{
  class Car
  {
    public int CarId { get; set; }
    public string Make { get; set; } = "";
    public string Model { get; set; } = "";
    public int Year { get; set; }
    public int Horsepower { get; set; }
    public bool IsElectric { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; } = "";
    public DateTime DateAdded { get; set; }

    public string toString()
    {
      return $"Car ID: {CarId}, Make: {Make}, Model: {Model}, Year: {Year}, Horsepower: {Horsepower}, Is Electric: {IsElectric}, Price: {Price}, Color: {Color}, Date Added: {DateAdded}";
    }
  }
}