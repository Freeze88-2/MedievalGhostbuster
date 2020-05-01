using System.Collections;
using UnityEngine;

/// <summary>
/// Calculates and applies to the gameobject the movement
/// </summary>
public class AIMovement : AIGhost, IDebug
{
    // Provides a point for the AI to move to
    private AILogic ailogic;
    // Line for debugging the path
    private LineRenderer line;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        // Gets the rigidbody of this gameobject
        rb = GetComponent<Rigidbody>();
        // Sets the color to the one of the editor
        GColor = gcolor;
        // Sets the Maximum hp to the one of the editor
        MaxHp = maxHp;
        // Sets the Maximum speed to the one of the editor
        MaxSpeed = maxSpeed;
        // Sets the current hp to the one of the editor
        Hp = hp;
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
            Vector3 dir =  nextPoint.Value - transform.position;
            // Resets the value of Y to 0
            dir.y = 0;

            // Rotates gradually the Ghost towards the direction
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir), Time.fixedDeltaTime * MaxSpeed * 6f);

            // Moves the Ghost foward
            rb.velocity = transform.forward * MaxSpeed;
        }
    }

    /// <summary>
    /// Setups a debug line on the game
    /// </summary>
    /// <param name="active"> Activate or deactivate the debug </param>
    public void RunDebug(bool active)
    {
        // Stops the drawing of the lines
        StopCoroutine(DebugLine());
        // Destroys the current line
        Destroy(line);

        // If its to activate the line
        if (active)
        {
            // Adds a line render to the gameobject
            line = gameObject.AddComponent<LineRenderer>();

            // Creates a sorting layer
            line.sortingLayerName = "Debug";
            // Sets the sorting layer order to 5
            line.sortingOrder = 5;
            // Sets the number of positions of line to 1
            line.positionCount = 1;
            // Set's the first position to the current position
            line.SetPosition(0, transform.position);
            // Sets the width of the line at the start
            line.startWidth = 0.05f;
            // Sets the width of the line at the end
            line.endWidth = 0.05f;
            // The line uses worldspace coordinates
            line.useWorldSpace = true;

            // Starts the drawing of the line
            StartCoroutine(DebugLine());
        }
    }

    /// <summary>
    /// Coroutine for drawing the line everyframe
    /// </summary>
    /// <returns> A wait timer </returns>
    private IEnumerator DebugLine()
    {
        // Performs a loop until the coroutine is stopped
        while (true)
        {
            // Sets the number of positions of line to 1
            line.positionCount = 1;
            // Set's the first position to the current position
            line.SetPosition(0, transform.position);

            // Runs through every point found by the pathfinding
            for (int i = 0; i < ailogic.Path.Count; i++)
            {
                if (i + 1 < ailogic.Path.Count)
                {
                    // Adds a point to the line
                    line.positionCount += 1;
                    // Sets the position of that point to a path point
                    line.SetPosition(line.positionCount - 1, ailogic.Path[i]);
                }
            }
            // Adds one last point
            line.positionCount += 1;
            // Sets the position of the point to the position of the target
            line.SetPosition(line.positionCount - 1, target.transform.position);
            // Waits for the end of the frame
            yield return new WaitForEndOfFrame();
        }
    }
}