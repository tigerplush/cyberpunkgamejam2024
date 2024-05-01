using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _room;
    [SerializeField]
    private float _roomHeight = 6f;
    [SerializeField]
    private int _roomNumber = 0;

    [SerializeField]
    private Vector3 _originalPosition;
    private Vector3? _targetPosition;
    [SerializeField]
    private float _t;

    public bool DestinationReached
    {
        get
        {
            return _targetPosition == null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnRoom();
    }

    public void SpawnRoom()
    {
        Instantiate(_room, transform).transform.localPosition = new Vector3(0f, _roomNumber * _roomHeight);
        _roomNumber += 1;
    }

    public void ScrollToRoom(int roomNumber)
    {
        _originalPosition = transform.position;
        _t = 0f;
        _targetPosition = new Vector3(0f, -1f * roomNumber * _roomHeight);
    }

    private void Update()
    {
        if(_targetPosition == null)
        {
            return;
        }
        _t += Time.deltaTime;
        transform.position = Vector3.Lerp(_originalPosition, (Vector3)_targetPosition, _t);
        if(_t >= 1f)
        {
            _targetPosition = null;
        }
    }

    public IEnumerator SpawnAndScrollToNextRoom()
    {
        SpawnRoom();
        yield return new WaitForNextFrameUnit();
        ScrollToRoom(_roomNumber - 1);
        yield return new WaitUntil(() => DestinationReached);
        yield return null;
    }
}
