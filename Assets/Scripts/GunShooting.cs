using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private LayerMask _targetLayer;

    private float _lastShootTime;

    void Update()
    {
        _lastShootTime += Time.deltaTime;

        if (Input.GetMouseButton(0) && _lastShootTime > _rateOfFire)
        {
            GameObject bullet = Instantiate(_bullet, transform.position + transform.up * 0.3f, transform.rotation);
            bullet.GetComponent<BulletScript>().TargetLayer = _targetLayer;
            bullet.transform.parent = null;
            _lastShootTime = 0;
        }
    }
}