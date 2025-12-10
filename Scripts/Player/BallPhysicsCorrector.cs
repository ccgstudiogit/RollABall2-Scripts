using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysicsCorrector : MonoBehaviour
{
    private struct PredictedPhysics
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public bool Collision;
        public RaycastHit Hit;
    }

    [SerializeField]
    //The Rigidbody being targeted.
    private Rigidbody myRigidbody;
    [SerializeField]
    //The radius of the ball. This should typically be the same as the Sphere Collider radius, or very slightly less.
    private float ballRadius = 0.1375f;
    [SerializeField]
    //LayerMask for the OverlapSphere check so you can avoid always getting the ball itself as a result.
    private LayerMask layerMask;
    [SerializeField]
    //Threshold for detecting unexpected changes in velocity. This is the maximum angle between the expected velocity and actual velocity after simulation.
    private float velocityVectorDeviationThreshold = 10;
    [SerializeField]
    //Threshold for detecting incorrect contact point normals. This is the maximum angle between the contact point's normal and raycast's normal.
    private float contactNormalDeviationThreshold = 0.01f;
    [SerializeField]
    //Threshold used to ignore sphere cast intersecting at shallow angles.
    private float sphereCastVelocityNormalAngleThreshold = 5f;
    [SerializeField]
    //Flag allowing you to disable correction for testing purposes.
    private bool enableCorrection = true;

    private bool addForceFlag = false;
    private bool collisionEnterFlag = false;
    private bool hasInvalidCollisionEnter = false;

    private PredictedPhysics predictedPhysics;
    private Vector3 currentVelocity;
    private Vector3 currentAngularVelocity;

    private Collider[] overlapShereColliders = new Collider[2];

    //Use this whenever you add force or torque in code so that it will be accounted for on the next simulation.
    public void SetAddForceFlag()
    {
        addForceFlag = true;
    }

    //This should be called before Physics.Simulate.
    public void PreSimulate()
    {
        if (addForceFlag)
            return;

        predictedPhysics = getPrediction(transform.position, myRigidbody.velocity);
        currentVelocity = myRigidbody.velocity;
        currentAngularVelocity = myRigidbody.angularVelocity;
    }

    //This should be called after Physics.Simulate.
    public void PostSimulate()
    {
        //Various situations where we don't want to do any correction.
        if (!addForceFlag && (!collisionEnterFlag || hasInvalidCollisionEnter) && enableCorrection && !predictedPhysics.Collision && !myRigidbody.isKinematic)
        {
            float velocityDeviation = Vector3.Angle(myRigidbody.velocity, predictedPhysics.Velocity);

            //If the expected velocity doesn't match the actual velocity, reset to the expected values.
            if (velocityDeviation > velocityVectorDeviationThreshold)
            {
                transform.position = predictedPhysics.Position;
                myRigidbody.velocity = currentVelocity;
                myRigidbody.angularVelocity = currentAngularVelocity;
            }
        }

        addForceFlag = false;
        collisionEnterFlag = false;
        hasInvalidCollisionEnter = false;
    }

    private PredictedPhysics getPrediction(Vector3 currentPosition, Vector3 velocity)
    {
        RaycastHit hit;
        PredictedPhysics predictedPhysics = new PredictedPhysics();

        //Apply
        Vector3 velocityWithGravity = velocity + Physics.gravity * Time.fixedDeltaTime;
        float distance = velocityWithGravity.magnitude * Time.fixedDeltaTime;
        Ray ray = new Ray(currentPosition, velocityWithGravity);

        //Account for cases where ball may be slightly intersecting with a collider, as this will cause the SphereCast to not hit anything.
        int count = Physics.OverlapSphereNonAlloc(currentPosition, ballRadius, overlapShereColliders, layerMask);
        if (count > 0)
        {
            predictedPhysics.Collision = true;
            return predictedPhysics;
        }

        //Do a sphere cast across the predicted motion of the ball
        if (Physics.SphereCast(ray, ballRadius, out hit, distance))
        {
            float velocityNormalAngle = Mathf.Abs(90f - Vector3.Angle(hit.normal, velocityWithGravity));

            //Since we are always applying gravity to the velocity, many times the spherecast will hit the ground we are rolling over.
            //Deal with this by ignoring very shallow angles.
            if (velocityNormalAngle > sphereCastVelocityNormalAngleThreshold)
            {
                //Don't do any corrections if collision is expected
                predictedPhysics.Collision = true;
                predictedPhysics.Hit = hit;
                return predictedPhysics;
            }
        }

        //Getting to here means no collision is expected.
        //Note there is no accounting for drag or friction.
        predictedPhysics.Position = currentPosition + velocityWithGravity * Time.fixedDeltaTime;
        predictedPhysics.Velocity = velocityWithGravity;

        return predictedPhysics;
    }

    //Account for collisions that occur that cannot be predicted.
    //i.e. other moving objects that hit us.
    private void OnCollisionEnter(Collision collision)
    {
        //Set the collision flag
        collisionEnterFlag = true;

        ContactPoint[] contactPoints = collision.contacts;

        //OnCollisionEnter will be called when rolling over the edge between different objects.
        //Since this is exactly the scenario we are trying to fix, we can't just ignore it so we need to do some validation.
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint cp = collision.GetContact(i);

            //In the cases where the ball bounces while rolling over an edge, the contact point normal is usually different than expected
            //Raycasting on the other hand typically returns a normal in-line with what we expect, so compare the raycast normal to the contact point normal
            RaycastHit hit;
            Ray ray = new Ray(cp.point + cp.normal * 0.1f, -cp.normal);
            if (Physics.Raycast(ray, out hit, 0.2f))
            {
                float normalDeviation = Vector3.Angle(cp.normal, hit.normal);

                //If the normals deviate too much then something has gone wrong.
                //Make sure we do correction event though collisionEnterFlag is true.
                if (normalDeviation > contactNormalDeviationThreshold)
                {
                    hasInvalidCollisionEnter = true;
                    return;
                }
            }
        }
    }
}
