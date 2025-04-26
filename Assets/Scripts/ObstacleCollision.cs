using System.Diagnostics;
using System.Numerics;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Player hit the obstacle!");
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = UnityEngine.Vector2.zero;
        }
    }
}
