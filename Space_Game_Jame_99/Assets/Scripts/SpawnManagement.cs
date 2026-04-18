using UnityEngine;

public class SpawnManagement : MonoBehaviour
{
    [SerializeField] private GameObject[] meteors; 
    [SerializeField] public float globalSpeed = 10f; 
    [SerializeField] public AudioSource musicSource; 

    [SerializeField] private Transform chemin1, chemin2, chemin3, chemin4; 
    private Transform[] pointsCh1, pointsCh2, pointsCh3, pointsCh4; 

    void Start()
    {
        pointsCh1 = GetPoints(chemin1);
        pointsCh2 = GetPoints(chemin2);
        pointsCh3 = GetPoints(chemin3);
        pointsCh4 = GetPoints(chemin4);
    }

    private Transform[] GetPoints(Transform parent) {
        if (parent == null) return null;
        Transform[] pts = new Transform[parent.childCount];
        for(int i = 0; i < parent.childCount; i++) pts[i] = parent.GetChild(i);
        return pts;
    }

    public void SpawnMeteor(int meteorX, Transform[] pointsChX)
    {
        // On vérifie si l'index demandé existe dans ton tableau de prefabs
        if(meteors == null || meteorX >= meteors.Length || meteors[meteorX] == null) return;

        GameObject mete = Instantiate(meteors[meteorX], pointsChX[0].position, meteors[meteorX].transform.rotation);
        FollowPath sc = mete.GetComponent<FollowPath>();
        
        if (sc != null) {
            sc.points = pointsChX;
            sc.speed = globalSpeed;
            sc.SetMusicSource(musicSource); 
        }
    }

    public void SpawnMeteorsExceptChemin(int oubli)
    {
        // On définit nos 4 listes de points
        Transform[][] listes = { pointsCh1, pointsCh2, pointsCh3, pointsCh4 };
        
        for (int i = 0; i < listes.Length; i++)
        {
            // (i + 1) car tes emplacements dans le rythme vont de 1 à 4
            if ((i + 1) != oubli) 
            {
                // On reprend l'index 0 comme avant pour être sûr d'avoir le bon prefab
                SpawnMeteor(0, listes[i]);
            }
        }
    }

    public float GetTravelTime(int cheminIndex, float speed)
    {
        Transform[] points = null;
        switch(cheminIndex) {
            case 1: points = pointsCh1; break;
            case 2: points = pointsCh2; break;
            case 3: points = pointsCh3; break;
            case 4: points = pointsCh4; break;
        }

        if (points == null || points.Length < 3) return 0f;

        float distanceTotale = 0f;
        // Distance jusqu'au point d'impact (index 2)
        for (int i = 0; i < 2; i++)
        {
            distanceTotale += Vector3.Distance(points[i].position, points[i+1].position);
        }
        return distanceTotale / speed;
    }
}