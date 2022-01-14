using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class switchAnimation : MonoBehaviour {

    public GameObject Character;
    private Animator myController;
    //private float startTime;
    private static int state = 0;

    void Awake()
    {
        myController = Character.GetComponent<Animator>();
    }

    public void playIdle(int myC = 0)
    {
        myController.Rebind();
        if (state == 1 & myC > 5)
        {
            myController.SetInteger("State", state);
            turnSatisfied();
        }
        else if (state == -1 & myC > 5)
        {
            myController.SetInteger("State", state);
            turnDisatisfied();
        }
        else
        {
            myController.SetInteger("State", state);
            myController.Rebind();
        }
    }

    /*public void myPlayIdle()
    {
        if (state == 1)
        {
            myController.SetInteger("State",state);
            turnSatisfied();
        }
        else if (state == -1)
        {
            myController.SetInteger("State", state);
            turnDisatisfied();
        }
        else
        {
            myController.SetInteger("State", state);
            myController.Rebind();
        }
    }*/

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
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSecnQ3CTL0wPrgtIl9E_W_Pbnzoz0C0PJIo0MOTgTvnGpD9xQ/viewform?usp=sf_link");
    }

    public void GotoFinish()
    {
        SceneManager.LoadScene("Finish");
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSf833siq8Xe5KL-kYus55y5ST0YIDy5cNo2jrngTOg1kzA-RA/viewform?usp=sf_link");
    }

    public void GotoCondOne()
    {
        SceneManager.LoadScene("GotoCond1");
    }

    public void turnSatisfied()
    {
        myController.SetTrigger("dps");



        //myController.Rebind();
        //AnimatorStateMachine asm = myController.layers[0].stateMachine;
        //AnimatorState newState = asm.AddState("dps");
        //asm.defaultState = newState;
        //SceneManager.LoadScene("Satisfied-Main_A1");
        //myController.SetTrigger("default_P_small");
    }

    public void turnDisatisfied()
    {
        myController.SetTrigger("dns");
    }

    public void GotoA1Prep()
    {
        SceneManager.LoadScene("A1Prep");
    }

    public void GotoA2Prep()
    {
        SceneManager.LoadScene("A2Prep");
    }

    public void setInteger(int variableValue, string variableName = "state")
    {
        state = variableValue;
        //myController.SetInteger(variableName, variableValue);
    }
}
