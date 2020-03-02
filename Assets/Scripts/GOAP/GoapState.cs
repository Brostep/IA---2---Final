using System;
using System.Collections.Generic;

public class GoapState {
    public List<GoapAction> Actions { get; set; }
    public GoapAction GeneratedAction { get; set; }
    public WorldModel CurrentWorldModel { get; set; }
    public Func<WorldModel, bool> Goal { get; set; }
    public Func<WorldModel, WorldModel, float> Heuristic { get; set; }

    public GoapState(List<GoapAction> actions, GoapAction generatedAction, WorldModel initialValues, Func<WorldModel, bool> goal, Func<WorldModel, WorldModel, float> h) {
        Actions = actions;
        GeneratedAction = generatedAction;
        CurrentWorldModel = initialValues;
        Goal = goal;
        Heuristic = h;
    }

    public bool IsEqual(GoapState state) {
        if (Equals(state.CurrentWorldModel, CurrentWorldModel))
            return true;

        if (state.GeneratedAction != GeneratedAction)
            return false;

        return false;
    }
}
