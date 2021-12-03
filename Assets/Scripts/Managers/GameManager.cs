using UnityEngine;
using UnityEngine.SceneManagement;

namespace CARGAME.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; //GameManager Singleton

        [Tooltip("Set false for test level")]
        public bool loadLastPlayedScene = true; //Set false for editor

        private int currentLevel = 0; //Player current level index

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else
                Destroy(this.transform.root.gameObject);

            currentLevel = PlayerPrefs.GetInt("LevelID");

            if(loadLastPlayedScene && currentLevel != SceneManager.GetActiveScene().buildIndex) //Check player in current level
                LoadGameLevel();
        }

        /// <summary>
        /// Load next level if player's all cars to target
        /// </summary>
        public void OnLevelCompleted()
        {
            if(!loadLastPlayedScene) 
            {
                RestartLevel();
                return;
            }

            currentLevel++;

            if(currentLevel >= SceneManager.sceneCountInBuildSettings) //Load first level if all levels played
                currentLevel = 0;
            
            PlayerPrefs.SetInt("LevelID", currentLevel);

            LoadGameLevel();
        }

        /// <summary>
        /// Load next car if player's car reached target
        /// </summary>
        public void OnCarReachedExit()
        {
            foreach (var item in FindObjectsOfType<Obstacles.MovingObstacle>())
                item.ResetPosition();

            RecordManager.Recorder.ResetAllRecords();
            FindObjectOfType<CarManager>().CurrentControllingCar++;    
        }

        /// <summary>
        /// Respawn current player car and previous records
        /// </summary>
        public void OnCarFailed()
        {
            foreach (var item in FindObjectsOfType<Obstacles.MovingObstacle>())
                item.ResetPosition();

            RecordManager.Recorder.ResetAllRecords();
            FindObjectOfType<CarManager>().ResetAllCars();  
        }

        /// <summary>
        /// Load player level
        /// </summary>
        private void LoadGameLevel() { if(SceneManager.sceneCountInBuildSettings > currentLevel) SceneManager.LoadScene(currentLevel); }

         /// <summary>
        /// Restart game level
        /// </summary>
        private void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    }
}
