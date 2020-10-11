using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject CellPrefab;

    public GameObject KingPrefab;

    public int DeskSize;
    
    private List<KingMovement> _kingsMovements;
    
    private List<Cell> _cells;
    public List<KingMovement> KingsMovements => _kingsMovements;

    private void OnEnable()
    {
        _kingsMovements = new List<KingMovement>(4);
        _cells = new List<Cell>();
        SpawnKing();
        SpawnCells();
        
    }

    private void OnDisable()
    {
        foreach (var cell in _cells)
        {
            UnsubscribeCell(cell);
        }
    }


    private void SpawnCells()
    {
        var posX = transform.position.x;
        var posy = transform.position.y;
        for (int i = 0; i < DeskSize; i++)
        {
            for (int j = 0; j < DeskSize; j++)
            {
                var go = Instantiate(CellPrefab, new Vector3(posX, posy), Quaternion.identity, transform);
                var cell = go.GetComponent<Cell>();
                if (cell != null)
                {
                    _cells.Add(cell);
                    SubscribeCellOnKingMovement(cell);
                }

                posy += 11;
            }
            posX += 11;
            posy = transform.position.y;
        }
    }

    private void SpawnKing()
    {
        var king = Instantiate(KingPrefab, transform.position, Quaternion.identity);
        var movement = king.GetComponent<KingMovement>();
        if(movement != null)
            _kingsMovements.Add(movement);
    }

    private void SubscribeCellOnKingMovement(Cell cell)
    {
        foreach (var king in _kingsMovements)
        {
            cell.ClickEvent += king.OnClicked;
        }
    }

    private void UnsubscribeCell(Cell cell)
    {
        foreach (var king in _kingsMovements)
        {
            cell.ClickEvent -= king.OnClicked;
        }    
    }



}
