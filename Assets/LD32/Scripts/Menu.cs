using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
	
public class Menu : MonoBehaviour {

    private const string typeName = "LD32CookFight";
    private HostData[] hostList;

	// Spawning
	public GameObject playerPrefab;
	public GameObject spawnPointA;
	public GameObject spawnPointB;

	// UI
	public GameObject NetUI;
	public GameObject hostsUI;
	public GameObject statusTextUI;
	public GameObject hostLineUI;
	public GameObject hostNameInputUI;

	private List<GameObject> hostLines;
	

	void Start () {
		hostLines = new List<GameObject>();
		RefreshHostList();
	}
	

	void Update () {
	
	}

	// User functions
	// ------------------------------------------
    public void RefreshHosts(){
        Debug.Log("Refreshing available hosts");
		statusTextUI.GetComponent<Text>().text = "Looking for hosts...";
        RefreshHostList();
    }

	
	public void JoinGame(HostData hostData)
	{
		Network.Connect(hostData);
	}

	// Network Events
	// ------------------------------------------
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
		NetUI.gameObject.SetActive(false);
		SpawnPlayer(spawnPointB);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
		NetUI.gameObject.SetActive(false);
		SpawnPlayer(spawnPointA);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived){ 
			hostList = MasterServer.PollHostList();
			statusTextUI.GetComponent<Text>().text = "Hosts found: " + hostList.Length;

			// Clean previous host list
			hostLines.ForEach(child => Destroy(child));
			
			int count = 0;
			foreach(HostData host in hostList){
				// Create a new join button
				GameObject hostline = Instantiate(hostLineUI);
				// Keep reference to button so we can delete them on refresh
				hostLines.Add(hostline);
				// Position button correctly as a list (TO DO: Scroll support for large host lists)
				hostline.transform.SetParent(hostsUI.transform, false);
				hostline.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -30f*count);

				// Rename button with Game name chosen by remote hosts
				Transform b = hostline.transform.FindChild("Button");
				b.FindChild("HostLineText").GetComponent<Text>().text = host.gameName;
				hostline.GetComponent<HostInfo>().payload = host;
				Button joinbutton = b.GetComponent<Button>();
				joinbutton.onClick.AddListener(() => JoinGame(host));

				count += 1;
			}
			
		}
		
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else
			if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");
	}
	
	// Utility Functions
	// ------------------------------------------
	private void ReloadGame(){

	}
	
	private void SpawnPlayer(GameObject spawnPoint)
	{
		Network.Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity, 0);
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	public void StartServer()
    {
		string hname = hostNameInputUI.GetComponent<Text>().text;
		if(hname.Length > 0){
			bool useNat = !Network.HavePublicAddress();
			NetworkConnectionError con = Network.InitializeServer(1, 25000, useNat);
			if(con == NetworkConnectionError.NoError){
				MasterServer.RegisterHost(typeName, hname);
				statusTextUI.GetComponent<Text>().text = "Waiting for an opponent...";
			} else {
				statusTextUI.GetComponent<Text>().text = "Failed to start a Kitchen fight. Choose a different Kitchen name!";
			}

		} else {
			statusTextUI.GetComponent<Text>().text = "Please enter a Kitchen name";
		}

    }

}
