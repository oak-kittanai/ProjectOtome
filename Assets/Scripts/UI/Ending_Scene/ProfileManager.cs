using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ProfileManager : UIClosable
{
    public struct Param
    {
        public List<CharacterProfileStats> characters;
    }

    [System.Serializable]
    public struct Tab
    {
        public Button button;
        public CharacterType type;
    }

    [SerializeField] Transform profileContainer;
    [SerializeField] GameObject obj;

    [SerializeField] Button closeButton;

    [SerializeField] List<Tab> tabs = new List<Tab>();
    [SerializeField] CharacterType currentType;

    [SerializeField] GameObject profileObj;

    private Database database;

    private void Start()
    {
        database = new Database();
        var allCharacters = database.GetCharacterStats();

        foreach (Transform child in profileContainer)
            Destroy(child.gameObject);

        print(allCharacters);

        foreach (var character in allCharacters)
        {
            print($"Name: {character.Name}");
            ProfileHandler.Param param = new ProfileHandler.Param()
            {
                name = character.Name,
                locked = character.islocked,
                available = character.isavailable,
                type = character.Type
            };

            ProfileHandlerSetup(param);
        }
        
    }

    public void Setup(Param param, Action onClose = null)
    {
        RegisterCleanup(onClose);
        closeButton.onClick.AddListener(Close);

        OnSelectTab();
    }

    private void OnSelectTab()
    {
        foreach (var tab in tabs)
        {
            bool isSelected = tab.type == currentType;
        }
    }
    public void ProfileHandlerSetup(ProfileHandler.Param param)
    {
        GameObject profileGO = Instantiate(obj, profileContainer);
        var accessButton = profileGO.GetComponentInChildren<Button>();
        profileGO.name = param.name;

        print(profileGO.name);

        var profilehandler = profileGO.GetComponent<ProfileHandler>();
        if (profilehandler != null) profilehandler.Setup(param);

        Tab newTab = new Tab
        {
            button = accessButton,
            type = param.type
        };

        tabs.Add(newTab);

        accessButton.onClick.AddListener(() =>
        {
            currentType = param.type;
            OnSelectTab();
        });
    }
}

public class CharacterProfileStats
{
    public string Name;
    public bool islocked;
    public bool isavailable;
    public CharacterType Type;

    public CharacterProfileStats(string name, bool locked, bool available, CharacterType type)
    {
        Name = name;
        islocked = locked;
        isavailable = available;
        Type = type;
    }
}

public enum CharacterType
{
    yak,
    ginnorn,
    phomor,
    payanak,
    karut
}
