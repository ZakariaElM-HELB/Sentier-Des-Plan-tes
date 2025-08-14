using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    [Tooltip("Vitesse de rotation en degrés par seconde.")]
    public float rotationSpeed = 10f;

    void Update()
    {
        // Effectue une rotation autour de l'axe Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
