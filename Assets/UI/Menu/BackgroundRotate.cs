using UnityEngine;

public class BackgroundRotate : MonoBehaviour
{
    private float rotationSpeed = 10f; // Vitesse de la rotation

    void Update()
    {
        // Le fond d'écran du menu du jeu effectue une légère rotation en boucle
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
