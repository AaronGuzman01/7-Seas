using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CannonLoader : MonoBehaviour
{
    public void SetCannonScene()
    {
        SceneManager.LoadScene("Cannon", LoadSceneMode.Additive);

        StartCoroutine(ProcessCannonScene());
    }

    IEnumerator ProcessCannonScene()
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByName("Cannon").isLoaded);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Cannon"));

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    public void ExitCannonScene()
    {
        MapLoad.ContinueGame();

        SceneManager.UnloadSceneAsync("Cannon");
    }

    public void ExitResultsScene()
    {
        MapLoad.ContinueGame();

        SceneManager.UnloadSceneAsync("CannonResults");
    }
}
