using System.Collections;
using TMPro;
using UnityEngine;

namespace TurnBase
{
    public class QTE : Singleton<QTE>
    {
        [Header("Ref")]
        public Transform startingPoint;
        public Transform endPoint;
        public RectTransform greenZone;
        public RectTransform mediumZone;
        public RectTransform pointerTransform;
        public GameObject QTEObject;

        [Header("Setting")]
        public float moveSpeed;
        private float direction;
        private Vector3 targetPosition;


        private void Update()
        {
            if (pointerTransform.position.x > endPoint.position.x + 0.01f)
            {
                Vector3 currentPosition = pointerTransform.position;
                float newX = Mathf.MoveTowards(currentPosition.x, endPoint.position.x, moveSpeed * Time.deltaTime);
                pointerTransform.position = new Vector3(newX, currentPosition.y, currentPosition.z);
            }
            else
            {
                ForceEndQTE();
                Debug.Log("Reached the end");
                pointerTransform.position = new Vector3(endPoint.position.x, endPoint.position.y, endPoint.position.z);
            }

            GetInput();
        }

        public void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CheckPressThePoint();
            }
        }

        public void ForceEndQTE()
        {
            moveSpeed = 0;
            StartCoroutine(DestroySelf());
        }

        IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(this.gameObject);
        }

        public void CheckPressThePoint()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(greenZone, pointerTransform.position)) // dodge the damage | Dodge
            {
                Debug.Log("Dodge");
                ForceEndQTE();
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(mediumZone, pointerTransform.position)) // Take damage but less | Def
            {
                Debug.Log("Def");
                ForceEndQTE();
            }
            else // Take the damage
            {
                Debug.Log("Take Damage");
                ForceEndQTE();
            }
        }

        private void OnEnable()
        {
            pointerTransform.position = startingPoint.position;
            targetPosition = endPoint.position;

            direction = 1;
            Debug.Log("QTE Start");
        }

        
    }
}


