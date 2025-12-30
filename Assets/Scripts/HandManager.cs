//using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Splines;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
    [SerializeField] public int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    //[SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public GameObject HandPosition;
    [SerializeField] public RightCardDropArea[] HandSlot;
    public Transform[] HandPositionTrans;

    [SerializeField] public int EnemymaxHandSize;
    [SerializeField] private GameObject EnemycardPrefab;
    //[SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform EnemyspawnPoint;
    [SerializeField] public GameObject EnemyHandPosition;
    [SerializeField] public RightCardDropArea[] EnemyHandSlot;
    public Transform[] EnemyHandPositionTrans;

    private List<GameObject> handCards = new();

    [SerializeField] public bool playerTurn;
    [SerializeField] public bool enemyTurn;

    public void Start()
    {
        playerTurn = false;
    }

    public void Awake()
    {
        HandSlot = HandPosition.GetComponentsInChildren<RightCardDropArea>();
        HandPositionTrans = HandPosition.GetComponentsInChildren<Transform>();
        //EnemyHandPositionTrans = EnemyHandPosition.GetComponentsInChildren<Transform>();
        // SpawnEnemyCard();
        

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnCard();
            Debug.Log("log");
        }
    }

    public void DrawCard()
    {
        if(handCards.Count >= maxHandSize) return;
        SpawnCard();
        
        //UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        if (handCards.Count == 0) { return; }
        float cardSpacing = 1f / maxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardSpacing / 2;
        //Spline spline = splineContainer.Spline;
        for (int i = 0; i < handCards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            //Vector3 splinePosition = spline.EvaluatePosition(p);
            //Vector3 forward = spline.EvaluateTangent(p);
            //Vector3 up = spline.EvaluateUpVector(p);
            //Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up,forward).normalized);
            //handCards[i].transform.DOMove(splinePosition, 0.25f);
            //handCards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }

    public void SpawnCard()
    {
        if (HandPositionTrans.Length == 0) { return; }
        for (int i = 1; i < HandPositionTrans.Length; i++) 
        {
            bool fulls = HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == false && i < maxHandSize + 1)
            {
                HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().Chek();
                GameObject g = Instantiate(cardPrefab, HandPositionTrans[i].position, HandPositionTrans[i].rotation);
                g.transform.SetParent(HandPositionTrans[i].transform);
                handCards.Add(g);
            }
            else
            {
                Debug.Log("slot " + i + " penuh anjay");
            }
        }
    }

    public void SpawnEnemyCard()
    {
        if (EnemyHandPositionTrans.Length == 0) { return; }
        for (int i = 1; i < EnemyHandPositionTrans.Length; i++)
        {
            bool fulls = EnemyHandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == false && i < EnemymaxHandSize + 1)
            {

                GameObject g = Instantiate(EnemycardPrefab, EnemyHandPositionTrans[i].position, EnemyHandPositionTrans[i].rotation);
                //g.transform.SetParent(EnemyHandPositionTrans[i].transform);
                handCards.Add(g);
                EnemyHandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().Chek();

            }
            else
            {
                Debug.Log("slot " + i + " penuh anjay");
            }
        }
    }
    public void TriggerEnemyCard()
    {
        if (EnemyHandPositionTrans.Length == 0) { return; }
        for (int i = 1; i < EnemyHandPositionTrans.Length; i++)
        {
            bool fulls = EnemyHandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == true)
            {
                //coroutine = TungguCard(2);
                //StartCoroutine(coroutine);

                EnemyHandPositionTrans[i].GetComponentInChildren<Card>().ActionCard();

            }
            else
            {

            }
        }
        //stages = stage.end;
    }

}
