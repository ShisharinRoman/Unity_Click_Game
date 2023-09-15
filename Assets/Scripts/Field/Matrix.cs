using UnityEngine;

/// <summary>
/// Ñlass working with the game matrix, 
/// which is the basis for the playing field
/// </summary>
/// 
public class Matrix
{
    private const int       MAX_SIZE =              10;
    private const double    MAX_ASSYMETRY =         0.9;  
    

    private int[,]  id;
    public int[,]   Id
    {
        get { return id; }
        set { id = value; }
    }
    private double[,]   evaluation;
    public double[,]    Evaluation
    {
        get { return evaluation; }
        set { evaluation = value; }
    }
    private Vector2Int  size;
    public Vector2Int   Size
    {
        get { return size; }
        set { size = value; }
    }

    public Matrix()
    {
        id =            new int[MAX_SIZE, MAX_SIZE];
        evaluation =    new double[MAX_SIZE, MAX_SIZE];
        size =              new Vector2Int();
    }

    public bool isExistCell( Vector2Int position, int[,] matrix = null )
    {
        matrix ??= Id;

        if (
            ( position.x > size.x ) || 
            ( position.x < 0 ) || 
            ( position.y > size.y ) || 
            ( position.y < 0 ) ||
            ( matrix[ position.y, position.x ] != ( int )IdCell.free )
            )
            return false;
        return true;
    }
    public static bool isExistCell(Vector2Int position, int[,] matrix, Vector2Int size )
    {
        if (
            ( position.x > size.x ) ||
            ( position.x < 0 ) ||
            ( position.y > size.y ) ||
            ( position.y < 0 ) ||
            ( matrix[ position.y, position.x ] != ( int )IdCell.free )
            )
            return false;
        return true;
    }
    public int calcNumbCell( int[,] matrix = null )
    {
        matrix ??= Id;

        int res = 0;
        for ( int y = size.y - 1; y >= 0; y-- )
        {
            if ( !isExistCell( new Vector2Int( 0, y ), matrix ))
                break;
            for (int x = 0; x < size.x; x++)
            {
                if ( !isExistCell( new Vector2Int( x, y ), matrix ))
                    break;
                res++;
            }
        }
        return res;
    }
    public static int calcNumbCell( int[,] matrix, Vector2Int size )
    {
        int res = 0;
        for ( int y = size.y - 1; y >= 0; y-- )
        {
            if ( !isExistCell( new Vector2Int( 0, y ), matrix, size ))
                break;
            for (int x = 0; x < size.x; x++)
            {
                if ( !isExistCell( new Vector2Int( x, y ), matrix, size ))
                    break;
                res++;
            }
        }
        return res;
    }

    /// <summary>
    /// Method that calculates the asymmetry
    /// of the matrix along the side diagonal
    /// </summary>
    private int calcAssymetric( int[,] matrix = null )
    {
        matrix ??=  Id;
        int res =   0;

        for ( int y = size.y - 1; y >= 0; y-- )
            for ( int x = ( size.y - y ); x < size.x; x++ )
                // Checking the coincidence of opposite
                // cells along the side diagonal
                if ( isExistCell( new Vector2Int( x, y ), matrix ) != isExistCell( new Vector2Int( size.x - x - 1, size.y - y - 1 ), matrix ))
                    res++;
        return res;
    }

    /// <summary>
    /// Method that calculates the asymmetry
    /// of the matrix along the side diagonal
    /// </summary>
    private static int calcAssymetric( int[,] matrix, Vector2Int size )
    {
        int res = 0;

        for ( int y = size.y - 1; y >= 0; y-- )
            for ( int x = ( size.y - y ); x < size.x; x++ )
                // Checking the coincidence of opposite
                // cells along the side diagonal
                if ( isExistCell( new Vector2Int( x, y ), matrix, size ) != isExistCell( new Vector2Int( size.x - x - 1, size.y - y - 1 ), matrix, size ))
                    res++;
        return res;
     }

    private double calcEvaluation( int[,] matrix = null )
    {
        matrix ??= Id;

        double res;
        int numbCell =      calcNumbCell( matrix );
        double assymetry =  calcAssymetric( matrix ) / ( double )numbCell;

        // The formula for calculating
        // the evaluation of the current
        // situation
        res = ( MAX_ASSYMETRY / ( assymetry + 0.001 )) + ( 1 / numbCell );

        // If the number of cells is even,
        // then the score is the opposite
        // of the calculated one
        if ( numbCell % 2 == 0 ) res *= -1;

        return res;
    }

