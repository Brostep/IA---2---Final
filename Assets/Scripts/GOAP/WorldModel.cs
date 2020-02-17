using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel
{
    public int tomatoes;
    public int seeds;
    public float money;
    public int item;
	public bool isAThief;
	public bool hasPayedTaxes;
    public bool isPizzaMaster;
	public bool givenAdvice;
	public int stealCount;
	public bool hasflour;

	public static bool IsEqual(WorldModel a, WorldModel b)
    {
        return a.tomatoes == b.tomatoes &&
            a.seeds == b.seeds &&
            a.money == b.money &&
            a.item == b.item &&
            a.hasPayedTaxes == b.hasPayedTaxes &&
            a.isPizzaMaster == b.isPizzaMaster &&
			a.givenAdvice == b.givenAdvice &&
			a.hasflour == b.hasflour &&
			a.isAThief == b.isAThief &&
			a.stealCount == b.stealCount;
    }

    public static WorldModel Clone(WorldModel baseModel)
    {
        var clone = new WorldModel();
        clone.tomatoes = baseModel.tomatoes;
        clone.seeds = baseModel.seeds;
        clone.money = baseModel.money;
        clone.item = baseModel.item;
        clone.hasPayedTaxes = baseModel.hasPayedTaxes;
        clone.isPizzaMaster = baseModel.isPizzaMaster;
        clone.givenAdvice = baseModel.givenAdvice;
		clone.hasflour = baseModel.hasflour;
		clone.stealCount = baseModel.stealCount;
		clone.isAThief = baseModel.isAThief;

		return clone;
    }

    public static WorldModel UpdateValues(WorldModel wm, List<Func<WorldModel,WorldModel>> effects)
    {
        var newValue = Clone(wm);
        foreach (var e in effects)
        {
            newValue = e(newValue);
        }

        return newValue;
    }
}
