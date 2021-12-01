using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData : MonoBehaviour
{
    public int recordID
    {
        get;
        private set;
    } = 0;

    public int currentFrame = 0;
    public int endFrame = 0;


    public List<Vector3> positionData;

    public List<Quaternion> rotationData;

    private Transform _recordedObject;
    private bool _isRecording = false;
    private bool _isReplaying = false;

    public void StartRecording()
    {
        _isRecording = true;
    }

    public void SetRecordData(int recordID, Transform recordedObject)
    {
        this.recordID = recordID;
        _recordedObject = recordedObject;
        positionData = new List<Vector3>();
        rotationData = new List<Quaternion>();
    }

    public void StopRecording()
    {
        _isRecording = false;
    }

    public void StartReplay(Transform recordedObject)
    {
        _recordedObject = recordedObject;
        _isReplaying = true;
    }

    public void StopReplay()
    {
        _isReplaying = false;
    }

    public void ResetReplay()
    {
        currentFrame = 0;
        _isReplaying = false;
        _isRecording = false;

        /*if(_recordedObject)
        {
            _recordedObject.position = positionData[0];
            _recordedObject.rotation = rotationData[0];
        }*/
    }
    private void FixedUpdate() 
    {
        if(_isReplaying && currentFrame != endFrame)
        {
            _recordedObject.transform.position = positionData[currentFrame];
            _recordedObject.transform.rotation = rotationData[currentFrame];
            currentFrame++;
        }
        else if(_isRecording)
        {
            positionData.Add(_recordedObject.position);
            rotationData.Add(_recordedObject.rotation);
            endFrame++;
        }
    }
}
