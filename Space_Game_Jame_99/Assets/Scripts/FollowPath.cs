using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public Transform[] points;
    public float speed = 5f;

    private int currentPoint = 0;

    void Update()
    {
        Transform target = points[currentPoint]; //Récupère le transform du point actuel

        transform.position = Vector3.MoveTowards( //déplace objet vers position du point progressivement sans dépasser cible
            transform.position, //position actuelle
            target.position, //destination
            speed * Time.deltaTime //vitesse constante
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f) //vérifie si on est arrivé (petite valeur pour éviter que point oscille)
        {
            currentPoint++;

            if (currentPoint >= points.Length) //fin du parcours
            {
                Destroy(gameObject); //détruit en fin de boucle
            }
        }
    }
}