using System.Collections;
using UnityEngine;

/// <summary>
/// Calculates and applies to the gameobject the movement
/// </summary>
public class AIMovement : MonoBehaviour, IEntity, IDebug
{
    // -- Target given --
    [SerializeField] private GameObject target = null;
    // -- Designated area --
    [SerializeField] private GameObject area = null;

    // Provides a point for the AI to move to
    private AILogic ailogic;
    // Line for debugging the path
    private LineRenderer line;
    // The rigidbody attached to this gameobject
    private Rigidbody rb;

    /// <summary>
    /// Color of this ghost
    /// </summary>
    public GhostColor GColor { get; }
    /// <summary>
    /// Current HP of this ghosts
    /// </summary>
    public float Hp { get; set; }
    /// <summary>
    /// The Maximun HP it can have
    /// </summary>
    public float MaxHp { get; }
    /// <summary>
    /// The maximum velocity it can have
    /// </summary>
    public float MaxSpeed { get; }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        // Gets the rigidbody of this gameobject
        rb = GetComponent<Rigidbody>();
        // Creates a new AILogic passing in the grid
        ailogic = new AILogic(area.GetComponent<GridGenerator>());
    }

    /// <summary>
    /// This is called every physics update
    /// </summary>
    private void FixedUpdate()
    {
        // Gets a vector3 form the pathfinding
        Vector3? nextPoint = ailogic.GetPoint(gameObject, target);

        // Checks if the point received has a value
        if (nextPoint.HasValue)
        {
            // Calculates the direction of current position to the next point
            Vector3 dir = transform.position - nextPoint.Value;
            // Resets the value of Y to 0
            dir.y = 0;

            // Rotates gradually the Ghost towards the direction
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir), Time.unscaledDeltaTime * 3f);

            // Moves the Ghost foward
            rb.velocity = -transform.forward * MaxSpeed;
        }
    }
    public void DealDamage(float amount)
    {
        Hp -= amount;
    }
    public void Heal(float amount)
    {
        Hp = Mathf.Min(Hp + amount, MaxHp);
    }

    public void RunDebug(bool active)
    {
        StopCoroutine(DebugLine());
        Destroy(line);

        if (active)
        {
            line = gameObject.AddComponent<LineRenderer>();

            line.sortingLayerName = "Debug";
            line.sortingOrder = 5;
            line.positionCount = 1;
            line.SetPosition(0, transform.position);
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.useWorldSpace = true;

            StartCoroutine(DebugLine());
        }
    }

    private IEnumerator DebugLine()
    {
        while (true)
        {
            line.positionCount = 1;
            line.SetPosition(0, transform.position);

            for (int i = 0; i < ailogic.path.Count; i++)
            {
                if (i + 1 < ailogic.path.Count)
                {
                    line.positionCount += 1;
                    line.SetPosition(line.positionCount - 1, ailogic.path[i]);
                }
            }
            line.positionCount += 1;
            line.SetPosition(line.positionCount - 1, target.transform.position);
            yield return new WaitForFixedUpdate();
        }
    }
}