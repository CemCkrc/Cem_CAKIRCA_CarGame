using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARGAME
{
    public class CarEndChecker : MonoBehaviour
    {
        public int carID;

        private void OnTriggerEnter(Collider other) 
        {
            CarController car = other.GetComponent<CarController>();

            if(car.carID == carID)
            {
                Managers.GameManager.Manager.OnCarReachedExit();
            }
        }
    }   
}