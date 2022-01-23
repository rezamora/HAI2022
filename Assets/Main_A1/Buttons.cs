using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

public class Buttons : MonoBehaviour
{

    public UnityEngine.UI.Text[] myButtons;
    public GameObject Obj;
    public GameObject Obj2;
    public GameObject gObj;


    public UnityEngine.UI.Text notificationText;


    driver Drv;
    List<Question> questions;
    Question q;
    switchAnimation SA;
    List<string> options;
    public int[] rep;
    int myC = 0;
    int myC1 = 0;
    int correct, wrong = 0;
    int btnFlg = 0;

    //Statistics variables
    int[] tfQ = new int[50];
    double[] timeQ = new double[50];
    double percentage;
    double overalTime;
    string[] answerQ = new string[50];
    string[] userA = new string[50];
    string[] ahpA = new string[50];
    string tableQ;
    int matchCount = 0;
    int userComplianceRateWithAgent = 0;
    System.TimeSpan startTime;
    System.TimeSpan currentTime;
    System.TimeSpan diff;
    System.TimeSpan endTime;
    System.DateTime today = System.DateTime.Today;

    //spreadsheet tables
    public System.Data.DataTable table = new System.Data.DataTable();
    public System.Data.DataTable table2 = new System.Data.DataTable();

    //Study information
    static string participantID;
    //static int studyCode;
    static int agnt = 0;
    static int cond = -1;
    static int coopAgent = 31;  //31 for cooperative, 71 for uncooperative
    static int coopAgentID = 4;
    Stack myStack = new Stack();

    //to keep track of hovering
    Stopwatch stopWatch = new Stopwatch();
    static int numberOfChoices = 4;
    int[] myRep= new int[numberOfChoices];
    int repCounter = 0;
    static int trackBtn = -1;
    static bool btnActive = true;

    void Start()
    {
        //create table for spreadsheet
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Question", typeof(string));
        table.Columns.Add("Correct Answer", typeof(string));
        table.Columns.Add("User Answer", typeof(string));
        table.Columns.Add("Agent HP Answer", typeof(string));
        table.Columns.Add("Correct?", typeof(string));
        table.Columns.Add("Time (seconds)", typeof(string));
        table.Columns.Add("Overal Time", typeof(string));
        table.Columns.Add("Percentage", typeof(double));
        table.Columns.Add("Time Started", typeof(string));
        table.Columns.Add("Time Finished", typeof(string));
        //table.Columns.Add("Agent", typeof(string));
        table.Columns.Add("Condition", typeof(string));
        table.Columns.Add("Match?", typeof(string));

        //create second table
        table2.Columns.Add("ID", typeof(int));
        table2.Columns.Add("Question", typeof(string));
        table2.Columns.Add("Option1", typeof(string));
        table2.Columns.Add("Option2", typeof(string));
        table2.Columns.Add("Option3", typeof(string));
        table2.Columns.Add("Option4", typeof(string));

        //rest of the code!

        System.Random rnd = new System.Random();
        for (int it = 0; it < 100; it++)
            myStack.Push(it);
        var values = myStack.ToArray();
        myStack.Clear();
        foreach (var value in values.OrderBy(x => rnd.Next()))
        {
            myStack.Push(value);
        }


        myButtons = GetComponentsInChildren<UnityEngine.UI.Text>();
        Obj2 = GameObject.Find("ScriptHolder");
        Drv = Obj2.GetComponent<driver>();
        questions = Drv.getQuetions();
        q = Obj2.GetComponent<Question>();
        q = questions[myC++];
        rep = Enumerable.Range(1, numberOfChoices).OrderBy(r => rnd.Next()).ToArray();
        startTime = System.DateTime.Now.TimeOfDay;
        currentTime = startTime;
        StartCoroutine(myRoutine(0));
    }


