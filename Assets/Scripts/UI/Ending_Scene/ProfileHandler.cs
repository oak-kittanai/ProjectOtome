using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileHandler : Singleton<ProfileHandler>
{
    public struct Param
    {
        public string name;
        public bool locked;
        public bool available;
        public CharacterType type;
    }

    [SerializeField] string nameCharacter;
    [SerializeField] Image imageSprite;
    [SerializeField] Button selectButton;
    [SerializeField] CharacterType characterType;

    [Header("Setting")]
    [SerializeField] GameObject lockedImage;
    [SerializeField] bool isLocked;
    [SerializeField] bool isAvailable;

    private void Start()
    {
        CheckAvailable();
    }

    public void Setup(Param param)
    {
        nameCharacter = param.name;
        isLocked = param.locked;
        isAvailable = param.available;
        characterType = param.type;

        if (!string.IsNullOrEmpty(nameCharacter))
        {
            /*string characterSprite = $"characters/SpriteCharacter/{nameCharacter}/{nameCharacter}_nomal/";
            imageSprite = Resources.Load<Image>(characterSprite);*/
        }

        CheckAvailable();
    }

    private void Update()
    {

    }

    void CheckAvailable()
    {
        if (isAvailable)
        {
            if (isLocked)
            {
                OnLocked();
            }
            else
            {
                imageSprite.color = new Color(1f, 1f, 1f, 1f);
                lockedImage.SetActive(false);
            }

            selectButton.GetComponentInChildren<TextMeshProUGUI>().text = nameCharacter;
        }
        else
        {
            imageSprite.color = new Color(0f, 0f, 0f, 1f);
            lockedImage.SetActive(false);
        }
    }

    void OnLocked()
    {
        imageSprite.color = new Color(1f, 1f, 1f, 0.705f);
        lockedImage.SetActive(true);
    }

}
