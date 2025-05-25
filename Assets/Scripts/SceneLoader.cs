using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    public TextMeshProUGUI loadingText;
    [SerializeField]
    private Animator transitionAnimation;
    [SerializeField]
    private AnimationClip fadeInClip;
    [SerializeField]
    private AnimationClip fadeOutClip;

    private AsyncOperation asyncOperation;
    private Coroutine loadingCoroutine;
    private string fadeIn;
    private string fadeOut;

    void Start()
    {
        fadeIn = fadeInClip.name;
        fadeOut = fadeOutClip.name;
        loadingScreen.SetActive(false);
    }

    public void LoadScene(string sceneName, Action onComplete = null)
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }
        loadingCoroutine = StartCoroutine(LoadSceneAsync(sceneName, onComplete));
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onComplete)
    {
        Debug.Log("Loading scene: " + sceneName);
        loadingScreen.SetActive(true);

        if (transitionAnimation != null)
        {
            transitionAnimation.Play(fadeIn);
            yield return new WaitForSeconds(fadeInClip.length);
        }
     
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingText.text = $"Loading... {progress * 100f}%";

            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                if (transitionAnimation != null)
                {
                    transitionAnimation.Play(fadeOut);
                    yield return new WaitForSeconds(fadeOutClip.length);
                }             
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
        onComplete?.Invoke();
    }
}