using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool PlayerIsInside { get; private set; }
    public List<GameObject> Enemies { get; private set; }
    public RoomManager[] AccessibleRooms;

    private void Start()
    {
        Enemies = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterMovement>())
        {
            PlayerIsInside = true;
        }
        else if (collision.gameObject.GetComponent<EnemyMovement>())
        {
            collision.gameObject.GetComponent<EnemyMovement>().CurrentRoom = this;
            Enemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterMovement>())
        {
            PlayerIsInside = false;
        }
        else if (collision.gameObject.GetComponent<EnemyMovement>())
        {
            Enemies.Remove(collision.gameObject);
        }
    }
}
