using System.Collections.Generic;
using UnityEngine;

namespace CARGAME.GameData
{
    public class RecordData : MonoBehaviour
    {
        #region  Public Values
            public int currentFrame = 0; //Current Frame
            public int endFrame = 0; //End frame

        #endregion

        #region  Private Values

            private Data _data; //Store recorded data
            private Transform _recordedObject; //Recorded object

            private bool _isRecording = false; //True if object is recording
            private bool _isReplaying = false; //True if object is replaying
            private bool _isDataCreated = false; //Check if record data created

        #endregion

        /// <summary>
        /// Create data for record
        /// </summary>
        /// <param name="recordID"> ID for created data </param>
        /// <param name="recordedObject"> Object transform for record position and rotation </param>
        public void CreateRecordData(int recordID, Transform recordedObject)
        {
            _isDataCreated = true;
            
            _recordedObject = recordedObject;

            _data.DataID = recordID;
            _data.positionData = new List<Vector3>();
            _data.rotationData = new List<Quaternion>();
        }
        
        /// <summary>
        /// Start record
        /// </summary>
        public void StartRecording()
        {
            if(!_isDataCreated)
            {
                ThrowDataNotCreatedError();
                return;
            }

            _isRecording = true;
        }
        
        /// <summary>
        /// Stop record
        /// </summary>
        public void StopRecording()
        {
            _isRecording = false;

            if(endFrame > 1000) ThrowFrameSizeError(); 
        }

        /// <summary>
        /// Start replay
        /// </summary>
        /// <param name="recordedObject"> Override recordedObject position and rotation via using data </param>
        public void StartReplay(Transform recordedObject)
        {
            if(!_isDataCreated)
            {
                ThrowDataNotCreatedError();
                return;
            }

            _recordedObject = recordedObject;
            _isReplaying = true;
        }

        /// <summary>
        /// Stop replay
        /// </summary>
        public void StopReplay() => _isReplaying = false;
        
        /// <summary>
        /// Return to first frame and stops
        /// </summary>
        public void ResetReplay()
        {
            currentFrame = 0;
            _isReplaying = false;
            _isRecording = false;
        }
        
        private void FixedUpdate()
        {
            if (_isReplaying && currentFrame < endFrame) //Replay data
            {
                _recordedObject.transform.position = _data.positionData[currentFrame];
                _recordedObject.transform.rotation = _data.rotationData[currentFrame];
                currentFrame++;
            }
            else if (_isRecording) //Record data
            {
                _data.positionData.Add(_recordedObject.position);
                _data.rotationData.Add(_recordedObject.rotation);     
                endFrame++;
            }
        }

        private void ThrowDataNotCreatedError() => Debug.LogError($"Record data not created.");
        private void ThrowFrameSizeError() => Debug.LogWarning($"Recorded data frame count bigger than 1000.");
    }

    struct Data //Data struct for store record
    {
        public int DataID; //Record Data unique ID

        public List<Vector3> positionData; //Record Data positions list

        public List<Quaternion> rotationData; //Record Data rotations list
    }
}