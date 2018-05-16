using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class driver : MonoBehaviour {
    //contain data from .txt file in a line by line basis
    List<string> fileRow = new List<string>();
    //set up a vector of questions
    List<Question> listOfQuestions = new List<Question>();
    GameObject gObj;
    static int conditions = -1;

    // Use this for initialization
    void Start()
    {
        gObj = GameObject.Find("ScriptHolder");
        //run();
    }

    public void changeCondition()
    {
        conditions++;
    }

    //set up questions
    protected void setupQuestionList()
    {
        List<Question> questions = new List<Question>();
        //instantiate question
        Question q = gObj.GetComponent<Question>();//new Question();
        //count the lines that have passed, every 10 lines have a question
        int lineCounter = 0;

        //loop through fileRow with each line having
        for (int i = 0; i < fileRow.Count; i++)
        {
            //the question file follows a very specific pattern, question prompt followed by 9 possible answers

            //string last = fileRow[i][fileRow[i].Length - 1];

            //set up a new question object when the "?" appears
            if (fileRow[i].Contains("?") || fileRow[i].Contains(":"))
            //if(fileRow[i].EndsWith("?"))
            {
                q = new Question();
                q.setQuestionPrompt(fileRow[i]);
            }
            //lines marked with a "*" indicate that it is the answer to the question
            else if (fileRow[i].Contains("*"))
            //else if (fileRow[i].EndsWith("*"))
            {
                q.setCorrectAnswer(fileRow[i].TrimEnd().Substring(0, fileRow[i].Length - 2));
                q.setIndividualPromptOption(fileRow[i].TrimEnd().Substring(0, fileRow[i].Length - 2));
            }
            //if a line is not an answer or question prompt, then it is a option
            else
            {
                q.setIndividualPromptOption(fileRow[i]);
            }

            //increment lineCounter
            lineCounter++;

            //once lineCounter is = 9, then a new question will begin so add the current question to the vector
            if (lineCounter% 10 == 9)
            {
                questions.Add(q);
                //reset lineCounter
                //lineCounter = 0;
            }
        }

        //now set the global version of questions
        listOfQuestions = questions;
        //Debug.Log("Questions count: " + questions.Count);
    }

    public List<Question> getQuetions()
    {
        run();
        return listOfQuestions;
    }

	public void run()
	{
        //set up file interaction object and return each line of data
        //fileInteraction file = new fileInteraction ("Set1.txt");
        //fileInteraction file = gObj.GetComponent("Set1.txt") as fileInteraction;
        gObj = GameObject.Find("ScriptHolder");
        fileInteraction file = gObj.GetComponent<fileInteraction>();//new fileInteraction("Questions.txt");
        if (conditions == -1 || conditions == 0)
            file.setFileName("./Assets/Set0.txt");
        else if (conditions == 1)
        {
            file.setFileName("./Assets/Set1.txt");
        }
        else
        {
            file.setFileName("./Assets/Set2.txt");
        }
        //readFile so that data is now set
        file.readFile ();
		//fileRow is now set up with data from each line in each index
		fileRow = file.getFileData ();
		//set up the questions
		setupQuestionList();
        //Debug.Log(listOfQuestions.Count);

		//testing
		/*for (int i = 0; i < listOfQuestions.Count; i++) 
		{
			Question q = new Question ();
			q = listOfQuestions [i];
			//display question prompt, answer and options
			q.displayQuestionPrompt ();
			q.displayCorrectAnswer ();
			q.displayQuestionPromptOptions ();
		}*/
	}



	
	// Update is called once per frame
	void Update () {
		
	}
}
