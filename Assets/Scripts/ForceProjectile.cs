using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class ForceProjectile : MonoBehaviour
    {
        Rigidbody2D rigidbody2d;
        public float speed = 600f;
        public float lifetime = 5f;
        public AudioClip LaunchSound;
        public int power = 10;

        private Rigidbody2D Body;
        private SpriteRenderer Renderer;
        private AudioSource Audio;
        private GameObject ignoreCollisionWithGameObject;

        void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();

            Destroy(gameObject, lifetime);
        }

        IEnumerator EnableProjectile(int layer)
        {
            gameObject.layer = Constants.Layers.IgnoreEnemyBullets;
            yield return new WaitForSeconds(0.2f);
            gameObject.layer = layer;
        }

        public void Launch(Vector2 direction, float force)
        {
            rigidbody2d.AddForce(rigidbody2d.transform.up * force);
        }

        public void Launch()
        {
            if (LaunchSound != null)
            {
                AudioSource.PlayClipAtPoint(LaunchSound, (Vector2)transform.position);
            }

            rigidbody2d.AddForce(rigidbody2d.transform.up * speed);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            other.collider.GetComponent<IDamageable>()?.Damage(power);
            GetComponent<IExplodeable>()?.Explode();
            //Destroy(gameObject);
        }

        internal void SetInitialVelocity(Vector2 velocity)
        {
            rigidbody2d.velocity = velocity;
        }

        internal void LaunchAt(Vector3 target)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
            //transform.LookAt(target);
            Launch();
        }

        internal void LaunchAt(Transform target)
        {
            transform.LookAt(target);
            Launch();
        }

        internal void BrieflyIgnoreCollisionsWithEnemies(int layer)
        {
            StartCoroutine(EnableProjectile(layer));
        }

        internal void SetPlayerGunUpgradeProperties()
        {
            if (GameManager.Instance.GetGunLevel() > 2)
            {
                GetComponent<SpriteRenderer>().color = Color.red; // new Color(238, 10, 18);
                transform.localScale = new Vector2(transform.localScale.x * 1.2f, transform.localScale.y * 1.2f);
                power += 10;
            }
        }

        internal void SetPlayerTurretUpgradeProperties()
        {
            if (GameManager.Instance.GetTurretLevel() > 2)
            {
                GetComponent<SpriteRenderer>().color = Color.yellow; // new Color(238, 10, 18);
                transform.localScale = new Vector2(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f);
                power += 15;
            }
        }
    }
}