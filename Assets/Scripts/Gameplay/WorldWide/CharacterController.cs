using UnityEngine;

namespace FreeWorld
{
    public class CharacterController : Singleton<CharacterController>
    {
        public float speed = 5f;
        public float usedRunStamina;

        public Animator animator;
        public SpriteRenderer spriteRenderer;

        public float runSpeed;
        public bool isRunning;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Update()
        {
            Move();
        }

        void Move()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (movement != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && !CharacterStats.Instance.isExhausted)
            {
                transform.Translate(movement * runSpeed * Time.deltaTime);
                CharacterStats.Instance.UseStamina(usedRunStamina, true);
                isRunning = true;
            }
            else
            {
                transform.Translate(movement * speed * Time.deltaTime);
                CharacterStats.Instance.UseStamina(0, false);
                isRunning = false;
            }

            UpdataAnimation(movement);

            Vector3 clampedPosition = transform.position;
            if (transform.position.z >= 2.5f)
            {
                clampedPosition.z = 2.5f;
                transform.position = clampedPosition;
            }

            if (transform.position.z <= -2.5f)
            {
                clampedPosition.z = -2.5f;
                transform.position = clampedPosition;
            }
        }

        public void UpdataAnimation(Vector3 direction)
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
    }
}

