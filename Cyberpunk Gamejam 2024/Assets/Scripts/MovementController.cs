using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _t;

    [SerializeField]
    private Vector3 _originalPosition;
    [SerializeField]
    private Vector3? _targetPosition;

    [SerializeField]
    private RuntimeSet _movementControllerSet;

    private void Start()
    {
        _movementControllerSet.Register(gameObject);
    }

    private void OnDestroy()
    {
        _movementControllerSet.Unregister(gameObject);
    }

    public bool ReachedDestination
    {
        get
        {
            return _targetPosition == null;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _originalPosition = transform.position;
        _targetPosition = targetPosition;
        _t = 0f;
    }

    private void Update()
    {
        if(_targetPosition == null)
        {
            return;
        }
        _t += Time.deltaTime * _speed;
        transform.position = Vector3.Lerp(_originalPosition, (Vector3)_targetPosition, _t);
        if(_t >= 1f)
        {
            _targetPosition = null;
            Debug.Log("Reached destination");
        }
    }
}
