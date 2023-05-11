using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float _speed;
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
            if (collision.gameObject.GetComponent<CharacterMovement>())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else Destroy(gameObject);
        }
        else if ((_obstacleLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
