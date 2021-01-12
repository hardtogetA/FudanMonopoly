using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("System")]
    public int PlayerNum;

    [Header("BoxesPosition")]
    public List<Transform> BoxPosition = new List<Transform>();
    public List<Transform> EmptyBoxPosition = new List<Transform>();
    [HideInInspector] public int currentBoxID;

    [Header("DiceFactors")]
    public Transform dicePrefab;
    private GameObject dice;   
    private bool hasDice;
    private bool detectionComplete;
    private Vector3 diceOffset = new Vector3(0, -7f, 0);
    private Vector3 diceForce1 = new Vector3(-15, 30, -25);
    private Vector3 diceForce2 = new Vector3(-25, 30, 15);
    private Vector3 diceForce3 = new Vector3(15, 30, 25);
    private Vector3 diceForce4 = new Vector3(25, 30, -15);

    public float diceTorque;
    private int dicePoint = -1;


    [Space]
    private bool walking;  
    
    void Start()
    {
        hasDice = true;
        detectionComplete = false;
        walking = false;

        currentBoxID = 0;
    }

    void Update()
    {
        if (MainSystem.instance.currentPlayerNum != PlayerNum)
            return;

        if (Input.GetKeyDown("space") && hasDice) {
            hasDice = false;
            RollDice(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f), Random.onUnitSphere);
        }

        if (detectionComplete && !walking) {
            walking = true;
            detectionComplete = false;
            WalkForSteps(dicePoint);
        }
    }

    public void RollDice(float eulerx, float eulery, float eulerz, Vector3 randomPoint)
    {
        Vector3 basePosition = Camera.main.transform.position;
        Vector3 positionWithOffset1 = basePosition + diceOffset;

        dice = Instantiate(dicePrefab.gameObject, positionWithOffset1, Quaternion.Euler(eulerx, eulery, eulerz));

        Vector3 directedForce;
        switch (currentBoxID / 9)
        {
            case 0: directedForce = diceForce1; break;
            case 1: directedForce = diceForce2; break;
            case 2: directedForce = diceForce3; break;
            default: directedForce = diceForce4; break;
        }
        dice.GetComponent<Rigidbody>().AddForce(directedForce, ForceMode.Impulse);
        dice.GetComponent<Rigidbody>().AddTorque(randomPoint * diceTorque, ForceMode.Impulse);

        StartCoroutine("WaitDiceStop");
    }

    private IEnumerator WaitDiceStop(){
        yield return new WaitForSeconds(2.5f);
        float stoptime = 3f; //Make sure it's gonna stop.

        while (dice.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.01 && stoptime > 0)
        {
            stoptime -= .2f;
            yield return new WaitForSeconds(.2f);
        }

        DetectDicePoint(); 
        detectionComplete = true;
    }

    private void DetectDicePoint() {
        float maxHeight = dice.transform.GetChild(0).position.y;
        float nextHeight = 0;

        dicePoint = 1;
        for(int i = 1; i < 6; i++){

            nextHeight = dice.transform.GetChild(i).position.y;

            if(nextHeight > maxHeight) {
                maxHeight = nextHeight;
                dicePoint = i + 1;
            }   
        }

        Debug.Log(dicePoint);
    }

    private void WalkForSteps(int steps){
        
        Sequence walkSeq = DOTween.Sequence();

        while(steps-- > 0){
            if(currentBoxID % 9 == 0)
                walkSeq.Append(transform.DOMove(EmptyBoxPosition[2 * (currentBoxID / 9)].position, .4f));
            if (currentBoxID % 9 == 8)
                walkSeq.Append(transform.DOMove(EmptyBoxPosition[2 * (currentBoxID / 9) + 1].position, .4f));

            currentBoxID = (currentBoxID + 1) % 36;
            walkSeq.Append(transform.DOMove(BoxPosition[currentBoxID].position, .4f));
            
            if(currentBoxID % 9 == 0){
                walkSeq.Append(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 90, 0), .5f, RotateMode.Fast).SetEase(Ease.OutCubic));
            }
        }
        walkSeq.AppendCallback(() => {
            walking = false; 
            hasDice = true; 
        });
        walkSeq.AppendCallback(() => Destroy(dice));

    }

    public void SetPosition(int boxID)
    {
        transform.position = BoxPosition[boxID].position;
        switch (boxID / 9)
        {
            case 0:  transform.localEulerAngles = new Vector3(0, 0, 0); break;
            case 1:  transform.localEulerAngles = new Vector3(0, 90, 0); break;
            case 2:  transform.localEulerAngles = new Vector3(0, 180, 0); break;
            default: transform.localEulerAngles = new Vector3(0, 270, 0); break;
        }
    }
}
