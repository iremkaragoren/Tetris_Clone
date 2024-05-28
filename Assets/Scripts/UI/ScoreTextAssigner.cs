using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ScoreTextAssigner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_scoreText;
    
    private int m_score;
    public int Score => m_score;
    
    
    private void Start()
    {
        m_score = 0;
    }

    private void OnEnable()
    {
        TetrominoController.OnTetrominoMatches += OnMatches;
        
    }
    
    private void OnDisable()
    {
        TetrominoController.OnTetrominoMatches -= OnMatches;
        
    }

    private void OnMatches()
    {
        m_score ++;
        m_scoreText.text = m_score.ToString();
    }
}
