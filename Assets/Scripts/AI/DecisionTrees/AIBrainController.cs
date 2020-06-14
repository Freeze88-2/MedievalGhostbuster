using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.PathFinding.GridGeneration;
using AI.Movement;

public class AIBrainController
{
    private readonly GridGenerator _area;
    private readonly GameObject _ai;
    private IDecisionTreeNode root;
    private Vector3 _desiredPos;
    private GameObject[] _interactables;
    private GameObject _choosenObj;
    private IEntity _choosenGhost;
    private int counter;
    private GameObject _player;

    public AIBrainController(GridGenerator area, GameObject ai, GameObject player)
    {
        _area = area;
        _ai = ai;
        _player = player;

        GenerateTree();
    }
    private void GenerateTree()
    {
        IDecisionTreeNode freeRoam = new ActionNode(FreeRoam);
        IDecisionTreeNode objectInteraction = new ActionNode(ObjectInteraction);
        IDecisionTreeNode ghostInteraction = new ActionNode(GhostInteraction);

        IDecisionTreeNode interactionNodes = new DecisionNode(RandomBinaryDecision, objectInteraction, ghostInteraction);
        root = new DecisionNode(RandomBinaryDecision, freeRoam, interactionNodes);
    }

    public Vector3 GetDecision()
    {
        counter++;

        if (counter >= 200)
        {
            ActionNode act = root.MakeDecision() as ActionNode;
            act.Execute();
            counter = 0;
        }
        return _desiredPos;
    }
    private bool GetDesiredBehaviour() =>_area.PlayerIsInside;

    private bool GetPlayerIsNear()
    {
        float distanceToPlayer = Vector3.Distance(
            _ai.transform.position, _player.transform.position);

        return distanceToPlayer <= 1.5f ? true : false;
    }

    //private bool HasSpaceNearPlayer()
    //{

    //}
    private bool RandomBinaryDecision()
    {
        float binaryDesision = Random.value;

        return binaryDesision > 0.5f ? true : false;
    }

    private void FreeRoam()
    {
        Debug.Log("Roaming");

        float x = Random.Range(-(_area.areaSize.x -1), _area.areaSize.x);
        x += _area.transform.position.x;
        
        float z = Random.Range(-(_area.areaSize.z -1), _area.areaSize.z);
        z +=  _area.transform.position.z;

        _desiredPos = new Vector3(x, 0, z);
    }

    private void ObjectInteraction()
    {
        Debug.Log("Interacting with objects");

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
        Debug.Log("Interacting with ghost");

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
}
