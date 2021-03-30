using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks , IOnEventCallback
{
    public GameProxy GameProxy;

    public Canvas Canvas;
    public event Action<bool> RoomJoinEvent;

    public GameObject GameMenuView;
    
    public GameObject HUD;
    
    public string TowerPrefab;

    public GameObject CellPrefab;

    public string KingPrefab;

    public Text TowerCountText;

    public Text PlayerRankText;
    
    public int DeskSize;

    public int TowerCount;

    public FollowCamera Camera;

    public GameObject BattleAgreementPopup;

    private PhotonView _photonView;
    
    private List<Player> _players;
    
    private List<Cell> _cells;
    private List<Cell.CellData> _cellDatas;
    private List<Tower> _towers;
    private Player _localPlayer;
    private bool _mapCreated = false;

    private void Awake()
    {
        GameProxy.GameManager = this;
    }

    private void OnConnectedToServer()
    {
        Debug.Log("OnConnctedToServer");
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1";

        PhotonPeer.RegisterType(typeof(Cell.CellData), 242, Cell.CellData.Serialize, Cell.CellData.Deserialize);
        
        _players = new List<Player>(4);
        _cells = new List<Cell>();
        _towers = new List<Tower>(TowerCount);
        _cellDatas = new List<Cell.CellData>();
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        base.OnJoinedRoom();
        GameMenuView.SetActive(false);
        HUD.SetActive(true);
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 1 && !_mapCreated)
        {
            StartGame();
            _mapCreated = true;
        }
        CreatePlayer();
        

        RoomJoinEvent?.Invoke(true);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.RaiseEvent(42, _cellDatas.ToArray(), new RaiseEventOptions {Receivers = ReceiverGroup.Others},
                new SendOptions {Reliability = true});
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        foreach (var cell in _cells)
        {
            cell.GetComponent<Cell>().ClickEvent -= _localPlayer.OnCellClicked;
        }
        _localPlayer = null;
        
        RoomJoinEvent?.Invoke(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
		
       
        RoomJoinEvent?.Invoke(false);
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        base.OnJoinRandomFailed(returnCode, message);
        
        RoomJoinEvent?.Invoke(false);
    }
	
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
        base.OnCreateRoomFailed(returnCode, message);
        
        RoomJoinEvent?.Invoke(false);
    }
    
    public void StartGame()
    {
        TowerCountText.text = string.Empty;
        PlayerRankText.text = "Ранк игрока : " + PlayerPrefs.GetInt("PlayerRank");
        SpawnCells();
    }
    
    public void CreateGame(string room)
    {
        PhotonNetwork.CreateRoom(room, new RoomOptions(){PublishUserId = true});
    }
    
    public void FindRandomGame()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPlayButtonClick()
    {
        if (PhotonNetwork.CountOfRooms == 0)
        {
            CreateGame("Room" + Random.Range(1, 1000));
        }
        else
        {
             FindRandomGame();
        }
    }
    
    

    private void OnDisable()
    {
        DestroyAll();
    }


    private void SpawnCells()
    {
        var posX = (int)transform.position.x;
        var posy = (int)transform.position.y;
        var typeArray = typeof(Cell.CellTypeEnum).GetEnumValues();
        var oneTypeCell = Random.Range(3, 12);
        var spawningCell = (Cell.CellTypeEnum) typeArray.GetValue(Random.Range(0, typeArray.Length));
        
        for (int i = 0; i < DeskSize; i++)
        {
            for (int j = 0; j < DeskSize; j++)
            {
                if (oneTypeCell == 0)
                {
                    oneTypeCell = Random.Range(3, 12);
                    spawningCell = (Cell.CellTypeEnum) typeArray.GetValue(Random.Range(0, typeArray.Length));
                }
                var cellData = new Cell.CellData(posX, posy, spawningCell);
                _cells.Add(Cell.CreateCell(CellPrefab, cellData));
                _cellDatas.Add(cellData);


                posy += 11;
                oneTypeCell--;
            }
            posX += 11;
            posy = (int)transform.position.y;
        }
    }

    private void CreatePlayer()
    {
        var king = PhotonNetwork.Instantiate(KingPrefab, new Vector3(transform.position.x + (Random.Range(0, 6) * 11),
            transform.position.y + (Random.Range(0, 6) * 11)), Quaternion.identity);
        _localPlayer = king.GetComponent<Player>();
        if (_localPlayer != null)
        {
            _players.Add(_localPlayer);
            Camera.SetFollowObject(_localPlayer.gameObject);
            _localPlayer.TowerHoldedEvent += i =>
            {
                TowerCountText.text = "Захвачено башен : " + i;
            };
            
            foreach (var cell in _cells)
            {
                cell.GetComponent<Cell>().ClickEvent += _localPlayer.OnCellClicked;
            }
            var go = PhotonNetwork.Instantiate(TowerPrefab, _localPlayer.transform.position + Vector3.right, Quaternion.identity);
            var tower = go.GetComponent<Tower>(); 
            tower.SetOwnerID(PhotonNetwork.LocalPlayer.UserId);
        }
    }
    
    

    private void DestroyAll()
    {
        foreach (var cell in _cells)
        {
            Destroy(cell?.gameObject);
        }

        foreach (var king in _players)
        {
            Destroy(king?.gameObject);
        }

        foreach (var tower in _towers)
        {
            Destroy(tower?.gameObject);
        }
    }
    

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 42 && _cells.Count == 0)
        {
            _cellDatas = new List<Cell.CellData>((Cell.CellData[]) photonEvent.CustomData);
            SpawnCellsByData();
        }
    }

    private void SpawnCellsByData()
    {
        foreach (var data in _cellDatas)
        {
            var cell = Cell.CreateCell(CellPrefab, data);
            _cells.Add(cell);
            cell.ClickEvent += _localPlayer.OnCellClicked;
        }
        Debug.Log(_cells.Count + " was recieved");
    }

    private Photon.Realtime.Player FindPLayerByUserId(string userId)
    {
        Photon.Realtime.Player result = null;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.UserId == userId)
            {
                result = player;
            }
        }

        return result;
    }

    public void SendInviteToPlayer(string fromId, string toId)
    {
        var target = FindPLayerByUserId(toId);
        if(target != null)
            _photonView.RPC("Invite",target, fromId);
    }

    [PunRPC]
    private void Invite(string invitorId)
    {
        var win = Instantiate(BattleAgreementPopup, Canvas.transform);
        win.GetComponent<BattleAgreementPopup>().SetData(invitorId);
    }

    public void SendAnswerToInvitor(string invitorId, bool agreement)
    {
        var target = FindPLayerByUserId(invitorId);
        if (target != null)
        {
            _photonView.RPC("SendAnswer", target, agreement);
        }
    }

    [PunRPC]
    private void SendAnswer(bool agreement)
    {
        Debug.LogError("Player answe : " + agreement);
    }
    
}
