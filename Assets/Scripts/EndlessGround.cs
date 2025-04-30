using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGround : MonoBehaviour
{
    public float speed = 5f;
    public float resetPositionX = -10f;
    public float startPositionX = 10f;
    public List<Transform> grounds;
    private int index;
    
    private void Start()
    {
        index = 0;
    }
    private void Update()
    {
        grounds[index].Translate(Vector2.left * speed * Time.deltaTime);

        if (grounds[index].position.x < resetPositionX)
        {
            Vector3 newPos = new Vector3(startPositionX, grounds[index].position.y, grounds[index].position.z);
            index = (index + 1) % grounds.Count;
            KillChildforGround(grounds[index]);
            grounds[index].position = newPos;
        }
    }
    private void KillChildforGround(Transform parent) {
        int size = parent.childCount;
        for (int i = 0; i < size; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
    public Transform GetActiveGround()
    {
        return grounds[index];
    }

}
