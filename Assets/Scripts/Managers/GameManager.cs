using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CARGAME.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Manager;

        public int currentLevel
        {
            get;
            private set;
        } = 0;

        private void Awake()
        {
            if (!Manager)
            {
                Manager = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);

            currentLevel = PlayerPrefs.GetInt("LevelID");

            if(currentLevel != SceneManager.GetActiveScene().buildIndex)
                LoadGameLevel();
        }

        public void OnLevelCompleted()
        {
            currentLevel++;
            PlayerPrefs.SetInt("LevelID", currentLevel);

            if(currentLevel > 0)
                currentLevel = 0;
                
            LoadGameLevel();
        }

        public void OnCarReachedExit()
        {
            FindObjectOfType<CarInputManager>().CurrentControllingCar++;
            FindObjectOfType<CarInputManager>().ResetAllCars();
            RecordManager.Recorder.ResetAllRecords();
        }

        public void OnCarFailed()
        {
            FindObjectOfType<CarInputManager>().ResetAllCars();
            RecordManager.Recorder.ResetAllRecords();
            //SceneManager.LoadScene(currentLevel);
        }

        private void LoadGameLevel() { if(SceneManager.sceneCount > currentLevel) SceneManager.LoadScene(currentLevel); }
    }
}
