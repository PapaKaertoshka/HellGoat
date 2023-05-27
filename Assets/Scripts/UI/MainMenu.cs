using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;

    [SerializeField] private Button _continueButton;


    private bool _isMusicPlayed = false;

    private void Awake()
    {
        if (IsSaveEmpty())
        {
            _continueButton.interactable = false;
        }
        
    }
    
    public void NewRise()
    {
        _levelLoader.LoadLevel("Scenes/MainScene");
    }
    
    public void Continue()
    {
        _levelLoader.LoadLevel("Scenes/MainScene");
    }
    
    public void Settings()
    {
        //_settingsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    private bool IsSaveEmpty()
    {
        return false;
    }
}