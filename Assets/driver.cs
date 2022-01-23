using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;

public class driver : MonoBehaviour {

    GameObject gObj;
    //contain data from .txt file in a line by line basis
    List<string> fileRow = new List<string>();
    //set up a vector of questions
    List<Question> listOfQuestions = new List<Question>();
    static int conditions = -1;

    // Use this for initialization
    void Start()
    {
        run();
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
        Question q = new Question();
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
        string Intro = "You are encouraged to interact with the interface to familiarize yourself with the format of the quizzes that will come up after the lectures. When you are ready to move to the next page, please click on the \"Next\" button below:\n Answer* \n Answer \n Answer \n Answer \n After you pick an answer, your score will be updated and you will be directed to the next question. When you are ready to move to the next page, please click on the \"Next\" button below: \n Answer2* \n Answer2 \n Answer2 \nAnswer2";
        string quiz1Contents = "1- What has been around for thousands of years and is at the heart of worldwide communications today? \n Cryptography* \n Locks \n Internet Protocol (IP) \n Homing Peigeons \n 2- Substitution cipher is believed to be used first by: \n Julius Caesar* \n Nero \n Alexander \n Xerxes \n 3- Using Caesar's cipher with 1 shift, what would be the encryption of the word \"send\" ? \n tfoe* \n rdmc \n dnes \n ends \n 4- What was responsible for breaking Caesar's cipher? \n Frequency analysis* \n Enigma machine \n Super computers \n Brute force approach \n 5- In random shifts, what should the length of list of shifts be? \n Same as the message* \n Shorter \n Longer \n Any length \n 6- Which one is a strength of random shifts? \n No repettition pattern \n No leak \n Uniform frequency distribution \n All* \n 7- What is the initial Enigma machine state known as? \n Key setting* \n Key space \n State space \n Machine configuration \n 8- What machine allowed the allies to read German commands within hours? \n Enigma machine* \n Pseudo machine \n One-time pad \n Cipher \n 9- What does reduce the key space? \n Repetition* \n Frequency \n Throwing dice \n One-time pad \n 10- In case of perferct secrecy, which one of the following has a different size? \n Message space \n Key space \n Ciphertext space* \n Machine state \n 11- One of the problems of one-time pad is that long keys must be shared in advance. What is a solution to this problem? \n Pseudo randomness* \n Perfect secrecy \n Enigma encryption \n Cipher \n 12- Which one is NOT a feature of random walk? \n Random sequence \n No pattern \n Net move is unpredictable \n Deterministic process \n 13- Who was involved in running computations for the US military which resulted in the design of Hydrogen bombs? \n Turing \n Einstein \n Neumann* \n Shannon \n 14- In pseudorandom number generation, the randomness of the sequence is dependent on: \n Initial seed* \n Length of the sequence \n Patterns in each sequence \n Balanced seed distribution \n 15- The length before a pseudorandom sequence repeats is called: \n Period* \n Frequency \n Cipher \n Cycle \n Moving from randomness to pseudorandomness shrinks the ..... : \n Key space* \n Message space \n Ciphertext space \n Seed space";
        string quiz2Contents = "1- All ..... are based on repetitive patterns which divides the flow of time into equal segments: \n Clocks* \n Ciphers \n Encryptions \n Pseudorandom numbers \n 2- Our ancestors kept track of passage of time by looking at the ....: \n Stars* \n Clouds \n Birds \n Direction of Rivers \n 3- The number of days between full moons is equal to: \n 29* \n 28 \n 30 \n 31 \n 4- A number that can be divided only by itself and 1 is called a .... number: \n Prime* \n Composite \n Real \n Imaginary \n 5- All numbers are built out of smaller, .... numbers: \n Prime* \n Composite \n Real \n Natural \n 6- Pick any number, and find all the prime numbers it divides into equally. What is this process called? \n Factorization* \n Elimination \n Commination \n Multiplication \n 7- Applications such as money transfer require .... to remain secure: \n Encryption* \n Cryptography \n Factorization \n Key space \n 8- Easy in one direction, hard in the reverse direction is the basis of: \n One-way function* \n Encryption \n Cryptography \n Factorization \n 9- In modular arithmetic which number is known as the generator? \n 3* \n 2 \n 5 \n 10  \n 10- What kind of keys was cryptography based on up until the 1970? \n Symmetric keys* \n Assymmetric keys \n Public keys \n Private keys \n 11- Encryption is a mapping from some message using a specific key to a ....... message: \n Ciphertext* \n Key space \n Modular \n One-way \n 12- What is NOT one of the drawbacks of symmetric keys? \n Prone to physical distance \n Extra overhead \n Need multiple keys \n Shared publicly* \n 13- What causes a problem to be called hard? \n Unreasonable time complexity* \n Unreasonable space complexity \n Proven to be unsolveable \n No solid basis in math \n 14- Who introduced Phi function? \n Euler* \n Euclid \n Ellis \n Cocks \n 15- If N=p1*p2,, then what is the value of phi(N)? \n (P1- 1)*(p2 - 1)* \n P1*P2 \n p1 - p2 \n p1 + p2 -2 \n 16- Which one is NOT a characteristic of the public exponent, e? \n Small \n Odd \n No shared factor with phi(n) \n e mod (n - 1) ≡ 0* \n 17- In RSA encryption, which of the following pairs is not hidden? \n n and e* \n m and e \n n and d \n m and d \n 18 - Which one of the equations below is correct? \n(3 ^ 15) ^ 13 mod 17 ≡	(3 ^ 13) ^ 15 mod 17 * \n(3 ^ 15) ^ 13 mod 17 ≡	(3 ^ 15) ^ 17 mod 13 \n(3 ^ 15) ^ 13 mod 17 ≡	(15 ^ 13) ^ 3 mod 17 \n(3 ^ 15) ^ 13 mod 17 ≡  (13 ^ 3) ^ 15 mod 17 \n 13- Which one of these is NOT a one-way function? \n Prime factorization \n Mixing colors \n Trap door \n and sth else";

        byte[] byteArray = Encoding.UTF8.GetBytes(quiz1Contents);
        //byte[] byteArray = Encoding.UTF8.GetBytes(quiz2Contents);
        //byte[] byteArray = Encoding.UTF8.GetBytes(Intro);
        MemoryStream stream = new MemoryStream(byteArray);
        StreamReader streamReader = new StreamReader(stream);
        //Debug.LogFormat("Stream Reader:", streamReader.ToString());
        return streamReader;
    }
}
