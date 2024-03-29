﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif

/// <summary>
/// The Game manager is a state machine, that will switch between state according to current gamestate.
/// </summary>
public class GameManager : MonoBehaviour
{
    
    static public GameManager instance { get { return s_Instance; } }
    static protected GameManager s_Instance;

    public AState[] states;
    public AState topState {  get { if (m_StateStack.Count == 0) return null; return m_StateStack[m_StateStack.Count - 1]; } }

    public ConsumableDatabase m_ConsumableDatabase;
    public MultipleLinearRegression regressionManager;

    protected List<AState> m_StateStack = new List<AState>();
    protected Dictionary<string, AState> m_StateDict = new Dictionary<string, AState>();


    protected CharacterInputController charactercontroller;
    protected GameObject playerPivot;
    protected Consumable consumables;
    protected Dictionary<Consumable.ConsumableType, int> countPowerups = new Dictionary<Consumable.ConsumableType, int>();
    protected ConsumableDatabase consumableDatabase;

    static string filename = "";
    static bool running;
    static protected float speed;
    static protected float testPredictedSpeed = 10f;
    static protected float score;
    static protected float coins;
    static protected float distance;
    static protected bool ReRunning;
    protected float[] dataset;
    static protected float predictedSpeed;

    static protected float increment = 2f;
    static protected float maxSpeed = 50f;

    protected bool DDA = false;

    protected void OnEnable()
    {
        PlayerData.Create();

        s_Instance = this;

        m_ConsumableDatabase.Load();

        // We build a dictionnary from state for easy switching using their name.
        m_StateDict.Clear();

        if (states.Length == 0)
            return;

        for(int i = 0; i < states.Length; ++i)
        {
            states[i].manager = this;
            m_StateDict.Add(states[i].GetName(), states[i]);
        }

        m_StateStack.Clear();

        PushState(states[0].GetName());

        // Name the file "/test.csv" and write the headings at the beginning
        filename = Application.dataPath + "/test.csv";
        //File.AppendAllText(filename, "Speed, Score, Coins, Total Distance" + System.Environment.NewLine);  
        //Debug.Log("Triggered file read = " + filename);
        //File.ReadAllText(filename);
        
    }

    // Will do the saving function on every seconds declared
    static public IEnumerator saveRecord()
    {
        MultipleLinearRegression instance = new MultipleLinearRegression();
        instance.LoadDataFromCSV(filename);
        running = true;
        while (running)
        {
            recordTime();
            
            Debug.Log("Triggered speed = " + TrackManager.instance.speed);
            
            //function 1
            // float[] dataset = new float[3];
            // dataset[0] = speed;
            // dataset[1] = score;
            // dataset[2] = coins;
            // Debug.Log("Triggered prediction = " + instance.PredictSpeed(speed,score,coins));
            
            // function 2
            //Debug.Log("Triggered prediction = " + instance.PredictSpeed(speed,score,coins));
            
            yield return new WaitForSeconds(10);
        }        
    }

    // Records the necessary variables to the test file 
    static protected void recordTime()
    {
        Debug.Log("Speed = " + speed);
        Debug.Log("Score = " + score);
        Debug.Log("Coins = " + coins);
        Debug.Log("Total Distance = " + distance);

        WriteCSV();
    }

    //protected IEnumerator ChangeSpeed()
    //{
    //    while (running)
    //    {
    //        speed = regressionManager.Predict(dataset);
    //        yield return new WaitForSeconds(2f);
    //    }
    //}
    // protected IEnumerator ChangeSpeed()
    // {
    //     while (running)
    //     {
    //         // speed = regressionManager.Predict(dataset);
    //         // speed = regressionManager.Predict(dataset);
    //         // TrackManager.instance.predictedSpeed = testPredictedSpeed;
    //         yield return new WaitForSeconds(2f);
    //     }
    // }

    protected void Start(){
        Debug.Log("Call Start "+ regressionManager);
        regressionManager.LoadDataFromCSV(filename);
    }

