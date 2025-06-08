using UnityEngine;

namespace FreeWorld
{
    public class CharacterController : Singleton<CharacterController>
    {
        public float speed = 5f;
        public float usedRunStamina;

        public float runSpeed;
        public bool isRunning;

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

        /*private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }*/
    }
}

