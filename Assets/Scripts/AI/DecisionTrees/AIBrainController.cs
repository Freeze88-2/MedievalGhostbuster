using AI.PathFinding.GridGeneration;
using UnityEngine;

public class AIBrainController
{
    private readonly GridGenerator _area;
    private readonly GameObject _ai;
    private IDecisionTreeNode root;
    private Vector3 _desiredPos;
    private IEntity _choosenGhost;
    private GameObject _choosenObj;
    private int counter;
    private int attackDelayTimer;
    private DummyPlayer _player;
    private Animator _anim;
    private bool _wasPlayerInArea;
    public bool attackingTag { get; set; }

    public AIBrainController(GridGenerator area, GameObject ai, DummyPlayer player, Animator anim)
    {
        _player = player;
        _anim = anim;
        _area = area;
        _ai = ai;

        attackDelayTimer = 100;

        GenerateTree();
    }

    private void GenerateTree()
    {
        IDecisionTreeNode freeRoam = new ActionNode(FreeRoam);
        IDecisionTreeNode objectInteraction = new ActionNode(ObjectInteraction);
        IDecisionTreeNode ghostInteraction = new ActionNode(GhostInteraction);
        IDecisionTreeNode circlePlayer = new ActionNode(CirclePlayer);
        IDecisionTreeNode attackPlayer = new ActionNode(Attack);
        IDecisionTreeNode getToPlayer = new ActionNode(GetPlayerPosition);

        IDecisionTreeNode interactionNodes = new DecisionNode(RandomBinaryDecision, objectInteraction, ghostInteraction);
        IDecisionTreeNode canAttack = new DecisionNode(GetPlayerIsNear, attackPlayer, getToPlayer);
        IDecisionTreeNode attackingNodes = new DecisionNode(HasSpaceNearPlayer, canAttack, circlePlayer);
        IDecisionTreeNode normalBehaviour = new DecisionNode(RandomBinaryDecision, freeRoam, interactionNodes);

        root = new DecisionNode(GetDesiredBehaviour, attackingNodes, normalBehaviour);
    }

    public Vector3 GetDecision()
    {
        counter++;
        
        if (counter >= 200 || GetDesiredBehaviour())
        {
            ActionNode act = root.MakeDecision() as ActionNode;
            act.Execute();
            counter = 0;
        }
        return _desiredPos;
    }

    private bool GetDesiredBehaviour() 
    {
        if(!_wasPlayerInArea && _area.PlayerIsInside)
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
        => _player.NOfGhostsAround < 4 || attackingTag;

    private bool GetPlayerIsNear()
    {
        float distanceToPlayer = Vector3.Distance(
            _ai.transform.position, _player.transform.position);

        if (!(distanceToPlayer <= 2f) && attackingTag)
        {
            _player.NOfGhostsAround -= 1;
            attackingTag = false;
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

            if (!attackingTag)
            {
                _player.NOfGhostsAround += 1;
            }

            attackingTag = true;

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

    private bool RandomBinaryDecision() => Random.value > 0.5f ? true : false;

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
        Collider[] col = Physics.OverlapSphere(_ai.transform.position,
            10f, LayerMask.GetMask("Interactable"));

        if (col.Length <= 0)
        {
            FreeRoam();
            return;
        }

        Collider choosenCol = col[Random.Range(0, col.Length)];
        _choosenObj = choosenCol.gameObject;

        _desiredPos = choosenCol.transform.position;
    }

    private void GhostInteraction()
    {
        Collider[] col = Physics.OverlapSphere(_ai.transform.position,
            10f, LayerMask.GetMask("Entity"));

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

                _choosenGhost = ghost.gameObject.GetComponent<IEntity>();
                _choosenGhost.IsTargatable = false;

                _desiredPos = ghost.gameObject.transform.position;
                break;
            }
        }
    }

    private void GetPlayerPosition()
    {
        _desiredPos = _player.gameObject.transform.position;
    }
}