using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTetrominoHolder : MonoBehaviour
{
    [SerializeField] private float m_PositionOffset = 50;
    [SerializeField] private float m_SpriteSize = 50;
    
    private Image[] m_pieces;

    private void Awake()
    {
        m_pieces = GetComponentsInChildren<Image>();
    }

    private void OnEnable()
    {
        TetrominoController.OnNewTetrominoInstantiated += HandleNewTetrominoInformation;
    }

    private void HandleNewTetrominoInformation((Sprite, Vector2[]) obj)
    {
        Sprite currentSprite = obj.Item1;
        Vector2[] currentLocalPositions = obj.Item2;

        for (int i = 0; i < m_pieces.Length; i++)
        {
            m_pieces[i].sprite = currentSprite;
            m_pieces[i].rectTransform.sizeDelta = Vector2.one * m_SpriteSize;
            m_pieces[i].rectTransform.anchoredPosition = currentLocalPositions[i] * m_PositionOffset;
        }
    }

    private void OnDisable()
    {
        TetrominoController.OnNewTetrominoInstantiated -= HandleNewTetrominoInformation;
    }
}