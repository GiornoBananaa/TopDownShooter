using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayerMask;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("Knife");

            Collider2D[] colliders = Physics2D.OverlapCapsuleAll(
                transform.position + (transform.up * 0.8f), new Vector2(0.9f,1.1f),
                CapsuleDirection2D.Vertical, 0, _enemyLayerMask);
            
            foreach (Collider2D collider in colliders)
            {
                Debug.Log(collider.name);
                Destroy(collider.gameObject);
            }
        }
    }
}
