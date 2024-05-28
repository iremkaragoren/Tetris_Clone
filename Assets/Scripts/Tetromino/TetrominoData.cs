using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TetrominoData", menuName = "Tetris/Tetromino Data")]
public class TetrominoData : ScriptableObject
{
    public GameObject tetrominoPrefab;

    [SerializeField] private Sprite pieceTetromino;

    public (Sprite, Vector2[]) GetTetrominoInformations()
    {
        Vector2[] positions = new Vector2[4];

        for (int i = 0; i < tetrominoPrefab.transform.childCount; i++)
            positions[i] = tetrominoPrefab.transform.GetChild(i).localPosition;

        return (pieceTetromino, positions);
    }
}