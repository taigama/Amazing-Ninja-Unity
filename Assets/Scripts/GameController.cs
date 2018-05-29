using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // --- calculate ---
    private static GameController thisInstance;
    public float timeLapse;
    private static float timer;

    private delegate void DoNowDelegate();
    DoNowDelegate funcDoNow = null;

    public bool isRunning;

    //  --- UI ---
    public Text txtScore;
    private int score = 0;
    public GameObject RestartUI;

    // --- gameplay ---
    public GameObject[] poolBlock;
    public GameObject player;

    private PrefabsHolder prefabs;

    // respawn 
    public Vector3 positionBlock;
    public float yRange;
    public Vector3 velocityBlock;

    // --- audio ---
    new private AudioSource audio;
    public AudioClip clipGameOver;
    public AudioClip clipGameStart;
    public AudioClip[] clipPains;

    #region start
    void Start ()
    {
        timer = 0;
        score = 0;
        txtScore.text = 0.ToString();

        thisInstance = this;

        audio = GetComponent<AudioSource>();

        funcDoNow = new DoNowDelegate(DoNothing);

        prefabs = new PrefabsHolder("Prefabs");

        prefabs.InstantiateMain();
    }

    public static void StartGame()
    {
        thisInstance.isRunning = true;
        thisInstance.funcDoNow = new DoNowDelegate(SpawnBlock);

        
    }
    
    public Transform transCamera;
    static void ActiveBlock(int number)
    {
        // làm nó active và cả con cháu nó nữa
        thisInstance.poolBlock[number].SetActive(true);
        foreach (Transform child in thisInstance.poolBlock[number].transform)
        {
            child.gameObject.SetActive(true);
        }

        // reset lại vị trí của toàn block
        thisInstance.poolBlock[number].transform.position =
            new Vector3(thisInstance.positionBlock.x + thisInstance.transCamera.position.x,
            Random.Range(thisInstance.positionBlock.y - thisInstance.yRange, thisInstance.positionBlock.y + thisInstance.yRange));
    }

    #endregion

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("player_test");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            funcDoNow = new DoNowDelegate(SpawnBlock);
            isRunning = true;
        }

        funcDoNow();
    }

    public static void Stop()
    {
        // nếu nó stop sẵn rồi thì thôi
        if (!thisInstance.isRunning)
            return;
        SoundStop();

        thisInstance.isRunning = false;

        
        
        // timer dành cho các hàm delegate
        timer = 2.0f;
        thisInstance.funcDoNow = new DoNowDelegate(WaitRestart);
    }

    


    #region delegate
    private static void WaitRestart()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            // show the UI for restarting
            thisInstance.funcDoNow = new DoNowDelegate(ShowRestart);
            timer = 0.5f;
        }
    }

    private static void ShowRestart()
    {
        // làm hiện ui lên từ từ
            // <chưa làm tính năng này>
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            thisInstance.prefabs.DestroyMain();
            thisInstance.RestartUI.SetActive(true);
            thisInstance.funcDoNow = new DoNowDelegate(DoNothing);
        }
    }

    // hàm này có nghĩa là: không làm gì hết.
    private static void DoNothing() { }




    private static int random = 0;
    static void SpawnBlock()
    {
        // gọi block random
        if (timer <= 0)
        {
            random = Random.Range(1, thisInstance.poolBlock.Length);
            // nếu như block này ko phải inactive => bỏ qua, vòng lặp sau tính lại
            if (thisInstance.poolBlock[random].activeInHierarchy)
                return;

            timer += thisInstance.timeLapse;
            // gọi block ra
            ActiveBlock(random);
            return;
        }

        timer -= Time.deltaTime;
    }



    #endregion
    // --------------------------------------

    

    public static void ScoreAdd(int scoreBonus)
    {
        thisInstance.score += scoreBonus;
        thisInstance.txtScore.text = thisInstance.score.ToString();
    }

    public static int Score() { return thisInstance.score; }

    public static bool IsRun() { return thisInstance.isRunning; }

    #region restart
    public static void Restart()
    {
        ResetBlocks();
        ResetPlayer();
        ResetScore();

        thisInstance.transCamera.GetComponent<ShakeCamera>().Reset();

        thisInstance.funcDoNow = new DoNowDelegate(DoNothing);

        thisInstance.prefabs.InstantiateMain();
    }

    static void ResetBlocks()
    {
        for(int i = 0; i < thisInstance.poolBlock.Length; i++)
        {
            thisInstance.poolBlock[i].SetActive(false);
        }

        thisInstance.poolBlock[0].GetComponent<Block>().Reset();
    }

    static void ResetPlayer()
    {
        thisInstance.player.GetComponent<PlayerController>().Reset();
    }

    static void ResetScore()
    {
        thisInstance.score = 0;
        thisInstance.txtScore.text = 0.ToString();
        timer = 0f;
    }
    #endregion

    #region sound
    public static void SoundStart()
    {
        thisInstance.audio.clip = thisInstance.clipGameStart;
        thisInstance.audio.Play();
    }

    public static void SoundStop()
    {
        thisInstance.audio.clip = thisInstance.clipGameOver;
        thisInstance.audio.Play();
    }

    // dành cho các con đỏ, nhiều con quá nhưng chỉ kêu 1 lần thôi
    public static void SoundPain()
    {
        thisInstance.audio.clip = thisInstance.clipPains[Random.Range(0, thisInstance.clipPains.Length)];
        thisInstance.audio.Play();
    }
    #endregion
}

public class PrefabsHolder
{
    public GameObject[] backgrounds { get; set; }
    private GameObject mainObject = null;

    public PrefabsHolder(string path)
    {
        //backgrounds = (GameObject[])Resources.LoadAll(path);
        Object[] obList = Resources.LoadAll(path);
        backgrounds = new GameObject[obList.Length];

        for(int i = 0; i < obList.Length; i++)
        {
            backgrounds[i] = (GameObject)obList[i];
        }
    }

    public void InstantiateMain()
    {
        InstantiateMain(Random.Range(0, backgrounds.Length));
    }

    public void InstantiateMain(int i)
    {
        if(i >= backgrounds.Length || i < 0)
        {
            Debug.Log("Prefabs Holder: số nhập không hợp lệ!");
            return;
        }

        mainObject = GameObject.Instantiate(backgrounds[i]);
        mainObject.SetActive(true);
        mainObject.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void DestroyMain()
    {
        GameObject.Destroy(mainObject);
    }
}
