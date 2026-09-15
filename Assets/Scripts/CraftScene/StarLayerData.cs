using System;

[Serializable]
public class StarLayerData
{
    public FireworkStar star;
    public int layer;
    public int amount;

    public StarLayerData(FireworkStar star, int layer, int amount)
    {
        this.star = star;
        this.layer = layer;
        this.amount = amount;
    }
}