using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public Transform[] points;
    public float speed = 5f;

    private int currentPoint = 0;

    void Update()
    {
        Transform target = points[currentPoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint++;

            if (currentPoint >= points.Length)
            {
                Destroy(gameObject); //détruit en fin de boucle
            }
        }
    }
}