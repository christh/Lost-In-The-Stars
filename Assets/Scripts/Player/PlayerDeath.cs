using IR.Enemies;
using IR.Factories;
using System.Collections;
using UnityEngine;

namespace IR
{
    public class PlayerDeath : MonoBehaviour, IExplodeable
    {
        [SerializeField] int deadAnimationCount;
        [SerializeField] string nextLevel;
        public GameObject DeathPrefab;
        public AudioClip DeathAudio;

        private Animator animator;
        private bool triggered;
        private string currentDeadState;


        public void Explode()
        {
            GetComponent<PointEffector2D>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            ExplosionFactory.SpawnExplosion(DeathPrefab, DeathAudio, transform.position);

            if (triggered == true) return;

            triggered = true;
            gameObject.layer = Constants.Layers.DeadThings;

            StartCoroutine(Resurrect());
        }

        IEnumerator Resurrect()
        {
            yield return new WaitForSeconds(2f);

            if (GameManager.Instance.LevelWinStateReached)
            {
                var minerals = FindObjectsOfType<Pickup>();
                foreach (var item in minerals)
                {
                    Destroy(item.gameObject);
                }

                var enemies = FindObjectsOfType<Enemy>();
                foreach (var item in enemies)
                {
                    Destroy(item.gameObject);
                }
                var bullets = FindObjectsOfType<ForceProjectile>();
                foreach (var item in bullets)
                {
                    Destroy(item.gameObject);
                }

                GameManager.Instance?.GotoNextLevel(nextLevel);
            }
            
            var levelEntrance = FindObjectOfType<LevelEntrance>();

            if (levelEntrance)
            {
                transform.position = levelEntrance.transform.position;
            }
            else
            {
                Debug.LogError("No LevelEntrance found in scene! Resurrecting on the spot.");
            }

            transform.rotation = Quaternion.identity;
            triggered = false;
            gameObject.layer = Constants.Layers.Player;

            GetComponent<PointEffector2D>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            var player = GetComponent<Player>();
            player.health = GameManager.Instance.GetMaxHealth();
            GameManager.Instance.SetHealth(player.health);
        }
    }
}