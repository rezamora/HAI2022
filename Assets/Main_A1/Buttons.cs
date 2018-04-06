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
    static int cond = 1;
    static int coopAgent = 21;  //21 for cooperative, 81 for uncooperative
    static int coopAgentID = 4;

    //to keep track of hovering
    Stopwatch stopWatch = new Stopwatch();
    int[] myRep= new int[9];
    int repCounter = 0;
    static int trackBtn = -1;
    static bool btnActive = true;

    void Start()
    {
        //craete table for spreadsheet
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Question", typeof(string));
        table.Columns.Add("Correct Answer", typeof(string));
        table.Columns.Add("User Answer", typeof(string));
        table.Columns.Add("Agent HP Answer", typeof(string));
        table.Columns.Add("Correct?", typeof(int));
        table.Columns.Add("Time (seconds)", typeof(string));
        table.Columns.Add("Overal Time", typeof(string));
        table.Columns.Add("Percentage", typeof(double));
        table.Columns.Add("Time Started", typeof(string));
        table.Columns.Add("Time Finished", typeof(string));
        table.Columns.Add("Agent", typeof(int));
        table.Columns.Add("Condition", typeof(string));

        //create second table
        table2.Columns.Add("ID", typeof(int));
        table2.Columns.Add("Question", typeof(string));
        table2.Columns.Add("Option1", typeof(string));
        table2.Columns.Add("Option2", typeof(string));
        table2.Columns.Add("Option3", typeof(string));
        table2.Columns.Add("Option4", typeof(string));
        table2.Columns.Add("Option5", typeof(string));
        table2.Columns.Add("Option6", typeof(string));
        table2.Columns.Add("Option7", typeof(string));
        table2.Columns.Add("Option8", typeof(string));
        table2.Columns.Add("Option9", typeof(string));

        //rest of the code!
        myButtons = GetComponentsInChildren<UnityEngine.UI.Text>();
        Obj2 = GameObject.Find("ScriptHolder");
        Drv = Obj2.GetComponent<driver>();
        questions = Drv.getQuetions();
        //Debug.Log(questions.Count);
        q = Obj2.GetComponent<Question>();
        q = questions[myC++];
        System.Random rnd = new System.Random();
        rep = Enumerable.Range(1, 9).OrderBy(r => rnd.Next()).ToArray();
        //myRep = rep;
        startTime = System.DateTime.Now.TimeOfDay;
        currentTime = startTime;
        StartCoroutine(myRoutine(0));
    }

    public void BeenClicked()
    {
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        stopWatch.Reset();
        UnityEngine.Debug.Log(ts.ToString());
        for (int i = 0; i < 9; i++)
        {
            if(options[myRep[i]] == EventSystem.current.currentSelectedGameObject.GetComponentInChildren<UnityEngine.UI.Text>().text)
            {
                repCounter = i;
                trackBtn = myRep[i];
            }
        }
        DataRow dr = table2.NewRow();
        //UnityEngine.Debug.Log(repCounter);
        //UnityEngine.Debug.Log(myRep[repCounter]);
        UnityEngine.Debug.Log(options[myRep[repCounter]]);
        dr[myRep[repCounter] + 2] = ts.ToString();
        table2.Rows.Add(dr);
        stopWatch.Start();


        System.Random rnd = new System.Random();
        rep = Enumerable.Range(1, 9).OrderBy(r => rnd.Next()).ToArray();
        //myRep = rep;
        diff = System.DateTime.Now.TimeOfDay - currentTime;
        StartCoroutine(myRoutine(1));
        //Debug.Log("myC is:      " + myC);
        if (myC < 5)
        {
            q = questions[myC++];
            currentTime = System.DateTime.Now.TimeOfDay;
            //questions.RemoveAt(0);
        }
        else
        {
            endTime = System.DateTime.Now.TimeOfDay;
            if (cond == 2) agnt++;
            table.Rows.Add(null, null, null, null, null, null, null, (endTime - startTime).ToString(), Convert.ToDouble(correct) / 0.5,
                today.ToString("MM/dd/yyyy") + "-" + string.Format("{0}:{1}:{2}", startTime.Hours, startTime.Minutes, startTime.Seconds),
                today.ToString("MM/dd/yyyy") + "-" + string.Format("{0}:{1}:{2}",
                endTime.Hours, endTime.Minutes, endTime.Seconds), (agnt % 2) * (-1) + 2, participantID[coopAgentID]);
            //Debug.Log("PID: "+participantID);
            string address = @".\" + participantID + "-" + today.ToString("MM-dd-yyyy") + "-" + string.Format("{0}-{1}-{2}", startTime.Hours, startTime.Minutes, startTime.Seconds) + ".csv";
            CreateCSVFile(ref table, address);
            string address2 = @".\dump-" + participantID + "-" + today.ToString("MM-dd-yyyy") + "-" + string.Format("{0}-{1}-{2}", startTime.Hours, startTime.Minutes, startTime.Seconds) + ".csv";
            CreateCSVFile(ref table2, address2);
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            if (cond == 1)
                StartCoroutine(SA.GotoTransition());
            else
                SA.GotoFinish();

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
                //Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<UnityEngine.UI.Text>().text);
                wrong++;
                EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Button>().image.color = UnityEngine.Color.red;
                tfQ[myC - 1] = 0;
            }
            else
            {
                correct++;
                EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Button>().image.color = UnityEngine.Color.green;
                tfQ[myC - 1] = 1;
            }

            table.Rows.Add(myC, tableQ, answerQ[myC - 1], userA[myC - 1], ahpA[myC - 1], tfQ[myC - 1], diff.ToString());
        }

        EventSystem.current.SetSelectedGameObject(null);
        if (flag == 1)
        {
            foreach (UnityEngine.UI.Text btn in myButtons)
            {
                if (btn.GetComponent<UnityEngine.UI.Text>().name == "Text")
                {
                    btn.GetComponentInParent<Button>().interactable = false;
                    btnActive = false;
                }
            }
            yield return new WaitForSeconds(2);
        }
        myC1 = 0;

        int c = 0, cnt = 0;
        options = q.displayQuestionPromptOptions();
        options.Shuffle<string>();
        table2.Rows.Add(myC, q.displayQuestionPrompt(), options[0], options[1], options[2], options[3], options[4], options[5],
                options[6], options[7], options[8]);

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




                //Random rnd = new Random();
                var num = UnityEngine.Random.value * 100;
                //Debug.Log("Num value is:" + num);
                if (num < coopAgent)
                {
                    btnFlg = 0;
                }
                else
                {
                    //btnFlg = c;
                    ahpA[myC - 1] = btn.GetComponent<UnityEngine.UI.Text>().text;
                    UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_hp);
                    entry.callback.AddListener(call);
                    for (int i = 0; i < 9; i++)
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
            //Debug.Log(btn.GetComponent<Text>().text);
            //Debug.Log(btn.GetComponent<Text>().text.Length);

            /***********************************************************************/
            if (btn.GetComponent<UnityEngine.UI.Text>().name == "ScoreText")
            {
                btn.GetComponentInChildren<UnityEngine.UI.Text>().text = "Correct: " + correct.ToString() + ", Wrong: " + wrong.ToString();
            }
            else if (btn.GetComponent<UnityEngine.UI.Text>().name == "Text")
            {
                //UnityEngine.Debug.Log(counter + "---" + btn.GetComponent<UnityEngine.UI.Text>().text);
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

                //UnityEngine.Debug.Log("c is: " + c + "and btnFlg is: " + btnFlg);
                if (c != btnFlg)
                {
                    if (rep[counter] == 0 && counter < 8)
                        counter++;
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
                                dr[c + 1] = "Moderately Possitive";
                                myRep[1] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_mp);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 3:
                            {
                                dr[c + 1] = "Slightly Possitive";
                                myRep[2] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_sp);
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
                        case 5:
                            {
                                dr[c + 1] = "Moderately Negative";
                                myRep[4] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_mn);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 6:
                            {
                                dr[c + 1] = "Slightly Negative";
                                myRep[5] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_sn);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 7:
                            {
                                dr[c + 1] = "Moderately Positive";
                                myRep[6] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_mp2);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 8:
                            {
                                dr[c + 1] = "Idle";
                                myRep[7] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_idle2);
                                entry.callback.AddListener(call2);
                                rep[counter++] = 0;
                                break;
                            }
                        case 9:
                            {
                                dr[c + 1] = "Idle";
                                myRep[8] = c-1;
                                UnityEngine.Events.UnityAction<BaseEventData> call2 = new UnityEngine.Events.UnityAction<BaseEventData>(myAnims_idle3);
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

                btn.GetComponentInParent<Button>().image.color = UnityEngine.Color.white;
            }
            else
            {
                btn.GetComponentInChildren<UnityEngine.UI.Text>().text = q.displayQuestionPrompt();
            }
            /***********************************************************************/
        }
        table2.Rows.Add(dr);
    }


    public void myAnims_idle(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        if (btnActive)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            stopWatch.Reset();
            UnityEngine.Debug.Log(ts.ToString());
            DataRow dr = table2.NewRow();
            //UnityEngine.Debug.Log(repCounter);
            //UnityEngine.Debug.Log(myRep[repCounter]);
            if (trackBtn == -1)
            {
                UnityEngine.Debug.Log(options[myRep[repCounter]]);
                dr[myRep[repCounter] + 2] = ts.ToString();
            }
            else
            {
                UnityEngine.Debug.Log(options[trackBtn]);
                dr[trackBtn + 2] = ts.ToString();
                trackBtn = -1;
            }
            table2.Rows.Add(dr);
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.playIdle();
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
            SA.playIdle();
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
            SA.playIdle();
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

    public void myAnims_mp2(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        repCounter = 6;
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
        //GameObject inputFieldGo2 = GameObject.Find("Study");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        //InputField inputFieldCo2 = inputFieldGo2.GetComponent<InputField>();
        Exception myEx = null;

        participantID = inputFieldCo.text;
//        try
//        {
//            studyCode = Convert.ToInt32(inputFieldCo2.text);
//            if (studyCode < 1 || studyCode > 4)
//                studyCode /= 0;
//        }
//        catch (Exception ex)
//        {
//            myEx = ex;
//#if UNITY_EDITOR
//            EditorUtility.DisplayDialog("Invalid Input", "Study Number must be an integer number between 1 and 4!", "Ok");
//#endif
//        }

        try
        {
            if (!(participantID[4] != 'c' || participantID[4] != 'n'))
                agnt /= 0;

            if (!(participantID[5] != 'c' || participantID[5] != 'n'))
                agnt /= 0;

            if (Char.IsNumber(participantID[2]))
                agnt = Convert.ToInt16(participantID[2]);
            else
                agnt /= 0;
        }
        catch (Exception ex)
        {
            myEx = ex;
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Invalid Input", "Participant ID must be of form [three digit number]-[two letter word]-[two letter initial]!", "Ok");
#endif
        }

        if (participantID == "" || myEx != null)
        {
            if (participantID == "")
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Invalid Input", "You must enter Participant ID!", "Ok");
#endif
            }
        }
        else
        {
            //Debug.Log("Particiant ID: " + participantID);
            Obj = GameObject.Find("ScriptHolder");
            SA = Obj.GetComponent<switchAnimation>();
            SA.GotoInstructions();
        }
    }

    public void showAgents()
    {
        if (participantID[coopAgentID] == 'c')
            coopAgent = 21;
        else
            coopAgent = 81;
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        if (agnt % 2 == 1)
            SA.GotoAgentOne();
        else
            SA.GotoAgentTwo();
    }

    public void continueAgents()
    {
        Application.OpenURL("https://goo.gl/forms/GxPuqVUmngBMzZk32");
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("Wait!", "Please complete the survey first!", "Ok");
#endif
        coopAgentID = 5;
        if (participantID[coopAgentID] == 'c')
            coopAgent = 21;
        else
            coopAgent = 81;
        cond = 2;
        Obj = GameObject.Find("ScriptHolder");
        SA = Obj.GetComponent<switchAnimation>();
        if (agnt % 2 == 0)
            SA.GotoAgentOne();
        else
            SA.GotoAgentTwo();
    }

    public void openSurvey()
    {
        Application.OpenURL("https://goo.gl/forms/GxPuqVUmngBMzZk32");
    }

    public void appQuit()
    {
        Application.Quit();
    }

    public void nullFunction()
    {

    }


}
