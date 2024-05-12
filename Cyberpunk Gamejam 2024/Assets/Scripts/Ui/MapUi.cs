using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUi : MonoBehaviour
{
    [SerializeField]
    private GameObject _iconPrefab;
    [SerializeField]
    private RunManager _runManager;

    public void OnEnable()
    {
        _runManager.OnProbabilityCollapse += BuildMap;
    }

    public void OnDisable()
    {
        _runManager.OnProbabilityCollapse -= BuildMap;
    }

    public void BuildMap()
    {
        List<RoomConfiguration> configurations = _runManager.RoomConfigs;
        for(int i = configurations.Count - 1; i >= 0; i--)
        {
            Instantiate(_iconPrefab, transform)
                .GetComponent<RoomPreviewUi>()
                .BuildRoomPreview(configurations[i]);
        }
    }
}