    public static double calcEvaluation( int[,] matrix, Vector2Int size )
    {
        double res;
        int numbCell =      calcNumbCell( matrix, size );
        double assymetry =  calcAssymetric( matrix, size ) / ( double )numbCell;

        // The formula for calculating
        // the evaluation of the current
        // situation
        res = ( MAX_ASSYMETRY / ( assymetry + 0.001 )) + ( 1 / numbCell );

        // If the number of cells is even,
        // then the score is the opposite
        // of the calculated one
        if ( numbCell % 2 == 0 ) res *= -1;

        return res;
    }

    /// <summary>
    /// Method that changes the 
    /// matrix for the next move 
    /// </summary>
    public void modifMatrix( Vector2Int position, int[,] matrix = null, bool isPlayerTurn = true )
    {
        matrix ??= Id;

        for ( int y = position.y; y >= 0; y-- )
            for ( int x = position.x; x < size.x; x++ )
                if ( isExistCell( new Vector2Int( x, y )))
                    if ( isPlayerTurn )
                        matrix[ y, x ] = ( int )IdCell.playerOne;
                    else
                        matrix[ y, x ] = ( int )IdCell.playerTwo;
        return;
    }

    /// <summary>
    /// Method that changes the 
    /// matrix for the next move 
    /// </summary>
    public static void modifMatrix( Vector2Int coord, int[,] matrix, Vector2Int size, bool isPlayerTurn = true )
    {
        for ( int y = coord.y; y >= 0; y-- )
            for ( int x = coord.x; x < size.x; x++ )
                if ( isExistCell( new Vector2Int( x, y ), matrix, size ))
                    if ( isPlayerTurn )
                        matrix[ y, x ] = ( int )IdCell.playerOne;
                    else
                        matrix[ y, x ] = ( int )IdCell.playerTwo;
        return;
    }

    /// <summary>
    /// Method that calculates an estimate 
    /// of the next situation after selecting 
    /// each cell in the matrix.
    /// </summary>
    private void calcEvaluateMatrix()
    {
        int[,] temp;

        temp = new int[ MAX_SIZE, MAX_SIZE ];

        for ( int y = 0; y < size.y; y++ )
            for ( int x = 0; x < size.x; x++ )
                if ( isExistCell( new Vector2Int( x, y )) && !( x == 0 && y == size.y - 1 ))
                {
                    temp = ( int[,] )Id.Clone();
                    modifMatrix( new Vector2Int( x, y ), temp );
                    evaluation[ y, x ] = calcEvaluation( temp );
                }
                else
                    evaluation[ y, x ] = double.MinValue;
    }

    /// <summary>
    /// Method that calculates an estimate 
    /// of the next situation after selecting 
    /// each cell in the matrix.
    /// </summary>
    private static void calcEvaluateMatrix( int[,] matrix, double[,] evaluationMatrix, Vector2Int size )
    {
        int[,] temp;
        temp = new int[ MAX_SIZE, MAX_SIZE ];

        for ( int y = 0; y < size.y; y++ )
            for ( int x = 0; x < size.x; x++ )
                if ( isExistCell( new Vector2Int( x, y ), matrix, size ) && !( x == 0 && y == size.y - 1 ))
                {
                    temp = ( int[,] )matrix.Clone();
                    modifMatrix( new Vector2Int( x, y ), matrix, size );
                    evaluationMatrix[ y, x ] = calcEvaluation( temp, size );
                }
                else
                    evaluationMatrix[ y, x ] = double.MinValue;
    }
    public Vector2Int findBestPosition()
    {
        Vector2Int res =    size - new Vector2Int( size.x, 1 );
        double max =        double.MinValue;

        calcEvaluateMatrix();

        for ( int y = 0; y < size.y; y++ )
            for ( int x = 0; x < size.x; x++ )
                if ( evaluation[ y, x ] > max )
                {
                    max = evaluation[ y, x ];
                    res.Set( x, y );
                }
        return res;
    }

    public static Vector2Int findBestPosition( int[,] matrix, Vector2Int size )
    {
        Vector2Int res =                Vector2Int.zero;
        double max =                    double.MinValue;
        double[,] evaluationMatrix =    new double[ size.x, size.y ];

        calcEvaluateMatrix( matrix, evaluationMatrix, size );

        for ( int x = 0; x < size.x; x++ )
            for ( int y = 0; y < size.y; y++ )
                if ( evaluationMatrix[ y, x ] >= max )
                {
                    max = evaluationMatrix[ y, x ];
                    res.Set( x, y );
                }
        return res;
    }
    public void clear()
    {
        for ( int y = 0; y < size.y; y++ )
            for ( int x = 0; x < size.x; x++ )
                id[y, x] = ( int )IdCell.free;
        return;
    }

    public static void clear(int[,] matrix, Vector2Int size ) 
    {
        for ( int y = 0; y < size.y; y++ )
            for ( int x = 0; x < size.x; x++ )
                matrix[ y, x ] = ( int )IdCell.free;
    }

}
