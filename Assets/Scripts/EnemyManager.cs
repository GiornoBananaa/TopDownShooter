using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _deadEnemyPrefab;
    [SerializeField] private GameObject _winPanel;

    private int _enemiesCount;

    private void Start()
    {
        _enemiesCount = transform.childCount;
    }

    public void KillEnemy(GameObject enemy)
    {
        GameObject body = Instantiate(_deadEnemyPrefab, enemy.transform);
        body.transform.parent = null;
        Destroy(enemy);

        _enemiesCount--;
        if (_enemiesCount <= 0) 
        {
            _winPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void RaiseTheAlarm()
    {
        foreach(EnemyMovement enemy in GetComponentsInChildren<EnemyMovement>())
        {
            enemy.RiseAlarm();
            enemy.GetComponent<EnemyVision>().SeekForPlayer();
        }
    }
}
