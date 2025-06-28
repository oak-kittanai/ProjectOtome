using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float walkDelay;
    public Transform[] WalkPoint;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public float walkSpeed;

    private bool reachFinalPoint;
    private int pointIndex;

    private bool continueWalk;
    private bool stopWalk;

    [Header("Sprite Setting")]
    public string spriteName;
    [SerializeField] RuntimeAnimatorController controller;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SkinPath();
    }

    private void Update()
    {
        ControlMovement();
    }

    void ControlMovement()
    {
        float speed = walkSpeed * Time.deltaTime;

        if (pointIndex == WalkPoint.Length)
        {
            reachFinalPoint = true;
        }

        //transform.position = Vector2.MoveTowards
        /*if (Vector3.Distance(transform.position, WalkPoint[pointIndex].position) >= 0.01)
        {
            if (continueWalk)
            {
                Vector3.MoveTowards(transform.position, WalkPoint[pointIndex].position, speed);
                if (Vector3.Distance(transform.position, WalkPoint[pointIndex].position) <= 0.01)
                {
                    continueWalk = false;
                    StartCoroutine(WalkDelay());
                }
            }
        }*/
    }

    void SkinPath()
    {
        if (string.IsNullOrEmpty(spriteName))
        {
            controller = Resources.Load<RuntimeAnimatorController>($"characters/MainChar/Assests/Nomal/{spriteName}");
        }else
        {
            Debug.LogWarning(this.gameObject.name + "Can't find Path or NullOrEmpty");
        }
    }

    IEnumerator WalkDelay()
    {
        yield return new WaitForSeconds(walkDelay);
        if (reachFinalPoint)
        {
            pointIndex--;
            stopWalk = false;
            continueWalk = true;
        }
        else
        {
            pointIndex++;
            stopWalk = false;
            continueWalk = true;
        }
    }
}
