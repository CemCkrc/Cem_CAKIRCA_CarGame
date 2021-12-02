using System.Collections.Generic;
using UnityEngine;

namespace CARGAME.Managers
{
    public class RecordManager : MonoBehaviour
    {
        public static RecordManager Recorder;

        private Dictionary<int,RecordData> _recordDatas;

        [SerializeField] private RecordData _dataPrefab;

        private void Awake()
        {
            if(!Recorder)
                Recorder = this;
            else
                Destroy(this.gameObject);

            _recordDatas = new Dictionary<int,RecordData>();
        }

        public void CreateRecordData(int recordID, Transform recordObject)
        {
            RecordData createdData = Instantiate(_dataPrefab, Vector3.zero, Quaternion.identity, this.transform);
            createdData.SetRecordData(recordID, recordObject);

            _recordDatas.Add(recordID, createdData);
        }

        public void StartRecording(int recordID)
        {
            if (_recordDatas.ContainsKey(recordID)) _recordDatas[recordID].StartRecording();
        }

        public void StopRecording(int recordID)
        {
            if (_recordDatas.ContainsKey(recordID)) _recordDatas[recordID].StopRecording();
        }

        public void PlayReplay(int recordID, Transform recordedObject)
        {
            if (_recordDatas.ContainsKey(recordID))
            {
                recordedObject.GetComponent<MeshRenderer>().enabled = true;
                _recordDatas[recordID].StartReplay(recordedObject);
            } 
        }

        public void StopReplay(int recordID)
        {
            if (_recordDatas.ContainsKey(recordID)) _recordDatas[recordID].StopReplay();
        }

        public void ResetAllRecords()
        {
            foreach (var item in _recordDatas)
            {
                item.Value.ResetReplay();
            }
            Debug.Log(_recordDatas.Count);
        }

        public void DestroyReplay(int recordID)
        {
            RecordData item = _recordDatas[recordID];

            if(item != null)
            {
                _recordDatas.Remove(recordID);
                Destroy(item.gameObject);
                return;
            }
            else
            {
                Debug.Log($"Replay Item not found. Replay ID: {recordID}");
            }
        }
    }

}
