using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileStateSO tileState {  get; private set; }
    public Cell cell { get; private set; }
    public int number { get; private set; }
    public bool isLocked { get; set; }

    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text numberText;

    public void SetState(TileStateSO tileState, int number)
    {
        this.tileState = tileState;
        this.number = number;

        backgroundImage.color = tileState.backgroundColor;
        numberText.color = tileState.textColor;
        numberText.text = number.ToString();
    }

    public void SpawnTile(Cell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }
}
