using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour {
	public string _correctAnswer;
	public string _questionPrompt;
    public List<string> _questionOptions;// = new List<string>();
    //public GameObject gObj;


	// Use this for initialization
	void Start () {
        //gObj = GameObject.Find("ScriptHolder");
        //_questionOptions = new List<string>();
    }

	//set up question object, initially empty
	public Question()
	{
		_questionOptions = new List<string>();
	}

	//return the number of options for question
	public int getNumOptions()
	{
		return _questionOptions.Count;
	}
	
	// return the answer to the question
	public string getQuestionPrompt() 
	{
		return _questionPrompt;
	}

	//return the question options
	public List<string> getQuestionOptions()
	{
		return _questionOptions;
	}

	//returns the correct answer
	public string getCorrectAnswer()
	{
		return _correctAnswer;
	}

	//sets the correct answer for question
	public void setCorrectAnswer(string correct)
	{
		_correctAnswer = correct;
	}

	//set question options for prompt
	public void setQuestionOptions(List<string> options)
	{
        _questionOptions = options;
	}

	//return question prompt
	public void setQuestionPrompt(string prompt)
	{
		_questionPrompt = prompt;
	}


	public string displayCorrectAnswer()
	{
		if (_correctAnswer != null) {
            //Debug.Log ("The correct answer is: " + _correctAnswer);
            return _correctAnswer;
        } 
		else
		{
            //Debug.Log ("There is no answer set");
            return "There is no answer set";
        }

	}

	public string displayQuestionPrompt()
	{
		//Debug.Log ("Question prompt is: " + _questionPrompt);
        return _questionPrompt;

    }

	public List<string> displayQuestionPromptOptions()
	{
        for (int i = 0; i < _questionOptions.Count; i++)
		{
			int dispNum = i + 1;
			//Debug.Log ("option " + dispNum + " is: " + _questionOptions [i]);
		}
        return _questionOptions;
	}

	//set the option at a specific index
	public void setIndividualPromptOption( string option)
	{
        _questionOptions.Add( option);
	}
}
