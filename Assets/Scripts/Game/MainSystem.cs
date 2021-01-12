using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainSystem : MonoBehaviour
{
    [Header("System")]
    public static MainSystem instance;
    public Transform Characters;

    [HideInInspector] public int playerAmount;
    [HideInInspector] public int turns;
    [HideInInspector] public int currentPlayerNum;
    
    void Start()
    {
        instance = this;
        Characters = GameObject.Find("Characters").transform;

        //Receive Data From Last Scene
        currentPlayerNum = GameObject.Find("LocalData").GetComponent<LocalData>().firstid + 1;
        playerAmount = 4; 

        ChangeCamera(currentPlayerNum, 1);
    }

   
    void Update()
    {
        
    }

    public void ChangeCamera(int PlayerNum, int CameraNum)
    {
        if (PlayerNum > MainSystem.instance.playerAmount || PlayerNum < 1 || CameraNum > 3 || CameraNum < 1) return;
        Characters.GetChild(PlayerNum - 1).GetChild(CameraNum - 1).transform.GetComponent<CinemachineVirtualCameraBase>().MoveToTopOfPrioritySubqueue();
    }

    public void GenerateBuilding(int BoxPos, int level)
    {

    }
    
    /*------------------------------------------------------------------------------*/
    /*Network*/
    public void OnDicingReceive(float eulerx, float eulery, float eulerz, float torquex)
    {

    }
    public void OnRoundEndReceive(bool sucess, string err,  int nextID)
    {
        currentPlayerNum = nextID + 1;
        ChangeCamera(currentPlayerNum, 1);
    }
}
