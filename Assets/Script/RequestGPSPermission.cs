using UnityEngine;
using UnityEngine.Android;

public class RequestGPSPermission : MonoBehaviour
{
    void Start()
    {
        // Vérifie si la permission GPS est déjà accordée
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // Demande la permission GPS à l'utilisateur
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }
}
