using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    GameObject sprites;

    public GameObject[] wallArray;
    public GameObject[] floorArray;
    public GameObject[] foodArray;
    public GameObject[] obstacleArray;
    public GameObject[] monsterArray;

    public GameObject player;
    public GameObject exit;

    //private GameMgr gameMgr;

    public int width = 10;
    public int height = 10;

    private List<Vector2> positionList = new List<Vector2>();

    public int days = 1;
    public int food = 100;
    public int hp = 5;
    public bool isEnd = false;

    private Text foodText;
    private Text hpText;
    private Text overText;
    private Text dayText;
    private Image dayImage;

    private GameObject mask;
    private static GameMgr _instance;

    public static GameMgr Instance
    {
        get
        {
            return _instance;
        }
    }

    private void GameInit()
    {
        sprites = new GameObject("Sprites");
        dayImage = GameObject.Find("DayImage").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "第 " + days + " 天";
        Invoke("HideDay",1f);
        MapInit();
        FoodInit();
        ObstacleInit();
        MonsterInit();
    }

    private void UIInit()
    {
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        hpText = GameObject.Find("HpText").GetComponent<Text>();
        overText = GameObject.Find("OverText").GetComponent<Text>();
        mask = GameObject.Find("Mask");
        mask.SetActive(false);
        overText.enabled = false;

    }


	// Use this for initialization
	void Start ()
    {
        //gameMgr = this.GetComponent<GameMgr>();



        _instance = this;
        GameInit();
        UIInit();


    }

	
	// Update is called once per frame
	void Update ()
    {
        foodText.text = "Food: " + food;
        hpText.text = "HP: " + hp;	
        if(food<=0)
        {
            food = 0;
            mask.SetActive(true);
            overText.enabled = true;
            overText.text = "你饿死了!!!!";
        }
        if(hp<=0)
        {
            hp = 0;
            mask.SetActive(true);
            overText.enabled = true;
            overText.text = "你被妖怪吃了!!!!";
        }
        if(isEnd==true)
        {
            Destroy(sprites);
            days++;
            dayImage.gameObject.SetActive(true);
            GameInit();
            isEnd = false;
        }
	}


    private void MapInit()
    {
        positionList.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == (width - 1) || y == 0 || y == (height - 1))
                {
                    int index = Random.Range(0, wallArray.Length);
                    Instantiate(wallArray[index], new Vector2(x, y), Quaternion.identity,sprites.transform);
                }
                else
                {
                    int index = Random.Range(0, floorArray.Length);
                    Instantiate(floorArray[index], new Vector2(x, y), Quaternion.identity,sprites.transform);
                    positionList.Add(new Vector2(x, y));
                }
            }
        }
        Instantiate(player, new Vector2(1, 1), Quaternion.identity,sprites.transform);
        Instantiate(exit, new Vector2(width - 2, height - 2), Quaternion.identity,sprites.transform);
        positionList.Remove(new Vector2(1, 1));
        positionList.Remove(new Vector2(width - 2, height - 2));
    }

    private void FoodInit()
    {
        int foodCount = Random.Range(days / 5 + 1, days / 3 + 1);
        if (foodCount > 10)
        {
            foodCount = 10;
        }
        for (int i = 0; i < foodCount; i++)
        {
            int index = Random.Range(0, foodArray.Length);
            int positionIndex = Random.Range(0, positionList.Count);
            Instantiate(foodArray[index], positionList[positionIndex], Quaternion.identity,sprites.transform);
            positionList.RemoveAt(positionIndex);
        }

    }
    private void ObstacleInit()
    {
        int obsracleCount = Random.Range(days + 5, days * 3);
        if (obsracleCount > 40)
        {
            obsracleCount = 40;
        }
        for (int i = 0; i < obsracleCount; i++)
        {
            int index = Random.Range(0, obstacleArray.Length);
            int positionIndex = Random.Range(0, positionList.Count);
            Instantiate(obstacleArray[index], positionList[positionIndex], Quaternion.identity,sprites.transform);
            positionList.RemoveAt(positionIndex);
        }
    }
    private void MonsterInit()
    {
        int monsterCount = Random.Range(days / 2, days / 3 + 1);
        if (monsterCount > 10)
        {
            monsterCount = 10;
        }
        for (int i = 0; i < monsterCount; i++)
        {
            int index = Random.Range(0, monsterArray.Length);
            int positionIndex = Random.Range(0, positionList.Count);
            Instantiate(monsterArray[index], positionList[positionIndex], Quaternion.identity,sprites.transform);
            positionList.RemoveAt(positionIndex);
        }
    }
    private void HideDay()
    {
        dayImage.gameObject.SetActive(false);
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
