using AI.PathFinding.GridGeneration;
using UnityEngine;

namespace AI.DecisionTrees
{
    public class AIBrainController
    {
        private readonly GridGenerator _area;
        private readonly GameObject _ai;
        private IDecisionTreeNode root;
        private Vector3 _desiredPos;
        private GameObject _choosenGhost;
        private GameObject _choosenObj;
        private readonly DummyPlayer _player;
        private readonly Animator _anim;

        private bool _wasPlayerInArea;
        private int counter;
        private int attackDelayTimer;
        private int _rndTimeForDecision;
        private int interactionTimer;
        public bool AttackingTag { get; private set; }

        public AIBrainController(GridGenerator area, GameObject ai, DummyPlayer player, Animator anim)
        {
            _player = player;
            _anim = anim;
            _area = area;
            _ai = ai;

            _rndTimeForDecision = Random.Range(190, 200);

            attackDelayTimer = 100;

            GenerateTree();
        }

        private void GenerateTree()
        {
            IDecisionTreeNode freeRoam = new ActionNode(FreeRoam);
            IDecisionTreeNode getObjectPos = new ActionNode(ObjectInteraction);
            IDecisionTreeNode getGhostPos = new ActionNode(GhostInteraction);
            IDecisionTreeNode circlePlayer = new ActionNode(CirclePlayer);
            IDecisionTreeNode attackPlayer = new ActionNode(Attack);
            IDecisionTreeNode getToPlayer = new ActionNode(GetPlayerPosition);
            IDecisionTreeNode interactWithObject = new ActionNode(InteractWithObject);
            IDecisionTreeNode interactWithGhost = new ActionNode(InteractWithGhost);


            IDecisionTreeNode ghostIteractionNodes = new DecisionNode(IsNearGhost, interactWithGhost, getGhostPos);
            IDecisionTreeNode objectInteractionNodes = new DecisionNode(IsNearObject, interactWithObject, getObjectPos);
            IDecisionTreeNode interactionNodes = new DecisionNode(ConditionalRandomDecision, objectInteractionNodes, ghostIteractionNodes);
            IDecisionTreeNode canAttack = new DecisionNode(GetPlayerIsNear, attackPlayer, getToPlayer);
            IDecisionTreeNode attackingNodes = new DecisionNode(HasSpaceNearPlayer, canAttack, circlePlayer);
            IDecisionTreeNode normalBehaviour = new DecisionNode(ConditionalRandomDecision, freeRoam, interactionNodes);

            root = new DecisionNode(GetDesiredBehaviour, attackingNodes, normalBehaviour);
        }

        public Vector3 GetDecision()
        {
            counter++;

            if (counter >= _rndTimeForDecision || GetDesiredBehaviour())
            {
                ActionNode act = root.MakeDecision() as ActionNode;
                act.Execute();
                counter = 0;
            }
            return _desiredPos;
        }

        private bool IsNearGhost()
        {

            if (_choosenGhost != null && 
                Vector3.Distance(_ai.transform.position, 
                _choosenGhost.transform.position) <= 3f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsNearObject()
        {
            if (_choosenObj != null && 
                Vector3.Distance(_ai.transform.position,
                _choosenObj.transform.position) <= 2f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetDesiredBehaviour()
        {
            if (!_wasPlayerInArea && _area.PlayerIsInside)
            {
                _wasPlayerInArea = true;
                _anim.SetTrigger("Cast Spell");
            }
            if (!_area.PlayerIsInside)
            {
                _wasPlayerInArea = false;
            }
            return _area.PlayerIsInside;
        }

        private bool HasSpaceNearPlayer()
            => _player.NOfGhostsAround < 4 || AttackingTag;

        private bool GetPlayerIsNear()
        {
            float distanceToPlayer = Vector3.Distance(
                _ai.transform.position, _player.transform.position);

            if (!(distanceToPlayer <= 2f) && AttackingTag)
            {
                _player.NOfGhostsAround -= 1;
                AttackingTag = false;
            }

            return distanceToPlayer <= 2f;
        }

        private void Attack()
        {
            attackDelayTimer++;

            Vector3 dir = _ai.transform.position - _player.transform.position;
            // Resets the value of Y to 0
            dir.y = 0;

            // Rotates gradually the Ghost towards the direction
            _ai.transform.rotation = Quaternion.Lerp(_ai.transform.rotation,
                Quaternion.LookRotation(-dir), Time.deltaTime * 30);

            if (attackDelayTimer >= 100)
            {

                if (!AttackingTag)
                {
                    _player.NOfGhostsAround += 1;
                }

                AttackingTag = true;

                _desiredPos = Vector3.zero;

                _anim.SetTrigger("Bite Attack");

                if (_player != null)
                    _player.DealDamage(1f);

                attackDelayTimer = 0;
            }
        }

        private void CirclePlayer()
        {
            Vector3 dir = _ai.transform.position - _player.transform.position;

            dir = dir.normalized * 5;

            _desiredPos = _player.transform.position + dir;
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
        private void FreeRoam()
        {
            float x = Random.Range(-(_area.areaSize.x - 1), _area.areaSize.x);
            x += _area.transform.position.x;

            float z = Random.Range(-(_area.areaSize.z - 1), _area.areaSize.z);
            z += _area.transform.position.z;

            _desiredPos = new Vector3(x, 0, z);
        }

        private void ObjectInteraction()
        {
            if (_choosenObj == null)
            {
                Collider[] col = Physics.OverlapSphere(_ai.transform.position,
                    5f, LayerMask.GetMask("Interactable"));

                if (col.Length <= 0)
                {
                    FreeRoam();
                    return;
                }

                Collider choosenCol = col[Random.Range(0, col.Length)];

                _choosenObj = choosenCol.gameObject;

                _desiredPos = choosenCol.transform.position;
            }
            else
            {
                _desiredPos = _choosenObj.transform.position;
            }
        }

        private void GhostInteraction()
        {
            if (_choosenGhost == null)
            {
                Collider[] col = Physics.OverlapSphere(_ai.transform.position,
                    5f, LayerMask.GetMask("Entity"));

                if (col.Length <= 0)
                {
                    FreeRoam();
                    return;
                }

                for (int i = 0; i < col.Length; i++)
                {
                    if (col[i].CompareTag("GhostEnemy"))
                    {
                        Collider ghost = col[i];

                        _choosenGhost = ghost.gameObject;

                        _desiredPos = ghost.gameObject.transform.position;
                        break;
                    }
                }
            }
            else
            {
                _desiredPos = _choosenGhost.transform.position;
            }
        }

        private void GetPlayerPosition()
        {
            _desiredPos = _player.gameObject.transform.position;
        }

        private void InteractWithGhost()
        {
            if (interactionTimer == 0)
            {
                _choosenGhost.GetComponent<Animator>().SetTrigger("Defend");

                _anim.SetTrigger("Defend");

                _desiredPos = Vector3.zero;
            }
            else if (interactionTimer >= 1)
            {
                _choosenGhost = null;

                interactionTimer = 0;
            }
            interactionTimer++;
        }

        private void InteractWithObject()
        {
            if (interactionTimer == 0)
            {
                _choosenObj.SetActive(false);

                _desiredPos = Vector3.zero;
            }
            else if (interactionTimer >= 60)
            {
                _choosenObj = null;

                interactionTimer = 0;
            }
            interactionTimer++;

        }
    }
}