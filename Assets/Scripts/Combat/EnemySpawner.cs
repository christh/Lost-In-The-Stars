using IR.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class EnemySpawner : MonoBehaviour
    {
        Grid grid;
        Player player;
        [SerializeField] Enemy enemyPrefab;
        [SerializeField] [Range(0, 10)] float minRange;
        [SerializeField] [Range(0, 10)] float maxRange;
        [SerializeField] [Range(0.01f, 10f)] float minSpawnTime = 0.2f;

        [SerializeField] [Range(0.02f, 10f)] float maxSpawnTime = 0f;
        [SerializeField] [Range(0.02f, 30f)] float delayBeforeStart = 1f;
        [SerializeField] bool runButton = true;
        [SerializeField] bool resetButton = false;
        bool isRunning = false;
        [SerializeField] int enemiesSpawned = 0;
        [SerializeField] int spawnLimit = 0;

        private IEnumerator enemySpawnRoutine;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
            isRunning = runButton;

            enemySpawnRoutine = SpawnEnemies();
        }

        public void Update()
        {
            if (isRunning == true && spawnLimit > 0 && enemiesSpawned > spawnLimit)
            {
                print("Stopping spawning because reached spawn limit.");
                StopSpawning();
                return;
            }
            if (isRunning != runButton) // For use with inspector GUI button
            {
                print("Run button toggled.");
                if (isRunning)
                {
                    StopCoroutine(enemySpawnRoutine);
                }
                else
                {
                    StartCoroutine(enemySpawnRoutine);
                }
                isRunning = runButton;
            }
        }

        IEnumerator SpawnEnemies()
        {
            print($"Waiting {delayBeforeStart} before starting spawning.");
            yield return new WaitForSeconds(delayBeforeStart);

            while (true)
            {
                if (enemiesSpawned >= spawnLimit)
                {
                    yield break;
                }
                print($"Spawning enemy #{enemiesSpawned + 1}.");
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

                var enemyObject = Instantiate(enemyPrefab,
                    new Vector3(
                        transform.position.x + Random.Range(minRange, maxRange),
                        transform.position.y + Random.Range(minRange, maxRange),
                        0),
                    Quaternion.identity);
                enemyObject.transform.parent = gameObject.transform;
                var enemy = enemyObject.GetComponent<Enemy>();
                enemiesSpawned++;
            }
        }

        public void StartSpawning()
        {
            if (!isRunning)
            {
                isRunning = true;
                runButton = true;
                StartCoroutine(enemySpawnRoutine);
            }
        }

        public void StopSpawning()
        {
            if (isRunning)
            {
                isRunning = false;
                runButton = false;
                StopCoroutine(enemySpawnRoutine);
            }
        }

        public void OnValidate()
        {
            if (resetButton)
            {
                enemiesSpawned = 0;
                resetButton = false;
            }
        }
    }
}