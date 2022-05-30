using UnityEngine;
using System.Collections;
using IR.Enemies;
using IR.Common;
using SpriteGlow;

namespace IR
{
    public class EnemyTargeting : MonoBehaviour
    {
        public float inAccuracy = .15f;
        public bool isActive = true;
        public bool inRangeOfPlayer = false;
        [SerializeField] float primaryCoolDown = 1.5f;
        [SerializeField] float spinRate = -500f;
        [SerializeField] GameObject BulletPrefab;
        [SerializeField] GameObject Muzzle;
        [SerializeField] float shootingRange = 10f;

        private float primaryTimer = 0;
        private float currentPrimaryCoolDown = 0.8f;
        private Player player;
        private Enemy enemy;
        private SpriteGlowEffect[] glowEffects;
        private bool glowing = false;

        // Start is called before the first frame update
        void Start()
        {
            if (Muzzle == null)
            {
                Muzzle = gameObject;
            }

            enemy = GetComponentInParent<Enemy>();
            player = FindObjectOfType<Player>();
            glowEffects = enemy.GetComponents<SpriteGlowEffect>();
            ResetCooldown();
        }

        private void ResetCooldown()
        {
            primaryTimer = 0;
            currentPrimaryCoolDown = primaryCoolDown * Random.Range(1, 1.3f);
        }

        public void Update()
        {
            if (!isActive) { return; }


            if (inRangeOfPlayer)
            {
                primaryTimer += Time.deltaTime;

                // Warning glow
                if (!glowing && primaryTimer >= currentPrimaryCoolDown - 0.3f)
                {
                    StartCoroutine(EnableGlow());
                }
                else if (primaryTimer >= currentPrimaryCoolDown)
                {
                    ResetCooldown();
                    ShootAtPlayer();
                }
            }
        }

        IEnumerator EnableGlow()
        {
            glowing = true;

            if (IsPlayerInLineOfSight())
            {
                foreach (var glowEffect in glowEffects)
                {
                    glowEffect.gameObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(.3f);
            DisableGlow();
        }

        private void DisableGlow()
        {
            foreach (var glowEffect in glowEffects)
            {
                glowEffect.gameObject.SetActive(false);
            }

            glowing = false;
        }

        void ShootAtPlayer()
        {
            if (!player) return;

            if (!IsPlayerInLineOfSight()) return;

            var target = player.transform.position - transform.position;

            var attackRotation = Quaternion.LookRotation(Vector3.forward, target);
            attackRotation = Utils.AddInaccuracyToRotation(attackRotation, inAccuracy);

            GameObject projectileObject = Instantiate(BulletPrefab, (Vector2)Muzzle.transform.position, attackRotation);

            var projectile = projectileObject.GetComponent<ForceProjectile>();
            projectile.BrieflyIgnoreCollisionsWithEnemies(Constants.Layers.EnemyBullets);
            projectile.Launch();
        }

        private bool IsPlayerInLineOfSight()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > shootingRange)
            {
                return false;
            }

            // Also need to be roughly facing the target
            Vector3 targetDelta = player.transform.position - transform.position;
            float angleDifference = Vector3.Angle(transform.up, targetDelta);
            Vector3 crossProduct = Vector3.Cross(transform.up, targetDelta);
            var torqueToApply = angleDifference * crossProduct.z;

            if (torqueToApply > 120)
            {
                return false;
            }

            RaycastHit2D hit = Physics2D.Linecast(
            transform.position,
            player.transform.position,
            1 << Constants.Layers.Environment);

            if (hit.collider != null)
            {
                return false;
            }

            return true;
        }
    }
}