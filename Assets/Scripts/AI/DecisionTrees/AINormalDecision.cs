using UnityEngine;

namespace AI.DecisionTrees
{
    public class AINormalDecision
    {
        private readonly float x;
        private readonly float z;
        private readonly GameObject _ai;
        private GameObject _choosenGhost;
        private GameObject _choosenObj;
        private Vector3 _areaPos;
        private IEntity _ghost;
        private bool _firstTimeInteracting;

        public IDecisionTreeNode NormalBehaviour { get; }

        public AINormalDecision(GameObject ai, float x, float z, Vector3 area)
        {
            this.x = x;
            this.z = z;

            _ai = ai;
            _areaPos = area;
            _firstTimeInteracting = true;

            IDecisionTreeNode freeRoam = new ActionNode(FreeRoam);

            IDecisionTreeNode getObjectPos = new ActionNode(ObjectInteraction);

            IDecisionTreeNode getGhostPos = new ActionNode(GhostInteraction);

            IDecisionTreeNode objInt = new ActionNode(InteractWithObject);

            IDecisionTreeNode ghostInt = new ActionNode(InteractWithGhost);

            IDecisionTreeNode ghostIntNode = new DecisionNode
                (IsNearGhost, ghostInt, getGhostPos);

            IDecisionTreeNode objIntNode = new DecisionNode
                (IsNearObject, objInt, getObjectPos);

            IDecisionTreeNode interactionNodes = new DecisionNode
                (ConditionalRandomDecision, objIntNode, ghostIntNode);

            NormalBehaviour = new DecisionNode
                (ConditionalRandomDecision, freeRoam, interactionNodes);
        }

        public void UpdateRotation()
        {
            if ((_choosenGhost != null || _choosenObj != null)
                && !_firstTimeInteracting)
            {
                Vector3 intPos = _choosenObj == null ?
                    _choosenGhost.transform.position :
                    _choosenObj.transform.position;

                Vector3 dir = _ai.transform.position - intPos;
                // Resets the value of Y to 0
                dir.y = 0;

                // Rotates gradually the Ghost towards the direction
                _ai.transform.rotation =
                    Quaternion.Lerp(_ai.transform.rotation,
                    Quaternion.LookRotation(-dir), Time.deltaTime * 30);

                if (_choosenGhost != null)
                {
                    Vector3 dir2 = intPos - _ai.transform.position;
                    // Resets the value of Y to 0
                    dir2.y = 0;

                    // Rotates gradually the Ghost towards the direction
                    _choosenGhost.transform.rotation =
                        Quaternion.Lerp(_choosenGhost.transform.rotation,
                        Quaternion.LookRotation(-dir2), Time.deltaTime * 30);
                }
            }
        }

        private bool ConditionalRandomDecision()
        {
            if (_choosenObj != null)
            {
                return true;
            }
            else if (_choosenGhost != null)
            {
                return false;
            }
            else
            {
                return Random.value > 0.5f ? true : false;
            }
        }

        private bool IsNearGhost()
        {
            return _choosenGhost != null &&
                Vector3.Distance(_ai.transform.position,
                _choosenGhost.transform.position) <= 3f ? true : false;
        }

        private bool IsNearObject()
        {
            return _choosenObj != null &&
                Vector3.Distance(_ai.transform.position,
                _choosenObj.transform.position) <= 2f ? true : false;
        }

        private Vector3 FreeRoam()
        {
            float rndX = Random.Range(-(x - 1), x) + _areaPos.x;

            float rndZ = Random.Range(-(z - 1), z) + _areaPos.z;

            return new Vector3(rndX, 0, rndZ);
        }

        private Vector3 ObjectInteraction()
        {
            if (_choosenObj == null)
            {
                Collider[] col = Physics.OverlapSphere(_ai.transform.position,
                    5f, LayerMask.GetMask("Interactable"));

                if (col.Length <= 0)
                {
                    return FreeRoam();
                }

                Collider choosenCol = col[Random.Range(0, col.Length)];

                _choosenObj = choosenCol.gameObject;
            }
            return _choosenObj.transform.position;
        }

        private Vector3 GhostInteraction()
        {
            if (_choosenGhost == null)
            {
                Collider[] col = Physics.OverlapSphere(_ai.transform.position,
                    5f, LayerMask.GetMask("Entity"));

                for (int i = 0; i < col.Length; i++)
                {
                    if (col[i].CompareTag("GhostEnemy"))
                    {
                        Collider ghost = col[i];

                        _ghost = ghost.gameObject.GetComponent<IEntity>();

                        if (!_ghost.IsTargatable)
                        {
                            _ghost = null;
                            continue;
                        }

                        _choosenGhost = ghost.gameObject;

                        return ghost.gameObject.transform.position;
                    }
                }

                return FreeRoam();
            }
            return _choosenGhost.transform.position;
        }

        private Vector3 InteractWithGhost()
        {
            InteractionLogic();

            _ghost.IsTargatable = _firstTimeInteracting;

            return Vector3.zero;
        }

        private Vector3 InteractWithObject()
        {
            InteractionLogic();

            return Vector3.zero;
        }

        private void InteractionLogic()
        {
            if (_firstTimeInteracting)
            {
                _firstTimeInteracting = false;
            }
            else if (!_firstTimeInteracting)
            {
                _choosenObj = null;
                _choosenGhost = null;

                _firstTimeInteracting = true;
            }
        }
    }
}