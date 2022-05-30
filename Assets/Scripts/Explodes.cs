using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IR.Factories;

namespace IR
{
    public class Explodes : MonoBehaviour, IExplodeable
    {
        public GameObject ExplosionPreFab;
        public AudioClip ExplosionAudio;


        private bool Triggered;


        public void Explode()
        {
            Triggered = true;

            // TODO: Create and play fade out animation..?
            ExplosionFactory.SpawnExplosion(ExplosionPreFab, ExplosionAudio, (Vector2)transform.position);
            // TODO: Disable collision / remove tag ..?
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 2);
        }
    }
}