using UnityEngine;

namespace CARGAME.Cars
{
    public class CarSpawner : MonoBehaviour
    {
        [Header("Spawner Values")]
        public int spawnerID = 0;

        
        [Space(2)]
        [Header("Car Values")]
        public int carID = 0;
        public float carSpeed = 0;
        public float carRotationSpeed = 0;

        [Space(10)]
        public CarController carPrefab;
        public CarController spawnedCar
        {
            get;
            private set;
        }

        [SerializeField] Color _normalCarColor;
        [SerializeField] Color _currentCarColor;

        private Transform startPos;
        private CarTargetChecker endPos;

        private void Awake()
        {
            endPos = GetComponentInChildren<CarTargetChecker>();
            startPos = transform.Find("StartPos");

            spawnedCar = Instantiate(carPrefab, startPos.position, startPos.rotation, null);

            endPos.CarID = carID;

            SetupCar();
        }

        private void SetupCar()
        {
            spawnedCar.carID = this.carID;
            spawnedCar.carSpeed = this.carSpeed;
            spawnedCar.rotationSpeed = this.carRotationSpeed;
        }

        public void SetVisibility(bool carVisibility, bool spawnerVisibility)
        {
            startPos.gameObject.SetActive(spawnerVisibility);
            endPos.transform.gameObject.SetActive(spawnerVisibility);

            Color carColor = carVisibility ? _currentCarColor : _normalCarColor;

            if(spawnedCar) spawnedCar.GetComponent<MeshRenderer>().material.color = carColor;
        }

        public void ResetCar()
        {
            if(!spawnedCar) return;
            
            spawnedCar.transform.position = startPos.position;
            spawnedCar.transform.rotation = startPos.rotation;
        }
    }   
}