using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("Main UI Elements")]
    public GameObject languageButton;
    public GameObject soundButton;
    public GameObject creditsButton;
    public GameObject gotomenuButton;

    [Header("Language Panel")]
    public GameObject panelLanguage;   // Panel contenant les boutons de langue

    [Header("Language Panel Buttons")]
    public Button frenchButton;        // Bouton "French"
    public Button dutchButton;         // Bouton "Dutch"
    public Button englishButton;       // Bouton "English"
    public Button backButton;          // Bouton "Back"

    void Start()
    {
        // Assurez-vous que le panel de langue est désactivé au démarrage
        panelLanguage.SetActive(false);

        // Ajout des listeners
        languageButton.GetComponent<Button>().onClick.AddListener(ShowLanguagePanel);
        backButton.onClick.AddListener(HideLanguagePanel);

        // Gestion des langues
        frenchButton.onClick.AddListener(() => SelectLanguage("French"));
        dutchButton.onClick.AddListener(() => SelectLanguage("Dutch"));
        englishButton.onClick.AddListener(() => SelectLanguage("English"));
    }

    // Affiche le panel des langues et cache les boutons principaux
    void ShowLanguagePanel()
    {
        panelLanguage.SetActive(true);

        languageButton.SetActive(false);
        soundButton.SetActive(false);
        creditsButton.SetActive(false);
        gotomenuButton.SetActive(false);
    }

    // Cache le panel des langues et réaffiche les boutons principaux
    void HideLanguagePanel()
    {
        panelLanguage.SetActive(false);

        languageButton.SetActive(true);
        soundButton.SetActive(true);
        creditsButton.SetActive(true);
        gotomenuButton.SetActive(true);
    }

    // Gestion de la sélection de la langue
    void SelectLanguage(string language)
    {
        Debug.Log($"Langue sélectionnée : {language}");

        switch (language)
        {
            case "French":
                Debug.Log("Langue changée en Français.");
                break;

            case "Dutch":
                Debug.Log("Langue changée en Néerlandais.");
                break;

            case "English":
                Debug.Log("Langue changée en Anglais.");
                break;
        }

        // Retour aux boutons principaux après la sélection
        HideLanguagePanel();
    }
}
