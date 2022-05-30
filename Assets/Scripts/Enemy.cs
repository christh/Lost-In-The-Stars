using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IR.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public int health = 20;
        [SerializeField] bool invulnerable = false;
        public string[] TriggerableTags;


        public void Damage(float damageValue)
        {
            if (!InDamageableTags() || invulnerable) return;

            health = (int)(health - damageValue);

            if (health <= 0)
            {
                gameObject.layer = Constants.Layers.DeadThings;
                GameManager.Instance?.AddEnemyKillCount(1);
                GetComponent<IDropsLoot>()?.Drop();
                GetComponent<IExplodeable>()?.Explode();
                var enemyTargeting = GetComponentsInChildren<EnemyTargeting>();
                if (enemyTargeting.Length > 0)
                {
                    foreach (var item in enemyTargeting)
                    {
                        item.enabled = false;
                    }
                }
            }
            else
            {
                // We'd spawn some damage components here or something
                // Or shield twinkles if shield still up
            }
        }

        private bool InDamageableTags()
        {
            if (TriggerableTags.Length == 0)
            {
                return true;
            }

            return false;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.gameObject.layer == Constants.Layers.PlayerBullets)
            {
                var targeting = GetComponent<EnemyTargeting>();
                if (targeting)
                {
                    targeting.inRangeOfPlayer = true;
                }
                var targetings = GetComponentsInChildren<EnemyTargeting>();
                foreach (var item in targetings)
                {
                    item.inRangeOfPlayer = true;
                }
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var targeting = GetComponent<EnemyTargeting>();
                if (targeting)
                {
                    targeting.inRangeOfPlayer = false;
                }
                var targetings = GetComponentsInChildren<EnemyTargeting>();
                foreach (var item in targetings)
                {
                    item.inRangeOfPlayer = false;
                }
            }
        }
    }


}
