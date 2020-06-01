using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionNode : IDecisionTreeNode , IGameAction
{
    private readonly Action gameAction;

    public ActionNode(Action gameAction)
    {
        this.gameAction = gameAction;
    }

    public void Execute()
    {
        gameAction();
    }

    public IDecisionTreeNode MakeDecision() => this;
}
