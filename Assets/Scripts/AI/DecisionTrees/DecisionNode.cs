using System;

public class DecisionNode : IDecisionTreeNode
{
    private readonly IDecisionTreeNode trueNode;
    private readonly IDecisionTreeNode falseNode;
    private readonly Func<bool> doDecision;

    public DecisionNode(Func<bool> decision,
        IDecisionTreeNode trueNode, IDecisionTreeNode falseNode)
    {
        doDecision = decision;
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public IDecisionTreeNode MakeDecision()
    {
        IDecisionTreeNode branch = doDecision() ? trueNode : falseNode;

        return branch.MakeDecision();
    }
}