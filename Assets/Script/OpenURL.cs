using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    // Dictionnaire des URLs associées aux tags
    private Dictionary<string, string> urlDictionary = new Dictionary<string, string>()
    {
        { "mars", "https://mars.nasa.gov" },  // Remplace par l'URL pour Mars
        { "earth", "https://www.earthday.org" } // Remplace par l'URL pour Earth
    };

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Détection du clic sur PC
        {
            DetectTouchOrClick(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Détection du toucher sur mobile
        {
            DetectTouchOrClick(Input.GetTouch(0).position);
        }
    }

    void DetectTouchOrClick(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("Hit detected on: " + hit.transform.name + " (Tag: " + hit.transform.tag + ")");

            // Vérifie si l'objet touché/cliqué a un tag correspondant à une URL
            if (urlDictionary.ContainsKey(hit.transform.tag))
            {
                string url = urlDictionary[hit.transform.tag];
                Application.OpenURL(url);
                Debug.Log("Opening URL: " + url);
            }
        }
    }
}
