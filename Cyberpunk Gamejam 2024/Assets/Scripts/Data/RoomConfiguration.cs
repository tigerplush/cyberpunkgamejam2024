using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomConfiguration
{
    public RoomType RoomType;
    public Element[] PossibleElements;
    public List<EnemyConfiguration> OffensiveEnemies;
    public List<EnemyConfiguration> DefensiveEnemies;
}
