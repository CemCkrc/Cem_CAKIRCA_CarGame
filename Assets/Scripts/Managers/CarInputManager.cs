using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARGAME.Managers
{
    public class CarInputManager : MonoBehaviour, IInputs
    {
        [Range(1,8)]
        public int controlledCars = 1;

        public int CurrentControllingCar
        {
            get => _currentControllingCar;
            set 
            {
                cars[_currentControllingCar].isRunningReplay = true;

                for(int i = 0; i < controlledCars; i++)
                    cars[i].GetComponent<MeshRenderer>().enabled = false;

                _currentControllingCar = value;
                if(value == 8)
                {
                    GameManager.Manager.OnLevelCompleted();
                    return;
                } 
                else
                {
                    controlledCars++;
                    cars[_currentControllingCar].GetComponent<MeshRenderer>().enabled = true;
                }

            }
        }

        private bool _isStarted = false;
        private List<CARGAME.CarController> cars;
        public CARGAME.CarSpawner[] carSpawners;
        private ROTATION inputRotation = ROTATION.NONE;

        private int _currentControllingCar = 0;

        private void Awake() => _currentControllingCar = controlledCars - 1;
        private void Start() 
        {
            cars = new List<CARGAME.CarController>();

            foreach (CarSpawner item in carSpawners)
            {
                cars.Add(item.spawnedCar);
            }
        
            SetSpawner();

            cars[_currentControllingCar].GetComponent<MeshRenderer>().enabled = true;
        }

        private void Update() 
        {
            if(!_isStarted) return;

            cars[_currentControllingCar]?.RotateCar(inputRotation);
        }
    
        public void TurnLeftButtonPressed()
        {
            if(!_isStarted) StartAllCars();

             if(inputRotation == ROTATION.RIGHT) 
                inputRotation = ROTATION.NONE;
            else
                inputRotation = ROTATION.LEFT;
        }

        public void TurnRightButtonPressed()
        {
            if(!_isStarted) StartAllCars();

            if(inputRotation == ROTATION.LEFT) 
                inputRotation = ROTATION.NONE;
            else
                inputRotation = ROTATION.RIGHT;
        }

        //TODO: change old rotation
        public void ButtonRelased() => inputRotation = ROTATION.NONE;

       
        public void StartAllCars()
        {
            _isStarted = true;

            for(int i = 0; i < controlledCars; i++)
                cars[i].IsStarted = true;
        }

        public void ResetAllCars()
        {
            _isStarted = false;

            SetSpawner();
            
            for(int i = 0; i < controlledCars; i++)
                cars[i].IsStarted = false;

            for(int i = 0; i < carSpawners.Length; i++)
                carSpawners[i].ResetCar();
        }

        public void SetSpawner()
        {
            for(int i = 0; i < 8; i++) //max cars
            {
                if(_currentControllingCar == i)
                    carSpawners[i].SetVisibility(true);
                else
                    carSpawners[i].SetVisibility(false);
            }
        }

    }
}

namespace CARGAME
{
    public enum ROTATION
    {
        LEFT,
        RIGHT,
        NONE
    }
}