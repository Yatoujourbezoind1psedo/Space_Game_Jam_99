using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public Transform[] points;
    public float speed;
    private AudioSource musicSource;

    private int currentPoint = 0;
    private bool impactLogged = false;

    public void SetMusicSource(AudioSource source) {
        musicSource = source;
    }

    void Update()
    {
        if (points == null || currentPoint >= points.Length) return;

        Transform target = points[currentPoint];
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float moveDistance = speed * Time.deltaTime;

        // Si on va dépasser le point cette frame, on se téléporte dessus
        if (moveDistance >= distanceToTarget)
        {
            transform.position = target.position;
            
            // Log de l'impact au Point 3 (index 2)
            if (currentPoint == 2 && !impactLogged)
            {
                LogImpactTime();
                impactLogged = true;
            }

            currentPoint++;
            if (currentPoint >= points.Length) Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveDistance);
        }
    }

    private void LogImpactTime()
    {
        float timeToLog = (musicSource != null) ? musicSource.time : Time.timeSinceLevelLoad;

        int minutes = Mathf.FloorToInt(timeToLog / 60f);
        int seconds = Mathf.FloorToInt(timeToLog % 60f);
        int milliseconds = Mathf.FloorToInt((timeToLog * 1000f) % 1000f);

        Debug.Log(string.Format("<color=cyan>IMPACT AUDIO à : {0:00}:{1:00}:{2:000}</color>", minutes, seconds, milliseconds));
    }
}