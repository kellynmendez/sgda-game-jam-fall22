using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] float _duration = 10f;
    RectTransform _transform;
    Vector3 from;
    Vector3 to;
    bool creditsDone = false;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        from = _transform.position;
        to = new Vector3(_transform.position.x, _transform.position.y + 2400, _transform.position.z);
        StartCoroutine(LerpPosition(_transform, from, to, _duration));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }
        if (creditsDone)
        {
            SceneManager.LoadScene(0);
        }
    }

    public IEnumerator LerpPosition(Transform target, Vector3 from, Vector3 to, float duration)
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
        creditsDone = true;
    }
}
