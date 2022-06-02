using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IR
{
    public class PlayerMovement : MonoBehaviour
    {
        public ParticleSystem LeftThruster;
        public ParticleSystem RightThruster;
        public ParticleSystem LeftReverseThruster;
        public ParticleSystem RightReverseThruster;

        public float baseThrust = 10f;
        public float baseMaxThrust = 30;
        public float baseTorque = 20f;
        public float baseMaxTorque = 150f; // degrees per second
        public float afterburnerModifier = 1.5f; // multiple of normal max thrust

        float thrust = 10f;
        float maxThrust = 30;
        float torque = 20f;
        float maxTorque = 150f; // degrees per second

        private Rigidbody2D playerBody;
        private ThrustStates ThrustState = ThrustStates.Idle;
        private TurnStates TurnState = TurnStates.Idle;
        private float particleStartSize;
        private bool afterburning = false;

        // Start is called before the first frame update
        void Start()
        {
            thrust = baseThrust;
            maxThrust = baseMaxThrust;
            torque = baseTorque;
            maxTorque = baseMaxTorque;

            playerBody = GetComponent<Rigidbody2D>();
            LeftThruster.Stop();
            RightThruster.Stop();
            LeftReverseThruster.Stop();
            RightReverseThruster.Stop();

            particleStartSize = LeftThruster.startSize;
        }

        void FixedUpdate()
        {
            switch (GameManager.Instance.GetShipLevel())
            {
                case 1:
                    break;
                case 2:
                    thrust = baseThrust * 1.5f;
                    maxThrust = baseMaxThrust * 1.5f;
                    break;
                case 3:
                    thrust = baseThrust * 1.75f;
                    maxThrust = baseMaxThrust * 1.75f;
                    break;
                default:
                    break;
            }

            ThrustState = ThrustStates.Idle;
            TurnState = TurnStates.Idle;

            if (GameManager.Instance.EasyMovement)
            {
                HandleEasySteering();
            }
            else
            {
                HandleProSteering();
            }

            StopAfterburner();

            ManageThrusterParticles();

            if (Input.GetKeyDown(KeyCode.M))
            {
                GameManager.Instance.EasyMovement = !GameManager.Instance.EasyMovement;
            }
        }

        private void HandleEasySteering()
        {
            Vector3 newTarget = new Vector3(transform.position.x, transform.position.y);

            if (Input.GetKey(KeyCode.A))
            {
                newTarget.x += 5;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                newTarget.x -= 5;
            }

            if (Input.GetKey(KeyCode.S))
            {
                newTarget.y += 5;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                newTarget.y -= 5;
            }

            if (newTarget == transform.position)
            {
                return;
            }

            Vector3 targetDelta = transform.position - newTarget;

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

            if (torqueToApply > 0 && playerBody.angularVelocity < maxTorque) //&& System.Math.Abs(angleDiff) > .05)
            {
                playerBody.AddTorque(torqueToApply);
                TurnState = TurnStates.Left;
            }
            else if (torqueToApply < 0 && playerBody.angularVelocity > -maxTorque)
            {
                playerBody.AddTorque(torqueToApply);
                TurnState = TurnStates.Right;
            }

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                && playerBody.velocity.y < maxThrust * afterburnerModifier)
            {
                playerBody.AddForce(transform.up * thrust * afterburnerModifier);
                ThrustState = ThrustStates.Afterburning;
            }
            else if (playerBody.velocity.y < maxThrust)
            {
                playerBody.AddForce(transform.up * thrust);
                ThrustState = ThrustStates.Thrusting;
            }
        }

        private void HandleProSteering()
        {
            if (Input.GetKey(KeyCode.A) && playerBody.angularVelocity < maxTorque)
            {
                playerBody.AddTorque(torque);
                TurnState = TurnStates.Left;
            }
            if (Input.GetKey(KeyCode.D) && playerBody.angularVelocity > -maxTorque)
            {
                playerBody.AddTorque(-torque);
                TurnState = TurnStates.Right;
            }

            if (Input.GetKey(KeyCode.W)
                && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                && playerBody.velocity.y < maxThrust * afterburnerModifier))
            {
                playerBody.AddForce(transform.up * thrust * afterburnerModifier);
                ThrustState = ThrustStates.Afterburning;
            }
            else if (Input.GetKey(KeyCode.W) && playerBody.velocity.y < maxThrust)
            {
                playerBody.AddForce(transform.up * thrust);
                ThrustState = ThrustStates.Thrusting;
            }
            if (Input.GetKey(KeyCode.S) && Math.Abs(playerBody.velocity.y) < maxThrust)
            {
                playerBody.AddForce(transform.up * -thrust / 2);
                ThrustState = ThrustStates.Reversing;
            }
        }

        private void StopAfterburner()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                StopThruster(LeftThruster);
                StopThruster(RightThruster);
            }
        }

        private void ManageThrusterParticles()
        {
            if (ThrustState == ThrustStates.Afterburning)
            {
                AfterburnerThruster(RightThruster);
                AfterburnerThruster(LeftThruster);
            }

            // Stop left thruster if it's active and we're not thrusting nor turning right
            if (LeftThruster.isPlaying && ThrustState == ThrustStates.Idle && TurnState != TurnStates.Right)
            {
                StopThruster(LeftThruster);
            }

            // Start left thruster if it's not active and we're thrusting or turning right
            else if (!LeftThruster.isEmitting && (ThrustState == ThrustStates.Thrusting || TurnState == TurnStates.Right))
            {
                StartThruster(LeftThruster);
            }

            if (RightThruster.isPlaying && ThrustState == ThrustStates.Idle && TurnState != TurnStates.Left)
            {
                StopThruster(RightThruster);
            }
            else if (!RightThruster.isEmitting && (ThrustState == ThrustStates.Thrusting || TurnState == TurnStates.Left))
            {
                StartThruster(RightThruster);
            }

            // Rear thrusters
            if (LeftReverseThruster.isPlaying && (ThrustState != ThrustStates.Reversing && TurnState != TurnStates.Left))
            {
                StopThruster(LeftReverseThruster);
            }
            else if (!LeftThruster.isEmitting && (ThrustState == ThrustStates.Reversing || TurnState == TurnStates.Right))
            {
                StartThruster(LeftReverseThruster);
            }

            if (RightReverseThruster.isPlaying && (ThrustState != ThrustStates.Reversing && TurnState != TurnStates.Right))
            {
                // Not thrusting - stop Right Thruster
                StopThruster(RightReverseThruster);
            }
            else if (!RightThruster.isEmitting && (ThrustState == ThrustStates.Reversing || TurnState == TurnStates.Left))
            {
                StartThruster(RightReverseThruster);
            }
        }

        private void StopThruster(ParticleSystem ps)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void StartThruster(ParticleSystem ps)
        {
            var main = ps.main;
            var emission = ps.emission;
            emission.enabled = true;
            main.startSize = particleStartSize;
            main.startDelay = 0;
            ps.Play();
        }

        private void AfterburnerThruster(ParticleSystem ps)
        {
            var c = ps.colorOverLifetime;
            c.color = new Color(0, 0.5f, 1, 0.5f);

            var main = ps.main;
            var emission = ps.emission;
            main.startSize = particleStartSize * 3;
            main.startColor = new Color(0, 0.5f, 1, 0.5f);
            emission.enabled = true;
            main.startDelay = 0;
            ps.Play();
        }
    }
}