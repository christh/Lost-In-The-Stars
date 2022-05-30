using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IR.Common
{
    public static class Utils
    {
        public static float GetAsteroidScale(AsteroidSize size)
        {
            float result = 0;

            switch (size)
            {
                case AsteroidSize.Huge:
                    result = 10;
                    break;
                case AsteroidSize.Large:
                    result = 5;
                    break;
                case AsteroidSize.Medium:
                    result = 2.5f;
                    break;
                case AsteroidSize.Small:
                    result = 1;
                    break;
                case AsteroidSize.Tiny:
                    result = 0.675f;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// The force that asteroid sprayers kick out: depends on asteroid size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns>The magnitude as a float.</returns>
        internal static float GetAsteroidForceMagnitude(AsteroidSize size)
        {
            float result = 0;
            switch (size)
            {
                case AsteroidSize.Huge:
                    result = 1000;
                    break;
                case AsteroidSize.Large:
                    result = 500;
                    break;
                case AsteroidSize.Medium:
                    result = 250;
                    break;
                case AsteroidSize.Small:
                    result = 125;
                    break;
                case AsteroidSize.Tiny:
                    result = 50;
                    break;
                default:
                    break;
            }

            return result;
        }

        internal static Vector2 GetAsteroidSpawnOffset(AsteroidSize size)
        {
            float result = 0;
            switch (size)
            {
                case AsteroidSize.Huge:
                    result = 3;
                    break;
                case AsteroidSize.Large:
                    result = 2;
                    break;
                case AsteroidSize.Medium:
                    result = 1;
                    break;
                case AsteroidSize.Small:
                    result = 0.5f;
                    break;
                case AsteroidSize.Tiny:
                    result = 0.25f;
                    break;
                default:
                    break;
            }

            // Lil bit of maths...
            var randX = Random.Range(-result * 1.5f, result);

            if (randX < 0 && randX > result)
            {
                randX = result;
            }

            if (randX > 0 && randX < result)
            {
                randX = result;
            }

            return new Vector2(randX, randX);
        }

        public static Quaternion AddInaccuracyToRotation(Quaternion rotation, float inaccuracy)
        {
            inaccuracy = Random.Range(-inaccuracy, inaccuracy); // roll for positive or negative accuracy
            var newAngle = inaccuracy * Constants.MaximumDegreesOfInaccuracy;

            rotation *= Quaternion.Euler(0, 0, newAngle);

            return rotation;
        }


        public static void PlayRandomAudioClip(AudioSource audioSource, AudioClip[] audioClips, bool randomPitch = true)
        {
            if (audioSource && audioClips?.Length > 0)
            {
                if (randomPitch)
                {
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                }

                audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
            }
        }

        public static void PlayAudioClip(AudioSource audioSource, AudioClip audioClip, bool randomPitch = true)
        {
            if (audioSource && audioClip)
            {
                if (randomPitch)
                {
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                }

                audioSource.PlayOneShot(audioClip);
            }
        }
    }
}
