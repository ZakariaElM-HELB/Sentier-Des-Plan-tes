using UnityEngine;
using UnityEngine.Android;

public class RequestGPSPermission : MonoBehaviour
{
    void Start()
    {
        // V�rifie si la permission GPS est d�j� accord�e
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // Demande la permission GPS � l'utilisateur
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }
}
