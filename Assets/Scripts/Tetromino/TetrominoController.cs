using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrominoController : MonoBehaviour
{
    public static event System.Action<(Sprite, Vector2[])> OnNewTetrominoInstantiated;
    public static event Action OnTetrominoMatches;
    public static event Action OnTetrominoOutOfWidth;
   

    [SerializeField] private List<TetrominoData> m_tetrominoData;
    public GameObject m_currentPiece { get; private set; }
    private readonly List<TetrominoData> m_tetrominoBag = new();

    private static readonly int W = 10;
    private static readonly int H = 20;
    private static readonly Transform[,] m_grid = new Transform[W, H];

    private float m_moveTime;
    private float m_moveSpeedInterval = 0.5f;
    private float m_fallTime;
    private float m_timer;

    private int nextPieceIndex = 0;

    private bool m_IsGamePlayActive;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        UIManager.OnLevelStart += OnLevelStart;
    }

    private void OnLevelStart()
    {
        InstantiateNextPiece();
        m_IsGamePlayActive = true;
    }

    private void Initialize()
    {
        
            nextPieceIndex = Random.Range(0, m_tetrominoData.Count);
            FillTetrominoBag();
        
       
    }

    private void Update()
    {
        if (!m_IsGamePlayActive)
            return;

        PieceInputAndSetMovement();

        if (CanSetOnGrid(0, -1))
            PieceParentGridMovement();
        else
        {
            if (IsAboveHeightLimit())
            {
                m_IsGamePlayActive = false;
                OnTetrominoOutOfWidth?.Invoke();
            }

            FillGridArray();
            InstantiateNextPiece();
        }
    }


    private bool IsAboveHeightLimit()
    {
        for (int x = 0; x < W; x++)
        {
            if (m_grid[x, H - 2] != null)
                return true;
        }

        return false;
    }

    private void PieceParentGridMovement()
    {
        HoldAndMove(0, -1, true, m_moveSpeedInterval);

        m_moveTime += Time.deltaTime;
    }

    private void PieceInputAndSetMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && CanSetOnGrid(-1, 0))
            Move(-1, 0);

        else if (Input.GetKeyDown(KeyCode.RightArrow) && CanSetOnGrid(1, 0))
            Move(1, 0);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            Rotate();

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            m_moveSpeedInterval = 0.1f;

        else if (Input.GetKeyUp(KeyCode.DownArrow))
            m_moveSpeedInterval = 0.5f;
    }

    private void FillTetrominoBag()
    {
        m_tetrominoBag.Clear();

        foreach (var tetrominoData in m_tetrominoData)
            m_tetrominoBag.Add(tetrominoData);
    }

    private void InstantiateNextPiece()
    {
        CheckForMatches();

        var randomIndex = nextPieceIndex;

        var _selectedPiece = m_tetrominoBag[randomIndex];
        m_currentPiece = Instantiate(_selectedPiece.tetrominoPrefab, transform.position, quaternion.identity);

        m_tetrominoBag.Remove(_selectedPiece);

        if (m_tetrominoBag.Count == 0) FillTetrominoBag();

        nextPieceIndex = Random.Range(0, m_tetrominoBag.Count);

        var information = m_tetrominoBag[nextPieceIndex].GetTetrominoInformations();
        OnNewTetrominoInstantiated?.Invoke(information);
    }


    private void FillGridArray()
    {
        var blocks = new Transform[m_currentPiece.transform.childCount];

        for (var i = 0; i < m_currentPiece.transform.childCount; i++)
        {
            blocks[i] = m_currentPiece.transform.GetChild(i);
            var blockX = blocks[i].transform.position.x;
            var blockY = blocks[i].transform.position.y;

            var gridX = Mathf.RoundToInt(blockX);
            var gridY = Mathf.RoundToInt(blockY);

            //    Debug.Log(gridX + " " + gridY + " " + blocks[i].name);
            m_grid[gridX, gridY] = blocks[i];
        }
    }

    private void HoldAndMove(int tx, int ty, bool isVertical, float moveSpeedInterval)
    {
        if (m_moveTime > moveSpeedInterval && CanSetOnGrid(tx, ty))
        {
            Move(tx, ty);

            if (isVertical)
                m_moveTime = 0;
        }
    }

    private void Move(int tx, int ty)
    {
        var initialPosition = m_currentPiece.transform.position;
        var newPosition = initialPosition + new Vector3(tx, ty, 0);
        m_currentPiece.transform.position = newPosition;
    }

    private void Rotate()
    {
        m_currentPiece.transform.Rotate(new Vector3(0, 0, 1), 90);

        //var blocks = new GameObject[m_currentPiece.transform.childCount];

        for (var i = 0; i < m_currentPiece.transform.childCount; i++)
        {
            /*blocks[i] = m_currentPiece.transform.GetChild(i).gameObject;
            var blockX = blocks[i].transform.position.x;
            var blockY = blocks[i].transform.position.y;*/

            var block = m_currentPiece.transform.GetChild(i).gameObject;
            var blockX = block.transform.position.x;
            var blockY = block.transform.position.y;


            if (blockX > W)
            {
                Move(-1, 0);
            }
            else if (blockX < 0)
            {
                Move(1, 0);
            }
        }
    }

    private bool CanSetOnGrid(int tx, int ty)
    {
        var canSet = true;
        var blocks = new GameObject[m_currentPiece.transform.childCount];

        for (var i = 0; i < m_currentPiece.transform.childCount; i++)
        {
            blocks[i] = m_currentPiece.transform.GetChild(i).gameObject;
            var blockX = blocks[i].transform.position.x;
            var blockY = blocks[i].transform.position.y;

            var gridX = Mathf.RoundToInt(blockX);
            var gridY = Mathf.RoundToInt(blockY);


            // Debug.Log("GridY " + gridY);
            if (gridX + tx < 0 || gridX + tx >= W || gridY + ty >= H || gridY + ty < 0)
                canSet = false;

           

            else if (gridY + ty < H && gridX + tx < W && m_grid[gridX + tx, gridY + ty] != null)
                canSet = false; //oppucied
        }
        

        // Debug.Log(set);

        // Debug.Log(set);

        return canSet;
    }
    
    

    public void CheckForMatches()
    {
        for (int y = H - 1; y >= 0; y--)
        {
            bool _isFullRow = true;

            for (int x = 0; x < W; x++)
            {
                if (m_grid[x, y] == null)
                {
                    _isFullRow = false;
                    break;
                }
            }

            if (_isFullRow)
            {
                ClearRow(y);
                ShiftRowsDown(y);
            }
        }
    }


    void ClearRow(int row)
    {
        for (int x = 0; x < W; x++)
        {
            Destroy(m_grid[x, row].gameObject);
            m_grid[x, row] = null;
        }

        OnTetrominoMatches?.Invoke();
    }


    void ShiftRowsDown(int clearedRow)
    {
        for (int y = clearedRow + 1; y < H; y++)
        {
            for (int x = 0; x < W; x++)
            {
                if (m_grid[x, y] != null)
                {
                    m_grid[x, y - 1] = m_grid[x, y];
                    m_grid[x, y] = null;
                    m_grid[x, y - 1].position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    
    private void OnDisable()
    {
        UIManager.OnLevelStart -= OnLevelStart;
    }
}