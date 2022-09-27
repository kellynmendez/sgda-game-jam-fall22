using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAlphaText : MonoBehaviour
{
    bool coroutineCalled = false;
    // Start is called before the first frame update
    void Update()
    {
        if (!coroutineCalled)
        {
            coroutineCalled = true;
            CallBlinkText();
        }
    }

    public void CallBlinkText()
    {
        Debug.Log("calling blink text");
        StartCoroutine(BlinkText(gameObject));
    }

    public static IEnumerator BlinkText(GameObject text)
    {
        CanvasGroup group = text.GetComponent<CanvasGroup>();
        // initial value
        group.alpha = 1;

        bool startPressed = false;
        // animate value
        while (!startPressed)
        {
            group.alpha = 1;
            yield return new WaitForSeconds(0.6f);
            group.alpha = 0;
            yield return new WaitForSeconds(0.3f);
        }
    }

}
