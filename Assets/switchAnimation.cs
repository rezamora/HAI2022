using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class switchAnimation : MonoBehaviour {

    public GameObject Character;
    private Animator myController;
    //private float startTime;

    void Awake()
    {
        myController = Character.GetComponent<Animator>();
    }

    public void playIdle()
    {
        myController.Rebind();
    }

    public void playPositive_large()
    {
        myController.SetTrigger("P_l");
    }

    public void playPositive_medium()
    {
        myController.SetTrigger("P_m");
    }

    public void playPositive_small()
    {
        myController.SetTrigger("P_s");
    }

    public void playNegative_large()
    { 
        myController.SetTrigger("N_l");
    }

    public void playNegative_medium()
    {
        myController.SetTrigger("N_m");
    }

    public void playNegative_small()
    {
        myController.SetTrigger("N_s");
    }

   public void GotoAgentTwo()
    {
        SceneManager.LoadScene("Main_A2");
    }

    public void GotoAgentOne()
    {
        SceneManager.LoadScene("Main_A1");
    }

    public void GotoInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public IEnumerator GotoTransition()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Transition");
    }

    public IEnumerator GotoFinish()
    {
        SceneManager.LoadScene("Finish");
        Application.OpenURL("https://goo.gl/forms/GxPuqVUmngBMzZk32");
#if UNITY_EDITOR
        yield return new WaitForSeconds(2);
        EditorUtility.DisplayDialog("Wait!", "Please complete the survey first!", "Ok");
#endif
    }
}
