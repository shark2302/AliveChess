using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public GameObject TowerPrefab;

    public GameObject[] CellPrefabs;

    public GameObject KingPrefab;

    public Text TowerCountText;

    public Text PlayerRankText;
    
    public int DeskSize;

    public int TowerCount;

    public FollowCamera Camera;
    
    private List<Player> _kingsMovements;
    
    private List<Cell> _cells;
    private List<Tower> _towers;
    private void OnEnable()
    {
        TowerCountText.text = string.Empty;
        PlayerRankText.text = "Ранк игрока : " + PlayerPrefs.GetInt("PlayerRank");
        _kingsMovements = new List<Player>(4);
        _cells = new List<Cell>();
        _towers = new List<Tower>(TowerCount);
        SpawnKing();
        SpawnCells();
        SpawnTowers();
        Camera.SetFollowObject(_kingsMovements[0].gameObject);
    }

    private void OnDisable()
    {
        foreach (var cell in _cells)
        {
            UnsubscribeCell(cell);
        }

        foreach (var tower in _towers)
        {
            UnsubscribeKingOnTowerClick(tower);
        }
        DestroyAll();
    }
    

    private void SpawnCells()
    {
        var posX = transform.position.x;
        var posy = transform.position.y;
        
        var oneTypeCell = Random.Range(3, 12);
        var spawningCellPrefab = CellPrefabs[Random.Range(0, CellPrefabs.Length)];
        
        for (int i = 0; i < DeskSize; i++)
        {
            for (int j = 0; j < DeskSize; j++)
            {
                if (oneTypeCell == 0)
                {
                    oneTypeCell = Random.Range(3, 12);
                    spawningCellPrefab = CellPrefabs[Random.Range(0, CellPrefabs.Length)];
                }
                
                var go = Instantiate(spawningCellPrefab, new Vector3(posX, posy), Quaternion.identity, transform);
                var cell = go.GetComponent<Cell>();
                
                if (cell != null)
                {
                    _cells.Add(cell);
                    SubscribeCellOnKingMovement(cell);
                }

                posy += 11;
                oneTypeCell--;
            }
            posX += 11;
            posy = transform.position.y;
        }
    }

    private void SpawnKing()
    {
        var king = Instantiate(KingPrefab, new Vector3(transform.position.x + (Random.Range(0, 25) * 11),
            transform.position.y + (Random.Range(0, 25) * 11)), Quaternion.identity);
        var movement = king.GetComponent<Player>();
        if (movement != null)
        {
            _kingsMovements.Add(movement);
            movement.TowerHoldedEvent += i =>
            {
                TowerCountText.text = "Захвачено башен : " + i;
                if (i == TowerCount)
                {
                    RankUp();
                }
            };

        }
    }

    private void SpawnTowers()
    {
        var xCell = Random.Range(2, 7);
        var yCell = Random.Range(2, 7);
        var tCount = TowerCount;
        while (tCount > 0)
        {
            var go = Instantiate(TowerPrefab, new Vector3(xCell * 11 + 3, yCell * 11 - 2), Quaternion.identity);
            var tower = go.GetComponent<Tower>();
            SubscribeKingOnTowerClick(tower);
            _towers.Add(tower);
            var rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    xCell += Random.Range(2,7);
                    break;
                case 1:
                    yCell += Random.Range(2, 7);
                    break;
                case 2:
                    xCell += Random.Range(2, 7);
                    yCell += Random.Range(2, 7);
                    break;
            }
            tCount--;
        }
    }

    public void RankUp()
    {
        PlayerPrefs.SetInt("PlayerRank", PlayerPrefs.GetInt("PlayerRank") + 1);
        OnDisable();
        OnEnable(); 
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

    private void SubscribeKingOnTowerClick(Tower tower)
    {
        foreach (var king in _kingsMovements)
        {
            tower.TowerClickEvent += king.OnTowerClicked;
        }    
    }
    
    private void UnsubscribeKingOnTowerClick(Tower tower)
    {
        foreach (var king in _kingsMovements)
        {
            tower.TowerClickEvent -= king.OnTowerClicked;
        }    
    }

    private void DestroyAll()
    {
        foreach (var cell in _cells)
        {
            Destroy(cell?.gameObject);
        }

        foreach (var king in _kingsMovements)
        {
            Destroy(king?.gameObject);
        }

        foreach (var tower in _towers)
        {
            Destroy(tower?.gameObject);
        }
    }

}
