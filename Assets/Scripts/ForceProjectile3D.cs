using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceProjectile3D : MonoBehaviour
{
    public float speed = 20f;
    public float maxSpeed = 50f;
    public float lifetime = 5f;
    public AudioClip LaunchSound;

    private Rigidbody Body;
    private SpriteRenderer Renderer;
    private AudioSource Audio;

    private Vector3 fixedDirection;

    void Awake()
    {
        Body = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }


    public void Launch()
    {
        if (LaunchSound != null)
        {
            AudioSource.PlayClipAtPoint(LaunchSound, (Vector2)transform.position);
        }

        Body.AddForce(transform.forward * speed);
    }

    private void FixedUpdate()
    {
        Body.AddForce(transform.forward * speed);
        Body.velocity = Vector3.ClampMagnitude(Body.velocity, maxSpeed);
    }

    internal void SetInitialVelocity(Vector3 velocity)
    {
        Body.velocity = velocity;
    }
}
