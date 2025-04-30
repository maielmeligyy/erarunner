using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyX = -15f;

    public void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
