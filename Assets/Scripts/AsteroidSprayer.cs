using IR.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{


    public class AsteroidSprayer : MonoBehaviour
    {
        public int NumberOfAsteroids;
        public GameObject AsteroidPrefab;

        [SerializeField]
        public AsteroidSize size;
        public GameObject AsteroidType;

        public float SpawnOffSet = 1.5f;
        private PointEffector2D effectorExplosion;

        void Start()
        {
            effectorExplosion = GetComponent<PointEffector2D>();
            SpawnAsteroids();
        }

        void SpawnAsteroids()
        {
            for (int i = 0; i < NumberOfAsteroids; i++)
            {
                Vector2 spawnOffset = Utils.GetAsteroidSpawnOffset(size);

                var msObj = Instantiate(AsteroidType, (Vector2)transform.position + spawnOffset, Quaternion.identity, transform.parent);

                var ms = msObj.GetComponent<Asteroid>();
                ms.size = size;
                ms.Setup();
            }

            // Force is relative to asteroid size
            effectorExplosion.forceMagnitude = Utils.GetAsteroidForceMagnitude(size);
            effectorExplosion.enabled = true;

            Destroy(gameObject, .5f);

        }
    }
}