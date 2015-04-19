using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
	
public class Menu : MonoBehaviour {

    private const string typeName = "LD32CookFight";
    private HostData[] hostList;

	// UI
	public GameObject hostsUI;
	public GameObject statusTextUI;
	public GameObject hostLineUI;
	public GameObject hostNameInputUI;

	private List<GameObject> hostLines;

	// Network level loading
	//private string[] supportedNetworkLevels= [ "mylevel" ];
	//private string disconnectedLevel = "loader";
	//private int lastLevelPrefix = 0;

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

	// Click Events
    public void HostGame()
    {
		StartServer();
    }

	public void JoinGame()
	{

	}
	
	
	//----------------------------------------------
    // Host a game

    void StartServer()
    {
		string hname = hostNameInputUI.GetComponent<Text>().text;
		if(hname.Length > 0){
			bool useNat = !Network.HavePublicAddress();
			NetworkConnectionError con = Network.InitializeServer(32, 25000, useNat);
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
        //SpawnPlayer();
    }

    //----------------------------------------------
    // Host refresh

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived){ 
            hostList = MasterServer.PollHostList();
            //Debug.Log(hostList[0].gameName);
			statusTextUI.GetComponent<Text>().text = "Hosts found: " + hostList.Length;

			hostLines.ForEach(child => Destroy(child));

			int count = 0;
			foreach(HostData host in hostList){
				count += 1;
				GameObject hostline = Instantiate(hostLineUI);
				hostLines.Add(hostline);
				hostline.transform.SetParent(hostsUI.transform, false);
				hostline.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -30f*count);
			}

        }
        
    }
}
