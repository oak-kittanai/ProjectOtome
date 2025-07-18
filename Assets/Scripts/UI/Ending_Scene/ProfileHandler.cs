using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileHandler : MonoBehaviour
{
    [SerializeField] string nameCharacter;
    [SerializeField] Image imageSprite;
    [SerializeField] Button selectButton;

    [Header("Setting")]
    [SerializeField] GameObject lockedImage;
    [SerializeField] bool isLocked;
    [SerializeField] bool isAvailable;

    private void Start()
    {
        imageSprite = imageSprite.GetComponent<Image>();
    }

    private void Update()
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
                OnButtonClick();
            }

            selectButton.GetComponentInChildren<TextMeshProUGUI>().text = nameCharacter;
        }else
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

    void OnButtonClick() // Add to show all cut scene when click
    {
        selectButton.GetComponent<Button>().onClick.AddListener(() =>
        {

        });
    }
}
