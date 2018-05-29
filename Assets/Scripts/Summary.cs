using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour {

    public Text txtScore;
    public Text txtBest;

    public Image medal;

    public Sprite[] sprites;

	void OnEnable()
    {
        int maxScore = PlayerPrefs.GetInt("max_score", 0);

        int score = GameController.Score();

        if (score > maxScore)
        {
            PlayerPrefs.SetInt("max_score", score);
        }

        txtBest.text = PlayerPrefs.GetInt("max_score", 0).ToString();
        txtScore.text = score.ToString();

        if (score > 40)
        {
            medal.sprite = sprites[4];
        }
        else if (score > 30)
        {
            medal.sprite = sprites[3];
        }
        else if (score > 20)
        {
            medal.sprite = sprites[2];
        }
        else if (score > 10)
        {
            medal.sprite = sprites[1];
        }
        else
        {
            medal.sprite = sprites[0];
        }
    }
}
