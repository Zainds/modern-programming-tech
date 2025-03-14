class PreciousFurniturePart : FurniturePart
{
    private double preciousMetalsPricePer100g;

    public PreciousFurniturePart() : base()
    {
        preciousMetalsPricePer100g = 0;
    }

    public PreciousFurniturePart(double preciousMetalsPricePer100g, FurniturePart fp) : 
        base(fp.GetWeight(), fp.getPriceForGram())
    {
        this.preciousMetalsPricePer100g = preciousMetalsPricePer100g;
    }

    public void Init(int weight, double priceForGram, double preciousPrice)
    {
        base.Init(weight, priceForGram);
        this.preciousMetalsPricePer100g = preciousPrice;
    }

    public new double GetPartPrice()
    {
        return base.GetPartPrice() + (preciousMetalsPricePer100g / 100 * GetWeight());
    }
}