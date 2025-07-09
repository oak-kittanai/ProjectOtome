using System;
using UnityEngine;
using UnityEngine.WSA;

public class NpcTrigger : MonoBehaviour
{
    [SerializeField] Collider coll;

    [Header("Path Setting")]
    public string path;
    public string folderPath;

    [Header("Setting")]
    public bool isPlayerInRange;
    public bool isQuest;
    [SerializeField] RuntimeAnimatorController controller;

    [Header("Child Set")]
    public Animator animator;

    void Start()
    {
        if (isQuest)
        {
            coll = GetComponentInChildren<Collider>();
        }

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        JustStand(0);
        CheckPlayer();

        if (animator.runtimeAnimatorController == null)
        {
            animator.runtimeAnimatorController = controller;
        }
    }

    void CheckPlayer()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            print("Press E");
            GetPath();
        }
    }

    void GetPath()
    {
        print("Get Path Press");
        print(path + " " + folderPath);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("invalid dialogue id");
            return;
        }

        if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(folderPath))
        {
            UIManager.Instance.ShowDialogue(new()
            {
                FolderID = folderPath,
                DialogueID = path
            });
        }

        if (string.IsNullOrEmpty(folderPath) && !string.IsNullOrEmpty(path))
        {
            UIManager.Instance.ShowDialogue(new()
            {
                DialogueID = path
            });
            Debug.Log("open dialogue " + path);
            return;
        }
    }

    void JustStand(float setfloat)
    {
        animator.SetFloat("X", setfloat);
        animator.SetFloat("Y", setfloat);
    }

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        if (isQuest)
        {
            if (other.tag == "Player")
            {
                isPlayerInRange = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (isQuest)
        {
            if (other.tag == "Player")
            {
                isPlayerInRange = false;
            }
        }
    }

    #endregion
}
