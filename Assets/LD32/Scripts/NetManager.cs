using UnityEngine;
using System.Collections;

// http://doc-api.exitgames.com/en/pun/current/pun/doc/class_photon_1_1_pun_behaviour.html
public class NetManager : Photon.MonoBehaviour {

	public static NetManager Instance;
	
	public PhotonLogLevel logLevel;
	
	public const string gameType = "LD32CookFight";
	
	public string roomName {
		get; set;
	}
	
	public string playerName {
		get {
			return PhotonNetwork.playerName;
		}
		set {
			PhotonNetwork.playerName = value;
		}
	}
	
	public RoomInfo[] roomList {
		get; private set;
	}
	
	// request host list (callback is OnMasterServerEvent)
	public void RefreshRoomList() {
		roomList = PhotonNetwork.GetRoomList();
		Menu.Instance.DisplayRoomList();
	}
	
	// start a server and register with masterserver
	public void StartRoom() {
		
		if (string.IsNullOrEmpty(roomName) || roomName.Length < 1)
			roomName = "FoodRoom";
		
		PhotonNetwork.CreateRoom(roomName);
		Menu.Instance.state = Menu.State.WaitingForPlayer;
	}
	
	// join an existing server
	public void JoinRoom(RoomInfo room) {
		PhotonNetwork.JoinRoom(room.name);
	}
	
	public void LeaveRoom() {
		PhotonNetwork.LeaveRoom();
	}
	
	void Awake() {
		if (Instance == null) Instance = this;
		else Destroy(this);
	}
	
	void Start() {
		Debug.Log("Connecting to PhotonMaster...");
		PhotonNetwork.logLevel = logLevel;
		PhotonNetwork.ConnectUsingSettings(gameType);
	}
	
	// PUN events
	
	// Called after the connection to the master is established and authenticated but only when PhotonNetwork.autoJoinLobby is false.
	void OnConnectedToMaster() {
		
		Debug.Log("Connected to PhotonMaster.");
		Menu.Instance.state = Menu.State.FindGame;
	}
	
	// Called for any update of the room listing (no matter if "new" list or "update for known" list). Only called in the Lobby state (on master server).
	void OnReceivedRoomListUpdate() {
		roomList = PhotonNetwork.GetRoomList();
	}
	
	// called when joined the PUN lobby
	void OnJoinedLobby() {
		Menu.Instance.state = Menu.State.CreateGame;
	}
	
	// Called when a CreateRoom() call failed. The parameter provides ErrorCode and message (as array). 
	void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
		Menu.Instance.status = (string)codeAndMsg[1];
		Menu.Instance.state = Menu.State.CreateGame;
	}
	
	// Called when a JoinRoom() call failed. The parameter provides ErrorCode and message (as array)
	void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
		Menu.Instance.status = (string)codeAndMsg[1];
		Menu.Instance.state = Menu.State.CreateGame;
	}
	
	// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
	void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
		Menu.Instance.status = (string)codeAndMsg[1];
		Menu.Instance.state = Menu.State.CreateGame;
	}
	
	// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client)
	void OnJoinedRoom() {
		Game.Instance.JoinedGame();
		Menu.Instance.status = "Waiting for opponent...";
		Menu.Instance.state = Menu.State.WaitingForPlayer;
	}
	
	// Called when a remote player entered the room. This PhotonPlayer is already added to the playerlist at this time.
	void OnPhotonPlayerConnected(PhotonPlayer player) {
		Game.Instance.PlayerJoined(player);
		Menu.Instance.status = "Player joined!";
	}
	
	// Called when a remote player left the room. This PhotonPlayer is already removed from the playerlist at this time.
	void OnPhotonPlayerDisconnected(PhotonPlayer player) {
		PhotonNetwork.LeaveRoom();
		Game.Instance.GameStop();
		Menu.Instance.status = "Player disconnected: " + player.name;
		Menu.Instance.state = Menu.State.WaitingForPlayer;
	}
	
	// Called when the local user/client left a room.
	void OnLeftRoom() {
		Game.Instance.GameStop();
		Menu.Instance.state = Menu.State.CreateGame;
	}
	
	// Called when something causes the connection to fail (after it was established), followed by a call to OnDisconnectedFromPhoton(). 
	void OnConnectionFail(DisconnectCause cause) {
		Menu.Instance.status = cause.ToString();
		Game.Instance.GameStop();
		Menu.Instance.state = Menu.State.CreateGame;
	}
	
	// Called if a connect call to the Photon server failed before the connection was established, followed by a call to OnDisconnectedFromPhoton().
	void OnFailedToConnectToPhoton(DisconnectCause cause) {
		Menu.Instance.status = cause.ToString();
		Menu.Instance.state = Menu.State.CreateGame;
	}
	

}
