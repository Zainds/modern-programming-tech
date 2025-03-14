public class FurniturePart
{
    private int weight;
    private double priceForGram;

    public void Display()
    {
        Console.WriteLine($"Вес детали: {weight}, Стоимость за грамм: {priceForGram}");
    }

    public FurniturePart(int weight, double priceForGram)
    {
        this.weight = weight;
        this.priceForGram = priceForGram;
    }
    public FurniturePart()
    {
        this.weight = 0;
        this.priceForGram = 0;
    }
    public void Init(int _weight, double _priceForGram)
    {
        weight = _weight;
        priceForGram = _priceForGram;
    }

    public void Read()
    {
        Console.Write("Введите вес детали: ");
        weight = int.Parse(Console.ReadLine());

        Console.Write("Введите стоимость за грамм: ");
        priceForGram = double.Parse(Console.ReadLine());
    }

    public double GetPartPrice()
    {
        return weight * priceForGram;
    }

    public int GetWeight()
    {
        return weight;
    }

    public double getPriceForGram()
    {
        return priceForGram;
    }
}