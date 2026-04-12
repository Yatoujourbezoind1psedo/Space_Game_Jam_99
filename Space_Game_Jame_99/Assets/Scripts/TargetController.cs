using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] LaserManager laserManager;
    [SerializeField] private float ptScan = 1f;
    [SerializeField] private float interval = 3f;

    private float timer = 0f;
    private bool isBeingScanned = false;

    void Update()
    {
        if (!isBeingScanned) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            laserManager.IncrementScan(ptScan);
            timer = 0f;
        }
    }

    public void StartScan()
    {
        isBeingScanned = true;
    }

    public void StopScan()
    {
        isBeingScanned = false;
        timer = 0f;
    }
}