using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float walkDelay;
    public Transform[] WalkPoint;

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] NavMeshAgent agent;

    [Header("Ai Setting")]
    public float walkArea;
    public float walkSpeed;
    private bool reachFinalPoint;
    private int pointIndex;
    private Vector3 _selfOriginalDistance;
    public bool continueWalk;
    public bool stopWalk;

    private Vector3 lastPosition;

    [Header("Sprite Setting")]
    public bool isMen;
    public string spriteName;
    public string cityPath;
    [SerializeField] RuntimeAnimatorController controller;

    private void Start()
    {
        continueWalk = true;

        agent = GetComponentInChildren<NavMeshAgent>();
        GetComponentInChildren<NavMeshAgent>().updateRotation = false;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SkinPath();
    }

    private void Update()
    {
        Vector3 moveDirection = agent.desiredVelocity.normalized;

        ControlMovement();
        CheckDirection(moveDirection);

        lastPosition = transform.position;

        if (controller == null)
        {
            SkinPath();
        }
    }

    void ControlMovement()
    {
        if (pointIndex == WalkPoint.Length)
        {
            reachFinalPoint = true;
        }

        agent.speed = walkSpeed;
        RandomWalk();
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

    void RandomWalk()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            reachFinalPoint = true;
            if (reachFinalPoint)
            {
                StartCoroutine(WalkDelay());
                reachFinalPoint = false;
            }
        }


        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (continueWalk)
            {
                reachFinalPoint = false;
                if (RandomPoint(this.transform.position, walkArea, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                    point = _selfOriginalDistance;
                }
            }
        }
    }

    void CheckDirection(Vector3 direction)
    {
        animator.SetFloat("X", direction.x);
        if (direction.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }

        if (direction.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }


        animator.SetFloat("Y", direction.y);
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
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, walkArea);
    }
}
