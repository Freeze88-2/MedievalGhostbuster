using System;
namespace AI.DecisionTrees
{
    /// <summary>
    /// Stores and calls an Action
    /// </summary>
    public class ActionNode : IDecisionTreeNode, IGameAction
    {
        // Stores an Action
        private readonly Action gameAction;

        /// <summary>
        /// Constructor of the ActionNode
        /// </summary>
        /// <param name="gameAction"> The method to be run </param>
        public ActionNode(Action gameAction)
        {
            // Assigns the gameAction the Action given
            this.gameAction = gameAction;
        }

        /// <summary>
        /// Runs the Action 
        /// </summary>
        public void Execute()
        {
            gameAction();
        }

        /// <summary>
        /// Returns this ActionNode when is called
        /// </summary>
        /// <returns> A Decision or Action node</returns>
        public IDecisionTreeNode MakeDecision() => this;
    }
}