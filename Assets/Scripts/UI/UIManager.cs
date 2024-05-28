using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_startScene;
    [SerializeField] private GameObject m_inGameScene;
    [SerializeField] private GameObject m_endGameScene;

    public static event Action OnLevelStart;

    private void OnEnable()
    {
        TetrominoController.OnTetrominoOutOfWidth += OnTetrominoOut;

    }

    private void OnDisable()
    {
        TetrominoController.OnTetrominoOutOfWidth -= OnTetrominoOut;
    }

    private void OnTetrominoOut()
    {
        m_inGameScene.SetActive(false);
        m_endGameScene.SetActive(true);
    }

    public void LevelStart()
    {
        m_startScene.SetActive(false);
        m_inGameScene.SetActive(true);
        OnLevelStart?.Invoke();

    }
   
   
   
   public void RetryLevel()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
   }
}
