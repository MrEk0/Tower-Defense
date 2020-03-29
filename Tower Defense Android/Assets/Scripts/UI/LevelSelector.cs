
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


    public class LevelSelector : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform levelPanel;
        [SerializeField] float swipeThreshold = 0.2f;
        [SerializeField] float smoothSwipeTime = 0.5f;

        private Vector3 panelPosition;
        private int currentPage = 1;

        public float NumberOfPages { get; set; }

        private void Start()
        {
            panelPosition = transform.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float offset = eventData.pressPosition.x - eventData.position.x;
            transform.localPosition = panelPosition - new Vector3(offset, 0f, 0f);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //AudioManager.PlayUISwipeAudio();

            float touchDifference = (eventData.pressPosition.x - eventData.position.x) / levelPanel.rect.width;
            if (Mathf.Abs(touchDifference) >= swipeThreshold)
            {
                Vector3 newPosition = panelPosition;
                if (touchDifference > 0 && currentPage < NumberOfPages)
                {
                    currentPage++;
                    newPosition += new Vector3(-levelPanel.rect.width, 0f, 0f);
                }
                else if (touchDifference < 0 && currentPage > 1)
                {
                    currentPage--;
                    newPosition += new Vector3(levelPanel.rect.width, 0f, 0f);
                }

                StartCoroutine(SmoothMovement(transform.localPosition, newPosition, smoothSwipeTime));
                panelPosition = newPosition;
            }
            else
            {
                StartCoroutine(SmoothMovement(transform.localPosition, panelPosition, smoothSwipeTime));
            }
        }

        private IEnumerator SmoothMovement(Vector3 startPosition, Vector3 targetPosition, float time)
        {
            float t = 0f;
            while (t <= 1)
            {
                t += Time.deltaTime / time;
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
        }
    }

