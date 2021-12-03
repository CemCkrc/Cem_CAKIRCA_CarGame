using UnityEngine;

namespace CARGAME.Cars
{
    /// <summary>
    /// Check if player's car reached the target
    /// </summary>
    public class CarTargetChecker : MonoBehaviour
    {
        public int carID; //Store carID

        private void OnTriggerEnter(Collider other) 
        {
            CarController car = other.GetComponent<CarController>();

            if(car?.carID == this.carID)
                Managers.GameManager.Instance.OnCarReachedExit();
        }
    }
}