    //bool satisfy = true;
    public void BeenClicked()
    {
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        stopWatch.Reset();
        UnityEngine.Debug.Log("TSSSSSSSSSS");
        UnityEngine.Debug.Log(ts);
        UnityEngine.Debug.Log(ts.TotalSeconds);

        for (int i = 0; i < numberOfChoices; i++)
        {
            if(options[myRep[i]] == EventSystem.current.currentSelectedGameObject.GetComponentInChildren<UnityEngine.UI.Text>().text)
            {
                repCounter = i;
                trackBtn = myRep[i];
            }
        }
        DataRow dr = table2.NewRow();

        dr[myRep[repCounter] + 2] = ts.TotalSeconds.ToString();
        table2.Rows.Add(dr);
        stopWatch.Start();


        System.Random rnd = new System.Random();
        rep = Enumerable.Range(1, numberOfChoices).OrderBy(r => rnd.Next()).ToArray();
        //UnityEngine.Debug.Log(rep.Length);
        diff = System.DateTime.Now.TimeOfDay - currentTime;
        StartCoroutine(myRoutine(1));

        

        if (cond == -1)
        {
            //appQuit();
            cond++;
            Drv.changeCondition();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            //SA.GotoA2Prep();
            //SA.turnSatisfied();

        }
        /*else if (cond == 0)
        {
            //appQuit();
            cond++;
            /*Drv.changeCondition();
            //openSurvey();

        }
        else */
        //UnityEngine.Debug.LogFormat("ghabl az if");
        if (myC < 2)
        {
            q = questions[myC++];
            currentTime = System.DateTime.Now.TimeOfDay;
        }
        else
        {
            endTime = System.DateTime.Now.TimeOfDay;
            if (cond == 2) agnt++;
            
            //table.Rows.Add(null, null, null, null, null, null, null, (endTime - startTime).ToString(), Convert.ToDouble(correct) / 0.5,
                //today.ToString("MM/dd/yyyy") + "-" + string.Format("{0}:{1}:{2}", startTime.Hours, startTime.Minutes, startTime.Seconds),
                //today.ToString("MM/dd/yyyy") + "-" + string.Format("{0}:{1}:{2}",
               // endTime.Hours, endTime.Minutes, endTime.Seconds), (agnt % 2) * (-1) + 2, participantID[coopAgentID],matchCount);
            //UnityEngine.Debug.LogFormat("dakhele else");
            //UnityEngine.Debug.LogFormat("PID: "+participantID);
            //string address = @".\" + participantID + "-" + today.ToString("MM-dd-yyyy") + "-" + string.Format("{0}-{1}-{2}", startTime.Hours, startTime.Minutes, startTime.Seconds) + ".csv";
            //CreateCSVFile(ref table, address);
            //StartCoroutine(postToGoogleForm());
            //string address2 = @".\dump-" + participantID + "-" + today.ToString("MM-dd-yyyy") + "-" + string.Format("{0}-{1}-{2}", startTime.Hours, startTime.Minutes, startTime.Seconds) + ".csv";
            //CreateCSVFile(ref table2, address2);
            //UnityEngine.Debug.LogFormat("ghabl az amal");




            /*StartCoroutine(postToGoogleForm());*/
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.GotoFinish();

            /*if (cond == 1)
                StartCoroutine(SA.GotoTransition());
            else
                SA.GotoFinish();*/
        }

        if (myC > 500)
        {
            //UnityEngine.Debug.Log("UCR: " + userComplianceRateWithAgent);
            int actualQuestionCount = myC - 1;
            //UnityEngine.Debug.Log("Count: " + actualQuestionCount);
            //UnityEngine.Debug.Log("division: " + (userComplianceRateWithAgent * 100 / actualQuestionCount));
            //UnityEngine.Debug.Log("Coop Agent: " + coopAgent);
            if ((userComplianceRateWithAgent * 100 / actualQuestionCount > ((102 - coopAgent) - 10)) && 
                (userComplianceRateWithAgent * 100 / actualQuestionCount < ((102 - coopAgent) + 10)))
            {
                //SA.defaultValues();
                SA.setInteger(1, "state");
                SA.playIdle(myC);
            }
            else
            {
                //SA.defaultValues();
                SA.setInteger(-1, "state");
                SA.playIdle(myC);
            }
        }

    }

