using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _recruitPrefab;
    [SerializeField]
    private GameObject _spawnPoint;
    [SerializeField]
    private GameObject _targetPoint;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private List<GameObject> _recruits = new List<GameObject>();

    /// <summary>
    /// Spawns a recruit and let's it walk to the target point
    /// </summary>
    /// <param name="element"></param>
    public void SpawnRecruit(Element element, int price, int position)
    {
        GameObject recruit = Instantiate(_recruitPrefab, _spawnPoint.transform.position, Quaternion.identity.normalized, transform)
            .GetComponent<Recruit>()
            .SetElement(element)
            .SetCost(price)
            .GetComponent<MovementController>()
            .SetTargetPosition(_targetPoint.transform.position + position * _offset);
        _recruits.Add(recruit);
    }

    public void RemoveAllRecruits()
    {
        foreach(GameObject recruit in _recruits)
        {
            Destroy(recruit);
        }
        _recruits.Clear();
    }
}
