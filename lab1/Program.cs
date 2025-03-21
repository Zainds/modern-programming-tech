using System;
using System.Collections.Generic;

// 1. Фабричный метод (Factory Method)
abstract class FurnitureFactory
{
    public abstract Furniture CreateFurniture();
}

class ChairFactory : FurnitureFactory
{
    public override Furniture CreateFurniture() => new Chair();
}

class TableFactory : FurnitureFactory
{
    public override Furniture CreateFurniture() => new Table();
}

class Chair : Furniture 
{
    public Chair() { SetName("Стул"); }
}

class Table : Furniture 
{
    public Table() { SetName("Стол"); }
}

// 2. Одиночка (Singleton)
class FurnitureWarehouse
{
    private static FurnitureWarehouse instance;
    private FurnitureWarehouse() { }
    public static FurnitureWarehouse Instance => instance ??= new FurnitureWarehouse();
}

// 3. Заместитель (Proxy)
abstract class AbstractFurniture
{
    public abstract void Display();
}

class RealFurniture : AbstractFurniture
{
    public override void Display() => Console.WriteLine("Отображение реальной мебели.");
}

class FurnitureProxy : AbstractFurniture
{
    private RealFurniture realFurniture = new RealFurniture();
    public override void Display()
    {
        Console.WriteLine("Заместитель контролирует доступ.");
        realFurniture.Display();
    }
}

// 4. Адаптер (Adapter)
class FurnitureAdapter
{
    private PreciousFurniturePart preciousPart;
    public FurnitureAdapter(PreciousFurniturePart part) => preciousPart = part;
    public double CalculatePrice() => preciousPart.GetPartPrice();
}

// 5. Посетитель (Visitor)
interface IFurnitureVisitor
{
    void Visit(Furniture furniture);
}

class PriceModifier : IFurnitureVisitor
{
    private double factor;
    public PriceModifier(double factor) => this.factor = factor;
    public void Visit(Furniture furniture) => furniture.SetName(furniture.GetName() + " (Изменено)");
}

// 6. Шаблонный метод (Template Method)
abstract class FurnitureTemplate
{
    public void Build()
    {
        AssembleParts();
        FinalizeBuild();
    }
    protected abstract void AssembleParts();
    protected virtual void FinalizeBuild() => Console.WriteLine("Завершение сборки.");
}

class WoodenChair : FurnitureTemplate
{
    protected override void AssembleParts() => Console.WriteLine("Сборка деревянного стула.");
}

class MetalTable : FurnitureTemplate
{
    protected override void AssembleParts() => Console.WriteLine("Сборка металлического стола.");
}

class Program
{
    static void Main()
    {
        // Factory Method
        FurnitureFactory chairFactory = new ChairFactory();
        Furniture chair = chairFactory.CreateFurniture();
        Console.WriteLine("Фабричный метод создал: " + chair.GetName());

        // Singleton
        var warehouse1 = FurnitureWarehouse.Instance;
        var warehouse2 = FurnitureWarehouse.Instance;
        Console.WriteLine("Одиночка работает: " + (warehouse1 == warehouse2));

        // Proxy
        AbstractFurniture proxy = new FurnitureProxy();
        proxy.Display();

        // Adapter
        PreciousFurniturePart preciousPart = new PreciousFurniturePart(500, new FurniturePart(10, 2));
        FurnitureAdapter adapter = new FurnitureAdapter(preciousPart);
        Console.WriteLine("Адаптированная цена: " + adapter.CalculatePrice());

        // Visitor
        Furniture furniture = new Furniture();
        furniture.Init(new FurniturePart(5, 10), new FurniturePart(3, 15), new FurniturePart(2, 20), 2, 3, 4, 500, "Шкаф");
        PriceModifier modifier = new PriceModifier(1.2);
        modifier.Visit(furniture);
        furniture.Display();

        // Template Method
        FurnitureTemplate woodenChair = new WoodenChair();
        woodenChair.Build();

        FurnitureTemplate metalTable = new MetalTable();
        metalTable.Build();
    }
}
