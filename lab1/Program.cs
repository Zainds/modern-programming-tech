FurniturePart basicPart = new FurniturePart(150, 4);
    PreciousFurniturePart advPart = new PreciousFurniturePart(20, basicPart);
    EnhancedFurniture enhFurn = new EnhancedFurniture(basicPart, advPart);

    Console.WriteLine($"Общая стоимость мебели: {enhFurn.GetTotalCost()}");