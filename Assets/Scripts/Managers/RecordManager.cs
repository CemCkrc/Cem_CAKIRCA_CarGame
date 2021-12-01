using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARGAME.Managers
{
    public class RecordManager : MonoBehaviour
    {
        public static RecordManager Recorder;

        Dictionary<int,RecordData> recordDatas;

        [SerializeField] private RecordData data;

        private void Awake()
        {
            if(!Recorder)
                Recorder = this;
            else
                Destroy(this.gameObject);

            recordDatas = new Dictionary<int,RecordData>();
        }

        public void CreateRecordData(int recordID, Transform recordObject)
        {
            RecordData createdData = Instantiate(data, Vector3.zero, Quaternion.identity, this.transform);
            createdData.SetRecordData(recordID, recordObject);

            recordDatas.Add(recordID, createdData);
        }

        public void StartRecording(int recordID)
        {
            if (recordDatas.ContainsKey(recordID)) recordDatas[recordID].StartRecording();
        }

        public void StopRecording(int recordID)
        {
            if (recordDatas.ContainsKey(recordID)) recordDatas[recordID].StopRecording();
        }

        public void PlayReplay(int recordID, Transform recordedObject)
        {
            if (recordDatas.ContainsKey(recordID))
            {
                recordedObject.GetComponent<MeshRenderer>().enabled = true;
                recordDatas[recordID].StartReplay(recordedObject);
            } 
        }

        public void StopReplay(int recordID)
        {
            if (recordDatas.ContainsKey(recordID)) recordDatas[recordID].StopReplay();
        }

        public void ResetAllRecords()
        {
            foreach (var item in recordDatas)
            {
                item.Value.ResetReplay();
            }
            Debug.Log(recordDatas.Count);
        }

        public void DestroyReplay(int recordID)
        {
            RecordData item = recordDatas[recordID];

            if(item != null)
            {
                recordDatas.Remove(recordID);
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
