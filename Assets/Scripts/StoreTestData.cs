using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoreTestData : MonoBehaviour
{
    static public StoreTestData instance { get { return s_Instance; } }
    static protected StoreTestData s_Instance;
    public int currentScore { get { return this.Score; } set { this.Score = value; } }
    public int coinsCollected { get { return this.CoinsCollected; } set { this.CoinsCollected = value; } }
    public int highscore { get { return this.Highscore; } set { this.Highscore = value; } }
    public Consumable[] consumables;
    public float currentSpeed { get { return this.CurrentSpeed; } set { this.CurrentSpeed = value; } }

    protected int Score;
    protected int CoinsCollected;
    protected int Highscore;
    protected float CurrentSpeed;

    public GameObject game;
    public GameObject playerPivot;
    public TrackManager trackManager;
    public CharacterInputController charInputCtrl;
    // Start is called before the first frame update
    void Start()
    {
        s_Instance = this;

        game = GameObject.Find("Game");
        playerPivot = GameObject.Find("PlayerPivot");     
        trackManager = GetComponent<TrackManager>();
        charInputCtrl = GetComponent<CharacterInputController>();        
    }

    // Update is called once per frame
    void Update()
    {
        //CurrentSpeed = TrackManager.instance.speed;      
        //Highscore = TrackManager.instance.score;
        //coinsCollected = charInputCtrl.coins;
        //StoreData();
    }

    public void StoreData()
    {
        Debug.Log("Score = " + Highscore);
        Debug.Log("Speed = " + CurrentSpeed);
    }

}
