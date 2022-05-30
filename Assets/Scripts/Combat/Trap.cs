using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] EnemySpawner[] spawners;
        [SerializeField] bool isEndLevelTrap;
        [SerializeField] bool isWinGameTrap;

        Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
            {
                return;
            }

            if (isWinGameTrap)
            {
                UIManager.Instance.WinState();
                GameManager.Instance?.GotoNextLevel("End");
            }

            if (isEndLevelTrap)
            {
                GameManager.Instance.LevelWinStateReached = true;
            }
            var player = collision.gameObject;
            //player.GetComponent<PlayerMovement>().enabled = false;

            foreach (var spawner in spawners)
            {
                print($"Spawning spawner: {spawner.name}");
                spawner.StartSpawning();
            }

            rb.simulated = false;
        }
    }
}