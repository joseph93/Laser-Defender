using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{

    public static int score;

    void Start()
    {
        UpdateScore(0);
    }
    public void UpdateScore(int pts)
    {
        score += pts;
        GetComponent<Text>().text = score.ToString();
    }

    public static void Reset()
    {
        score = 0;
    }
}
