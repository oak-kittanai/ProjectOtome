using System.Collections;
using System.IO;
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
    public bool isMen;
    public string spriteName;
    public string cityPath;
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

        if (controller == null)
        {
            SkinPath();
        }
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
        if (!string.IsNullOrEmpty(spriteName) && !string.IsNullOrEmpty(cityPath))
        {
            string path;
            if (isMen)
            {
                path = $"characters/MenNPC/{cityPath}/{spriteName}";
            }
            else
            {
                path = $"characters/GirlNPC/{cityPath}/{spriteName}";
            }

            controller = Resources.Load<RuntimeAnimatorController>(path);

            if (controller == null)
            {
                Debug.LogError($"Controller not found at path: {path}");
            }
            else
            {
                animator.runtimeAnimatorController = controller;
            }
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: spriteName or cityPath is missing.");
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
