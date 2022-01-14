using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;

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
        //count the lines that have passed, every 5 lines have a question
        int lineCounter = 0;
        int numberOfChoices = 4;

        //loop through fileRow with each line having
        for (int i = 0; i < fileRow.Count; i++)
        {
            //UnityEngine.Debug.LogFormat("File Row Count: ", fileRow.Count);
            //UnityEngine.Debug.LogFormat("File Row: ", fileRow[0]);
            //the question file follows a very specific pattern, question prompt followed by 'numberOfChoices' possible answers

            //set up a new question object when the "?" appears
            if (fileRow[i].Contains("?") || fileRow[i].Contains(":"))
            {
                q = new Question();
                q.setQuestionPrompt(fileRow[i]);
            }
            //lines marked with a "*" indicate that it is the answer to the question
            else if (fileRow[i].Contains("*"))
            {
                q.setCorrectAnswer(fileRow[i].TrimEnd().Substring(0, fileRow[i].Length - 2));
                q.setIndividualPromptOption(fileRow[i].TrimEnd().Substring(0, fileRow[i].Length - 2));
            }
            //if a line is not an answer or question prompt, then it is a filler choice
            else
            {
                q.setIndividualPromptOption(fileRow[i]);
            }

            //increment lineCounter
            lineCounter++;

            //once we reach the last choice for the current question, a new question will begin. So we add the current question to the vector
            if (lineCounter% (numberOfChoices + 1) == 0)
            {
                questions.Add(q);
                //Debug.Log(questions);
            }
        }

        //now set the global version of questions
        listOfQuestions = questions;
        //Debug.Log("Questions count: " + questions.Count);
    }

    public List<Question> getQuetions()
    {
        run();
        //UnityEngine.Debug.LogFormat("list of questuions", listOfQuestions);
        return listOfQuestions;
    }

	public void run()
	{
        //set up file interaction object and return each line of data
        gObj = GameObject.Find("ScriptHolder");
        fileInteraction file = gObj.GetComponent<fileInteraction>();

        file.fileLine = getSet0Text().ReadToEnd().Split('\n').ToList();
        //Debug.LogFormat("file line: ", getSet0Text().ReadToEnd().Split('\n').ToString());
        fileRow = file.getFileData();
        //Debug.LogFormat("file row: ", fileRow.ToString());
        //set up the questions
        setupQuestionList();
        //Debug.Log(listOfQuestions[3]);




        //gObj = GameObject.Find("ScriptHolder");
        //fileInteraction file = gObj.GetComponent<fileInteraction>();
        //if (conditions == -1)
        //    file.setFileName("./Assets/Q1-HAI2022.txt");
        //else if (conditions == 0)
        //    file.setFileName("./Assets/Q1-HAI2022.txt");
        //else if (conditions == 1)
        //{
        //    file.setFileName("./Assets/Q1-HAI2022.txt");
        //}
        //else
        //{
        //    file.setFileName("./Assets/Q1-HAI2022.txt");
        //}
        ////readFile so that data is now set
        //file.readFile();
        ////fileRow is now set up with data from each line in each index
        //fileRow = file.getFileData();
        ////set up the questions
        //setupQuestionList();


    }


    // Update is called once per frame
    void Update () {
		
	}


    public StreamReader getSet0Text()
    {
        //var set0Text = gObj.GetComponent<driver>();
        //string contents = "You are encouraged to interact with the interface to familiarize yourself with the format of the quizzes that will come up after the lectures. When you are ready to move to the next page, please click on the \"Next\" button below:\nAnswer*\nAnswer\nAnswer\nAnswer\nThis is question 2:\nAnswer2*\nAnswer2\nAnswer2\nAnswer2";
        string quiz1Contents = "What has been around for thousands of years and is at the heart of worldwide communications today? \n Cryptography* \n Locks \n Internet Protocol (IP) \n Homing Peigeons \n Substitution cipher is believed to be used first by: \n Julius Caesar* \n Nero \n Alexander \n Xerxes \n Using Caesar's cipher with 1 shift, what would be encryption of the word \"send\" ? \n tfoe* \n rdmc \n dnes \n ends \n What was responsible for breaking Caesar's cipher? \n Frequency analysis* \n Enigma machine \n Super computers \n Brute force approach \n In random shifts, what should the length of list of shifts? \n Same as the message* \n Shorter \n Longer \n Any length \n Which one is a strength of random shifts? \n No repettition pattern \n No leak \n Uniform frequency distribution \n All* \n What is the initial Enigma machine state known as? \n Key setting* \n Key space \n State space \n Machine configuration \n What machine allowed the allies to read German commands within hours? \n Enigma machine* \n Pseudo machine \n One-time pad \n Cipher \n What does reduce the key space? \n Repetition* \n Frequency \n Throwing dice \n One-time pad \n In case of perferct secrecy, which one of the following has a different size? \n Message space \n Key space \n Ciphertext space \n Machine state \n One of the problems of one-time pad is that long keys must be shared in advance. What is a solution to this problem? \n Pseudo randomness* \n Perfect secrecy \n Enigma encryption \n Cipher \n Which one is NOT a feature of random walk? \n random sequence \n no pattern \n net move is unpredictable \n deterministic process \n Who was involved in running computations for the US military which resulted in the design of Hydrogen bombs? \n Turing \n Einstein \n Neumann* \n Shannon \n In pseudorandom number generation, the randomness of the sequence is dependent on: \n Initial seed* \n Length of the sequence \n Patterns in each sequence \n Balanced seed distribution \n The length before a pseudorandom sequence repeats is called: \n Period* \n Frequency \n Cipher \n Cycle \n Moving from randomness to pseudorandomness shrinks the ..... : \n Key space* \n Message space \n Ciphertext space \n Seed space";
        string quiz2Contents = "1- All ..... are based on repetitive patterns which divides the flow of time into equal segments: \n Clocks* \n ciphers \n encryptions \n pseudorandom numbers \n 2- Our ancestors kept track of passage of time by looking at the ....: \n Stars* \n clouds \n birds \n direction of Rivers \n 3- The number of days between full moons is equal to: \n 29* \n 28 \n 30 \n 31 \n 4- A number that can be divided only by itself and 1 is called a .... number: \n Prime* \n composite \n real \n imaginary \n 5- All numbers are built out of smaller, .... numbers: \n Prime* \n composite \n real \n natural \n 6- pick any number, and find all the prime numbers it divides into equally. What is this process called? \n Factorization* \n elimination \n commination \n multiplication \n 7- applications such as money transfer require .... to remain secure: \n encryption* \n cryptography \n factorization \n key space \n 8- easy in one direction, hard in the reverse direction is the basis of: \n one-way function* \n encryption \n cryptography \n factorization \n 9- In modular arithmetic which number is known as the generator? \n 3* \n 2 \n 5 \n 10 \n 10- Which one of the equations below is correct? \n (3^15)^13 mod 17 ≡	(3^13)^15 mod 17* \n (3^15)^13 mod 17 ≡	(3^15)^17 mod 13 \n (3^15)^13 mod 17 ≡	(15^13)^3 mod 17 \n (3^15)^13 mod 17 ≡  (13^3)^15 mod 17 \n 11- What kind of keys was cryptography based on up until the 1970? \n Symmetric keys* \n assymmetric keys \n public keys \n private keys \n 12- Encryption is a mapping from some message using a specific key to a ....... message: \n ciphertext* \n key space \n modular \n one-way \n 13- What is NOT one of the drawbacks of symmetric keys? \n prone to physical distance \n extra overhead \n need multiple keys \n shared publicly* \n 14- which one of these is NOT a one-way function? \n prime factorization \n Mixing Colors \n trap door \n and sth else \n 15- What causes a problem tp be called hard? \n Unreasonable time complexity* \n Unreasonable space complexity \n proven to be unsolveable \n No solid basis in math \n 16- Who introduced Phi function? \n Euler* \n Euclid \n Ellis \n Cocks \n 17- If N=p1*p2,, then what is the value of phi(N)? \n (P1- 1)*(p2 - 1)* \n P1*P2 \n p1 - p2 \n p1 + p2 -2 \n 18- Which one is NOT a characteristic of the public exponent, e? \n small \n odd \n no shared factor with phi(n) \n e mod (n - 1) ≡ 0* \n 19- In RSA encryption, which of the following pairs is not hidden? \n n and e* \n m and e \n n and d \n m and d";

        byte[] byteArray = Encoding.UTF8.GetBytes(quiz1Contents);
        MemoryStream stream = new MemoryStream(byteArray);
        StreamReader streamReader = new StreamReader(stream);
        //Debug.LogFormat("Stream Reader:", streamReader.ToString());
        return streamReader;
    }
}
