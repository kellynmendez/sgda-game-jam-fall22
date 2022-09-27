using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] float _duration = 10f;
    Transform _transform;
    Vector3 from;
    Vector3 to;
    bool creditsDone = false;

    private void Awake()
    {
        _transform = gameObject.transform.GetChild(0).transform;
        from = gameObject.transform.GetChild(1).position;
        to = gameObject.transform.GetChild(2).position;
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
