using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestScoreAssigner : MonoBehaviour
{
    [SerializeField] private ScoreTextAssigner m_scoreTextAssigner;
    [SerializeField] private TextMeshProUGUI m_bestScoreText;
    
    private int m_bestScore;
    
    private void OnEnable()
    {
        OnBestScoreAssigned();
    }

    
    private void OnBestScoreAssigned()
    {
        m_bestScore= PlayerPrefs.GetInt("HighScore", 0);
        var score = m_scoreTextAssigner.Score;
        Debug.Log("Score: " + score);
        Debug.Log("BestScore " + m_bestScore); 
        if (score >= m_bestScore)
        {
            m_bestScore = score;
            PlayerPrefs.SetInt("HighScore", score);
            m_bestScoreText.text = m_bestScore.ToString();
        }
        else
        {
            m_bestScoreText.text = m_bestScore.ToString();
        }
        
    }

}
