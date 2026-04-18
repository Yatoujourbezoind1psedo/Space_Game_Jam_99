using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    [Header("Cible à suivre (ex: le réacteur)")]
    public Transform target;

    [Header("Décalage manuel (X et Y uniquement)")]
    public Vector2 offset;

    void LateUpdate()
    {
        if (target != null)
        {
            // On récupère les X et Y de la cible + l'offset
            // Mais pour le Z, on garde 'transform.position.z' (lui-même)
            transform.position = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                transform.position.z 
            );
            
            // On ne touche pas à la rotation
        }
    }
}