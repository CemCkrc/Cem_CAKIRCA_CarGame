using UnityEngine;

namespace CARGAME.Cars
{
    public class CarTargetChecker : MonoBehaviour
    {
        private int _carID;

        public int CarID
        {
            get => _carID;
            set { _carID = value; }
        }

        private void OnTriggerEnter(Collider other) 
        {
            CarController car = other.GetComponent<CarController>();

            if(car?.carID == _carID)
                Managers.GameManager.Manager.OnCarReachedExit();
        }
    }
}