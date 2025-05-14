using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [Header("Pre & Post")]
    public bool isEnded = false;
    public Image backgroundImage;
    public Image mainImage;
    public Image otherImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject choicePanel, dialoguePanel;
    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;

    private JSONArray storyArray;
    private JSONArray choiceArray;
    private int currentIndex = 0;
    private bool isTyping = false;
    private string currentFullText = "";
    private Coroutine typingCoroutine;

    private AudioSource audioSource;
    private AudioClip typingSound;

    // Dialogue Data
    public string eventDialogueData;

    [Header("Language Setting")]
    private TMP_FontAsset enFont; // En Fonts
    private TMP_FontAsset thFont; // Th Fonts

    public bool changeLanguage; // true = Th || false = En
    private bool isEpisodeLoading = false;

    [Header("Setting")]

    public bool isBackgroundActive; // true = Background || false = No Background
    public bool isMiniDialogue; // true = MiniDialogue || false = Dialogue

    void Start()
    {
        //province = PlayerPrefs.GetString(Const.PROVINCE, Const.BANGKOK);
        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        typingSound = Resources.Load<AudioClip>("sounds/typing");
        LoadFont();

        /*if (Game.Instance.currentLanguage == "TH")
        {
            changeLanguage = true;
        }
        else
        {
            changeLanguage = false;
        }*/

        CheckLauguage(changeLanguage);
        LoadEventDialogueData();
        typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));
    }

    void LoadFont()
    {
        enFont = Resources.Load<TMP_FontAsset>("Fonts/");
        thFont = Resources.Load<TMP_FontAsset>("Fonts/");
    }

    void CheckLauguage(bool getJsonLanguageBool)
    {
        if (getJsonLanguageBool)
        {
            dialogueText.font = thFont;
            characterNameText.font = thFont;
        }
        else
        {
            dialogueText.font = enFont;
            characterNameText.font = enFont;
        }
    }

    Sprite LoadSpriteFromPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        Sprite loaded = Resources.Load<Sprite>(path);
        if (loaded == null)
        {
            Debug.LogWarning("Sprite not found at: " + path);
        }
        return loaded;
    }

    void LoadEventDialogueData()
    {
        TextAsset json;
        if (!isEnded)
        {
            json = Resources.Load<TextAsset>("data/th/" + eventDialogueData);
        }
        else
        {
            json = Resources.Load<TextAsset>("data/th/pre/" + eventDialogueData);
        }

        var root = JSON.Parse(json.text)[0]; // assume first node

        storyArray = root["levels"][0]["story"].AsArray;
        choiceArray = root["stat"][0]["choices"].AsArray;
    }

    public void SkipToChoice()
    {
        ShowChoices();
    }

    IEnumerator PlayDialogue(int index)
    {
        isTyping = true;

        if (index >= storyArray.Count)
        {
            Debug.Log("Dialogue ended. Showing choices...");
            ShowChoices();
            yield break;
        }

        var node = storyArray[index];

        string character = node["character"];
        string text = node["text"];

        if (changeLanguage)
        {
            character = node["character_th"];
            text = node["text_th"];
        }
        currentFullText = text;

        string bgPath = null; // Set เป็นพื้นหลังที่เป็นพื้นหลังใส
        if (isBackgroundActive)
        {
            bgPath = node["background"];
        }
        
        string mainPath = node["main"];
        string otherPath = node["other"];
        
        backgroundImage.sprite = LoadSpriteFromPath(bgPath); // Need to fix path
        mainImage.sprite = LoadSpriteFromPath(mainPath); // Need to fix path
        otherImage.sprite = LoadSpriteFromPath(otherPath); // Need to fix path

        characterNameText.text = character;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            if (typingSound != null)
                audioSource.PlayOneShot(typingSound);
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    void ShowChoices()
    {
        choicePanel.SetActive(true);
        dialoguePanel.SetActive(false);
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject); // clear old choices

        foreach (JSONNode choice in choiceArray)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice["text"];
            if (changeLanguage)
            {
                btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice["text_th"];
            }

            btnObj.GetComponent<Button>().onClick.AddListener(() => // Choice select impact
            {
                ApplyImpact(choice["impact"].AsObject);
                /*choicePanel.SetActive(false);
                Debug.Log("Choice selected and stats updated.");

                GameObject controller = GameObject.FindWithTag("GameController");*/
            });
        }
    }

    void ApplyImpact(JSONNode impactNode)
    {
        foreach (KeyValuePair<string, JSONNode> stat in impactNode.AsObject) // อัพเดตค่าสถานะ ต่างๆ ต้องแก้เพิ่ม
        {
            /*
            string key = stat.Key;
            int current = PlayerPrefs.GetInt(key, 0);
            int delta = stat.Value.AsInt;
            int updated = current + delta;

            PlayerPrefs.SetInt(key, updated);
            Debug.Log($"📊 Updated {key}: {current} → {updated}");*/
        }

        PlayerPrefs.Save();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentFullText;
                isTyping = false;
            }
            else
            {
                currentIndex++;
                typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));
            }
        }
    }

    public void NextPhase()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentFullText;
            isTyping = false;
        }
        else
        {
            currentIndex++;
            typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));
        }
    }
}
