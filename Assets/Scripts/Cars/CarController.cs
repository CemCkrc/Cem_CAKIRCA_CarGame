using System.Collections.Generic;
using UnityEngine;

using CARGAME.Inputs;
using CARGAME.Managers;

namespace CARGAME.Cars
{
    public class CarController : MonoBehaviour, IInputs
    {
        public int CurrentControllingCar // Encapsulated Player's current car
        {
            get => _currentControllingCar;
            set 
            {
                if(value == _maxCars) // All cars reached target
                {
                    GameManager.Instance.OnLevelCompleted();
                    return;
                }
                else // Set next car ready
                {
                    carSpawners[_currentControllingCar].spawnedCar.isRunningReplay = true;
                    _currentControllingCar = value;
                    _controlledCars++;
                    ResetAllCars();
                }
            }
        }

        private bool _isStarted = false; // Is player touched?
        private  List<CarSpawner> carSpawners; // All car spawners

        private int _maxCars = 0; // Store number of cars in level
        private int _controlledCars = 1; // Store played number of cars
        private int _currentControllingCar = 0; // Player's current car

        private ROTATION inputRotation = ROTATION.NONE; // Rotation data
        private bool _leftButtonPressed = false; // Left rotation
        private bool _rightButtonPressed = false; // Right rotation

        private void Awake()
        {
            carSpawners = new List<CarSpawner>();

            _currentControllingCar = _controlledCars - 1;
        }

        private void Start()
        {
            foreach(CarSpawner spawner in FindObjectsOfType<CarSpawner>()) // Get all carSpawners
                carSpawners.Add(spawner);

            carSpawners.Sort((spawner0,spawner1)=>spawner0.spawnerID.CompareTo(spawner1.spawnerID)); // Sort list by spawnerID

            _maxCars = carSpawners.Count;

            ResetAllCars();
        }

        private void Update() 
        {
            if(!_isStarted) return;

            inputRotation = ROTATION.NONE;

            if(_leftButtonPressed && _rightButtonPressed) //Check button press
                inputRotation = ROTATION.NONE;
            else if(_leftButtonPressed)
                inputRotation = ROTATION.LEFT;
            else if(_rightButtonPressed)
                inputRotation = ROTATION.RIGHT;

            carSpawners[_currentControllingCar].spawnedCar?.RotateCar(inputRotation);
        }
    
        #region Player Input Functions

        public void TurnLeftButtonPressed()
        {
            if(!_isStarted) StartAllCars();

            _leftButtonPressed = true;
        }

        public void TurnRightButtonPressed()
        {
            if(!_isStarted) StartAllCars();

            _rightButtonPressed = true;
        }

        public void TurnLeftButtonRelased() => _leftButtonPressed = false;

        public void TurnRightButtonRelased() => _rightButtonPressed = false;

        #endregion

        /// <summary>
        /// Start all previously driven cars
        /// </summary>
        public void StartAllCars()
        {
            _isStarted = true;

            for(int i = 0; i < _controlledCars; i++)
            {
                if(_currentControllingCar != i)
                    carSpawners[i].SetVisibility(true, false);

                carSpawners[i].spawnedCar.IsStarted = true;
            }

            foreach (var item in FindObjectsOfType<Obstacles.MovingObstacle>())
                item.StartObstacle();
        }

        /// <summary>
        /// Reset all cars
        /// </summary>
        public void ResetAllCars()
        {
            _isStarted = false;

            for(int i = 0; i < _maxCars; i++) // Reset all cars
            {
                if(_currentControllingCar == i)
                    carSpawners[i].SetVisibility(true, true);
                else
                    carSpawners[i].SetVisibility(false, false);
                
                carSpawners[i].spawnedCar.IsStarted = false;
                carSpawners[i].ResetCar();       
            }
        }
    }
}

namespace CARGAME
{
    public enum ROTATION //Player's car rotations
    {
        LEFT,
        RIGHT,
        NONE
    }
}