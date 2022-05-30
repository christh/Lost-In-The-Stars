using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR {
    public class Checkpoint : MonoBehaviour
    {
        LevelEntrance entrance;

        void Start()
        {
            entrance = FindObjectOfType<LevelEntrance>();
        }

        // Update is called once per frame
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
            {
                return;
            }

            entrance.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}