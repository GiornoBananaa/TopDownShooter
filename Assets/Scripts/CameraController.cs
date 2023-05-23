using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float k = 2.0f;
    [SerializeField] float maxOffset = 1.0f;

    void FixedUpdate()
    {
        Vector3 p = CharacterMovement.Position;
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p += Vector3.ClampMagnitude(mp - p, maxOffset) / k;
        p.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, p, 10.0f * Time.deltaTime);
    }
}
