using UnityEngine;

namespace CARGAME.Cars
{
    public class CarSpawner : MonoBehaviour
    {
        #region Public Values

        [Header("Spawner Values")]
        [Tooltip("Must be a unique ID")]
        public int spawnerID = 0; //Unique spawnerID


        [Space(2)]
        [Header("Car Values")]
        public float carSpeed = 0; //Car speed
        public float carRotationSpeed = 0; //Car rotationSpeed

        [Space(10)]
        public Car carPrefab; //Car prefab
        public Car spawnedCar //Spawned car
        {
            get;
            private set;
        }

        #endregion

        #region Private Values

        [SerializeField] private Color _normalCarColor;
        [SerializeField] private Color _currentCarColor;

        private Transform _entrancePos;
        private CarTargetChecker _endPos;
        private MeshRenderer _carMeshRenderer;

        #endregion

        private void Awake()
        {
            _entrancePos = transform.Find("EntrancePos");
            _endPos = GetComponentInChildren<CarTargetChecker>();

            spawnedCar = Instantiate(carPrefab, _entrancePos.position, _entrancePos.rotation, null);

            SetupSpawner();
        }

        private void Start() 
        {
            // Activate entrance and target meshes
            _entrancePos.GetChild(0).gameObject.SetActive(true);
            _endPos.transform.GetChild(0).gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Spawner Setup
        /// </summary>
        private void SetupSpawner()
        {
            _endPos.carID = spawnerID;

            spawnedCar.carID = this.spawnerID;
            spawnedCar.carSpeed = this.carSpeed;
            spawnedCar.rotationSpeed = this.carRotationSpeed;

            
            _carMeshRenderer = spawnedCar.GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// Set car and entrance-target visibility in game
        /// Change car color if car is controlling by player
        /// </summary>
        /// <param name="carVisibility"> Car visible/invisible </param>
        /// <param name="spawnerVisibility"> Entrance-target visible/invisible </param>
        public void SetVisibility(bool carVisibility, bool spawnerVisibility)
        {
            _entrancePos.gameObject.SetActive(spawnerVisibility);
            _endPos.transform.gameObject.SetActive(spawnerVisibility);

            Color carColor = spawnerVisibility ? _currentCarColor : _normalCarColor; //Check if player controlling current car
            _carMeshRenderer.material.color = carColor;
            
            spawnedCar.gameObject.SetActive(carVisibility);
        }

        /// <summary>
        /// Set car position to entrance point
        /// </summary>
        public void ResetCar()
        {
            if (!spawnedCar) return;

            spawnedCar.transform.position = _entrancePos.position;
            spawnedCar.transform.rotation = _entrancePos.rotation;
        }
    }
}