using UnityEngine;

public class PlanetClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                RemovePlanet();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                RemovePlanet();
            }
        }
    }

    void RemovePlanet()
    {
        Debug.Log($"🪐 {gameObject.name} cliquée, suppression en cours...");

        Destroy(gameObject);
    }
}
