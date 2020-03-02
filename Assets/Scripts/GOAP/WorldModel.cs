using System;
using System.Collections.Generic;

public class WorldModel {
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

	public static bool IsEqual(WorldModel worldModel1, WorldModel worldModel2) {
        return worldModel1.tomatoes == worldModel2.tomatoes &&
            worldModel1.seeds == worldModel2.seeds &&
            worldModel1.money == worldModel2.money &&
            worldModel1.item == worldModel2.item &&
            worldModel1.hasPayedTaxes == worldModel2.hasPayedTaxes &&
            worldModel1.isPizzaMaster == worldModel2.isPizzaMaster &&
			worldModel1.givenAdvice == worldModel2.givenAdvice &&
			worldModel1.hasflour == worldModel2.hasflour &&
			worldModel1.isAThief == worldModel2.isAThief &&
			worldModel1.stealCount == worldModel2.stealCount;
    }

    public static WorldModel Clone(WorldModel baseModel) {
        var clone = new WorldModel() {
            tomatoes = baseModel.tomatoes,
            seeds = baseModel.seeds,
            money = baseModel.money,
            item = baseModel.item,
            hasPayedTaxes = baseModel.hasPayedTaxes,
            isPizzaMaster = baseModel.isPizzaMaster,
            givenAdvice = baseModel.givenAdvice,
            hasflour = baseModel.hasflour,
            stealCount = baseModel.stealCount,
            isAThief = baseModel.isAThief
        };
        
		return clone;
    }

    public static WorldModel UpdateValues(WorldModel worldModel, List<Func<WorldModel,WorldModel>> effects) {
        var newValue = Clone(worldModel);

        foreach (var e in effects)
            newValue = e(newValue);
        
        return newValue;
    }
}
