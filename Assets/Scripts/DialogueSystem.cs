using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public struct Param
    {
        public string FolderID;
        public string DialogueID;
    }

    [Header("Pre & Post")]
    public Image backgroundImage;
    public Image mainImage;
    public Image otherImage;
    public Image soloMainImage;
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
    public string folderData;
    public string eventDialogueData;

    public bool changeLanguage; // true = Th || false = En

    [Header("Setting")]

    public bool isBackgroundActive; // true = Background || false = No Background
    public bool isMiniDialogue; // true = MiniDialogue || false = Dialogue

    private TextAsset json;

    public void Setup(Param param)
    {
        folderData = param.FolderID;
        eventDialogueData = param.DialogueID;

        Debug.Log(folderData);
        Debug.Log(eventDialogueData);

        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        typingSound = Resources.Load<AudioClip>("sounds/typing"); // add typing effect

        CheckLauguage(changeLanguage);

        string fileData;
        if (string.IsNullOrEmpty(folderData))
        {
            fileData = $"data/{eventDialogueData}";
            json = Resources.Load<TextAsset>(fileData);
        }
        else
        {
            fileData = $"data/{folderData}/{eventDialogueData}";
            json = Resources.Load<TextAsset>(fileData);
        }

        if (json == null)
        {
            if (string.IsNullOrEmpty(folderData))
            {
                Debug.LogError("Dialogue JSON not found: " + eventDialogueData);
            }
            else
            {
                Debug.LogError("Dialogue JSON not found: " + eventDialogueData + " or Folder not found: " + folderData);
            }
                
            DialogueEnd();
            return;
        }
        LoadEventDialogueData(0);
        typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));
    }

    void CheckLauguage(bool getJsonLanguageBool)
    {
        if (getJsonLanguageBool)
        {

        }
        else
        {

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

    void LoadEventDialogueData(int rootindex)
    {
        var rootArray = JSON.Parse(json.text).AsArray;

        int targetID = rootindex;

        foreach (JSONNode node in rootArray)
        {
            if (node["id"].AsInt == targetID) // need to fix
            {
                storyArray = node["dialogues"].AsArray;
                Debug.Log("Current ID is : " + node["id"]);
                break;
            }

            //if (node["id"].AsInt >= )
        }

        Debug.Log(storyArray);
    }

    public void SkipToChoice()
    {
        ShowChoices();
    }

    IEnumerator PlayDialogue(int index)
    {
        LoadDataCharacter(index);

        isTyping = true;

        if (index >= storyArray.Count)
        {
            DialogueEnd();
            yield break;
        }

        var node = storyArray[index];

        string mainSpeaker = node["speaker"];
        string currentSpeaker = mainSpeaker;

        string text = node["text_en"];

        text = changeLanguage ? node["text_th"] : node["text_en"];
        currentFullText = text;

        Debug.Log(text);

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

        characterNameText.text = currentSpeaker; // ขึ้นชื่อของคนที่พูดอยู่ !!!รอแก้
        dialogueText.text = "";

        if (text != null)
        {
            foreach (char c in text)
            {
                dialogueText.text += c;
                if (typingSound != null)
                    audioSource.PlayOneShot(typingSound);
                yield return new WaitForSeconds(0.05f);
            }
        }

        isTyping = false;
    }

    private void LoadDataCharacter(int index)
    {
        var node = storyArray[index];

        string mainSpeaker = node["speaker"];
        string speakerEmotional = node["emotional"];

        string leftCharacter = node["left"];
        string rightCharacter = node["right"];

        string background = node["background"];

        string bgPath = ($"backgrounds/ {background}");
        string mainSpeakerEmotional = ($"characters/SpriteCharacter/{mainSpeaker}/char_{mainSpeaker}_{speakerEmotional}");
        string leftCharacterSprite = ($"characters/SpriteCharacter/{leftCharacter}/listener");
        string rightCharacterSprite = ($"characters/SpriteCharacter/{rightCharacter}/listener");

        if (leftCharacter == rightCharacter && rightCharacter == mainSpeaker)
        {
            print("P and P and P");
            mainImage.gameObject.SetActive(false);
            otherImage.gameObject.SetActive(false);
            soloMainImage.gameObject.SetActive(true);
            soloMainImage.sprite = LoadSpriteFromPath(mainSpeakerEmotional);
            print(mainSpeakerEmotional);
        }
        else if (leftCharacter == rightCharacter && rightCharacter != mainSpeaker)
        {
            print("in site");
            mainImage.gameObject.SetActive(false);
            otherImage.gameObject.SetActive(false);
            soloMainImage.gameObject.SetActive(false);
        }
        else
        {
            print("Nomal Talk");
            mainImage.gameObject.SetActive(true);
            otherImage.gameObject.SetActive(true);
            soloMainImage.gameObject.SetActive(false);
            mainImage.sprite = mainSpeaker == leftCharacter ? LoadSpriteFromPath(mainSpeakerEmotional) : LoadSpriteFromPath(leftCharacterSprite);
            otherImage.sprite = mainSpeaker == rightCharacter ? LoadSpriteFromPath(mainSpeakerEmotional) : LoadSpriteFromPath(rightCharacterSprite);
        }

        backgroundImage.sprite = isBackgroundActive ? LoadSpriteFromPath(bgPath) : null;
        backgroundImage.gameObject.SetActive(false);
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
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                ApplyImpact(choice["impact"].AsObject);

                if (choice["next"] != null)
                {
                    int nextDialogueID = choice["next"].AsInt;

                    LoadEventDialogueData(nextDialogueID);
                    currentIndex = 0;
                    typingCoroutine = StartCoroutine(PlayDialogue(currentIndex));

                    hasChoice = false;
                    btnObj.GetComponent<Button>().onClick.RemoveAllListeners();
                }

                choicePanel.SetActive(false);
                dialoguePanel.SetActive(true);
                Debug.Log("Choice selected and stats updated");
            });
        }
    }

    void ApplyImpact(JSONNode impactNode)
    {
        foreach (KeyValuePair<string, JSONNode> stat in impactNode.AsObject) // อัพเดตค่าสถานะ ต่างๆ ต้องแก้เพิ่ม
        {

        }

        PlayerPrefs.Save();
    }

    public void DialogueEnd()
    {
        Destroy(gameObject);
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
}
