using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Animator _legsAnimator;
    [SerializeField] private Sprite _knifeIdle;
    [SerializeField] private Sprite _gunIdle;
    [SerializeField] private float _speed = 1;

    private Rigidbody2D _rigidbody;
    private bool _gunEquiped;


    private void Start()
    {
        _gunEquiped = true;
        Physics2D.gravity = Vector2.zero;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Inventory();
        Move();
        LookAtCursor();
    }

    void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _gunEquiped = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _gunEquiped = true;
        }
    }

    void Move()
    {
        float _x = 0;
        float _y = 0;

        if (Input.GetKey(KeyCode.W)) _y += _speed;
        if (Input.GetKey(KeyCode.S)) _y -= _speed;
        if (Input.GetKey(KeyCode.D)) _x += _speed;
        if (Input.GetKey(KeyCode.A)) _x -= _speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _y *= 1.5f;
            _x *= 1.5f;
        }

        if (_y != 0 || _x != 0)
            _legsAnimator.SetBool("Run", true);
        else
            _legsAnimator.SetBool("Run", false);

        _rigidbody.velocity = new Vector2(_x, _y);
    }

    void LookAtCursor()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        transform.up = direction;
    }
}
