using System;
using UnityEngine;

namespace AI.DecisionTrees
{
    /// <summary>
    /// Stores and calls an Action
    /// </summary>
    public class ActionNode : IDecisionTreeNode, IGameAction
    {
        // Stores an Action
        private readonly Func<Vector3> gameAction;

        /// <summary>
        /// Constructor of the ActionNode
        /// </summary>
        /// <param name="gameAction"> The method to be run </param>
        public ActionNode(Func<Vector3> gameAction)
        {
            // Assigns the gameAction the Action given
            this.gameAction = gameAction;
        }

        /// <summary>
        /// Runs the Action 
        /// </summary>
        public Vector3 Execute()
        {
            return gameAction();
        }

        /// <summary>
        /// Returns this ActionNode when is called
        /// </summary>
        /// <returns> A Decision or Action node</returns>
        public IDecisionTreeNode MakeDecision() => this;
    }
}