using IR.Factories;
using System;
using UnityEngine;

namespace IR
{
    public class Pickup : MonoBehaviour
    {
        public int Value = 1;
        public GameObject Animation;
        public AudioClip CollectSound;

        private Rigidbody2D Body;
        private SpriteRenderer Renderer;
        private AudioSource Audio;

        private void Start()
        {
            Audio = GetComponent<AudioSource>();
            Renderer = GetComponent<SpriteRenderer>();
            Body = GetComponent<Rigidbody2D>();
        }

        public event Action<Pickup> OnPickup;

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //HandlePickup(other);
                if (GameManager.IsInitialised)
                {
                    GameManager.Instance.AddMinerals(Value);
                }

                PickupFactory.PlaySpriteAnimation(Animation, transform.position);

                if (Body == null) return;

                Body.simulated = false;
                Renderer.enabled = false;

                if (CollectSound)
                {
                    Audio.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    Audio.PlayOneShot(CollectSound);
                    Destroy(gameObject, CollectSound.length);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}