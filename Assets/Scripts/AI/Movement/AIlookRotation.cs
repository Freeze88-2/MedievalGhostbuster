//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AIlookRotation : MonoBehaviour
//{
//    private GameObject lookFakeTarget;

//    private AIMovement ghost;
//    [SerializeField] private float targetAngleRadius = 1f;
//    [SerializeField] private float slowdownAngleRadius = 10f;
//    [SerializeField] private float timeToTarget = 0.1f;
//    [SerializeField] private float maxRotation = 5f;
//    [SerializeField] private float weight = 1f;
//    [SerializeField] private float maxAngularAccel = 1f;
//    public float Weight => weight;

//    // Use Awake() to initialize our fake target
//    private void Awake()
//    {
//        lookFakeTarget = new GameObject();
//        lookFakeTarget.transform.position = Vector3.zero;
//        ghost = GetComponent<AIMovement>();
//    }

//    // Look Where You're Going behaviour
//    public SteeringValue GetSteering(Vector3? target)
//    {
//        // Initialize linear and angular forces to zero
//        SteeringValue sout = new SteeringValue(Vector2.zero, 0);

//        // Do I have a target?
//        if (target != null)
//        {
//            // Determine the direction to our target
//            Vector2 direction =
//                target.Value - transform.position;

//            // Continue only if there is a distance between us and the target
//            if (direction.magnitude > 0)
//            {
//                // Determine the orientation for our temporary target, which
//                // should be as if it was looking away from me
//                float angle = Vec2Deg(direction);

//                Transform newTransform = transform;
//                newTransform.position = target.Value;
//                // Set orientation of our fake target
//                newTransform.eulerAngles =
//                    new Vector3(0f, 0f, angle);

//                // Use align superclass to determine the torque to return based
//                // on the temp. target orientation (i.e. looking away from me)
//                sout = CalculateSteering(newTransform);
//            }
//        }

//        // Output the steering
//        return sout;
//    }
//    public float Vec2Deg(Vector3 vector)
//    {
//        return Mathf.Atan2(vector.y, vector.z) * Mathf.Rad2Deg;
//    }
//    private SteeringValue CalculateSteering(Transform target)
//    {

//        // Initialize linear and angular forces to zero
//        Vector2 linear = Vector2.zero;
//        float angular = 0f; // Not used

//        // Do I have a target?
//        if (target != null)
//        {
//            // Orientation differences (actual and absolute values)
//            float orientation, orientationAbs;

//            // Desired angular velocity and absolute angular force to apply
//            float desiredAngularVelocity, angularAbs;

//            // Get the orientation difference to the target
//            orientation = Mathf.DeltaAngle(
//                transform.eulerAngles.z,
//                target.transform.eulerAngles.z);

//            // Get the absolute orientation difference
//            orientationAbs = Mathf.Abs(orientation);

//            // Are we within the target angle radius yet?
//            if (orientationAbs < targetAngleRadius)
//            {
//                // Return no steering whatsoever
//                return new SteeringValue(Vector2.zero, 0f);
//            }
//            // Are we within the slowdown angle radius?
//            else if (orientationAbs < slowdownAngleRadius)
//            {
//                // Adjust desired angular velocity depending current
//                // orientation
//                desiredAngularVelocity =
//                    maxRotation * orientationAbs / slowdownAngleRadius;
//            }
//            else
//            {
//                // If we're outside the slowdown radius, go for max rotation
//                desiredAngularVelocity = maxRotation;
//            }

//            // Set the correct sign in the desired angular velocity
//            desiredAngularVelocity *= orientation / orientationAbs;

//            // Determine the angular force (difference in desired angular
//            // velocity and current angular velocity, divided by the time
//            // to target)
//            angular = (desiredAngularVelocity - ghost.AngularVelocity.magnitude)
//                / timeToTarget;

//            // Check if angular force/acceleration is too great
//            angularAbs = Mathf.Abs(angular);
//            if (angularAbs > ghost.MaxAngularAccel)
//            {
//                // If so set it to the maximum allowed value
//                angular /= angularAbs;
//                angular *= ghost.MaxAngularAccel;
//            }
//        }

//        // Output the steering
//        return new SteeringValue(linear, angular);
//    }
//}
