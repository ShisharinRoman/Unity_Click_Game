using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class Node
{
    private const int   WIN_EVUALUTION = 1000;

    private List<Node>  children;
    private double      evaluation;
    private int         nowDepth;
    private int         depth;
    public int Depth
    {
        set => depth = value;
    }
    private Vector2Int  coordTurn;
    public Vector2Int   CoordTurn
    {
        get => coordTurn;
    }

    public Node()
    {
        children =  new List<Node>();
        nowDepth =  1;
        coordTurn = new Vector2Int();
    }

    private void createChildren( int[,] matrix, Vector2Int size, Vector2Int coord )
    {
        children.Add( new Node() );
        Node thisChildren =         children.Last();
        thisChildren.Depth =        depth;
        thisChildren.nowDepth =     nowDepth + 1;
        thisChildren.coordTurn =    coord;
        thisChildren.create( matrix, size );
    }

    private void analysisFollowingPosition( int[,] matrix, Vector2Int size )
    {
        int[,] temp;

        for ( int y = size.y - 1; y >= 0; y-- )
        {
            if ( !Matrix.isExistCell( new Vector2Int( 0, y ), matrix, size ))
                break;
            for ( int x = 0; x < size.x; x++ )
            {
                if ( !Matrix.isExistCell( new Vector2Int( x, y ), matrix, size ))
                    break;
                temp = ( int[,] )matrix.Clone();
                Matrix.modifMatrix( new Vector2Int( x, y ), temp, size );
                createChildren( temp, size, new Vector2Int( x, y ));
            }
        }
    }
    public void create( int[,] matrix, Vector2Int size )
    {
        if ( Field.isEndGame( matrix, size ))
        {
            // The closer the victory, the more valuable it
            evaluation = WIN_EVUALUTION / nowDepth;

            // If the opponent's move, then it is a loss
            if ( nowDepth % 2 == 0 )
                evaluation = -evaluation;

            return;
        }
        // Leaf processing trees
        if ( depth == nowDepth )
        {
            evaluation = Matrix.calcEvaluation( matrix, size );
            return;
        }

        analysisFollowingPosition( matrix, size );
    }

    // Minimax algorithm with alpha beta
    public double work( double alpha, double beta )
    {
        if ( children.Count == 0 )
            return evaluation;

        if ( nowDepth % 2 == 1 )
        {
            evaluation = double.NegativeInfinity;
            foreach ( Node item in children )
            {
                double temp =   item.work( alpha, beta );
                evaluation =    temp > evaluation ? temp : evaluation;
                alpha =         alpha > evaluation ? alpha : evaluation;
                if ( alpha > beta )
                    break;
            }
            return evaluation;
        }
        else
        {
            evaluation = double.PositiveInfinity;
            foreach ( Node item in children )
            {
                double temp =   item.work( alpha, beta );
                evaluation =    temp < evaluation ? temp : evaluation;
                beta =          beta < evaluation ? beta : evaluation;
                if ( alpha > beta )
                    break;
            }
            return evaluation;
        }
    }

    public Vector2Int findBestTurn()
    {
        Vector2Int res = new Vector2Int();

        foreach ( Node item in children )
            if ( evaluation == item.evaluation )
                res = item.coordTurn;

        return res;
    }
}

public class MiniMax
{
    private Node        head;
    private int         depth;
    private double      alpha;
    private double      beta;
    private Vector2Int  bestTurn;
    public Vector2Int   BestTurn
    {
        get { return bestTurn; }
    }

    public int Depth
    {
        set { depth = value; }
    }

    private List<int[,]> uniqueMatrix;
    public void create( int[,] matrix, Vector2Int size )
    {
        alpha =         double.NegativeInfinity;
        beta =          double.PositiveInfinity;
        uniqueMatrix =  new List<int[,]>();
        head =          new Node();
        head.Depth =    depth;
        head.create( matrix, size );
    }
    // Running the minimax algorithm
    public void work()
    {
        head.work( alpha, beta );
        bestTurn = head.findBestTurn();
    }
    public void clear()
    {
        head = null;
    }
}
