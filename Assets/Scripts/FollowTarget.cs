using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class FollowTarget : MonoBehaviour
    {
        public GameObject Target;

        [SerializeField] float thrust = 10f;
        [SerializeField] float maxThrust = 35f;
        [SerializeField] float torque = 0.1f;
        [SerializeField] float maxTorque = 60f; // degrees per second
        [SerializeField] bool needsLineOfSight = false;
        [SerializeField] float detectionRange = 30f;
        [SerializeField] float maintainedDistance = 2f;
        private Rigidbody2D Body;
        private float FudgeFactor = 0.05f; // chance to get lazy on the throttle

        void Start()
        {
            Body = GetComponent<Rigidbody2D>();
            Target = GameObject.FindWithTag("Player");
        }

        void FixedUpdate()
        {
            if (Target)
            {
                if (needsLineOfSight && !IsPlayerInLineOfSight())
                {
                    return;
                }

                TurnTowardsTarget();
                MoveTowardsTarget();
            }
        }

        private void MoveTowardsTarget()
        {
            if (Vector2.Distance(transform.position, Target.transform.position) < maintainedDistance)
            {
                return;
            }
            // Apply thrust
            if (thrust > 0 && Body.velocity.magnitude < maxThrust)
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

            var torqueToApply = angleDifference * crossProduct.z;

            // Clamp torque
            if (torqueToApply > maxTorque)
            {
                torqueToApply = torque;
            }
            else if (torqueToApply < -torque)
            {
                torqueToApply = -torque;
            }

            if (System.Math.Abs(Body.angularVelocity) < maxTorque) //&& System.Math.Abs(angleDiff) > .05)
            {
                Body.AddTorque(torqueToApply);
            }
        }

        private bool IsPlayerInLineOfSight()
        {
            if (Vector2.Distance(transform.position, Target.transform.position) > detectionRange)
            {
                return false;
            }

            RaycastHit2D hit = Physics2D.Linecast(
                transform.position,
                Target.transform.position,
                1 << Constants.Layers.Environment);

            if (hit.collider != null)
            {
                return false;
            }

            return true;
        }
    }
}