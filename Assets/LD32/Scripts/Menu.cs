using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
	
public class Menu : MonoBehaviour {

    public enum State {
    	Blank,
    	FindGame,
    	CreateGame,
    	WaitingForPlayer,
    	EscapeMenu,
    	WinnerScreen
    }
    
    public void SetStateBlank() {
    	state = State.Blank;
    }
    
    public void SetStateFindGame() {
    	state = State.FindGame;
    }
    
    public void SetStateHostGame() {
		state = State.CreateGame;
    }
    
    public void SetStateWaitingForPlayer() {
    	state = State.WaitingForPlayer;
    }
    
    public void SetStateEscapeMenu() {
    	state = State.EscapeMenu;
    }
    
    public State state {
    	get {
    		return _state;
    	}
    	set {
    		_state = value;
    		switch(_state) {
    		case State.FindGame:
				gameTitle.SetActive(true);
				preGameControls.SetActive(true);
				findRoomControls.SetActive(true);
				createRoomControls.SetActive(false);
				inRoomControls.SetActive(false);
				winnerScreen.SetActive(false);
				DisplayRoomList();
    			break;
			case State.CreateGame:
				gameTitle.SetActive(true);
    			preGameControls.SetActive(true);
    			createRoomControls.SetActive(true);
    			findRoomControls.SetActive(false);
    			inRoomControls.SetActive(false);
				winnerScreen.SetActive(false);
    			break;
    		case State.WaitingForPlayer:
				gameTitle.SetActive(true);
    			inRoomControls.SetActive(true);
    			preGameControls.SetActive(false);
    			createRoomControls.SetActive(false);
    			findRoomControls.SetActive(false);
				winnerScreen.SetActive(false);
    			break;
    		case State.EscapeMenu:
				gameTitle.SetActive(true);
    			inRoomControls.SetActive(true);
				preGameControls.SetActive(false);
				findRoomControls.SetActive(false);
				createRoomControls.SetActive(false);
				winnerScreen.SetActive(false);
    			break;
    		case State.WinnerScreen:
    			gameTitle.SetActive(true);
    			winnerScreen.SetActive(true);
				preGameControls.SetActive(false);
				findRoomControls.SetActive(false);
				createRoomControls.SetActive(false);
				inRoomControls.SetActive(false);
    			break;
			default:
				gameTitle.SetActive(false);
				preGameControls.SetActive(false);
				findRoomControls.SetActive(false);
				createRoomControls.SetActive(false);
				inRoomControls.SetActive(false);
				winnerScreen.SetActive(false);
				break;
    		}
			if (state == State.Blank) statusText.SetActive(false);
			else statusText.SetActive(true);
    	}
    }
    
    public static Menu Instance;
    
	public GameObject gameTitle;
	
	/// <summary>
	/// The pre game controls: FindGame button, CreateGame button, playerName textfield.
	/// </summary>
	public GameObject preGameControls;		
	
	/// <summary>
	/// The create room controls: room name text field, start game button
	/// </summary>
	public GameObject createRoomControls;

	/// <summary>
	/// The find room controls: join game buttons procedurally added
	/// </summary>
	public GameObject findRoomControls;

	/// <summary>
	/// The in room controls: leave room button
	/// </summary>
	public GameObject inRoomControls;

	/// <summary>
	/// The winner screen.
	/// </summary>
	public GameObject winnerScreen;

	public GameObject statusText;
	
	public GameObject joinGameButtonPrefab;

	private State _state;
	private List<GameObject> _joinGameButtons;
	
	public string status {
		get {
			return statusText.GetComponent<Text>().text;
		}
		set {
			statusText.GetComponent<Text>().text = value;
		}
	}
	
	public void DisplayRoomList() {
		status = "Rooms found: " + NetManager.Instance.roomList.Length;
		
		// Clean previous host list
		_joinGameButtons.ForEach(child => Destroy(child));
		
		int count = 0;
		foreach(RoomInfo room in NetManager.Instance.roomList){
			
			// Create a new join button
			GameObject joinGameButton = Instantiate(joinGameButtonPrefab);
			// Keep reference to button so we can delete them on refresh
			_joinGameButtons.Add(joinGameButton);
			// Position button correctly as a list (TO DO: Scroll support for large host lists)
			joinGameButton.transform.SetParent(findRoomControls.transform, false);
			joinGameButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -30f*count);
			
			// Rename button with Game name chosen by remote hosts
			Transform b = joinGameButton.transform.FindChild("Button");
			b.FindChild("HostLineText").GetComponent<Text>().text = room.name;
			Debug.Log(room.name);
			Button joinbutton = b.GetComponent<Button>();
			joinbutton.onClick.AddListener(() => NetManager.Instance.JoinRoom(room));
			
			count += 1;
		}
	}
	
	public void UpdateWinnerScreen() {
		winnerScreen.GetComponent<Text>().text = Game.Instance.winnerName;
	}
	
	void Awake() {
		if (Instance == null) Instance = this;
		else Destroy(this);
		_joinGameButtons = new List<GameObject>();
	}

	void Update() {
		if (PhotonNetwork.inRoom)
			if (Input.GetKey(KeyCode.Escape)) 
				state = State.EscapeMenu;
	}

}
