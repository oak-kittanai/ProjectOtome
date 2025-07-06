using UnityEngine;

public class NpcTrigger : MonoBehaviour
{
    [SerializeField] Collider coll;

    [Header("Setting")]
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

        if (animator.runtimeAnimatorController == null)
        {
            animator.runtimeAnimatorController = controller;
        }
    }

    void JustStand(float setfloat)
    {
        animator.SetFloat("X", setfloat);
        animator.SetFloat("Y", setfloat);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isQuest)
        {
            if (other.tag == "Player")
            {
                // Need to add Dialogue
            }
        }
        
    }
}
