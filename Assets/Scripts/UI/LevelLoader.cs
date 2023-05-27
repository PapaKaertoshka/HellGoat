using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _progressText;
    
    private void Awake()
    {
        _loadingScreen.SetActive(false);
    }

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadAsynchronously(name));
    }

    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        _loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            _slider.value = progress;
            _progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}