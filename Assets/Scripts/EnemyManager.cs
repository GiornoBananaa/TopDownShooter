using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void RaiseTheAlarm()
    {
        foreach(EnemyMovement enemy in GetComponentsInChildren<EnemyMovement>())
        {
            enemy.GetPlayerPosition();
        }
    }

    public void StartPatrol()
    {
        foreach (EnemyMovement enemy in GetComponentsInChildren<EnemyMovement>())
        {
            enemy.GetNewPatrolPath();
        }
    }
}
