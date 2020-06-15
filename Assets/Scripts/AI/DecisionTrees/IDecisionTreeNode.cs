namespace AI.DecisionTrees
{
    /// <summary>
    /// Interface to be used to call MakeDecision method
    /// </summary>
    public interface IDecisionTreeNode
    {
        /// <summary>
        /// Method Execute to run the method overwritten
        /// </summary>
        IDecisionTreeNode MakeDecision();
    }
}