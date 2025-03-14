internal class EnhancedFurniture
{
    private FurniturePart basicPart;
    private PreciousFurniturePart advancedPart;

    public EnhancedFurniture()
    {
        basicPart = new FurniturePart();
        advancedPart = new PreciousFurniturePart();
    }

    public EnhancedFurniture(FurniturePart basicPart, PreciousFurniturePart advancedPart)
    {
        this.basicPart = basicPart;
        this.advancedPart = advancedPart;
    }

    public double GetTotalCost()
    {
        return basicPart.GetPartPrice() + advancedPart.GetPartPrice();
    }
}