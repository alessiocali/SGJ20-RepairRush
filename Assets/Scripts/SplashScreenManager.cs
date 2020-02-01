using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    public GameObject[] m_ScreensToActivateInOrder;
    public float m_SecondForScreen = 2;

    private void Start()
    {
        foreach (GameObject go in m_ScreensToActivateInOrder)
        {
            go.SetActive(false);
        }
        StartCoroutine(SplashRoutine());
    }

    private IEnumerator SplashRoutine(int nextIndex = 0)
    {
        if (nextIndex > 0)
        {
            m_ScreensToActivateInOrder[nextIndex - 1].SetActive(false);
        }

        if (nextIndex < m_ScreensToActivateInOrder.Length)
        {
            m_ScreensToActivateInOrder[nextIndex].SetActive(true);
            yield return new WaitForSeconds(m_SecondForScreen);
            yield return SplashRoutine(nextIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("BasicArena");
        }
    }
}
