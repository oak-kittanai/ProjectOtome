using FreeWorld;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIClosable
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
    private GameObject itemShopObj;
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
    private TMP_Text priceText;
    [SerializeField]
    private Button clickButton;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private int itemPrice;
    [SerializeField]
    private Tab[] tabs;

    private ItemType currentType;
    private List<Item> items;

    private void Awake()
    {
        MessagingCenter.Subscribe<ShopControl, List<Item>>(this, ShopControl.MessageOnUpdateItem, (_, items) => OnUpdateMarket(items));
    }

    private void Start()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].toggle.onValueChanged.AddListener(_ => OnSelectTab());
        }
    }

    public void Setup(Param param, Action onClose = null)
    {
        RegisterCleanup(onClose);
        closeButton.onClick.AddListener(Close);

        OnUpdateMarket(param.Items);
        OnSelectTab();
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

            var obj = Instantiate(itemShopObj, leftContent);
            Utility.LoadItemSprite(item.Id, sprite =>
            {
                if (obj != null && sprite != null)
                {
                    var itemImage = obj.transform.Find("ItemImage")?.GetComponent<Image>();
                    if (itemImage != null) itemImage.sprite = sprite;

                    if (obj.TryGetComponent(out Button button)) button.onClick.AddListener(() => ShowDetail(item, sprite));
                    obj.SetActive(true);
                }
            });
        }
    }

    private void ShowDetail(Item item, Sprite sprite)
    {
        nameText.text = item.Name;
        //thumbnail.sprite = sprite;

        /*if (item.Type == ItemType.Consumable)
        {
            amountText.enabled = true;
            amountText.text = item.Quantity.ToString(); //  Fix this
        }
        else
        {
            amountText.enabled = false;
        }*/


        if (itemPrice <= 0)
        {
            priceText.enabled = false;
        }
        
        amountText.enabled = false; // can only buy 1 for now

        descriptionText.text = item.Description;

        buyButton.onClick.RemoveAllListeners();
        clickButton.onClick.RemoveAllListeners();

        clickButton.onClick.AddListener(() =>
        {
            
            
        });

        buyButton.onClick.AddListener(() =>
        {
            if (item.Type == ItemType.Consumable)
            {

            }
            else if (item.Type == ItemType.Gift)
            {

            }
            else
            {

            }

            Debug.Log("Buy Item Id : " +  item.Id);

            Game.Instance.GetShop().BuyItem(item.Id, 1); // add Num
            Game.Instance.GetShop().RemoveItem(item.Id);
        });

        
        detailObj.SetActive(true);
    }

    void OnDestroy()
    {
        MessagingCenter.Unsubscribe<ShopControl, List<Item>>(this, ShopControl.MessageOnUpdateItem);
    }
}
