using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _obstacleLayer;

    public LayerMask TargetLayer;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidbody.velocity = transform.TransformDirection(Vector2.up * _speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((TargetLayer & (1 << collision.gameObject.layer)) != 0)
        {
            EnemyMovement _enemyMovemrnt;
            CharacterMovement _characterMovemrnt;
            if (_characterMovemrnt = collision.gameObject.GetComponent<CharacterMovement>())
            {
                _characterMovemrnt.HP -= _damage;
            }
            else if (_enemyMovemrnt = collision.gameObject.GetComponent<EnemyMovement>())
            {
                _enemyMovemrnt.HP -= _damage;
            }
            Destroy(gameObject);
        }
        else if ((_obstacleLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
