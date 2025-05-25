using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    private GameObject loadingVisual;
    private TextMeshProUGUI progressText;
    private string sceneName;
    public string dataSceneName;
    private GameObject pausePanel;
    private bool isPaused = false;

    public static LoadSceneManager Instance;

    private void Awake()
    {
        Instance = this;


    }

    void Start()
    {
        
    }

    void Update() // need to be Fix
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
#if !UNITY_WEBGL
                Application.Quit();
#endif
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void PauseClick()
    {
        if (!isPaused)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            if (pausePanel == null)
            {
                GameObject prefab = Resources.Load<GameObject>("prefabs/PausePanel");
                if (prefab != null)
                {
                    pausePanel = Instantiate(prefab);
                }
            }
            else
            {
                pausePanel.SetActive(true);
            }
        }
        else
        {
            if (pausePanel != null)
                pausePanel.SetActive(false);
        }
    }

    public void LoadScenePrefab()
    {
        StartCoroutine(LoadSceneObject(sceneName));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneObject(sceneName));
    }

    public void forceLoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ConversationScene")
        {
            // Assign button normally
            Button skipButton = GameObject.Find("SkipButton")?.GetComponent<Button>();
            if (skipButton != null)
            {
                skipButton.onClick.RemoveAllListeners();
                skipButton.onClick.AddListener(() => LoadScene(dataSceneName));
            }
        }
        else
        {
            /* 🔥 Try destroy any leftover Title UI (safety net)
            GameObject titleCanvas = GameObject.Find("TitleCanvas");
            if (titleCanvas != null)
            {
                Destroy(titleCanvas);
            }*/
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove listener
    }



    public IEnumerator LoadSceneObject(string sceneName)
    {
        GameObject loadingPrefab = Resources.Load<GameObject>("prefabs/loading");
        GameObject loadingVisual = null;

        if (loadingPrefab != null)
        {
            loadingVisual = Instantiate(loadingPrefab);
        }

        Debug.Log("Start loading scene: " + sceneName);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        while (async.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            Debug.Log($"Loading Progress: {progress * 100f:F0}%");
            yield return null;
        }

        // ✅ ถึง 90% แล้วรออีกนิดก่อน activate
        Debug.Log("Scene loaded to 90%. Waiting for activation...");
        yield return new WaitForSeconds(0.5f);
        async.allowSceneActivation = true;

        // ✅ รอ scene เปลี่ยนจริง
        while (!async.isDone)
        {
            yield return null;
        }

        Debug.Log("🎉 Scene Activated!");

        if (sceneName != "Title")
        {
            GameObject titleCanvas = GameObject.Find("TitleCanvas");
            if (titleCanvas != null)
            {
                Destroy(titleCanvas);
            }
        }

        if (loadingVisual != null)
            Destroy(loadingVisual);
    }
}
