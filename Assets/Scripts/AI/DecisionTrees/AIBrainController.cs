using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.PathFinding.GridGeneration;

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

    public AIBrainController(GameObject area, GameObject ai)
    {
        _area = area.GetComponent<GridGenerator>();
        _ai = ai;

        GenerateTree();
    }
    private void GenerateTree()
    {
        IDecisionTreeNode freeRoam = new ActionNode(FreeRoam);
        IDecisionTreeNode interact = new ActionNode(Interaction);
        IDecisionTreeNode objectInt = new ActionNode(ObjectInteraction);
        IDecisionTreeNode ghostInt = new ActionNode(GhostInteraction);

        IDecisionTreeNode interactionNodes = new DecisionNode(RandomBinaryDecision, objectInt, ghostInt);
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
    private bool RandomBinaryDecision()
    {
        float binaryDesision = Random.value;

        return binaryDesision > 0.5f ? true : false;
    }




    private void FreeRoam()
    {
        Debug.Log("Roaming");

        float x = Random.Range(-_area.areaSize.x, -_area.areaSize.x + 1);
        x += _area.transform.position.x;
        
        float z = Random.Range(-_area.areaSize.z, -_area.areaSize.z + 1);
        z += _area.transform.position.z;

        _desiredPos = new Vector3(x, 0, z);
    }

    private void Interaction()
    {
        Debug.Log("Interacting");

        if (_choosenObj != null)
        {
            _choosenObj.SetActive(false);
        }

        _choosenGhost.IsTargatable = true;
        _choosenGhost = null;
        _choosenObj = null;

        _desiredPos = _ai.transform.position;
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
