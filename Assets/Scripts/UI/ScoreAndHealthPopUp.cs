using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScoreAndHealthPopUp : MonoBehaviour
{
    const float MOVE_UP_DISTANCE = 50f;

    [SerializeField] GameObject _popupTextPrefab;
    [SerializeField] Canvas _canvas;
    [SerializeField] float _fadeTime = 1f;
    [SerializeField] float _scrollTime = 1.2f;

    // if score change flag set to true, display score change pop up
    // if score change flag set to false, display hp change pop up
    public void DisplayScoreUpdate(int increment, bool scoreChange)
    {
        GameObject textObj = Instantiate(_popupTextPrefab, _canvas.transform);
        // Setting text
        TMP_Text textComp = textObj.GetComponent<TMP_Text>();
        // Getting canvas group
        CanvasGroup group = textObj.GetComponent<CanvasGroup>();
        // Getting start position of text upward movement
        Vector3 textStartPosition = textObj.transform.position;

        if (scoreChange) // score increase
        {
            // Want score increase popups to be more to the bottom right than health decrease popup
            textStartPosition.x += 7;
            textStartPosition.y += -10;
            // Randomizing location of popup
            float offsetX = Random.Range(-20f, 20f);
            textStartPosition.x += offsetX;
            // Setting text and color
            textComp.text = "+" + increment.ToString();
            textComp.color = Color.green;
        }
        else // health decrease
        {
            // Randomizing location of popup
            float offsetX = Random.Range(-20f, 20f);
            textStartPosition.x += offsetX;
            // Setting text and color
            textComp.text = "-" + increment.ToString() + "hp";
            textComp.color = Color.red;
        }

        // Getting end position of text upward movement
        Vector3 textEndPosition = new Vector3(textStartPosition.x, textStartPosition.y + MOVE_UP_DISTANCE, textStartPosition.z);

        // Fading and moving text upwards
        StartCoroutine(LerpAlpha(group, 1, 0, _fadeTime));
        StartCoroutine(LerpPosition(textObj.transform, textStartPosition, textEndPosition, _scrollTime,
            () =>
            {
                Destroy(textObj);
            }));
    }

    public static IEnumerator LerpAlpha(CanvasGroup group, float from, float to, float duration, System.Action OnComplete = null)
    {
        // initial value
        group.alpha = from;

        // animate value
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // final value
        group.alpha = to;
        if (OnComplete != null) { OnComplete(); }
        yield break;
    }

    public static IEnumerator LerpPosition(Transform target, Vector3 from, Vector3 to, float duration, System.Action OnComplete = null)
    {
        // initial value
        target.position = from;

        // animate value
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            target.position = Vector3.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // final value
        target.position = to;
        if (OnComplete != null) { OnComplete(); }
        yield break;
    }
}