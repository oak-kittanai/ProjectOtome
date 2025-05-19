using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class DialogueSystem : Singleton<DialogueSystem>
{
    [Header("Pre & Post")]
    public Image backgroundImage;
    public Image mainImage;
    public Image otherImage;
    public TextMeshProUGUI characterNameText; // make sure show only talk or show 2
    public TextMeshProUGUI dialogueText;
    public GameObject choicePanel, dialoguePanel;
    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;

    private JSONArray storyArray;
    private JSONArray choiceArray;
    public int currentIndex = 0;
    private bool isTyping = false;
    private bool isSkipAble;
    public bool hasChoice;
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
        /*//province = PlayerPrefs.GetString(Const.PROVINCE, Const.BANGKOK);
        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        typingSound = Resources.Load<AudioClip>("sounds/typing");
        LoadFont();

        if (Game.Instance.currentLanguage == "TH")
        {
            changeLanguage = true;
        }
        else
        {
            changeLanguage = false;
        }

        CheckLauguage(changeLanguage);
        LoadEventDialogueData();
        typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));*/
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
        json = Resources.Load<TextAsset>("dialogues/" + eventDialogueData);

        //var root = JSON.Parse(json.text)[0];
        var rootArray = JSON.Parse(json.text).AsArray;

        storyArray = rootArray;

        /*storyArray = root["levels"][0]["story"].AsArray;
        choiceArray = root["stat"][0]["choices"].AsArray;*/
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
            Debug.Log("Dialogue ended.");
            yield break;
        }

        var node = storyArray[index];
        Debug.Log(node);

        string mainSpeaker = node["speaker"];
        string currentSpeaker = mainSpeaker;

        string speakerEmotional = node["emotional"];

        string leftCharacter = node["left"];
        string rightCharacter = node["right"];

        string background = node["background"];

        string text = node["text_en"];

        text = changeLanguage ? node["text_th"] : node["text_en"];
        currentFullText = text;

        isSkipAble = true;

        var choiceNode = node["choice"];
        if (choiceNode != null && choiceNode is JSONArray)
        {
            choiceArray = choiceNode.AsArray;
            ShowChoices();
            isSkipAble = false;
            hasChoice = true;
        }
        else
        {
            isSkipAble = true;
            hasChoice = false;
        }

        /*string bgPath = ($"Resources / backgrounds / {background}");
        string mainSpeakerEmotional = ($"Resources / characters / {mainSpeaker} / {speakerEmotional}");
        string leftCharacterSprite = ($"Resources / characters / {leftCharacter} / listener");
        string rightCharacterSprite = ($"Resources / characters / {rightCharacter} / listener");

        mainImage.sprite = mainSpeaker == leftCharacter ? LoadSpriteFromPath(mainSpeakerEmotional) : LoadSpriteFromPath(leftCharacterSprite);
        otherImage.sprite = mainSpeaker == rightCharacter ? LoadSpriteFromPath(mainSpeakerEmotional) : LoadSpriteFromPath(rightCharacterSprite);


        thFont = Resources.Load<TMP_FontAsset>("Fonts/Kanit/Kanit-Regular");

        backgroundImage.sprite = isBackgroundActive ? LoadSpriteFromPath(bgPath) : null;*/

        //characterNameText.text = currentSpeaker; // ขึ้นชื่อของคนที่พูดอยู่ !!!รอแก้
        dialogueText.text = "";

        if (text != null)
        {
            foreach (char c in text)
            {
                dialogueText.text += c;
                if (typingSound != null)
                    audioSource.PlayOneShot (typingSound);
                yield return new WaitForSeconds(0.05f);
            }
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
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = changeLanguage ? choice["text_th"] : choice["text_en"];

            btnObj.GetComponent<Button>().onClick.AddListener(() => // Choice select impact
            {
                ApplyImpact(choice["impact"].AsObject);

                if (choice["next"] != null)
                {

                    string nextDialogueIDString = choice["next"].ToString();
                    int nextDialogueID = int.Parse(nextDialogueIDString);
                    typingCoroutine = StartCoroutine(PlayDialogue(nextDialogueID));
                    btnObj.GetComponent<Button>().onClick.RemoveAllListeners();

                    hasChoice = false;
                }
                choicePanel.SetActive(false);
                dialoguePanel.SetActive(true);
                Debug.Log("Choice selected and stats updated");
                /*
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
        if (Input.GetKeyDown(KeyCode.Space) && isSkipAble || Input.GetMouseButtonDown(0) && isSkipAble)
        {
            NextPhase();
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
            if (!hasChoice)
            {
                currentIndex++;
                typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));
            }
        }
    }

    public void ForcePlayDialogue(string name)
    {
        /*eventDialogueData = name;
        LoadEventDialogueData();
        typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));*/

        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        typingSound = Resources.Load<AudioClip>("sounds/typing");
        LoadFont();

        CheckLauguage(changeLanguage);
        LoadEventDialogueData();
        typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));
    }
}
