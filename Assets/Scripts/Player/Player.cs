using IR.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IR
{
    public class Player : MonoBehaviour, IDamageable
    {
        public int health = 20;

        public GameObject BulletPrefab;
        public GameObject Muzzle;
        public GameObject MuzzleL2A;
        public GameObject MuzzleL2B;
        public GameObject MuzzleL4A;
        public GameObject MuzzleL4B;
        public GameObject Turret;

        private Rigidbody2D playerBody;
        private float primaryCoolDown = 0.15f;
        private float secondaryCoolDown = 0.25f;

        private Camera cam;
        private Vector2 mousePosition = new Vector2();
        private Vector3 point = new Vector3();


        internal void Move(ThrustStates thrusting)
        {
            throw new NotImplementedException();
        }

        private float primaryTimer = 0;
        private float secondaryTimer = 0;

        void Start()
        {
            cam = Camera.main;

            playerBody = GetComponent<Rigidbody2D>();
            GameManager.Instance?.SetHealth(health);
            GameManager.Instance?.SetMaxHealth(health);
        }

        void Update()
        {
            primaryTimer += Time.deltaTime;
            secondaryTimer += Time.deltaTime;

            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(0)) && primaryTimer >= primaryCoolDown && !EventSystem.current.IsPointerOverGameObject())
            {
                primaryTimer = 0;
                Launch();
            }

            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1)) && secondaryTimer >= secondaryCoolDown)
            {
                secondaryTimer = 0;
                Vector3 direction = Input.mousePosition;
                LaunchToPoint(direction);
            }
        }
        void Launch()
        {
            if (GameManager.Instance.GetGunLevel() > 0)
            {
                SpawnAndLaunch(Muzzle);
            }

            if (GameManager.Instance.GetGunLevel() > 1)
            {
                SpawnAndLaunch(MuzzleL2A);
                SpawnAndLaunch(MuzzleL2B);
            }
            if (GameManager.Instance.GetGunLevel() > 3)
            {
                SpawnAndLaunch(MuzzleL4A);
                SpawnAndLaunch(MuzzleL4B);
            }

        }

        private void SpawnAndLaunch(GameObject muzzle)
        {
            GameObject projectileObject = Instantiate(BulletPrefab, (Vector2)muzzle.transform.position, muzzle.transform.rotation);
            var projectile = projectileObject.GetComponent<ForceProjectile>();
            projectile.SetInitialVelocity(playerBody.velocity);
            projectile.SetPlayerGunUpgradeProperties();
            projectile.Launch();
        }

        private void SpawnAndLaunch(GameObject muzzle, Quaternion rotation)
        {

            GameObject projectileObject = Instantiate(BulletPrefab, (Vector2)muzzle.transform.position, rotation);
            var projectile = projectileObject.GetComponent<ForceProjectile>();
            projectile.SetPlayerTurretUpgradeProperties();
            projectile.Launch();
        }

        void LaunchToPoint(Vector3 target)
        {
            target.z = -Camera.main.transform.position.z;
            target = Camera.main.ScreenToWorldPoint(target) - Turret.transform.position;

            var bulletRotation = Quaternion.LookRotation(Vector3.forward, target);

            if (GameManager.Instance.GetTurretLevel() > 0)
            {
                SpawnAndLaunch(Turret, bulletRotation);
            }

            if (GameManager.Instance.GetTurretLevel() > 1)
            {
                var attackRotation = Quaternion.LookRotation(Vector3.forward, target);
                attackRotation = Utils.AddInaccuracyToRotation(attackRotation, 0.05f);
                SpawnAndLaunch(Turret, attackRotation);
            }

            if (GameManager.Instance.GetTurretLevel() > 3)
            {
                var attackRotation = Quaternion.LookRotation(Vector3.forward, target);
                attackRotation = Utils.AddInaccuracyToRotation(attackRotation, 0.07f);
                SpawnAndLaunch(Turret, attackRotation);
            }
        }


        internal void Move(TurnStates right)
        {
            throw new NotImplementedException();
        }

        internal void Shoot()
        {
            throw new NotImplementedException();
        }

        public void Damage(float damageValue)
        {
            damageValue -= GameManager.Instance.GetShipLevel() + 2;
            health = (int)(GameManager.Instance.GetHealth() - damageValue);
            GameManager.Instance?.SetHealth(health);

            if (health <= 0)
            {
                if (GameManager.Instance.GetMinerals() > 0)
                {
                    var loot = GetComponent<IDropsLoot>();
                    loot.SetLootQuality(0f);
                    loot.SetLootValue((int)(GameManager.Instance.GetMinerals() * 0.9));
                    loot.Drop();
                    GameManager.Instance.AddMinerals(-GameManager.Instance.GetMinerals());
                }

                GetComponent<IExplodeable>()?.Explode();
            }
            else
            {
                // We'd spawn some damage components here or something
                // Or shield twinkles if shield still up
            }
        }
    }
}