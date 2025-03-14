public class Furniture
{
    private FurniturePart firstPart = new FurniturePart();
    private FurniturePart secondPart = new FurniturePart();
    private FurniturePart thirdPart = new FurniturePart();

    private int firstPartCount;
    private int secondPartCount;
    private int thirdPartCount;

    private double buildPrice;
    private string name;

    public void Display()
    {
        Console.WriteLine(name);
        Console.WriteLine("Деталь N1");
        firstPart.Display();
        Console.WriteLine("Деталь N2");
        secondPart.Display();
        Console.WriteLine("Деталь N3");
        thirdPart.Display();

        Console.WriteLine($"Количество деталей: N1 - {firstPartCount}, N2 - {secondPartCount}, N3 - {thirdPartCount}");
        Console.WriteLine($"Стоимость сборки: {buildPrice}");
    }

    public void Init(FurniturePart _firstPart, FurniturePart _secondPart, FurniturePart _thirdPart,
                     int _firstPartCount, int _secondPartCount, int _thirdPartCount,
                     double _buildPrice, string _name)
    {
        firstPart = _firstPart;
        secondPart = _secondPart;
        thirdPart = _thirdPart;

        firstPartCount = _firstPartCount;
        secondPartCount = _secondPartCount;
        thirdPartCount = _thirdPartCount;

        buildPrice = _buildPrice;
        name = _name;
    }

    public void Read()
    {
        Console.Write("Введите название мебели: ");
        name = Console.ReadLine();

        Console.WriteLine("Деталь N1:");
        firstPart.Read();
        Console.WriteLine("Деталь N2:");
        secondPart.Read();
        Console.WriteLine("Деталь N3:");
        thirdPart.Read();

        Console.Write("Введите количество детали N1: ");
        firstPartCount = int.Parse(Console.ReadLine());

        Console.Write("Введите количество детали N2: ");
        secondPartCount = int.Parse(Console.ReadLine());

        Console.Write("Введите количество детали N3: ");
        thirdPartCount = int.Parse(Console.ReadLine());

        Console.Write("Введите стоимость сборки: ");
        buildPrice = double.Parse(Console.ReadLine());
    }

    public FurniturePart GetMostExpensivePart()
    {
        double p1 = firstPart.GetPartPrice();
        double p2 = secondPart.GetPartPrice();
        double p3 = thirdPart.GetPartPrice();

        double maxPrice = Math.Max(Math.Max(p1, p2), p3);

        if (maxPrice == p1) return firstPart;
        if (maxPrice == p2) return secondPart;
        return thirdPart;
    }

    public double GetOverallPartCost()
    {
        return (firstPart.GetPartPrice() * firstPartCount) +
               (secondPart.GetPartPrice() * secondPartCount) +
               (thirdPart.GetPartPrice() * thirdPartCount) +
               buildPrice;
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string name)
    {
        this.name = name;
    }
}