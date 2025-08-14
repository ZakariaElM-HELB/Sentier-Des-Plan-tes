using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    // Dictionnaire des URLs associ�es aux tags
    private Dictionary<string, string> urlDictionary = new Dictionary<string, string>()
    {
        { "mars", "https://mars.nasa.gov" },  // Remplace par l'URL pour Mars
        { "earth", "https://www.earthday.org" } // Remplace par l'URL pour Earth
    };

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // D�tection du clic sur PC
        {
            DetectTouchOrClick(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // D�tection du toucher sur mobile
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

            // V�rifie si l'objet touch�/cliqu� a un tag correspondant � une URL
            if (urlDictionary.ContainsKey(hit.transform.tag))
            {
                string url = urlDictionary[hit.transform.tag];
                Application.OpenURL(url);
                Debug.Log("Opening URL: " + url);
            }
        }
    }
}
