using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    [SerializeField] private int _bulletLayer;
    [SerializeField] private int _enemyLayer;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(_bulletLayer, _bulletLayer);
        Physics2D.IgnoreLayerCollision(_enemyLayer, _enemyLayer);
    }
}
