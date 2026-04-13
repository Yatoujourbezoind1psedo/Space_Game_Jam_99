using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] LaserManager laserManager;
    [SerializeField] private float ptScan, ptScanMoins = 1f;
    [SerializeField] private float interval = 3f;
    [SerializeField] private float maxScan = 20f;

    private float timer, ptActuel = 0f;
    private bool isBeingScanned = false;

    void Update()
    {
        if (!isBeingScanned) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if(maxScan > ptActuel)
            {
                ptActuel += ptScan; 
                laserManager.IncrementScan(ptScan);
                timer = 0f;
            }
            else //donc le target est full
            {
                //Debug.Log("FULL");
                laserManager.DecrementScan(ptScanMoins); 
                timer = 0f; 
            }
            
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