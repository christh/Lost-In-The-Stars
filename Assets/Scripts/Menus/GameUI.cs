using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IR
{

    public class GameUI : MonoBehaviour
    {
        [SerializeField] TMP_Text MineralsCount;
        [SerializeField] TMP_Text Health;
        [SerializeField] TMP_Text EnemiesKilledCount;
        [SerializeField] TMP_Text GunLevel;
        [SerializeField] TMP_Text ShipLevel;
        [SerializeField] TMP_Text TurretLevel;
        [SerializeField] Button gunButton;
        [SerializeField] Button turretButton;
        [SerializeField] Button shipButton;

        TMP_Text gunButtonText;
        TMP_Text turretButtonText;
        TMP_Text shipButtonText;

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
            GameManager.Instance.OnGameUIChanged.AddListener(HandleGameUIChanged);
            gunButton.onClick.AddListener(GameManager.Instance.UpgradeGuns);
            turretButton.onClick.AddListener(GameManager.Instance.UpgradeTurret);
            shipButton.onClick.AddListener(GameManager.Instance.UpgradeShip);

            gunButtonText = gunButton.GetComponentInChildren<TMP_Text>();
            shipButtonText = shipButton.GetComponentInChildren<TMP_Text>();
            turretButtonText = turretButton.GetComponentInChildren<TMP_Text>();
        }

        private void HandleGameUIChanged()
        {
            MineralsCount.text = $"Minerals: {GameManager.Instance.GetMinerals()}";
            EnemiesKilledCount.text = $"Enemies Killed: {GameManager.Instance.GetEnemiesKilled()}";
            Health.text = $"Health: {GameManager.Instance.GetHealth()}/{GameManager.Instance.GetMaxHealth()}";
            GunLevel.text = $"Gun: {GameManager.Instance.GetGunLevel()}";
            ShipLevel.text = $"Ship: {GameManager.Instance.GetShipLevel()}";
            TurretLevel.text = $"Turret: {GameManager.Instance.GetTurretLevel()}";

            gunButton.enabled = UpdateUpgradeButtonText(GameManager.Instance.GetGunUpgradeCost(), gunButtonText);
            shipButton.enabled = UpdateUpgradeButtonText(GameManager.Instance.GetShipUpgradeCost(), shipButtonText);
            turretButton.enabled = UpdateUpgradeButtonText(GameManager.Instance.GetTurretUpgradeCost(), turretButtonText);
        }

        private bool UpdateUpgradeButtonText(int cost, TMP_Text field)
        {
            if (cost == int.MaxValue)
            {
                field.text = $"MAXED";
                return false;
            }

            if (GameManager.Instance.GetMinerals() < cost)
            {
                field.text = $"Requires {cost}";
                return false;
            }
            else
            {
                field.text = $"Upgrade ({cost})";
                return true;
            }
        }

        private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
        {
            switch (previousState)
            {
                case GameManager.GameState.PRE_GAME:
                    if (currentState == GameManager.GameState.RUNNING)
                        Show();
                    break;
                case GameManager.GameState.RUNNING:
                case GameManager.GameState.PAUSED:
                    if (currentState == GameManager.GameState.PRE_GAME)
                    {
                        Hide();
                    }
                    break;

                default:
                    break;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}