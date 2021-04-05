using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region variables
    public List<List<List<RoomMeta>>> RoomMeta_list = new List<List<List<RoomMeta>>>();
    public RoomMeta roomMeta_now; 
    public SubManagers sub_managers;
    public Pools pools;
    public GameObject room;
	public ObjectBase playerBase;
    public GarbageBox garbageBox;
    
	public bool player_turn;
    public bool enemy_turn;
    public bool object_turn;
    public bool anim_playing;    
	public int set_stage_nth;
	public bool skipmode;

	public int wellpoint;
    public int stagelv = 1;
    public int maxstagelv = 2;
    public StageType stageType;
    public GameObject test;
    bool gamestart = false;

	#endregion

	#region Awake
	public void Awake()
    {        
        //set_GameManager();
    }
    public void Start()
    {
        //set_GameManager();        
        StartCoroutine(gamesetter());
        //StartCoroutine("make_RoomMeta_list");
    }
    public void set_GameManager()
    {
        sub_managers = new SubManagers();
        pools = new Pools(gameObject);
        //pools가 안되는데 왜그럴까?
        garbageBox = GetComponentInChildren<GarbageBox>();
        
        //StartCoroutine("make_RoomMeta_list");
    }   
    

    void _make_RoomMeta_list()
    {
        for (int i = 0; i < Constant.MAX_INDEX; i++)
        {
            RoomMeta_list.Add(new List<List<RoomMeta>>());
            for (int j = 0; j < Constant.MAX_INDEX; j++)
            {
                RoomMeta_list[i].Add(new List<RoomMeta>());
                for (int k = 0; k < Constant.MAX_INDEX; k++)
                {
                    RoomMeta_list[i][j].Add(new RoomMeta(i, j, k));
                }
            }
        }

        //stage_start(1);
    }
    IEnumerator gamesetter()
    {        
        set_GameManager();
        yield return new WaitForSeconds(0.2f);
        StartCoroutine("make_RoomMeta_list");
    }
    IEnumerator make_RoomMeta_list() // 이거 코루틴으로 하면 생성 안될거같은데.? 한번 호출하고 말듯.
    {        
        for(int i = 0; i < Constant.MAX_INDEX; i++)
        {
            RoomMeta_list.Add(new List<List<RoomMeta>>());
            for(int j = 0; j < Constant.MAX_INDEX; j++)
            {
                RoomMeta_list[i].Add(new List<RoomMeta>());
                for (int k = 0; k < Constant.MAX_INDEX; k++)
                {
                    RoomMeta_list[i][j].Add(new RoomMeta(i,j,k));
				}                
            }            
        }

        yield return new WaitForSeconds(0.02f);

        stage_start(stagelv);
        player_setup();
        //set_stage_nth = 1;
        //is_setting = false;
        gamestart = true;
	}

	#endregion

	
	public Process_Type processType;

    #region fixedupdate
    private void FixedUpdate()
    {
		if(gamestart) process(Time.deltaTime);
    }
    #endregion

    #region fuctions
    void process(float time)
    {
		//process_set_stage();
		sub_managers.playerManager.process();
		sub_managers.enemyManager.process();
        sub_managers.objectManager.process();
        sub_managers.animManager.process();
        sub_managers.uIManager.process();
        sub_managers.soundManager.process();		      
	}

	void set_stage(int stagelv)
	{
	
		//link RoomMetanode, set RoomMetatype, etc...
		//룸 클래스에 룸의 메타정보를 넣어줌.
		//스테이지 시작할때 이 함수 콜함.
		init_stage();
        roomMeta_now = sub_managers.mapManager.setmap(stagelv); //set_roomMeta_list(RoomMeta_list, prev_roomMeta: roomMeta_now == null? null : roomMeta_now); //방연결. 방마다 room type 지정.
        
        GameManager.instance.anim_playing = true;
        GameManager.Instance.player_turn = true;
        GameManager.Instance.enemy_turn = false;
        GameManager.Instance.object_turn = false;
        set_stagetype(stagelv);
        room.GetComponent<Room>().setMapBase(stageType);
        sub_managers.boardManager.setRoom(roomMeta_now);
        //sub_managers.boardManager.room_transition(roomMeta_now);
	}
    void set_stagetype(int stagelv)
    {
        if(stagelv < 3)
        {
            stageType = StageType.DEFAULT;
        }
        else if (stagelv >= 3)
        {
            stageType = StageType.GHOST;
        }
    }
	void player_setup()
	{
        // Player setup
        GameManager.Instance.sub_managers.cameraManager.run_camera_anim(CameraAnimType.HIDESCREEN, 30, Vector3.zero, 0);

        if (playerBase == null)
		{
            playerBase = sub_managers.boardManager.init_player();
			sub_managers.playerManager.init_player(playerBase);
			sub_managers.enemyManager.init_player(playerBase);
		}
		else playerBase.node_now.object_here_list.Add(playerBase);

        player_turn = true;
    }

	void init_stage()
	{
		//
		wellpoint = 0;
		player_turn = false;
		anim_playing = false;
		set_stage_nth = -1;

		garbageBox.clean_all();

		// RoomMeta init..
		foreach (var roomMeta_list1 in RoomMeta_list)
		{
			foreach (var roomMeta_list2 in roomMeta_list1)
			{
				foreach (var roomMeta in roomMeta_list2)
				{
					//roomMeta.object_base_list.Clear();
					roomMeta.init_roomMeta();
				}
			}
		}


		/// 여기에 초기화 할거 다 정의 필요.
	}
	
	public void stage_start(int stagelv)
	{
		set_stage(stagelv);
        
	}

	#endregion

	#region singletone
	private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
	
	#endregion

}

#region submanagers
public class SubManagers
{
    public SubManagers()
    {
        cameraManager = Camera.main.GetComponent<CameraManager>();
		mapManager = new MapManager();
		boardManager = GameManager.Instance.GetComponentInChildren<BoardManager>();
		playerManager = GameManager.Instance.GetComponentInChildren<PlayerManager>();
		enemyManager = GameManager.Instance.GetComponentInChildren<EnemyManager>();
        objectManager = GameManager.Instance.GetComponentInChildren<ObjectManager>();
        animManager = new AnimManager(GameManager.Instance.gameObject.GetComponentInChildren<AnimCoroutinePool>());
        uIManager = Camera.main.GetComponentInChildren<UIManager>();
        soundManager = GameManager.Instance.GetComponentInChildren<SoundManager>();
        //set submanagers

        initialize();
    }
    public CameraManager cameraManager;
	public MapManager mapManager;
    public AnimManager animManager;
	public BoardManager boardManager;
	public PlayerManager playerManager;
	public EnemyManager enemyManager;
    public ObjectManager objectManager;
    public UIManager uIManager;
    public SoundManager soundManager;

	public void initialize()
	{
		playerManager.initManager(animManager);
		enemyManager.initManager(animManager);
        objectManager.initManager(animManager);
	}


}
#endregion submanagers

#region pools
public class Pools
{
    public Pools(GameObject _gameObject)
    {
        //set pools        
        objectBasePool = _gameObject.GetComponentInChildren<ObjectBasePool>();        
        objectBodyPool = _gameObject.GetComponentInChildren<ObjectBodyPool>();
        ImagePool = _gameObject.GetComponentInChildren<ImagePool>();
        effectPool = _gameObject.GetComponentInChildren<EffectPool>();
        roomPools = new RoomPools();
        
    }
    
    public ImagePool ImagePool;
    public ObjectBasePool objectBasePool;
    public ObjectBodyPool objectBodyPool;
    public EffectPool effectPool;
    public RoomPools roomPools;

}
#endregion pools