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
	
	// Use this for initialization
	void Start () {
		hostLines = new List<GameObject>();
		RefreshHostList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RefreshHosts(){
        Debug.Log("Refreshing available hosts");
		statusTextUI.GetComponent<Text>().text = "Looking for hosts...";
        RefreshHostList();
    }

	
	public void JoinGame(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
		NetUI.gameObject.SetActive(false);
		SpawnPlayer(spawnPointB);
	}

	private void SpawnPlayer(GameObject spawnPoint)
	{
		Network.Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity, 0);
	}
	

    // Host a game
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

    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
    }

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
		NetUI.gameObject.SetActive(false);
		SpawnPlayer(spawnPointA);
	}
	
	//----------------------------------------------
    // Host browser

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived){ 
            hostList = MasterServer.PollHostList();
			statusTextUI.GetComponent<Text>().text = "Hosts found: " + hostList.Length;

			hostLines.ForEach(child => Destroy(child));

			int count = 0;
			foreach(HostData host in hostList){

				GameObject hostline = Instantiate(hostLineUI);
				hostLines.Add(hostline);
				hostline.transform.SetParent(hostsUI.transform, false);
				hostline.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -30f*count);
				Transform b = hostline.transform.FindChild("Button");
				b.FindChild("HostLineText").GetComponent<Text>().text = host.gameName;
				hostline.GetComponent<HostInfo>().payload = host;
				count += 1;
				Button joinbutton = b.GetComponent<Button>();
				joinbutton.onClick.AddListener(() => JoinGame(host));

			}

        }
        
    }
}
