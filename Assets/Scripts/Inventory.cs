using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Sprite _knifeIdle;
    [SerializeField] private Sprite _gunIdle;
    [SerializeField] private KnifeHandler _knifeHandler;
    [SerializeField] private GameObject _gun;

    private bool _gunEquiped;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _gunEquiped = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _gunEquiped)
        {
            _gunEquiped = false;
            _spriteRenderer.sprite = _gunIdle;
            _knifeHandler.enabled = false;
            _gun.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !_gunEquiped)
        {
            _gunEquiped = true;
            _spriteRenderer.sprite = _knifeIdle;
            _knifeHandler.enabled = true;
            _gun.SetActive(false);
        }
    }
}
