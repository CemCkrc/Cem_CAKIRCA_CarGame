using UnityEngine;

namespace CARGAME.Cars
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        [Header("Car Stats")]
        public int carID;
        public float carSpeed = 3.0f;
        public float rotationSpeed = 1.0f;
        private bool _isStarted = false;

        private RecordData data;

        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;

                if(value)
                {
                    if(!isRunningReplay)
                    {
                        Managers.RecordManager.Recorder.CreateRecordData(carID, transform);
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
        
        public bool isRunningReplay = false;

        private Rigidbody _carRigid;

        private void Awake() 
        {
            _carRigid = GetComponent<Rigidbody>();
        }

        private void Start() 
        {
            GetComponent<MeshRenderer>().enabled = false;    
        }

        private void FixedUpdate()
        {
            if(!_isStarted) return;

            if(!isRunningReplay)
            {
                _carRigid.velocity = transform.forward * carSpeed * Time.fixedDeltaTime;
            }
        }

        private void OnCollisionEnter(Collision other) 
        {
            if(!isRunningReplay && other.gameObject.layer == 7) //hit two replayer
            {
                Managers.RecordManager.Recorder.DestroyReplay(carID);
                Managers.GameManager.Manager.OnCarFailed();
            } //change it to layermask value
                
        }
        
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