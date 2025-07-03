using UnityEngine;

public class NpcTrigger : MonoBehaviour
{
    [SerializeField] Collider coll;

    [Header("Setting")]
    [SerializeField] RuntimeAnimatorController controller;

    [Header("Child Set")]
    public Animator animator;

    void Start()
    {
        coll = GetComponentInChildren<Collider>();
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
        if (other.tag == "Player")
        {
            // Need to add Dialogue
        }
    }
}
