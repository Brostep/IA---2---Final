using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GOAP
{
    GoapState currentState;

    public GOAP(GoapState initialState)
    {
        currentState = initialState;
    }

    public IEnumerable<Tuple<bool,GoapState, IEnumerable<GoapState>>> Execute()
    {
        return LazyAStar.Run(currentState, SatisfiesGoal,Expand, Heuristic, Equals);
    }

    bool Equals(WorldModel wm1, WorldModel wm2)
    {
        return WorldModel.IsEqual(wm1, wm2);
    }

    bool SatisfiesGoal(GoapState state)
    {
        return state.Goal(state.CurrentWorldModel);
    }

    List<Tuple<GoapState,float>> Expand(GoapState state)
    {
        var list = new List<Tuple<GoapState, float>>();

        foreach (var action in state.Actions)
        {
            var preconditionsNotSuccess = false;

            foreach (var precon in action.Preconditions)
            {
                if (!precon(state.CurrentWorldModel))
                {
                    preconditionsNotSuccess = true;
                    break;
                }
            }

            if (!preconditionsNotSuccess)
            {
                var newState = Execute(action, state);
                list.Add(Tuple.Create(newState, newState.Heuristic(newState.CurrentWorldModel, state.CurrentWorldModel) + action.Cost));
            }
        }

        return list.OrderBy(i => i.Item2).ToList();
    }

    GoapState Execute(GoapAction action, GoapState state)
    {
        WorldModel newCurrentValues = WorldModel.UpdateValues(state.CurrentWorldModel, action.Effects);

        return new GoapState(state.Actions, action, newCurrentValues, state.Goal, state.Heuristic);
    }

    float Heuristic(GoapState newState, GoapState oldState)
    {
        return newState.Heuristic(newState.CurrentWorldModel, oldState.CurrentWorldModel);
    }
}
