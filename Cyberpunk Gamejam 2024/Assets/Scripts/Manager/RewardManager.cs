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

    /// <summary>
    /// Spawns a recruit and let's it walk to the target point
    /// </summary>
    /// <param name="element"></param>
    public void SpawnRecruit(Element element, int price)
    {
        Instantiate(_recruitPrefab, _spawnPoint.transform.position, Quaternion.identity.normalized, transform)
            .GetComponent<Recruit>()
            .SetElement(element)
            .SetCost(price)
            .GetComponent<MovementController>()
            .SetTargetPosition(_targetPoint.transform.position);
    }
}
