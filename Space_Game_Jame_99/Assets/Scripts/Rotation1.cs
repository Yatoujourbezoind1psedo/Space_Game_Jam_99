using UnityEngine;

public class Rotation1 : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    float currentAngle = 0f;

    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
    }
}
