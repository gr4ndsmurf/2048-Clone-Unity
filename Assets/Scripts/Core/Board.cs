using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileStateSO[] tileStates;

    [SerializeField] private float animationDuration = 0.1f;

    private List<Tile> tiles = new List<Tile>();

    private TileGrid grid;

    private bool isWaiting;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
    }

    private void Update()
    {
        if (!isWaiting)
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
    }

    public void ClearBoard()
    {
        foreach(Cell cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach(Tile tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
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
                    isChanged |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (isChanged)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Cell newCell = null;
        Cell adjacentCell = grid.GetAdjacentCell(tile.cell,direction);

        while (adjacentCell != null)
        {
            if (adjacentCell.isOccupied)
            {
                if (CanMerge(tile, adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);
                    return true;
                }

                break;
            }

            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.tileState) + 1, 0, tileStates.Length - 1);
        int number = b.number * 2;

        b.SetState(tileStates[index], number);

        AnimateTiles(b, animationDuration);

        GameManager.Instance.IncreaseScore(number);
    }

    private void AnimateTiles(Tile tileToAnimate, float animationDuration)
    {
        tileToAnimate.gameObject.transform.DOScale(1.25f, animationDuration).OnComplete(() =>
        {
            tileToAnimate.gameObject.transform.DOScale(1f, animationDuration);
        });
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.isLocked;
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], Consts.Numbers.NUMBER_2);
        tile.SpawnTile(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private int IndexOf(TileStateSO tileState)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (tileState == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        isWaiting = true;
        float waitingDuration = 0.1f;
        yield return new WaitForSeconds(waitingDuration);
        isWaiting = false;

        foreach (Tile tile in tiles)
        {
            tile.isLocked = false;
        }

        if (tiles.Count != grid.GetSize())
        {
            CreateTile();
        }

        if (CheckForGameOver())
        {
            GameManager.Instance.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if (tiles.Count != grid.GetSize())
        {
            return false;
        }

        foreach (Tile tile in tiles)
        {
            Cell upCell = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            Cell downCell = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            Cell leftCell = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            Cell rightCell = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (upCell != null && CanMerge(tile, upCell.tile))
            {
                return false;
            }
            else if (downCell != null && CanMerge(tile, downCell.tile))
            {
                return false;
            }
            else if (leftCell != null && CanMerge(tile, leftCell.tile))
            {
                return false;
            }
            else if (rightCell != null && CanMerge(tile, rightCell.tile))
            {
                return false;
            }
        }

        return true;
    }
}
