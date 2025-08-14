using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.Networking;

public class LocationStuff : MonoBehaviour
{
    public static LocationStuff Instance; // Permet un accès global
    public TMP_Text debugTxt;  // Affichage des informations GPS
    public bool gps_ok = false;

    private GPSLoc currLoc = new GPSLoc();      // Position GPS actuelle
    private GPSLoc initialUserLoc = null;         // Position initiale (origine)

    [Header("Liste des objets avec leurs coordonnées GPS (chargés depuis le JSON)")]
    [SerializeField] public List<GPSObject> objects = new List<GPSObject>();

    // Une fois instanciées, les planètes restent fixes dans le monde
    private Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>();

    [Header("Chemin du fichier JSON (dans StreamingAssets)")]
    [SerializeField] private string jsonFilePath = "JSON/planets.json";

    [SerializeField] private float gpsScale = 1.0f; // Tu peux augmenter cette valeur pour agrandir l'offset


    // Facteur de conversion approximatif : 1° de latitude ≈ 111320 m
    private const double metersPerDegreeLat = 111320.0;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        // Charger le JSON pour remplir la liste des planètes
        yield return StartCoroutine(LoadPlanetsFromJSON());

        // Initialisation du GPS
        if (!Input.location.isEnabledByUser)
        {
            debugTxt.text = "⚠ GPS désactivé sur l'appareil !";
            Debug.Log("⚠ GPS désactivé sur l'appareil !");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
            Debug.Log("⏳ Initialisation GPS en cours...");
        }

        if (maxWait < 1)
        {
            debugTxt.text = "⏳ Timeout : GPS trop long à répondre !";
            Debug.LogError("⏳ Timeout : GPS trop long à répondre !");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            debugTxt.text = "❌ Impossible d'obtenir la localisation";
            Debug.LogError("❌ Impossible d'obtenir la localisation");
            yield break;
        }
        else
        {
            gps_ok = true;
            Debug.Log("✅ GPS activé !");
        }

        // Attendre un instant pour obtenir une position stable
        yield return new WaitForSeconds(1f);
        currLoc.lat = Input.location.lastData.latitude;
        currLoc.lon = Input.location.lastData.longitude;
        initialUserLoc = new GPSLoc(currLoc.lon, currLoc.lat);  // Note : le constructeur prend (lon, lat)
        debugTxt.text = $"Position initiale : {initialUserLoc}\n";

        // Instancier toutes les planètes en position fixe
        InstantiatePlanets();
    }

    // Instancie chaque planète à une position fixe calculée à partir de la position initiale
    void InstantiatePlanets()
    {
        // On utilise la position de la caméra au moment de l'instanciation comme origine (ceci peut être modifié)
        Vector3 origin = Camera.main.transform.position;
        debugTxt.text += $"Origine (caméra) : {origin}\n";

        foreach (GPSObject obj in objects)
        {
            // Calcul de la différence en degrés
            double latDiff = obj.position.lat - initialUserLoc.lat;   // Différence en latitude (nord-sud)
            double lonDiff = obj.position.lon - initialUserLoc.lon;   // Différence en longitude (est-ouest)

            // Conversion en mètres :
            // Pour la latitude, 1° ≈ 111320 m.
            // Pour la longitude, 1° ≈ 111320 * cos(latitude en radians)
            double offsetZ = (latDiff * metersPerDegreeLat) * gpsScale;
            double offsetX = (lonDiff * metersPerDegreeLat * Math.Cos(initialUserLoc.lat * Math.PI / 180.0)) * gpsScale;


            Vector3 offset = new Vector3((float)offsetX, 0, (float)offsetZ);
            Vector3 planetPosition = origin + offset;

            if (obj.prefab != null)
            {
                GameObject planetObj = Instantiate(obj.prefab, planetPosition, Quaternion.identity);
                instantiatedObjects[obj.objectName] = planetObj;
                debugTxt.text += $"\n✅ {obj.objectName} instanciée à {planetPosition} (offset: {offset})";
                Debug.Log($"{obj.objectName} instanciée à {planetPosition} (offset: {offset})");
            }
            else
            {
                debugTxt.text += $"\n❌ Préfab manquant pour {obj.objectName}";
                Debug.LogError($"❌ Préfab manquant pour {obj.objectName}");
            }
        }
    }

    // Chargement du JSON depuis StreamingAssets
    private IEnumerator LoadPlanetsFromJSON()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
        debugTxt.text += $"🔍 Chemin du JSON : {fullPath}\n";
#if UNITY_ANDROID
        using (UnityWebRequest www = UnityWebRequest.Get(fullPath))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                debugTxt.text += $"❌ Erreur lors du chargement du JSON : {www.error}\n";
                Debug.LogError("Erreur JSON : " + www.error);
            }
            else
            {
                ProcessJson(www.downloadHandler.text);
            }
        }
#else
        if (!File.Exists(fullPath))
        {
            debugTxt.text += $"❌ Fichier JSON introuvable : {fullPath}\n";
            Debug.LogError("Fichier JSON introuvable : " + fullPath);
        }
        else
        {
            string jsonContent = File.ReadAllText(fullPath);
            ProcessJson(jsonContent);
        }
        yield return null;
#endif
    }

    // Traitement du contenu JSON pour remplir la liste des planètes
    private void ProcessJson(string jsonContent)
    {
        debugTxt.text += $"📝 JSON chargé : {jsonContent.Substring(0, Math.Min(jsonContent.Length, 200))}...\n";
        PlanetData planetData = JsonUtility.FromJson<PlanetData>(jsonContent);
        if (planetData == null || planetData.planets == null)
        {
            debugTxt.text += "❌ JSON mal formé ou vide !\n";
            return;
        }
        objects.Clear();
        foreach (PlanetInfo planet in planetData.planets)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + planet.prefabName);
            if (prefab == null)
            {
                debugTxt.text += $"❌ Préfab '{planet.prefabName}' non trouvé !\n";
                continue;
            }
            GPSObject gpsObject = new GPSObject(planet.name, planet.latitude, planet.longitude, prefab);
            objects.Add(gpsObject);
            debugTxt.text += $"✅ Planète ajoutée : {planet.name} (Lat: {planet.latitude:F6}, Lon: {planet.longitude:F6})\n";
        }
    }
}

// Structure JSON pour désérialisation
[Serializable]
public class PlanetData
{
    public List<PlanetInfo> planets;
}

[Serializable]
public class PlanetInfo
{
    public string name;
    public string prefabName;
    public double latitude;
    public double longitude;
    public string uniqueId;
}

// Classe pour stocker les données GPS
[Serializable]
public class GPSLoc
{
    public double lat;
    public double lon;
    public GPSLoc() { lon = 0; lat = 0; }
    public GPSLoc(double lon, double lat) { this.lon = lon; this.lat = lat; }
    public override string ToString() => $"Lat: {lat:F3}, Lon: {lon:F3}";
}

// Classe pour stocker les informations des objets (planètes)
[Serializable]
public class GPSObject
{
    public string objectName;
    public GPSLoc position;
    public GameObject prefab;
    public GPSObject(string name, double lat, double lon, GameObject prefab)
    {
        this.objectName = name;
        // Remarque : ici, on construit la position avec (lon, lat)
        this.position = new GPSLoc(lon, lat);
        this.prefab = prefab;
    }
}
