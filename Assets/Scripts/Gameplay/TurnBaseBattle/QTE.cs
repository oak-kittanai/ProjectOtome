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
        private Vector3 targetPosition;

        public string returnVa;

        string StatusReturn = null;

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
                ForceEndQTE("Take Damage");
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

        public void ForceEndQTE(string s)
        {
            moveSpeed = 0;
            BattleBaseSystem.Instance.StatusReturn = s;
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
                Debug.Log("QTE Dodge");
                StatusReturn = "Dodge";
                ForceEndQTE(StatusReturn);
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(mediumZone, pointerTransform.position)) // Take damage but less | Def
            {
                Debug.Log("QTE Def");
                StatusReturn = "Def";
                ForceEndQTE(StatusReturn);
            }
            else // Take the damage
            {
                Debug.Log("QTE Take Damage");
                StatusReturn = "Dmg";
                ForceEndQTE(StatusReturn);
            }

            Debug.Log("QTE Return : " + StatusReturn);
        }

        private void OnEnable()
        {
            pointerTransform.position = startingPoint.position;
            targetPosition = endPoint.position;

            Debug.Log("QTE Start");
        }

        
    }
}


