using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]private float _speed;
    [SerializeField]private float _maxSpeed;
    [SerializeField]private float _viewDistance;

    private Transform _player;
    private Transform _transform;
    private Vector3 _velocity;

    void Start()
    {
        _transform = transform;
        _velocity = Vector3.zero;
        _player = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        //Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;

        //Vector3 offset = (mouseScreenPosition - _player.position) * _viewDistance;

        _transform.position = Vector3.SmoothDamp(_transform.position, 
            new Vector3(_player.position.x, _player.position.y, _transform.position.z), 
            ref _velocity, _speed, _maxSpeed);
    }
}
