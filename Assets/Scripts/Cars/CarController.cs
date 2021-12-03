using UnityEngine;

namespace CARGAME.Cars
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        #region Public Values

        [Header("Car Stats")]
        public int carID = 0; // CarID and RecordID
        public float carSpeed = 100.0f; // Car speed
        public float rotationSpeed = 100.0f; // Car rotation speed
        public bool isRunningReplay = false; // Is car in replay mode?
        public bool IsStarted // Encapsulated Check player's car started to move
        {
            get => _isStarted;
            set
            {
                _isStarted = value;

                if(value) // Check car isStarted
                {
                    if(!isRunningReplay)
                    {
                        Managers.RecordManager.Recorder.CreateRecord(carID, transform);
                        Managers.RecordManager.Recorder.StartRecording(carID);
                    } 
                    else Managers.RecordManager.Recorder.PlayReplay(carID, transform);
                }
                else
                {
                    if(!isRunningReplay)
                        Managers.RecordManager.Recorder.StopRecording(carID);
                    else
                        Managers.RecordManager.Recorder.StopReplay(carID);
                }
            }
        }
        
        #endregion

        #region Private Values

        private bool _isStarted = false; // Is the car started to move?
        private Rigidbody _carRigid; // Car rigidbody

        #endregion

        private void Awake() => _carRigid = GetComponent<Rigidbody>();

        private void FixedUpdate()
        {
            if(!_isStarted) return;

            if(!isRunningReplay)
            {
                _carRigid.velocity = transform.forward * carSpeed * Time.fixedDeltaTime;
            }
        }

        private void OnCollisionEnter(Collision other)  // Check car hit
        {
            if(!isRunningReplay && other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                Managers.RecordManager.Recorder.DestroyReplay(carID);
                Managers.GameManager.Instance.OnCarFailed();
            }
                
        }
        
        /// <summary>
        /// Rotate car
        /// </summary>
        /// <param name="rotation"> Car rotation direction </param>
        public void RotateCar(ROTATION rotation)
        {
            if(isRunningReplay) return;

            switch (rotation)
            {
                case ROTATION.LEFT:
                    transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
                    break;
                case ROTATION.RIGHT:
                    transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
                    break;
                case ROTATION.NONE:
                    break;
                default:
                    break;
            }
        }
    }
}