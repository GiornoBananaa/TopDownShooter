using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _deadEnemyPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private PathManager _pathManager;
    [SerializeField] private bool _infinityEnimies;
    [SerializeField] private int _enimiesMaxCount;

    private int _enemiesCount;
    private int _killedEnemies;
    private bool _isAlarm;

    private void Start()
    {
        _killedEnemies = 0;
        _isAlarm = false;
        _enemiesCount = transform.childCount;

        if (_infinityEnimies)
            _killsText.text = _killedEnemies.ToString() + " kills";
        else
            _killsText.text = _enemiesCount.ToString() + " enimies left";
    }

    private void Update()
    {
        if(_infinityEnimies && _enemiesCount < _enimiesMaxCount)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        RoomManager playerRoom = _pathManager.FindPlayer();
        while (true)
        {
            int spawnIndex = Random.Range(0, _pathManager.transform.childCount);
            RoomManager room = _pathManager.transform.GetChild(spawnIndex).GetComponent<RoomManager>();
            foreach(RoomManager r in room.AccessibleRooms)
            {
                if (r.name == playerRoom.name) continue;
            }
            if(playerRoom != room)
            {
                EnemyMovement enemy = Instantiate(_enemyPrefab, room.transform).GetComponent<EnemyMovement>();
                enemy.transform.parent = transform;
                enemy.CurrentRoom = room;
                _enemiesCount++;
                if (_isAlarm == true) 
                    StartCoroutine(NewEnemyAlarm(enemy));
                break;
            }
        }
    }

    private IEnumerator NewEnemyAlarm(EnemyMovement enemy)
    {
        yield return new WaitForSeconds(0.1f);
        enemy.RiseAlarm();
        enemy.GetComponent<EnemyVision>().SeekForPlayer();
    }

    public void KillEnemy(GameObject enemy)
    {
        GameObject body = Instantiate(_deadEnemyPrefab, enemy.transform);
        body.transform.parent = null;
        Destroy(enemy);

        _enemiesCount--;
        _killedEnemies++;

        if(_infinityEnimies)
            _killsText.text = _killedEnemies.ToString() + " kills";
        else
            _killsText.text = _enemiesCount.ToString() + " enimies left";

        if (_enemiesCount <= 0)
        {
            _winPanel.SetActive(true);
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex+1);
            PlayerPrefs.Save();
            Time.timeScale = 0;
        }
    }

    public void RaiseTheAlarm()
    {
        if (_isAlarm == true) return;
        _isAlarm = true;
        StartCoroutine(RisingAlarm());
    }

    private IEnumerator RisingAlarm()
    {
        yield return new WaitForSeconds(0.4f);
        foreach (EnemyMovement enemy in GetComponentsInChildren<EnemyMovement>())
        {
            enemy.RiseAlarm();
            enemy.GetComponent<EnemyVision>().SeekForPlayer();
        }
    }

    public void FindEnemyRoom(GameObject enemy)
    {
        foreach (RoomManager room in GetComponentsInChildren<RoomManager>())
        {
            if (room.Enemies.Contains(enemy))
            {
                enemy.GetComponent<EnemyMovement>().CurrentRoom = room;
                return;
            }
        }
    }
}
