using IR.Factories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class SpawnAsteroids : MonoBehaviour, IExplodeable
    {
        public GameObject AsteroidSprayer;
        public int SpawnCount = 2;
        bool Triggered;
        AsteroidSize nextSize = AsteroidSize.Large;

        public void Explode()
        {
            var size = GetComponent<Asteroid>().size;

            switch (size)
            {
                case AsteroidSize.Huge:
                    nextSize = AsteroidSize.Large;
                    break;
                case AsteroidSize.Large:
                    nextSize = AsteroidSize.Medium;
                    break;
                case AsteroidSize.Medium:
                    nextSize = AsteroidSize.Small;
                    break;
                case AsteroidSize.Small:
                    nextSize = AsteroidSize.Tiny;
                    break;
                case AsteroidSize.Tiny:
                    return;
                default:
                    break;
            }

            AsteroidFactory.SpawnAsteroids(AsteroidSprayer, transform.position, SpawnCount, nextSize, gameObject);
        }
    }
}