using System;

namespace AI.DecisionTrees
{
    /// <summary>
    /// Makes a decision according to a Func of the DecisionNode given
    /// </summary>
    public class DecisionNode : IDecisionTreeNode
    {
        // Stores a node for if the Func is true
        private readonly IDecisionTreeNode trueNode;

        // Stores a node for if the Func is false
        private readonly IDecisionTreeNode falseNode;

        // Stores a Func returning a bool for if the method is true or false
        private readonly Func<bool> doDecision;

        /// <summary>
        /// Constructor of the DecisionNode
        /// </summary>
        /// <param name="decision"> The wanted method for the Func </param>
        /// <param name="trueNode"> The node for if the Func is true </param>
        /// <param name="falseNode"> The node for if the Func is false </param>
        public DecisionNode(Func<bool> decision,
            IDecisionTreeNode trueNode, IDecisionTreeNode falseNode)
        {
            // The Func that decides what node to choose
            doDecision = decision;
            // The node if the Func returns true
            this.trueNode = trueNode;
            // The node if the Func returns false
            this.falseNode = falseNode;
        }

        /// <summary>
        /// Calls the Func and returns the node according to the Func
        /// </summary>
        /// <returns> The IDecision Node chosen </returns>
        public IDecisionTreeNode MakeDecision()
        {
            // Trinary operator to choose the node
            IDecisionTreeNode branch = doDecision() ? trueNode : falseNode;

            // Returns the MakeDecision method of the node chosen
            return branch.MakeDecision();
        }
    }
}