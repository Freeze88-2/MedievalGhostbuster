using AI.PathFinding.GridGeneration;
using UnityEngine;

namespace AI.DecisionTrees
{
    public class AIBrainController
    {
        private readonly GridGenerator _area;

        private Vector3 _desiredPos;
        private readonly IDecisionTreeNode root;

        private bool _wasPlayerInArea;
        private int counter;
        private readonly int _rndTimeForDecision;
        
        private readonly AICombatDecision combat;
        private readonly AINormalDecision normal;

        public bool AttackingTag => combat.AttackingTag;

        public AIBrainController(GridGenerator area, GameObject ai, DummyPlayer player, Animator anim)
        {
            _area = area;

            

            _rndTimeForDecision = Random.Range(190, 200);
            _desiredPos = Vector3.zero;

            combat = new AICombatDecision(player, ai, anim);
            normal = new AINormalDecision(ai, area.areaSize.x, area.areaSize.z,
                area.transform.position);
            root = new DecisionNode(GetDesiredBehaviour, combat.AttackingNodes,
                normal.NormalBehaviour);
        }

        private bool GetDesiredBehaviour()
        {
            if (!_wasPlayerInArea && _area.PlayerIsInside)
            {
                _wasPlayerInArea = true;
                combat.PlayerIsInArea();
            }
            if (!_area.PlayerIsInside)
            {
                _wasPlayerInArea = false;
            }
            return _area.PlayerIsInside;
        }

        public Vector3 GetDecision()
        {
            counter++;

            if (counter >= _rndTimeForDecision || GetDesiredBehaviour())
            {
                ActionNode act = root.MakeDecision() as ActionNode;
                counter = 0;
                _desiredPos = act.Execute();
            }
            return _desiredPos;
        }
    }
}