using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

    private Text myText;

	// Use this for initialization
	void Start ()
	{
	    myText = GetComponent<Text>();
	    myText.text = ScoreKeeper.score.ToString();
        ScoreKeeper.Reset();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
