using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

//this class is used to interact with .txt files
public class fileInteraction : MonoBehaviour {
	//List to hold each line of file
	public List<string> fileLine = new List<string>();
	public string _filePath = "./Assets/";
	public string _fileName;

	//empty constructor
	public fileInteraction(string fileName)
	{
		_fileName = fileName;
	}

	//set the fileName
	public void setFileName(string f)
	{
		_fileName = f;
	}

	public string getFileName()
	{
		return _fileName;
	}

	//read a file and return its contents in a list structure
	public void readFile() {
		//An implicitly typed local variable is strongly typed just as if you had declared the type yourself, 
		//but the compiler determines the type. 
		var sr = File.OpenText(_filePath + _fileName);
		//append each line as an index in the list
		fileLine = sr.ReadToEnd().Split("\n"[0]).ToList();
		//close file
		sr.Close();

		//for (int i = 0; i < fileLine.Count; i++)
		//{
		//	Debug.Log (fileLine [i]);
		//}
	}

	//return list containing each line of the .txt document
	public List<string> getFileData()
	{
		return fileLine;
	}

	void Start()
	{
		

	}
}
