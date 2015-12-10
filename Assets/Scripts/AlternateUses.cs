using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AlternateUses : MonoBehaviour {

	public TextAsset WordFile;
	public GameObject[] Buttons;
	public Color[] Colors;
	public InputField[] responseFields;
	public GameObject selectedWordText;
	public GameObject commonUseText;
	
	public float timer; // 8 minutes, probably?
	private float timerStartVal;
	public Image timerImage;

	private bool taskFinished = false;

	private WordObject[] words;
	private int currentWord;

	// Use this for initialization
	void Awake () {
		timerStartVal = timer;
		words = new WordObject[6];
		readBuiltInFile();
		setWordPanel();
		selectWord(0);
		printDebugLines();
	}

	void Update() { // I don't like tying the timer to framerate but...meh.
		if (timer < 0) {
			taskFinished = true;
			grabResponses();
			loadFin();
		} else {
			timer -= Time.deltaTime;
			timerImage.fillAmount = timer / timerStartVal;
		}
	}

	public void loadFin() {
		SceneManager.LoadScene("Fin");
	}

	// I'm not completely happy with editing design stuff outside of the anims scripts.
	public void setWordPanel() {
		int count = 0;
		Text textObj;
		foreach (GameObject btn in Buttons) {
			btn.GetComponent<Image>().color = Colors[count % 3];
			textObj = btn.GetComponentInChildren<Text>();
			textObj.text = words[count].getWord();
			count++;
		}
	}

	public void selectWord(int selnum) {
		grabResponses();
		clearResponseFields();
		setSelectionPanel(selnum);
		this.currentWord = selnum;
		//printDebugLines();
	}

	public void clearResponseFields() {
		foreach (InputField inpf in responseFields) {
			inpf.text = "";
		}
	}

	public void grabResponses() {
		int i;
		string[] temp = new string[6];
		for (i = 0; i < 6; i++) {
			if (!responseFields[i].Equals("")) {
				temp[i] = responseFields[i].text;
			}
		}
		words[currentWord].setResponses(temp);
	}

	private void setSelectionPanel(int selnum) {
		int count = 0;
		selectedWordText.GetComponent<Text>().text = words[selnum].getWord();
		commonUseText.GetComponent<Text>().text = words[selnum].getCommonUse();
		// Now for input fields
		string[] resp = words[selnum].getResponses();
		foreach (InputField inpf in responseFields) {
			if (resp[count] != null) {
				inpf.text = resp[count];
			}
			count++;
		}
	}

	public void readBuiltInFile() {
		string[] inp;
		string[] temp;
		int i;
		inp = WordFile.text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
		if (inp.Length > 6) {
			Debug.Log("WARNING: Input length longer than 6 words. Everything after the 6th entry will be lost.");
		}
		for (i = 0; i < 6; i++) {
			temp = inp[i].Split('/');
			words[i] = new WordObject(temp[0], temp[1]);
		}
	}

	public void printDebugLines() {
		int i,j;
		for (i = 0; i < 6; i++) {
			Debug.Log(words[i].getWord() + " : " + words[i].getCommonUse());
			string[] tmp = words[i].getResponses();
			for (j = 0; j < 6; j++) {
				if (tmp[j] != null) {
					Debug.Log(tmp[j]);
				}
			}
		}
	}
}

class WordObject {
	private string word;
	private string commonUse;
	public string[] responseArray;

	public WordObject(string w, string s) {
		responseArray = new string[6];
		this.word = w;
		this.commonUse = s;
	}

	public void setCommonUse(string str){
		this.commonUse = str;
	}

	public string getCommonUse(){
		return this.commonUse;
	}

	public void setWord(string str) {
		this.word = str;
	}

	public string getWord() {
		return this.word;
	}

	public void setResponses(string[] resp) {
		int i;
		if (resp.Length > 6) {
			Debug.Log("WHAT ARE YOU DOING?????");
			return;
		}
		for (i = 0; i < 6; i++) {
			responseArray[i] = resp[i];
		}
	}

	public string[] getResponses() {
		return responseArray;
	}
}
