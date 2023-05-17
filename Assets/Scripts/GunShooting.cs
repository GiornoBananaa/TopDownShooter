using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private bool _alarmIsRised;
    [SerializeField] private LayerMask _targetLayer;

    private float _lastShootTime;
    private EnemyManager _enemyManager;

    private void Start()
    {
        _enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        _alarmIsRised = false;
    }

    void Update()
    {
        _lastShootTime += Time.deltaTime;

        if (Input.GetMouseButton(0) && _lastShootTime > _rateOfFire)
        {
            if (!_alarmIsRised)
            {
                _enemyManager.RaiseTheAlarm();
                _alarmIsRised = true;
            }
            GameObject bullet = Instantiate(_bullet, transform.position + transform.up * 0.3f, transform.rotation);
            bullet.GetComponent<BulletScript>().TargetLayer = _targetLayer;
            bullet.transform.parent = null;
            _lastShootTime = 0;
        }
    }
}