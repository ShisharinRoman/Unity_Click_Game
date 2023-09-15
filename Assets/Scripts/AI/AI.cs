using UnityEngine;
public class AI : MonoBehaviour
{

    private MiniMax     minimax;
    private int         level;
    private Vector2Int  sizeField;
    public Vector2Int   SizeField
    {
        set { sizeField = value; }
    }

    [SerializeField] GameObject field;
    Field                       actionField;
    public int Level
    {
        set { level = value; }
    }

    private void Awake()
    {
        minimax =       new MiniMax();
        actionField =   field.GetComponent<Field>();
    }

    public void makeTurn( int[,] matrix )
    {
        // Creating a tree of possible events
        minimax.Depth = level + 1;
        minimax.create( matrix, sizeField );
        // Finding the best move
        minimax.work();
        actionField.PositionTargetCell = minimax.BestTurn;

        actionField.selectTarget( isAiSelect : true );

        // Clearing the tree of possible events
        minimax.clear();
    }

}
