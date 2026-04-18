using UnityEngine;

public class KattyFollow : MonoBehaviour
{
    [SerializeField] private Camera menuCamera;
    [SerializeField] private float distanceDeLaCamera = 5f;
    [SerializeField] private Transform emptyBras; // Glisse l'empty de son bras ici

    void Update()
    {
        // 1. Récupère la position de la souris en pixels
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceDeLaCamera; // Distance entre Katty et la caméra

        // 2. Convertit les pixels en position 3D
        Vector3 worldPos = menuCamera.ScreenToWorldPoint(mousePos);

        // 3. Katty suit la souris (le corps entier)
        transform.position = worldPos;

        // Optionnel : Debug pour voir où le bras "clique"
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Katty a cliqué avec son bras à : " + emptyBras.position);
        }
    }
}