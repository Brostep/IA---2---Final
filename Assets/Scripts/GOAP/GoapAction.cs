using System.Collections.Generic;
using System;

public class GoapAction
{
    public List<Func<WorldModel, WorldModel>> Effects { get { return _effects; } }
    public List<Func<WorldModel, bool>> Preconditions { get { return _preConditions; } }
    public string Name { get { return _actionKey.ToString(); } }
    public float Cost { get { return _cost; } }

    PlayerActionKey _actionKey;
    List<Func<WorldModel, bool>> _preConditions;
    List<Func<WorldModel, WorldModel>> _effects;
    float _cost;

    public GoapAction(PlayerActionKey key)
    {
        _actionKey = key;
        _preConditions = new List<Func<WorldModel, bool>>();
        _effects = new List<Func<WorldModel, WorldModel>>();
    }

    public GoapAction AddPrecondition(Func<WorldModel, bool> pre)
    {
        _preConditions.Add(pre);
        return this;
    }

    public GoapAction AddEffect(Func<WorldModel, WorldModel> eff)
    {
        _effects.Add(eff);
        return this;
    }

    public GoapAction AddCost(float cost)
    {
        _cost = cost;
        return this;
    }
}
