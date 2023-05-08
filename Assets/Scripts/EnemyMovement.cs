using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private Transform[] path;

    private Rigidbody2D _rigidbody;
    private EnemyVision _vision;
    private Animator _animator;
    private int _target;
    private bool _goBack;


    private void Start()
    {
        _target = 0;

        Physics2D.gravity = Vector2.zero;
        _rigidbody = GetComponent<Rigidbody2D>();
        _vision = GetComponent<EnemyVision>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!_vision.CanSeePlayer())
        {
            Move();
        }
        else _rigidbody.velocity = Vector2.zero;

        if (_rigidbody.velocity.sqrMagnitude == 0) 
            _animator.SetBool("Walk", false);
        else
            _animator.SetBool("Walk", true);
    }

    void Move()
    {
        if (Vector2.Distance(transform.position, path[_target].position) < 0.2f)
        {
            if (_target == path.Length - 1) _goBack = true;
            else if (_target == 0) _goBack = false;

            if (_goBack) _target--;
            else _target++;
        }

        Vector3 norTar = (path[_target].position - transform.position).normalized;
        float angle = Mathf.Atan2(norTar.y, norTar.x) * Mathf.Rad2Deg;

        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0, angle - 90);
        transform.rotation = rotation;

        _rigidbody.velocity = transform.TransformDirection(Vector2.up * _speed);
    }
}
