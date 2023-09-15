using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Ñlass that works with the playing field
/// </summary>
public class Field : MonoBehaviour
{
    private const int MAX_SIZE =    10;


    [SerializeField] private GameObject prefabCell;
    private GameObject[,]               cell;
    private Cell[,]                     actionCell;

    [SerializeField] private GameObject ai;
    private AI                          actionAi;

    [SerializeField] private GameObject resultMenu;
    private ResultMenu                  actionResultMenu;

    private Vector2Int  size;
    public Vector2Int   Size
    {
        set { size = value; }
    }

    private Matrix      matrix;
    private bool        isPlayerTurn;
    private Vector2Int  positionGoodCell;
    private Vector2Int  positionTargetCell;
    public Vector2Int   PositionTargetCell
    {
        set => positionTargetCell = value;
    }

    private void Awake()
    {
        matrix =            new Matrix();
        cell =              new GameObject[ MAX_SIZE, MAX_SIZE ];
        actionCell =        new Cell[ MAX_SIZE, MAX_SIZE ];
        actionResultMenu =  resultMenu.GetComponent<ResultMenu>();
        actionAi =          ai.GetComponent<AI>();

        for ( int y = 0; y < MAX_SIZE; y++ )
        {
            for ( int x = 0; x < MAX_SIZE; x++ )
            {
                cell[ y, x ] =        Instantiate( prefabCell );
                actionCell[ y, x ] =  cell[ y, x ].GetComponent<Cell>();
                cell[ y, x ].SetActive( false );
            }
        }
    }

    public void start()
    {
        transform.position =    new Vector2( -size.x / 2 + 0.5F, size.y / 2 - 0.5F );
        matrix.Size =           size;
        matrix.clear();

        for ( int y = 0; y < size.y; y++ )
        {
            for ( int x = 0; x < size.x; x++ )
            {
                cell[ y, x ].SetActive( true );
                cell[ y, x ].transform.position = ( transform.position + new Vector3( x, -y ));
            }
        }
        startCoordSelect();
        isPlayerTurn =  true;
        positionGoodCell = matrix.findBestPosition();
    }
    private void startCoordSelect()
    {
        positionTargetCell = new Vector2Int( 0, size.y - 1 );
    }

    private void moveTarget()
    {
        if ( positionTargetCell.x > 0 )
            if ( matrix.isExistCell( positionTargetCell + Vector2Int.left, matrix.Id ))
                if ( Input.GetKeyDown( KeyCode.LeftArrow ))
                    positionTargetCell.x--;

        if ( positionTargetCell.x < size.x - 1 )
            if ( matrix.isExistCell( positionTargetCell + Vector2Int.right, matrix.Id ))
                if ( Input.GetKeyDown(KeyCode.RightArrow))
                    positionTargetCell.x++;

        if ( positionTargetCell.y > 0 )
            if ( matrix.isExistCell( positionTargetCell + Vector2Int.down, matrix.Id ))
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    positionTargetCell.y--;

        if ( positionTargetCell.y < size.y - 1 )
            if ( matrix.isExistCell( positionTargetCell + Vector2Int.up, matrix.Id ))
                if ( Input.GetKeyDown( KeyCode.DownArrow ))
                    positionTargetCell.y++;
        return;
    }

    public void selectTarget( bool isAiSelect = false )
    {
        if ( Input.GetKeyDown( KeyCode.Space ) || isAiSelect )
        {
            matrix.modifMatrix( positionTargetCell, isPlayerTurn: isPlayerTurn );
            endTurn();
            changeTargetPosition();
            positionGoodCell = matrix.findBestPosition();
            if ( isEndGame() )
            {
                result();
                gameover();
            }
        }
    }

    private bool isEndGame()
    {
        return 
            ( 
            !matrix.isExistCell( new Vector2Int( 0, matrix.Size.y - 1), matrix.Id )
            );
    }

    public static bool isEndGame( int[,] matrix, Vector2Int size )
    {
        return (
            !Matrix.isExistCell( new Vector2Int( 0, size.y - 1 ), matrix, size )
            );
    }

    private void gameover()
    {
        for ( int y = 0; y < size.y; y++ )
            for ( int x = 0; x < size.x; x++ )
                cell[ y, x ].SetActive( false );
        ai.SetActive( false );
        transform.gameObject.SetActive( false );
    }

    private void result()
    {
        resultMenu.SetActive( true );
        actionResultMenu.IsWin = isPlayerTurn;
        actionResultMenu.showResult();
    }

    /// <summary>
    /// Changing the position of the 
    /// selection cell after the move
    /// </summary>
    private void changeTargetPosition()
    {
        if ( positionTargetCell.x != 0 )
            positionTargetCell.x--;
        if ( positionTargetCell.y != size.y - 1 )
            positionTargetCell.y++;
    }
    
    public int[,] getMatrix()
    {
        return matrix.Id;
    }
    private void endTurn()
    {
        isPlayerTurn = !isPlayerTurn;
    }

    void Update()
    {
        if ( isPlayerTurn )
        {
            moveTarget();
            selectTarget();
        }
        else
            actionAi.makeTurn( matrix.Id );

        for ( int y = 0; y < size.y; y++ )
            for ( int x = 0; x < size.x; x++ )
                actionCell[ y, x ].spriteIndex = matrix.Id[ y, x ];

        actionCell[ positionGoodCell.y, positionGoodCell.x ].spriteIndex =     ( int )IdCell.good;
        actionCell[ positionTargetCell.y, positionTargetCell.x ].spriteIndex = ( int )IdCell.select;
    }
}
