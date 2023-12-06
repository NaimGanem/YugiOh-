using System;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_MenuLogic : MonoBehaviour
{
    #region Appwrap Keys
    private string apikey = "5d6509a8b7e390247e37eec298734ef2158348a15442fa119abed8457eefc2f1";
    private string secretkey = "3c55e3f358d2f2da520ac303a5f53cc52d8ff086e1386340168b1701dd52280b";
    #endregion
    public enum Screens
    {
        MainMenu, Loading, Options ,Multiplayer, StudentInfo , SingalPlayer
    };
    #region Singleton
    private void InitStart()
    {
        //unityObjects["Btn_MainMenu_Play1"].GetComponent<Button>().interactable = false;
    }
    private static SC_MenuLogic instance;
    public static SC_MenuLogic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_MenuLogic").GetComponent<SC_MenuLogic>();

            return instance;
        }

    }

    #endregion
    private void OnEnable()
    {
        Listener.OnConnect += OnConnect;
        Listener.OnRoomsInRange += OnRoomsInRange;
        Listener.OnCreateRoom+= OnCreateRoom;
        Listener.OnJoinRoom += OnJoinRoom;
        Listener.OnGetLiveRoomInfo += OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom += OnUserJoinRoom;
        Listener.OnGameStarted += OnGameStarted;

    }

    
    private void OnDisable()
    {
        Listener.OnConnect -= OnConnect;
        Listener.OnRoomsInRange -= OnRoomsInRange;
        Listener.OnCreateRoom -= OnCreateRoom;
        Listener.OnJoinRoom -= OnJoinRoom;
        Listener.OnGetLiveRoomInfo -= OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom -= OnUserJoinRoom;
        Listener.OnGameStarted -= OnGameStarted;
    }
    #region varilbels
   
    private Dictionary<string, object> passedParams;
    private List<string> roomIds;
    private int maxRoomUsers = 2;
   
    private string roomName = "ShenkarRoom";
    public string roomId;
    private int roomIndex = 0;




    #endregion
   
  

  

    private List<string> randomNames = new List<string>()
    {
        "Alice",
        "Bob",
        "Charlie",
        "David",
        "Naim",
        "Amir",
        "Sasuke",
        "naruto",
        "Ran",
        "2good4U",
        

        // Add more names as needed
    };
    private Listener listener;
    private void InitAwake()
    {
       
        if (listener == null)
            listener = new Listener();
        passedParams = new Dictionary<string, object>()
        {{"Password","Shenkar2023"}};
        WarpClient.initialize(apikey, secretkey);
        WarpClient.GetInstance().AddConnectionRequestListener(listener);
        WarpClient.GetInstance().AddChatRequestListener(listener);
        WarpClient.GetInstance().AddUpdateRequestListener(listener);
        WarpClient.GetInstance().AddLobbyRequestListener(listener);
        WarpClient.GetInstance().AddNotificationListener(listener);
        WarpClient.GetInstance().AddRoomRequestListener(listener);
        WarpClient.GetInstance().AddTurnBasedRoomRequestListener(listener);
        WarpClient.GetInstance().AddZoneRequestListener(listener);
        WarpClient.GetInstance().AddChatRequestListener(listener);
        GlobalVariables.userid = System.Guid.NewGuid().ToString();
        //unityObjects["Text_userId"].GetComponent<TMP_Text>().text = "UserId: " + userid;
        //unityObjects["Text_userId2"].GetComponent<TMP_Text>().text = "UserId: " + userid;
        //Debug.Log(userid);
        string randomName = GetRandomName();
        if (unityObjects.ContainsKey("Text_userId"))
        {
            unityObjects["Text_userId"].GetComponent<TMP_Text>().text = "UserId: " + randomName;
            unityObjects["Text_userId2"].GetComponent<TMP_Text>().text = "UserId: " + randomName;
        }
      
        Debug.Log(randomName);
        WarpClient.GetInstance().Connect(GlobalVariables.userid);
        UpdateStatus("Open Conection...");

    }
    private string GetRandomName()
    {
        // Generate a random index to select a name from the list
        int randomIndex = UnityEngine.Random.Range(0, randomNames.Count);
        return randomNames[randomIndex];
    }




    private void UpdateStatus(string _Str)
    {
        if (unityObjects.ContainsKey("Txt_status"))
        {
            unityObjects["Txt_status"].GetComponent<TMP_Text>().text = _Str;
            unityObjects["Txt_status2"].GetComponent<TMP_Text>().text = _Str;
        }
       
    }
    private void DoRoomSearchLogic()
    {
        if (roomIndex < roomIds.Count)
        {
            UpdateStatus("Bring room info (" + roomIds[roomIndex] + ")");
            WarpClient.GetInstance().GetLiveRoomInfo(roomIds[roomIndex]);
        }
        else
        {
            UpdateStatus("Creating Room...");
            int _randNumber = UnityEngine.Random.Range(100000, 999999);
            WarpClient.GetInstance().CreateTurnRoom(roomName + _randNumber, GlobalVariables.userid, maxRoomUsers, passedParams, GlobalVariables.MaxturnTime);
        }
    }
    #region Variables

    private Dictionary<string, GameObject> unityObjects;
    private Screens curScreen;
    private Screens prevScreen;


    #endregion
    #region ServerCallbacks

    private void OnConnect(bool _IsSuccess)
    {
        Debug.Log("OnConnect " + _IsSuccess);
        if (_IsSuccess)
        {
            UpdateStatus("Connected.");
            if (unityObjects.ContainsKey("Btn_MainMenu_Play"))
            {
                unityObjects["Btn_MainMenu_Play"].GetComponent<Button>().interactable = true;
            }
               
        }
        else
        {
            UpdateStatus("Failed to Connect.");
        }
    }
    private void OnRoomsInRange(bool _IsSuccess, MatchedRoomsEvent eventObj)
    {
        Debug.Log("OnRoomsInRange " + _IsSuccess);
        if (_IsSuccess)
        {
            UpdateStatus("Parsing Rooms...");
            roomIds = new List<string>();
            foreach (var RoomData in eventObj.getRoomsData())
            {
                Debug.Log("Room Id: " + RoomData.getId());
                Debug.Log("Room Owner: " + RoomData.getRoomOwner());
                roomIds.Add(RoomData.getId());
            }

            roomIndex = 0;
            DoRoomSearchLogic();
        }
    }
    private void OnCreateRoom(bool _IsSuccess, string _RoomId)
    {
        Debug.Log("OnCreateRoom " + _IsSuccess + " " + _RoomId);
        if (_IsSuccess)
        {
            roomId = _RoomId;
            UpdateStatus("Room have been created, RoomId: " + _RoomId);
            WarpClient.GetInstance().JoinRoom(roomId);
            WarpClient.GetInstance().SubscribeRoom(roomId);
        }
        else
        {
            Debug.Log("Cant Create room...");
        }
    }
    private void OnJoinRoom(bool _IsSuccess, string _RoomId)
    {
        if (_IsSuccess)
        {
            UpdateStatus("Joined Room: " + _RoomId);
            unityObjects["Txt_status3"].GetComponent<TextMeshProUGUI>().text = "Room Id: " + _RoomId;
        }
        else UpdateStatus("Failed to join Room: " + _RoomId);
    }
    private void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
    {
        Debug.Log("OnGetLiveRoomInfo ");
        if (eventObj != null && eventObj.getProperties() != null)
        {
            Dictionary<string, object> _properties = eventObj.getProperties();
            if (_properties.ContainsKey("Password") &&
                _properties["Password"].ToString() == passedParams["Password"].ToString())
            {
                roomId = eventObj.getData().getId();
                UpdateStatus("Received Room Info, joining room: " + roomId);
                WarpClient.GetInstance().JoinRoom(roomId);
                WarpClient.GetInstance().SubscribeRoom(roomId);
            }
            else
            {
                roomIndex++;
                DoRoomSearchLogic();
            }
        }
    }
    private void OnUserJoinRoom(RoomData eventObj, string _UserName)
    {
        UpdateStatus("User Joined Room " + _UserName);
        if (eventObj.getRoomOwner() == GlobalVariables.userid && GlobalVariables.userid != _UserName)
        {
            UpdateStatus("Starting Game...");
            WarpClient.GetInstance().startGame();
        }
    }
    private void OnGameStarted(string _Sender, string _RoomId, string _NextTurn)
    {
        UpdateStatus("Game Started, Next Turn: " + _NextTurn);
        //unityObjects["MenuScreen"].SetActive(false);
        //unityObjects["Game"].SetActive(true);
        SceneManager.LoadScene(1);

        //start game in game logic 
    }

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        Init();
        InitAwake();
    }

    private void Start()
    {
        InitLogic();
        InitStart();
    }

    #endregion
    
    #region Logic

    private void Init()
    {
        curScreen = Screens.MainMenu;
        prevScreen = Screens.MainMenu;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _unityObj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach(GameObject g in _unityObj)
        {
            if (unityObjects.ContainsKey(g.name) == false)
                unityObjects.Add(g.name, g);
            else Debug.LogError("This key " + g.name + " Is Already inside the Dictionary!!!");
        }
    }

    private void InitLogic()
    {
        if (unityObjects.ContainsKey("Screen_Loading"))
            unityObjects["Screen_Loading"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Options"))
            unityObjects["Screen_Options"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Multiplayer"))
            unityObjects["Screen_Multiplayer"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_StudentInfo"))
            unityObjects["Screen_StudentInfo"].SetActive(false);

    }

    public void ChangeScreen(Screens _ToScreen)
    {
        prevScreen = curScreen;
        curScreen = _ToScreen;

        if (unityObjects.ContainsKey("Screen_" + prevScreen))
            unityObjects["Screen_" + prevScreen].SetActive(false);

        if (unityObjects.ContainsKey("Screen_" + curScreen))
            unityObjects["Screen_" + curScreen].SetActive(true);
    }


    #endregion

    #region Controller
    public void Cv()
    {
        Application.OpenURL("http://se.shenkar.ac.il/software-engineers/Naim/");
    }
    public void Btn_PlayLogic()
    {
        Debug.Log("Btn_PlayLogic");
        unityObjects["Btn_MainMenu_Play"].GetComponent<Button>().interactable = false;
        WarpClient.GetInstance().GetRoomsInRange(1,2);
        UpdateStatus("Searching for availibe room...");


    }


    public void Btn_BackLogic()
    {
        Debug.Log("Btn_BackLogic");
        ChangeScreen(prevScreen);
    }

    public void Btn_MainMenu_PlayLogic()
    {
        Debug.Log("Btn_MainMenu_PlayLogic");
        ChangeScreen(Screens.Loading);
        SceneManager.LoadScene(1);
    }

    public void Btn_MainMenu_OptionsLogic()
    {
        Debug.Log("Btn_MainMenu_OptionsLogic");
        ChangeScreen(Screens.Options);
    }
    public void Btn_MainMenu_MultiPlayerLogic()
    {
        Debug.Log("Btn_MainMenu_MultiPlayer");
        ChangeScreen(Screens.Multiplayer);
    }
    public void Btn_MainMenu_StudentInfoLogic()
    {
        Debug.Log("Btn_MainMenu_StudentInfo");
        ChangeScreen(Screens.StudentInfo);
    }
    #endregion
}
