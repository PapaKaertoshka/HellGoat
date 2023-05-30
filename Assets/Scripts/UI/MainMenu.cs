using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _continueButton;

    [SerializeField] private GameObject _settingsMenu;
    
    [SerializeField] private GameObject _loadingScreen;
    
    private void Awake()
    {
        if (IsSaveEmpty())
        {
            _continueButton.interactable = false;
        }
        
    }
    
    public void NewRise()
    {
        _loadingScreen.SetActive(true);
        _loadingScreen.GetComponent<LevelLoader>().LoadLevel("Scenes/MainScene");
        gameObject.SetActive(false);
    }
    
    public void Continue()
    {
        _loadingScreen.SetActive(true);
        _loadingScreen.GetComponent<LevelLoader>().LoadLevel("Scenes/MainScene");
        gameObject.SetActive(false);
    }
    
    public void Settings()
    {
        _settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    private bool IsSaveEmpty()
    {
        return true;
    }
}