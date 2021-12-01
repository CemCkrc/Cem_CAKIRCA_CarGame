using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARGAME
{
    public class CarSpawner : MonoBehaviour
    {
        public Transform startPos;
        public CarEndChecker endPos;

        public CARGAME.CarController car;

        public CARGAME.CarController spawnedCar
        {
            get;
            private set;
        }

        private void Awake() 
        {
            spawnedCar = Instantiate(car, startPos.position + (Vector3.up * 0.5f), startPos.rotation, null);        
        }

        public void SetVisibility(bool value)
        {
            startPos.GetComponent<MeshRenderer>().enabled = value;
            endPos.GetComponent<Collider>().enabled = value;
            endPos.GetComponentInChildren<MeshRenderer>().enabled = value;
        }

        public void ResetCar()
        {
            if(!spawnedCar) return;
            
            spawnedCar.transform.position = startPos.position + (Vector3.up * 0.5f);
            spawnedCar.transform.rotation = startPos.rotation;
        }
    }   
}