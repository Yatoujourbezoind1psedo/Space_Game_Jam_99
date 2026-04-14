using Unity.VisualScripting;
using UnityEngine;

public class SpawnManagement : MonoBehaviour
{
    [SerializeField] private GameObject[] meteors; 

    [SerializeField] private Transform chemin1, chemin2, chemin3, chemin4; 
    private Transform[] pointsCh1, pointsCh2, pointsCh3, pointsCh4; //Listes des points des chemins 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialisation des points présents dans les chemins 
        pointsCh1 = new Transform[chemin1.childCount]; 
        for(int i = 0; i < chemin1.childCount; i++)
        {
            pointsCh1[i] = chemin1.GetChild(i); 
        }


        pointsCh2 = new Transform[chemin2.childCount]; 
        for(int i = 0; i < chemin2.childCount; i++)
        {
            pointsCh2[i] = chemin2.GetChild(i); 
        }

        pointsCh3 = new Transform[chemin3.childCount]; 
        for(int i = 0; i < chemin2.childCount; i++)
        {
            pointsCh3[i] = chemin3.GetChild(i); 
        }

        pointsCh4 = new Transform[chemin4.childCount]; 
        for(int i = 0; i < chemin2.childCount; i++)
        {
            pointsCh4[i] = chemin4.GetChild(i); 
        }

        //SpawnMeteorsExceptChemin(0); 
    }

    // Update is called once per frame
    void Update()
    {
    }

/*
    public void SpawnMeteor1()
    {
        SpawnMeteor(0, pointsCh1); 
    }

    public void SpawnMeteor2()
    {
        SpawnMeteor(1, pointsCh2); 
    }*/

    public void SpawnMeteor(int meteorX, Transform[] pointsChX)
    {
        GameObject mete = Instantiate(meteors[meteorX], pointsChX[0].transform.position, meteors[meteorX].transform.rotation);  //Instantie le météore X dnas la lsite au point 0 du chemin donné
        FollowPath sc = mete.GetComponent<FollowPath>(); // Récupère le script Follow Path
        sc.points = pointsChX; //Lui donne les points qu'il doit suivre
    }

    public void SpawnMeteorsExceptChemin (int oubli) //fait apparaitre météore sauf à l'emplacement dnné (0 = pas de météores); 
    {
        switch (oubli)
        {
            case 0:
                break; 

            case 1: //Donc on ne prend pas le premier météore 
                SpawnMeteor(0, pointsCh2); 
                SpawnMeteor(1, pointsCh3);
                SpawnMeteor(0, pointsCh4);
                break; 

            case 2: //ici pas le deuxième et bla bla bla
                SpawnMeteor(0, pointsCh1); 
                SpawnMeteor(1, pointsCh3);
                SpawnMeteor(0, pointsCh4);
                break; 

            case 3: 
                SpawnMeteor(0, pointsCh1); 
                SpawnMeteor(1, pointsCh2);
                SpawnMeteor(0, pointsCh4);
                break;


            case 4: 
                SpawnMeteor(0, pointsCh1); 
                SpawnMeteor(1, pointsCh2);
                SpawnMeteor(0, pointsCh3);
                break;

        }
    }

}