    protected void Update()
    {
        //regressionManager.CheckTest();
        if(m_StateStack.Count > 0)
        {          
            m_StateStack[m_StateStack.Count - 1].Tick();
        }
        if(TrackManager.instance != null){
            speed = TrackManager.instance.speed;
            score = TrackManager.instance.score;
            playerPivot = GameObject.Find("PlayerPivot");
            charactercontroller = playerPivot.GetComponent<CharacterInputController>();
            coins = charactercontroller.coins;
            distance = TrackManager.instance.worldDistance;
            dataset = new float[3];
            dataset[0] = speed;
            dataset[1] = score;
            dataset[2] = coins;
            //StartCoroutine(ChangeSpeed());
            //Debug.Log("successful call: " + regressionManager.Predict(dataset));
            //Debug.Log("Speed: " + speed);
        }       
    }

    protected void OnApplicationQuit()
    {
#if UNITY_ANALYTICS
        // We are exiting during game, so this make this invalid, send an event to log it
        // NOTE : this is only called on standalone build, as on mobile this function isn't called
        bool inGameExit = m_StateStack[m_StateStack.Count - 1].GetType() == typeof(GameState);

        Analytics.CustomEvent("user_end_session", new Dictionary<string, object>
        {
            { "force_exit", inGameExit },
            { "timer", Time.realtimeSinceStartup }
        });
#endif
    }

    // State management
    public void SwitchState(string newState)
    {
        AState state = FindState(newState);
        if (state == null)
        {
            Debug.LogError("Can't find the state named " + newState);
            return;
        } else if (state.GetName() == "GameOver"){
            running = false;
        }

        m_StateStack[m_StateStack.Count - 1].Exit(state);
        state.Enter(m_StateStack[m_StateStack.Count - 1]);
        m_StateStack.RemoveAt(m_StateStack.Count - 1);
        m_StateStack.Add(state);
    }

	public AState FindState(string stateName)
	{
		AState state;
		if (!m_StateDict.TryGetValue(stateName, out state))
		{
			return null;
		}

		return state;
	}

    public void PopState()
    {
        if(m_StateStack.Count < 2)
        {
            Debug.LogError("Can't pop states, only one in stack.");
            return;
        }

        m_StateStack[m_StateStack.Count - 1].Exit(m_StateStack[m_StateStack.Count - 2]);
        m_StateStack[m_StateStack.Count - 2].Enter(m_StateStack[m_StateStack.Count - 2]);
        m_StateStack.RemoveAt(m_StateStack.Count - 1);
    }

    public void PushState(string name)
    {
        AState state;
        if(!m_StateDict.TryGetValue(name, out state))
        {
            Debug.LogError("Can't find the state named " + name);
            return;
        }

        if (m_StateStack.Count > 0)
        {
            m_StateStack[m_StateStack.Count - 1].Exit(state);
            state.Enter(m_StateStack[m_StateStack.Count - 1]);
        }
        else
        {
            state.Enter(null);
        }
        m_StateStack.Add(state);
    }

    // Writes the values of the essential variables into the test file
    static public void WriteCSV()
    {
        if(TrackManager.instance.speed == 10){
            TrackManager.instance.predictedSpeed = 15;
        }else {
            TrackManager.instance.predictedSpeed = 10;
        }
        // StreamWriter reader = new StreamWriter(filename, true);
        // reader.WriteLine(speed + "," + score + "," + coins + "," + distance + System.Environment.NewLine);
        // reader.Close();
        File.AppendAllText(filename, speed + "," + score + "," + coins + "," + distance + System.Environment.NewLine);
    }
    public void ToggleDynamic(bool value)
    {
        Debug.Log("Dynamic toggled:" + value);
        DDA = value;
    }

}

public abstract class AState : MonoBehaviour
{
    [HideInInspector]
    public GameManager manager;

    public abstract void Enter(AState from);
    public abstract void Exit(AState to);
    public abstract void Tick();

    public abstract string GetName();
}