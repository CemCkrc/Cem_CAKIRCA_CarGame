using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARGAME.Obstacles
{
    public class MovingObstacle : MonoBehaviour
    {
        public int currentPos = 0; //Obstacle position
        public float obstacleSpeed = 2f; //Obstacle speed

        [Tooltip("Add parent which has transform component in children")]
        [SerializeField] private Transform _movingRouteParentPoint; 
        private Transform[] _obstacleRoute; // Obstacle points array
        private bool _isStarted = false;

        private void Awake() {
            
            int obstacleRoutePoints = _movingRouteParentPoint.childCount;
            
            Transform[] posList = _movingRouteParentPoint.GetComponentsInChildren<Transform>();
            _obstacleRoute = new Transform[posList.Length-1];

            for (int index = 0; index < posList.Length - 1; index++)
                _obstacleRoute[index] = posList[index + 1];
        }

        private void FixedUpdate() //Move Obstacle
        {
            if(!_isStarted) return;
            
            if(Vector3.Distance(transform.position, _obstacleRoute[currentPos].position) < 0.1f)
            {
                currentPos++;

                if(currentPos == _obstacleRoute.Length)
                    currentPos = 0;
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, _obstacleRoute[currentPos].position, obstacleSpeed * Time.fixedDeltaTime);
        }
    
        /// <summary>
        /// Start obstacle
        /// </summary>
        public void StartObstacle() => _isStarted = true;

        /// <summary>
        /// Reset obstacle position
        /// </summary>
        public void ResetPosition()
        {
            _isStarted = false;
            transform.position = _obstacleRoute[0].position;
        }
    }
}
