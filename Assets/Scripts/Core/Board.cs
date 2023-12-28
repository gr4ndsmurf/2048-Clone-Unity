using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileStateSO[] tileStates;

    private List<Tile> tiles = new List<Tile>();

    private TileGrid grid;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
    }
    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTiles(Vector2Int.down, 0, 1, grid.GetHeight() - 2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTiles(Vector2Int.right, grid.GetWidth() - 2, -1, 0, 1);
        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool isChanged = false;

        for (int x = startX; x >= 0 && x < grid.GetWidth(); x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.GetHeight(); y += incrementY)
            {
                Cell cell = grid.GetCell(x, y);

                if (cell.isOccupied)
                {
                    //isChanged |= MoveTile();
                }
            }
        }

        if (isChanged)
        {
            //WAIT FOR CHANGES
        }
    }

    /*private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Cell newCell = null;
        Cell adjacentCell = grid.GetAdjacentCell(tile.cell,direction);

        while (adjacentCell != null)
        {
            if (adjacentCell.isOccupied)
            {
                //TODO : MERGE
            }

            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }
    }*/

    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], Consts.Numbers.NUMBER_2);
        tile.SpawnTile(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
}
