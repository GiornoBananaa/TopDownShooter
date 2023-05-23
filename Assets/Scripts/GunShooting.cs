using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private bool _alarmIsRised;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private int _uiLayer;

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

        if (Input.GetMouseButton(0) && _lastShootTime > _rateOfFire && !IsPointerOverUIElement() && Time.timeScale != 0)
        {
            if (!_alarmIsRised)
            {
                _enemyManager.RaiseTheAlarm();
                _alarmIsRised = true;
            }
            _audioSource.Play();
            GameObject bullet = Instantiate(_bullet, transform.position + transform.up * 0.3f, transform.rotation);
            bullet.GetComponent<BulletScript>().TargetLayer = _targetLayer;
            bullet.transform.parent = null;
            _lastShootTime = 0;
        }
    }
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == 5)
                return true;
        }
        return false;
    }


    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}