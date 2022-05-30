using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget3D : MonoBehaviour
{
    public GameObject Target;

    public float thrust = 10f;
    public float maxThrust = 35f;
    public float torque = 0.1f;
    public float maxTorque = 60f; // degrees per second

    private Rigidbody Body;
    private float FudgeFactor = 0.05f; // chance to get lazy on the throttle

    void Start()
    {
        Body = GetComponent<Rigidbody>();
        Target = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        if (Target)
        {
            TurnTowardsTarget();
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        //// Apply thrust
        if ((thrust > 0 && Body.velocity.magnitude < maxThrust))
        {
            if (Random.value > FudgeFactor)
                Body.AddForce(transform.up * thrust);
        }
    }

    private void TurnTowardsTarget()
    {
        Vector3 targetDelta = Target.transform.position - transform.position;

        float angleDifference = Vector3.Angle(transform.up, targetDelta);

        // Axis of rotation to get from one vector to the other
        Vector3 crossProduct = Vector3.Cross(transform.up, targetDelta);

        var torqueToApply = angleDifference * crossProduct.z * Time.fixedDeltaTime;

        // Clamp torque
        if (torqueToApply > maxTorque)
        {
            torqueToApply = torque;
        }
        else if (torqueToApply < -torque)
        {
            torqueToApply = -torque;
        }

        if (System.Math.Abs(Body.angularVelocity.magnitude) < maxTorque) //&& System.Math.Abs(angleDiff) > .05)
        {
            Body.AddTorque(new Vector3(0, 0, torqueToApply));
        }
    }
}
