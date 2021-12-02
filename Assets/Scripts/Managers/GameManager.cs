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
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else
                Destroy(this.transform.root.gameObject);

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
            FindObjectOfType<CarManager>().CurrentControllingCar++;
            FindObjectOfType<CarManager>().ResetAllCars();
            RecordManager.Recorder.ResetAllRecords();
        }

        public void OnCarFailed()
        {
            FindObjectOfType<CarManager>().ResetAllCars();
            RecordManager.Recorder.ResetAllRecords();
            //SceneManager.LoadScene(currentLevel);
        }

        private void LoadGameLevel() { if(SceneManager.sceneCount > currentLevel) SceneManager.LoadScene(currentLevel); }
    }
}
