using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
	
public class Menu : MonoBehaviour {

    public static Menu Instance;
    
    private const string typeName = "LD32CookFight";

	// UI
	public GameObject NetUI;
	public GameObject hostsUI;
	public GameObject statusTextUI;
	public GameObject hostLineUI;
	public GameObject hostNameInputUI;

	private List<GameObject> hostLines;
	
	public string status {
		get {
			return statusTextUI.GetComponent<Text>().text;
		}
		set {
			statusTextUI.GetComponent<Text>().text = value;
		}
	}
	
	public void ShowServerBrowser(bool show) {
		NetUI.gameObject.SetActive(show);
	}
	
	public void DisplayRoomList() {
		status = "Rooms found: " + NetManager.Instance.roomList.Length;
		
		// Clean previous host list
		hostLines.ForEach(child => Destroy(child));
		
		int count = 0;
		foreach(RoomInfo room in NetManager.Instance.roomList){
			
			// Create a new join button
			GameObject hostline = Instantiate(hostLineUI);
			// Keep reference to button so we can delete them on refresh
			hostLines.Add(hostline);
			// Position button correctly as a list (TO DO: Scroll support for large host lists)
			hostline.transform.SetParent(hostsUI.transform, false);
			hostline.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -30f*count);
			
			// Rename button with Game name chosen by remote hosts
			Transform b = hostline.transform.FindChild("Button");
			b.FindChild("HostLineText").GetComponent<Text>().text = room.name;
			Debug.Log(room.name);
			Button joinbutton = b.GetComponent<Button>();
			joinbutton.onClick.AddListener(() => NetManager.Instance.JoinRoom(room));
			
			count += 1;
		}
	}
	
	
	void Awake() {
		if (Instance == null) Instance = this;
		else Destroy(this);
		hostLines = new List<GameObject>();
	}



}
