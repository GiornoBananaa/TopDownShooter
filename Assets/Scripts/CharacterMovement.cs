using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Animator _legsAnimator;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Image _hpImage;
    [SerializeField] private float _speed = 1;
    [SerializeField] private int _maxhealth = 100;

    private Rigidbody2D _rigidbody;
    private int _hp;

    public int HP
    {
        get => _hp;
        set
        {
            Color newColor;
            if (value > 0 ) newColor = new Color((float)value / _maxhealth < _maxhealth / 0.5f ? 1f - ((float)value / _maxhealth) : 1, (float)value / _maxhealth > 0.5f ? (float)value / _maxhealth : 1 , 0);
            else newColor = new Color(1,0 ,0);

             _hp = value;
            _hpText.text = value.ToString();
            _hpImage.color = newColor;

            if (_hp <= 0)
            {
                _hpText.text = "DEAD";
                _hpText.color = newColor;
                Time.timeScale = 0;
                _deathPanel.SetActive(true);
            }
        }
    }

    private void Start()
    {
        HP = _maxhealth;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        LookAtCursor();
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
        if (_y != 0 && _x != 0)
        {
            _y /= 2f;
            _x /= 2f;
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
