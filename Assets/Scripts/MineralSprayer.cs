using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class MineralSprayer : MonoBehaviour
    {
        public int NumberOfCoins = 4;
        public float Quality = 0.5f;
        public GameObject MineralPrefabBase;
        public GameObject MineralPrefabRare;
        public float SpawnOffSet = 0.1f;
        [SerializeField] bool overrideCoinValues = false;

        private Rigidbody2D rb;
        private PointEffector2D pickupEffectorExplosion;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            pickupEffectorExplosion = GetComponent<PointEffector2D>();

            SpawnMinerals();
        }

        void SpawnMinerals()
        {
            if (overrideCoinValues && NumberOfCoins > 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 spawnOffset = new Vector2(Random.Range(-SpawnOffSet, SpawnOffSet), Random.Range(-SpawnOffSet, SpawnOffSet));

                    var mineral = Instantiate(MineralPrefabRare, (Vector2)transform.position + spawnOffset, Quaternion.identity, transform.parent);
                    var pickup = mineral.GetComponent<Pickup>();
                    pickup.Value = NumberOfCoins / 20;
                }

                return;
            }
            else
            {
                for (int i = 0; i < NumberOfCoins; i++)
                {
                    Vector2 spawnOffset = new Vector2(Random.Range(-SpawnOffSet, SpawnOffSet), Random.Range(-SpawnOffSet, SpawnOffSet));

                    if (Random.value > Quality)
                    {
                        Instantiate(MineralPrefabBase, (Vector2)transform.position + spawnOffset, Quaternion.identity, transform.parent);
                    }
                    else
                    {
                        Instantiate(MineralPrefabRare, (Vector2)transform.position + spawnOffset, Quaternion.identity, transform.parent);
                    }
                }
            }
            pickupEffectorExplosion.enabled = true;

            Destroy(gameObject, .5f);

        }
    }
}