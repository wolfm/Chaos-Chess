using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    //public Material boardMaterial;
    public GameObject referenceTile;
    public int rows = 8;
    public int cols = 8;
    public Color[] tileColors = { Color.black, Color.white };

    [HideInInspector]
    public float scaleFactor;

    [HideInInspector]
    public Tile[,] tiles;

    private HashSet<Vector2Int> highlightedTargets;
    public Tile selectedTile;

    public HashSet<Vector2Int> threatened;
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    private Piece whiteKing;
    private Piece blackKing;

    public GameController game;

    private void Awake()
    {
        highlightedTargets = new HashSet<Vector2Int>();
        game = FindObjectOfType<GameController>();
        whitePieces = new List<Piece>();
        blackPieces = new List<Piece>();
        threatened = new HashSet<Vector2Int>();
    }

    // Draw the board, centered on the transform
    public void DrawBoard()
    {
        // Set up reference tile as a quad with input material
        //GameObject referenceTile = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //referenceTile.GetComponent<Renderer>().material = boardMaterial;

        tiles = new Tile[rows, cols];

        // Generate board with input number of rows and columns, centered at center of screen
        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)  
            {
                GameObject tile = Instantiate(referenceTile, transform);
                tile.GetComponent<SpriteRenderer>().color = tileColors[(r + c) % 2];

                tiles[r, c] = tile.GetComponent<Tile>();
                tiles[r, c].row = r;
                tiles[r, c].col = c;
                tiles[r, c].board = this;
                tiles[r, c].normalColor = tileColors[(r + c) % 2];

                float posX = (c + 0.5f) - (cols / 2.0f);
                float posY = (r + 0.5f) - (rows / 2.0f);

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        // Move board into view
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = 2 * Camera.main.ViewportToWorldPoint(topRightCorner);

        bool portrait = edgeVector.x < edgeVector.y;
        float shortestScreenDim = Mathf.Min(edgeVector.x, edgeVector.y);
        int numTilesInDim = portrait ? cols : rows;

        // Scale factor = (size we want the board to be) / (size the board is)
        scaleFactor = (shortestScreenDim) / (numTilesInDim  + 2);

        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }

    public void spawnPiece(int r, int c, Team team, string piece)
    {
        string teamColor = team == Team.WHITE ? "White" : "Black";
        string location = "Pieces/" + piece + "/" + teamColor + piece;
        Piece newpiece = ((GameObject)Instantiate(Resources.Load(location))).GetComponent<Piece>();

        if (team == Team.WHITE)
        {
            whitePieces.Add(newpiece);
            if (piece == "King")
            {
                newpiece.isKingPiece = (piece == "King");
                whiteKing = newpiece;
            }
        }
        else
        {
            blackPieces.Add(newpiece);
            if (piece == "King")
            {
                newpiece.isKingPiece = (piece == "King");
                blackKing = newpiece;
            }
        }

        // tiles[r, c].Piece = newpiece;
        newpiece.transform.localScale *= scaleFactor;
        newpiece.team = team;
        newpiece.type = piece;
        // newpiece.Tile = tiles[r, c];
        newpiece.moveToTile(tiles[r, c]);
    }

    public void HighlightTiles(Piece piece)
    {
        UnhighlightTiles();

        // Select newly selected tile
        piece.tile.State = TileState.SELECTED;
        selectedTile = piece.tile;

        // Get the next array of targets
        HashSet<Vector2Int> targets = piece.GetTargets();

        //Highlight the next wave of targets
        foreach (Vector2Int target in targets)
        {
            tiles[target.y, target.x].State = TileState.TARGETED;
        }

        // Set the highlighted targets list to be the new highlighted targets list
        highlightedTargets = targets;
    }

    public void UnhighlightTiles()
    {
        // Un-highlight already highlighted targets
        foreach (Vector2Int target in highlightedTargets)
        {
            tiles[target.y, target.x].State = TileState.NONE;
        }

        // Unselect previous tile
        if (selectedTile) selectedTile.State = TileState.NONE;
    }

    /*
     * Returns true if game over
     */
    public bool CheckEndOfTurnConditions(Team attackingTeam)
    {
        Piece king = attackingTeam == Team.WHITE ? blackKing : whiteKing;
        bool check = CheckForCheck(attackingTeam, king.tile.row, king.tile.col);

        if (check)
        {
            if(!ValidNonCheckMoveExists(attackingTeam, king))
            {
                Debug.Log("Checkmate");
                return true;
            }
        }
        else
        {
            if(!ValidNonCheckMoveExists(attackingTeam, king))
            {
                Debug.Log("Stalemate");
                return true;
            }
        }

        return false;
    }

    // Returns true if valid non-check move exists
    private bool ValidNonCheckMoveExists(Team attackingTeam, Piece king)
    {
        List<Piece> defenders = (attackingTeam == Team.WHITE) ? blackPieces : whitePieces;

        // For each defending piece (including the king)
        foreach(Piece defender in defenders)
        {
            HashSet<Vector2Int> moves = defender.GetTargets();

            // Save this piece's current position
            Tile originalTile = tiles[defender.tile.row, defender.tile.col];

            foreach (Vector2Int move in moves)
            {
                // Simulate the move
                defender.simulateMove(tiles[move.y, move.x]);

                // If this move isn't check, we've found a valid non-check move! return true
                if (!CheckForCheck(attackingTeam, king.tile.row, king.tile.col))
                {
                    // Move back to original position
                    defender.rewindSimulatedMove();

                    /*
                    defender.tile.Piece = null;
                    defender.tile = tiles[orig_r, orig_c];
                    defender.tile.Piece = defender;
                    */

                    return true;
                }

                // Move back to original position   
                defender.rewindSimulatedMove();
            }


            /*
            defender.tile.Piece = null;
            defender.tile = tiles[orig_r, orig_c];
            defender.tile.Piece = defender;
            */
        }

        // We found no non-check moves. Return false
        return false;
    }

    // Returns true if check
    private bool CheckForCheck(Team attackingTeam, int king_r, int king_c)
    {

        CalculateThreatened(attackingTeam == Team.WHITE ? whitePieces : blackPieces);

        // Debug.Log($"Checking position {king_r}, {king_c} for check");

        if (threatened.Contains(new Vector2Int(king_c, king_r)))
        {
            Debug.Log("Check");
            return true;
        }

        return false;
    }
    public void CalculateThreatened(List<Piece> attackers)
    {
        threatened.Clear();

        foreach(Piece piece in attackers)
        {
            threatened.UnionWith(piece.GetTargets());
        }
    }
}
