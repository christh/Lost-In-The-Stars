using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IR.Enemies;

namespace IR
{
    public class OpenAllBarriersOnDeath : MonoBehaviour
    {
        Enemy enemy;
        GameObject[] barriers;

        public void Start()
        {
            enemy = GetComponent<Enemy>();
            barriers = GameObject.FindGameObjectsWithTag("Confiner");
        }
        // Update is called once per frame
        void Update()
        {
            if (enemy.health <= 0)
            {
                print("destroying all barriers");
                foreach (var item in barriers)
                {
                    Destroy(item);
                }
                Destroy(gameObject);
            }
        }
    }
}