using UnityEngine;
using System.Collections;
using System;

namespace IR.Factories
{
    public class ExplosionFactory : MonoBehaviour
    {
        public static void SpawnExplosion(GameObject explosion, AudioClip audio, Vector2 position)
        {
            if (explosion != null)
            {
                var obj = Instantiate(explosion, position, Quaternion.identity);
                var component = obj.GetComponent<ParticleSystem>();
                Destroy(obj, component.main.duration);

            }

            if (audio != null)
            {
                AudioSource.PlayClipAtPoint(audio, position);
                //Destroy(audioSource, audio.length);
            }
        }

        internal static void SpawnExplosion3D(GameObject explosion, AudioClip audio, Vector3 position)
        {
            if (explosion != null)
            {
                var obj = Instantiate(explosion, position, Quaternion.identity);
                var component = obj.GetComponent<ParticleSystem>();
                Destroy(obj, component.main.duration);

            }

            if (audio != null)
            {
                AudioSource.PlayClipAtPoint(audio, position);
                //Destroy(audioSource, audio.length);
            }
        }
    }
}