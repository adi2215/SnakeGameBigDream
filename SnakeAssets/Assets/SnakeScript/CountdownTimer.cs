using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public int countdownTimer = 3;
    public Text countdownText;
    public GameObject sankeController;

    private void Start() => StartCoroutine(CountdownStart());

    private IEnumerator CountdownStart()
    {
        sankeController.GetComponent<ControllerCharacter>().enabled = false;

        while(countdownTimer > 0)
        {
            countdownText.text = countdownTimer.ToString();

            yield return new WaitForSeconds(1f);

            countdownTimer--;
        }

        countdownText.text = "";

        yield return new WaitForSeconds(0.1f);

        sankeController.GetComponent<ControllerCharacter>().enabled = true;
    }

    public void Restart() => UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");

    public void Exit() => Application.Quit();
}
