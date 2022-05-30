using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace IR
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            PRE_GAME,
            RUNNING,
            PAUSED,
            ENDGAME
        }

        public GameObject[] SystemPrefabs;
        public Events.EventGameState OnGameStateChanged;
        public Events.EventGameUIUpdate OnGameUIChanged;
        public Events.EventEnemyKilled OnEnemyKilledChanged;
        [SerializeField] string firstLevel = "Level 1";

        public bool LevelWinStateReached { get; set; }

        private List<GameObject> instantiatedSystemPrefabs;
        private string currentLevel;
        private List<AsyncOperation> loadOperations;
        private int mineralCount = 0;
        private int enemiesKilledCount = 0;

        private int maxUpgradeLevel = 2; // Level + 1

        private int gunLevel = 1;
        private int shipLevel = 1;
        private int turretLevel = 1;

        private int initialGunUpgradeCost = 90;
        private int initialShipUpgradeCost = 90;
        private int initialTurretUpgradeCost = 90;
        private int initialBaseUpgradeCost = 10;

        private int gunUpgradeCost = 90;
        private int shipUpgradeCost = 90;
        private int turretUpgradeCost = 90;
        private int baseUpgradeCost = 10;

        private int shipLevelHealthBonus = 20;

        private int playerHealth;
        private int playerMaxHealth;


        bool levelLoadInProgress = false;

        internal void RestartGame()
        {
            UpdateState(GameState.PRE_GAME);
        }

        internal void AddMinerals(int value)
        {
            mineralCount += value;
            // redraw UI
            OnGameUIChanged.Invoke();
        }


        internal void AddEnemyKillCount(int value)
        {
            enemiesKilledCount += value;
            // redraw UI
            OnGameUIChanged.Invoke();
        }



        internal int GetHealth()
        {
            return playerHealth;
        }

        internal int GetMaxHealth()
        {
            return playerMaxHealth;
        }

        internal int GetMaxUpgradeLevel()
        {
            return maxUpgradeLevel;
        }

        internal void IncrementMaxUpgradeLevel()
        {
            maxUpgradeLevel++;
        }

        internal void SetHealth(int value)
        {
            playerHealth = value;
            OnGameUIChanged.Invoke();
        }

        internal void SetMaxHealth(int health)
        {
            playerMaxHealth = health;
            OnGameUIChanged.Invoke();
        }

        internal void ResetUpgrades()
        {
            shipLevel = 1;
            turretLevel = 1;
            gunLevel = 1;
            gunUpgradeCost = initialGunUpgradeCost;
            shipUpgradeCost = initialShipUpgradeCost;
            turretUpgradeCost = initialTurretUpgradeCost;
            baseUpgradeCost = initialBaseUpgradeCost;
            OnGameUIChanged.Invoke();
        }

        internal int GetGunUpgradeCost()
        {
            if (gunLevel >= maxUpgradeLevel)
            {
                return int.MaxValue;
            }

            return gunUpgradeCost + baseUpgradeCost;
        }

        internal int GetShipUpgradeCost()
        {
            if (shipLevel >= maxUpgradeLevel)
            {
                return int.MaxValue;
            }

            return shipUpgradeCost + baseUpgradeCost;
        }

        internal int GetTurretUpgradeCost()
        {
            if (turretLevel >= maxUpgradeLevel)
            {
                return int.MaxValue;
            }

            return turretUpgradeCost + baseUpgradeCost;
        }


        public void UpgradeGuns()
        {
            gunLevel++;
            UpdateUpgradeCosts(ref gunUpgradeCost);

            // redraw UI
            OnGameUIChanged.Invoke();
        }


        public void UpgradeTurret()
        {
            turretLevel++;
            UpdateUpgradeCosts(ref turretUpgradeCost);

            // redraw UI
            OnGameUIChanged.Invoke();
        }

        public void UpgradeShip()
        {
            shipLevel++;
            playerHealth += shipLevelHealthBonus;
            playerMaxHealth += shipLevelHealthBonus;
            UpdateUpgradeCosts(ref shipUpgradeCost);

            // redraw UI
            OnGameUIChanged.Invoke();
        }

        private void UpdateUpgradeCosts(ref int upgradeCost)
        {
            mineralCount -= upgradeCost;
            upgradeCost = (upgradeCost * 2);
            baseUpgradeCost += Math.Min(30, (int)(baseUpgradeCost * 1.1f));
        }

        internal void QuitGame()
        {
            // TODO: implement quit / tidyup stuff here
            Application.Quit();
        }

        public GameState CurrentGameState { get; private set; } = GameState.PRE_GAME;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            instantiatedSystemPrefabs = new List<GameObject>();
            loadOperations = new List<AsyncOperation>();
            currentLevel = string.Empty;

            InstantiateSystemPrefabs();

            UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        }

        private void HandleMainMenuFadeComplete(FadeTypes fadeType)
        {
            Debug.Log($"HandleMainMenuFadeComplete: {fadeType}");
            if (fadeType == FadeTypes.FADE_IN)
            {
                Debug.Log("HandleMainMenuFadeComplete - Unloading level");
                UnloadLevel(currentLevel);
            }
        }

        public void GotoNextLevel(string level)
        {
            if (levelLoadInProgress)
            {
                Debug.LogWarning($"Couldn't load level - level load already in progress.");
                return;
            }
            levelLoadInProgress = true;

            LevelWinStateReached = false;
            ResetUpgrades();
            IncrementMaxUpgradeLevel();
            UnloadLevel(currentLevel);
            LoadLevel(level);
        }

        private void Update()
        {
            if (CurrentGameState == GameManager.GameState.PRE_GAME)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        void InstantiateSystemPrefabs()
        {
            GameObject prefabInstance;

            foreach (var item in SystemPrefabs)
            {
                prefabInstance = Instantiate(item);
                instantiatedSystemPrefabs.Add(prefabInstance);
            }
        }

        //public void LoadLevel(string level)
        //{
        //    var operation = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

        //    if (operation == null)
        //    {
        //        Debug.LogError($"[GameManager] unable to load {level}");
        //        return;
        //    }
        //    operation.completed += OnLoadOperationComplete;
        //    loadOperations.Add(operation);
        //    currentLevel = level;
        //}

        public void LoadLevel(string level)
        {
            StartCoroutine(StartLoadLevel(level));
        }

        IEnumerator StartLoadLevel(string level)
        {
            var ao = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

            ao.allowSceneActivation = false;
            while (ao.progress < 0.9f)
            {
                yield return null;
            }
            ao.allowSceneActivation = true;

            while (!ao.isDone)
            {
                yield return null;
            }

            UpdateState(GameState.RUNNING);
            currentLevel = level;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevel));
            levelLoadInProgress = false;
            MusicManager.Instance.Play(level);
            Debug.Log("Load complete." + level);
        }

        private void OnLoadOperationComplete(AsyncOperation ao)
        {
            if (loadOperations.Contains(ao))
            {
                loadOperations.Remove(ao);

                if (loadOperations.Count == 0)
                {
                    UpdateState(GameState.RUNNING);
                }
            }
            Debug.Log("Load complete.");
        }

        public void UnloadLevel(string level)
        {
            var operation = SceneManager.UnloadSceneAsync(level);

            if (operation == null)
            {
                Debug.LogError($"[GameManager] unable to load {level}");
                return;
            }

            operation.completed += OnUnloadOperationComplete;
        }

        private void OnUnloadOperationComplete(AsyncOperation ao)
        {
            Debug.Log("Unload complete.");
        }

        void UpdateState(GameState state)
        {
            var previousGameState = CurrentGameState;
            CurrentGameState = state;

            switch (CurrentGameState)
            {
                case GameState.PRE_GAME:
                    Time.timeScale = 1.0f;
                    break;
                case GameState.RUNNING:
                    Time.timeScale = 1.0f;
                    break;
                case GameState.PAUSED:
                    Time.timeScale = 0.0f;
                    break;
                case GameState.ENDGAME:
                    Time.timeScale = 0.0f;
                    break;
                default:

                    break;
            }

            OnGameStateChanged.Invoke(CurrentGameState, previousGameState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var item in instantiatedSystemPrefabs)
            {
                Destroy(item);
            }

            instantiatedSystemPrefabs.Clear();
        }

        public void StartGame()
        {
            CurrentGameState = GameState.RUNNING;
            Instance.LoadLevel(firstLevel);
        }

        public void TogglePause()
        {
            UpdateState(CurrentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
        }

        public int GetMinerals()
        {
            return mineralCount;
        }
        public int GetEnemiesKilled()
        {
            return enemiesKilledCount;
        }

        public int GetGunLevel()
        {
            return gunLevel;
        }

        public int GetTurretLevel()
        {
            return turretLevel;
        }

        public int GetShipLevel()
        {
            return shipLevel;
        }
    }
}