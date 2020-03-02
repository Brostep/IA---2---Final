using System.Collections.Generic;
using System;

public class GoapAction {
    private PlayerActionKey _actionKey;

    public string Name { get { return _actionKey.ToString(); } }
    public float Cost { get; set; }
    public List<Func<WorldModel, bool>> Preconditions { get; set; }
    public List<Func<WorldModel, WorldModel>> Effects { get; set; }
    
    public GoapAction(PlayerActionKey key) {
        _actionKey = key;
        Preconditions = new List<Func<WorldModel, bool>>();
        Effects = new List<Func<WorldModel, WorldModel>>();
    }
    
    public GoapAction AddCost(float cost) {
        Cost = cost;
        return this;
    }

    public GoapAction AddPrecondition(Func<WorldModel, bool> precondition) {
        Preconditions.Add(precondition);
        return this;
    }

    public GoapAction AddEffect(Func<WorldModel, WorldModel> effect) {
        Effects.Add(effect);
        return this;
    }
}
