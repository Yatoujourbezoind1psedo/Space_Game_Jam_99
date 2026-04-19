using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ClicAction : MonoBehaviour
{
    [SerializeField] private UnityEvent _redirection; 

void Update()
{
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("Texte 3D cliqué !");
                _redirection.Invoke(); 
            }
        }
    }
}
}
