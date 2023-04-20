using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    [SerializeField] private GameObject[] _maskObjects;

    void Start()
    {
        foreach (GameObject obj in _maskObjects)
        {
            obj.GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
    }
}
