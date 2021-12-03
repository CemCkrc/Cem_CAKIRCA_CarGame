using System.Collections.Generic;
using CARGAME.GameData;
using UnityEngine;

namespace CARGAME.Managers
{
    public class RecordManager : MonoBehaviour
    {
        public static RecordManager Recorder; //RecordManager Singleton

        private Dictionary<int,RecordData> _recordDatas; //All recorded datas

        [SerializeField] private RecordData _dataPrefab; //Record data prefab

        private void Awake()
        {
            if(!Recorder)
                Recorder = this;
            else
                Destroy(this.gameObject);

            _recordDatas = new Dictionary<int,RecordData>();
        }

        /// <summary>
        /// Create a record for object
        /// </summary>
        /// <param name="recordID"> ID for created data </param>
        /// <param name="recordedObject"> Object for record data </param>
        public void CreateRecord(int recordID, Transform recordObject)
        {
            RecordData createdData = Instantiate(_dataPrefab, Vector3.zero, Quaternion.identity, this.transform);
            createdData.CreateRecordData(recordID, recordObject);

            _recordDatas.Add(recordID, createdData);
        }

        /// <summary>
        /// Start recording object
        /// </summary>
        /// <param name="recordID"> Start recording object which has equal recordID </param>
        public void StartRecording(int recordID)
        {
            if (_recordDatas.ContainsKey(recordID)) _recordDatas[recordID].StartRecording();
        }

        /// <summary>
        /// Stop recording object
        /// </summary>
        /// <param name="recordID"> Stop recording object which has equal recordID </param>
        public void StopRecording(int recordID)
        {
            if (_recordDatas.ContainsKey(recordID)) _recordDatas[recordID].StopRecording();
        }

        /// <summary>
        /// Play object replay 
        /// </summary>
        /// <param name="recordID"> Play object replay which has equal recordID </param>
        public void PlayReplay(int recordID, Transform recordedObject)
        {
            if (_recordDatas.ContainsKey(recordID))
            {
                recordedObject.GetComponent<MeshRenderer>().enabled = true;
                _recordDatas[recordID].StartReplay(recordedObject);
            }
            else ThrowRecordNotFoundWarning(recordID);
        }

        /// <summary>
        /// Stop object replay
        /// </summary>
        /// <param name="recordID"> Stop object replay which has equal recordID </param>
        public void StopReplay(int recordID)
        {
            if (_recordDatas.ContainsKey(recordID)) _recordDatas[recordID].StopReplay();
            else ThrowRecordNotFoundWarning(recordID);
        }

        /// <summary>
        /// Reset all replays and set replay's current frame to 0
        /// </summary>
        public void ResetAllRecords()
        {
            foreach (var item in _recordDatas)
                item.Value.ResetReplay();
        }

        /// <summary>
        /// Destroy failed replay
        /// </summary>
        /// <param name="recordID"> Destroy failed replay which has equal recordID </param>
        public void DestroyReplay(int recordID)
        {
            if (!_recordDatas.ContainsKey(recordID))
            {
                ThrowRecordNotFoundWarning(recordID);
                return;
            }

            RecordData item = _recordDatas[recordID];

            if(item != null)
            {
                _recordDatas.Remove(recordID);
                Destroy(item.gameObject);
                return;
            }
            else
                ThrowRecordNotFoundWarning(recordID);
        }
       
        private void ThrowRecordNotFoundWarning(int recordID) => Debug.LogWarning($"Replay Item not found. Record ID: {recordID}");
    }

}
