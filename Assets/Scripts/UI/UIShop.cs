using FreeWorld;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public struct Param
    {
        public List<Item> Items;
    }

    [System.Serializable]
    public struct Tab
    {
        public Toggle toggle;
        public ItemType type;
    }

    [SerializeField]
    private GameObject itemObj;
    [SerializeField]
    private Transform leftContent;
    [SerializeField]
    private GameObject detailObj;
    [SerializeField]
    private Image thumbnail;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private TMP_Text amountText;
    [SerializeField]
    private Button clickButton;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Tab[] tabs;

    private ItemType currentType;
    private List<Item> items;

    private void Awake()
    {
        MessagingCenter.Subscribe<InventoryController, List<Item>>(this, InventoryController.MessageOnUpdateItem, (_, items) => OnUpdateMarket(items));
    }

    private void Start()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].toggle.onValueChanged.AddListener(_ => OnSelectTab());
        }
    }

    private void OnSelectTab()
    {
        foreach (var tab in tabs)
        {
            if (tab.toggle.isOn)
            {
                currentType = tab.type;
                break;
            }
        }
        UpdateUI();
        detailObj.SetActive(false);
    }

    private void OnUpdateMarket(List<Item> items)
    {
        this.items = items;
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform item in leftContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in items)
        {
            if (item.Type != currentType) continue;

            var obj = Instantiate(itemObj, leftContent);
            Utility.LoadItemSprite(item.Id, sprite =>
            {
                if (obj != null && sprite != null)
                {
                    if (obj.TryGetComponent(out Image image))
                    {
                        image.sprite = sprite;
                    }
                    if (obj.TryGetComponent(out Button button)) button.onClick.AddListener(() => ShowDetail(item, sprite));
                    obj.SetActive(true);
                }
            });
        }
    }

    private void ShowDetail(Item item, Sprite sprite)
    {
        nameText.text = item.Name;
        thumbnail.sprite = sprite;

        if (item.Type == ItemType.Consumable)
        {
            amountText.enabled = true;
            amountText.text = item.Quantity.ToString();
        }
        else
        {
            amountText.enabled = false;
        }

        descriptionText.text = item.Description;
        buyButton.onClick.RemoveAllListeners();


        if (item.Type == ItemType.Consumable)
        {
            buyButton.gameObject.SetActive(true);
            buyButton.onClick.AddListener(() =>
            {

            });
        }
        else
        {

        }
        detailObj.SetActive(true);
    }

    void OnDestroy()
    {
        MessagingCenter.Unsubscribe<InventoryController, List<Item>>(this, InventoryController.MessageOnUpdateItem);
    }
}
