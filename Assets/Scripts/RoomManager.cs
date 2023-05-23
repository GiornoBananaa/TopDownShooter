using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]private EnemyManager _enemyManager;

    public bool PlayerIsInside { get; private set; }
    public List<GameObject> Enemies { get; private set; }
    public RoomManager[] AccessibleRooms;

    private void Start()
    {
        Enemies = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMovement enemyMovement;
        if (collision.gameObject.GetComponent<CharacterMovement>())
        {
            PlayerIsInside = true;
        }
        else if (enemyMovement = collision.gameObject.GetComponent<EnemyMovement>())
        {
            enemyMovement.CurrentRoom = this;
            enemyMovement.GetNewPatrolPath();
            Enemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyMovement enemyMovement;
        if (collision.gameObject.GetComponent<CharacterMovement>())
        {
            PlayerIsInside = false;
        }
        else if (enemyMovement = collision.gameObject.GetComponent<EnemyMovement>())
        {
            Enemies.Remove(collision.gameObject);
            if (enemyMovement.CurrentRoom is null && enemyMovement.CurrentRoom == this) _enemyManager.FindEnemyRoom(enemyMovement.gameObject);
        }
    }
}