    IEnumerator myRoutine(int flag)
    {
        int counter = 0;
        if (flag == 1)
        {
            userA[myC - 1] = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<UnityEngine.UI.Text>().text;
            answerQ[myC - 1] = q.displayCorrectAnswer();
            tableQ = q.displayQuestionPrompt();

            if (!EventSystem.current.currentSelectedGameObject.GetComponentInChildren<UnityEngine.UI.Text>().text.Equals(q.displayCorrectAnswer()))
            {
                wrong++;
                /*if (cond != -1 && cond != 0)*/
                EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Button>().image.color = UnityEngine.Color.red;
                tfQ[myC - 1] = 0;
            }
            else
            {
                correct++;
                EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Button>().image.color = UnityEngine.Color.green;
                tfQ[myC - 1] = 1;
            }
            int isMatch = 0;
            /*UnityEngine.Debug.Log("userA[myC - 1]: ");
            UnityEngine.Debug.Log(userA[myC - 1].ToString());
            UnityEngine.Debug.LogFormat(" ahpA[myC - 1]: ");
            UnityEngine.Debug.Log(ahpA[myC - 1]);
            UnityEngine.Debug.LogFormat(" answerQ[myC - 1]: ");
            UnityEngine.Debug.Log(answerQ[myC - 1]);*/
            if (userA[myC - 1] == ahpA[myC - 1]) { userComplianceRateWithAgent++; }
            if (answerQ[myC - 1] == ahpA[myC - 1]) { isMatch = 1; matchCount++; }
            //object p = table.Rows.Add(myC, tableQ, answerQ[myC - 1], userA[myC - 1], ahpA[myC - 1], tfQ[myC - 1], diff.ToString(),
            //  null, null, null, null, null, isMatch.ToString());
        }

        //Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSf5sdsvQophCx85ofeV2Zkgn4ehlm35AV1nugNFGX05zp2ySg/viewform?usp=sf_link?Kiiir");
        //notificationText.text = "Kiiiiiiiir";
        EventSystem.current.SetSelectedGameObject(null);
        //notificationText.text = "Kooon";
        //Application.OpenURL("https://docs.google.com/");
        if (flag == 1)
        {
            //notificationText.text = "Kosssssss";
            foreach (UnityEngine.UI.Text btn in myButtons)
            {
                UnityEngine.Debug.Log(btn.GetComponent<UnityEngine.UI.Text>().name);
                if (btn.GetComponent<UnityEngine.UI.Text>().name == "Text")
                {
                    btn.GetComponentInParent<Button>().interactable = false;
                    btnActive = false;
                }
            }
            //Application.OpenURL("https://google.com/");
            ////////////////////////////////////////////////////////////////////////yield return new WaitForSeconds(2);
            yield return new WaitForSecondsRealtime(2);
            //yield return "OK";
            //Application.OpenURL("https://doodle.com/");
        }
        myC1 = 0;

        int c = 0, cnt = 0;
        options = q.displayQuestionPromptOptions();
        //UnityEngine.Debug.LogFormat("Options = {0}", options);
        options.Shuffle<string>();
        table2.Rows.Add(myC, q.displayQuestionPrompt(), options[0], options[1], options[2], options[3]);

        DataRow dr = table2.NewRow();

        foreach (UnityEngine.UI.Text btn in myButtons)
        {
            c++;
            if (btn.GetComponent<UnityEngine.UI.Text>().name == "Text")
            {
                btn.GetComponentInParent<Button>().interactable = true;
                btnActive = true;
                btn.GetComponentInChildren<UnityEngine.UI.Text>().text = options[cnt++];
            }
            if (btn.GetComponent<UnityEngine.UI.Text>().text.Equals(q.displayCorrectAnswer()))
            {
                EventTrigger trigger = btn.GetComponentInParent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback = new EventTrigger.TriggerEvent();
                entry2.eventID = EventTriggerType.PointerExit;
                entry2.callback = new EventTrigger.TriggerEvent();
                UnityEngine.Events.UnityAction<BaseEventData> myCall = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_idle);
                entry2.callback.AddListener(myCall);
                btnFlg = c;




                
                var num = Convert.ToInt32(myStack.Pop());
               // UnityEngine.Debug.Log("Num value is:" + num);
                if (num < coopAgent)
                {
                    btnFlg = 0;
                }
                else
                {
                    ahpA[myC - 1] = btn.GetComponent<UnityEngine.UI.Text>().text;
                    UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_hp);
                    entry.callback.AddListener(call);
                    for (int i = 0; i < numberOfChoices; i++)
                    {
                        if (rep[i] == 1)
                        {
                            rep[i] = 0;
                            dr[c + 1] = "Highly Possitive";
                            myRep[0] = c-1;
                        }
                    }
                    trigger.triggers.Clear();
                    trigger.triggers.Add(entry);
                    trigger.triggers.Add(entry2);
                }
            }
        }
        
        c = 0;
        foreach (UnityEngine.UI.Text btn in myButtons)
        {
            c++;

            /***********************************************************************/
            if (btn.GetComponent<UnityEngine.UI.Text>().name == "ScoreText")
            {
                btn.GetComponentInChildren<UnityEngine.UI.Text>().text = "Correct: " + correct.ToString() + ", Incorrect: " + wrong.ToString();
            }
            else if (btn.GetComponent<UnityEngine.UI.Text>().name == "Text")
            {
                EventTrigger trigger = btn.GetComponentInParent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback = new EventTrigger.TriggerEvent();
                entry2.eventID = EventTriggerType.PointerExit;
                entry2.callback = new EventTrigger.TriggerEvent();
                UnityEngine.Events.UnityAction<BaseEventData> myCall = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_idle);
                entry2.callback.AddListener(myCall);
                options = q.displayQuestionPromptOptions();
                btn.GetComponentInChildren<UnityEngine.UI.Text>().text = options[myC1++];

                if (c != btnFlg)
                {
                    if (counter < numberOfChoices - 1 && rep[counter] == 0)
                        counter++;
                    UnityEngine.Debug.Log("rep[counter]: ");
                    UnityEngine.Debug.Log(rep[counter]);
                    UnityEngine.Debug.Log(counter);
                    switch (rep[counter])
                    {
                        case 1:
                            {
                                ahpA[myC - 1] = btn.GetComponent<UnityEngine.UI.Text>().text;
                                dr[c + 1] = "Highly Possitive";
                                myRep[0] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_hp);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 2:
                            {
                                dr[c + 1] = "Highly Negative";
                                myRep[1] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_hn);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 3:
                            {
                                dr[c + 1] = "Highly Negative";
                                myRep[2] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_hn);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 4:
                            {
                                dr[c + 1] = "Highly Negative";
                                myRep[3] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_hn);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        default:
                            break;
                    }
                    trigger.triggers.Clear();
                    trigger.triggers.Add(entry);
                    trigger.triggers.Add(entry2);
                }

                //yield return new WaitForSeconds(1);
                //yield return new WaitForSecondsRealtime(1);
                btn.GetComponentInParent<Button>().image.color = UnityEngine.Color.white;
            }
            else
            {
                string myQuestion = q.displayQuestionPrompt();
                if (myQuestion.Contains("**"))
                    myQuestion.TrimEnd().Substring(0, myQuestion.Length - 3);
                btn.GetComponentInChildren<UnityEngine.UI.Text>().text = q.displayQuestionPrompt();
            }
            /***********************************************************************/
        }
        table2.Rows.Add(dr);
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        SA.playIdle(myC);
        yield return new WaitForSecondsRealtime(1);
        //yield return new WaitForSeconds(1);
    }


    public void myAnims_idle(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        if (btnActive)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            stopWatch.Reset();
            
            DataRow dr = table2.NewRow();

            if (trackBtn == -1)
            {
                dr[myRep[repCounter] + 2] = ts.TotalSeconds.ToString();
            }
            else
            {
                dr[trackBtn + 2] = ts.TotalSeconds.ToString();
                trackBtn = -1;
            }
            table2.Rows.Add(dr);
           //UnityEngine.Debug.LogFormat("datarow = {0}", dr);
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playIdle(myC);
            //SA.turnSatisfied();
        }
    }


    public void myAnims_idle2(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 7;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playIdle(myC);
            //SA.turnSatisfied();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }

    public void myAnims_idle3(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 8;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playIdle(myC);
            //SA.turnSatisfied();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }


    public void myAnims_hp(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 0;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playPositive_large();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }

    public void myAnims_mp(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 1;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playPositive_medium();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }

    public void myAnims_sp(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 2;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playPositive_small();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }

    public void myAnims_hn(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 3;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playNegative_large();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }

    public void myAnims_mn(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 4;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playNegative_medium();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }

    public void myAnims_mn2(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 6;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playNegative_medium();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }

    public void myAnims_sn(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 5;
        if (btnActive)
        {
            stopWatch.Start();
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playNegative_small();
        }
        else
        {
            trackBtn = myRep[repCounter];
        }
    }








    [SerializeField]
    private string formURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeQnJkMO3anbZa9mMYDdDoK0GgCMyyIU7qcUkjEIl3tWDN-MA/formResponse";

    IEnumerator postToGoogleForm(/*ref DataTable dt*/)
    {
        WWWForm formToBePosted = new WWWForm();
        //formToBePosted.AddField("entry.419191488", "This is a test text!");
        table.Rows.Add(myC.ToString(), tableQ.ToString(), answerQ[myC - 1].ToString(), userA[myC - 1].ToString(), ahpA[myC - 1].ToString(), tfQ[myC - 1].ToString(), diff.ToString(), null, null, null, null, null, "0" /*isMatch.ToString()*/);

        List<string> fieldIDs = new List<string> { "entry.419191488", "entry.1807030595", "entry.1392795617", "entry.443101265", "entry.512539465", "entry.1042577509", "entry.775098338", "entry.606167278", "entry.783900823", "entry.1993330233", "entry.2048865249", "entry.739753004", "entry.401560801" };

        int iColCount = table.Columns.Count;
        //UnityEngine.Debug.Log(iColCount);
        //UnityEngine.Debug.Log(fieldIDs.Count());



        foreach (DataRow dr in table.Rows)
        {
            for (int i = 0; i < iColCount; i++)
            {
                if (!Convert.IsDBNull(dr[i].ToString()))
                {
                    //sw.Write(dr[i].ToString());
                    formToBePosted.AddField(fieldIDs[i], dr[i].ToString());
                }
                /*if (i < iColCount - 1)
                {
                    sw.Write("\",\"");
                }*/
            }

            //sw.Write("\"" + sw.NewLine + "\"");
        }
        /*
        formToBePosted.AddField("", "ID.ToString()"); //table.Columns.Add("ID", typeof(int));
        formToBePosted.AddField("", "Question");  //table.Columns.Add("Question", typeof(string));
        formToBePosted.AddField("", "CorrectAnswer");  //table.Columns.Add("Correct Answer", typeof(string));
        formToBePosted.AddField("", "UserAnswer");  //table.Columns.Add("User Answer", typeof(string));
        formToBePosted.AddField("", "AgentHPAnswer()");  //table.Columns.Add("Agent HP Answer", typeof(string));
        formToBePosted.AddField("", "IsUserAnswerCorrect");  //table.Columns.Add("Correct?", typeof(int));
        formToBePosted.AddField("", "Time(seconds)");  //table.Columns.Add("Time (seconds)", typeof(string));
        formToBePosted.AddField("", "OverallTime");  //table.Columns.Add("Overal Time", typeof(string));
        formToBePosted.AddField("", "Percentage");  //table.Columns.Add("Percentage", typeof(double));
        formToBePosted.AddField("", "TimeStarted");  //table.Columns.Add("Time Started", typeof(string));
        formToBePosted.AddField("", "TimeFinished");  //table.Columns.Add("Time Finished", typeof(string));
        //table.Columns.Add("Agent", typeof(int));
        formToBePosted.AddField("", "Condition");  //table.Columns.Add("Condition", typeof(string));
        formToBePosted.AddField("", "HPMatchCorrect");  //table.Columns.Add("Match?", typeof(string));*/
        //UnityEngine.Debug.Log("ALooooooooooooooooo");
        byte[] formRawData = formToBePosted.data;


        WWW dataToPost = new WWW(formURL, formRawData);
        yield return dataToPost;
    }















    public void CreateCSVFile(ref DataTable dt, string strFilePath)
    {
        try
        {
            // Create the CSV file to which grid data will be exported.
            StreamWriter sw = new StreamWriter(strFilePath, false);
            // First we will write the headers.
            //DataTable dt = m_dsProducts.Tables[0];
            int iColCount = dt.Columns.Count;
            sw.Write("\"");
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i].ToString());
                if (i < iColCount - 1)
                {
                    sw.Write("\",\"");
                }
            }
            sw.Write("\"" + sw.NewLine + "\"");

            // Now write all the rows.

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i].ToString()))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write("\",\"");
                    }
                }

                sw.Write("\"" + sw.NewLine + "\"");
            }
            sw.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    public void startStudy()
    {
        GameObject inputFieldGo = GameObject.Find("Participant");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        Exception myEx = null;
        participantID = inputFieldCo.text;

        GameObject inputFieldGo2 = GameObject.Find("Error");
        UnityEngine.UI.Text Error = inputFieldGo2.GetComponent<Text>();

        try
        {
            if (!(participantID[4] == 'c' || participantID[4] == 'n'))
                agnt /= 0;

            if (!(participantID[5] == 'c' || participantID[5] == 'n'))
                agnt /= 0;

            if (Char.IsNumber(participantID[2]))
                agnt = Convert.ToInt16(participantID[2]);
            else
                agnt /= 0;
        }
        catch (Exception ex)
        {
            myEx = ex;
            Error.text = "Participant ID must be of form \n[three digit number]-[two letter word]-[two letter initial]!";
                
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Invalid Input", "Participant ID must be of form [three digit number]-[two letter word]-[two letter initial]!", "Ok");
#endif
        }


        if (participantID == "" || myEx != null)
        {
            if (participantID == "")
            {
                Error.text = "You must enter Participant ID!";
                    
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Invalid Input", "You must enter Participant ID!", "Ok");
#endif
            }
        }
        else
        {
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.startQuizOne();
        }
    }

    public void A1Prep()
    {
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        SA.GotoA1Prep();
    }

    public void A2Prep()
    {
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        if (agnt % 2 == 0)
            SA.GotoAgentOne();
        else
            SA.GotoAgentTwo();
    }

    public void showAgents()
    {
        if (participantID[coopAgentID] == 'c')
            coopAgent = 31;
        else
            coopAgent = 71;
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        if (agnt % 2 == 1)
            SA.GotoAgentOne();
        else
            SA.GotoAgentTwo();
    }

    /*public void continueAgents()
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("Wait!", "Please complete the survey first!", "Ok");
#endif
        //SA.GotoAgentOne();
        Drv.changeCondition();
        coopAgentID = 5;
        if (participantID[coopAgentID] == 'c')
            coopAgent = 31;
        else
            coopAgent = 71;
        cond = 2;
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        if (agnt % 2 == 0)
            SA.GotoAgentOne();
        else
            SA.GotoAgentTwo();
    }*/

    public void openSurvey()
    {
        //Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSf5sdsvQophCx85ofeV2Zkgn4ehlm35AV1nugNFGX05zp2ySg/viewform?usp=sf_link");
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        //SA.GotoCondOne();
    }

    public void appQuit()
    {
        Application.Quit();
    }
    

}
