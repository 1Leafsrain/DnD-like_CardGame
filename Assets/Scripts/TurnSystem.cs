using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SerializeField] public enum stage { first, firstMiddle, middleEnd, end }

public class TurnSystem : MonoBehaviour
{
    [SerializeField] public bool playerTurn;
    [SerializeField] public GameObject draw;
    [SerializeField] public GameObject EndTurnButton;
    [SerializeField] public GameObject end;

    public GameObject textMeshPro;
    public Text text;
    public GridMove playerObj;
    public AigridMove enemyObj;
    public GameObject panelMenang;


    [SerializeField] public bool enemyTurn;
  

    [SerializeField] public HandManager handManager;
    [SerializeField] public GameObject handEnemy;
    [SerializeField] public LeftCardDropArea[] handEnemyTrans;
    [SerializeField] public GameObject HandPosition;
    [SerializeField] public GameObject HandPositions;
    [SerializeField] public GameObject[] playerHand;
    [SerializeField] public GameObject[] enemyHand;
    [SerializeField] public Card[] playerHandCard;
    [SerializeField] public EnemyCard[] enemyHandCard;
    public LeftCardDropArea[] HandPositionTrans;

    [SerializeField] public enum enemyTipes {}

    public stage stages;

    private IEnumerator coroutine;

    private GameObject GetHandPositions()
    {
        return HandPositions;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshPro = GameObject.FindGameObjectWithTag("Text");
       // text = textMeshPro.GetComponent<Text>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerObj = player.GetComponent<GridMove>();
        GameObject enemy = GameObject.FindGameObjectWithTag("EnemyHand");
        enemyObj = enemy.GetComponent<AigridMove>();
        //slotEnemy = handEnemy.GetComponentsInChildren<EnemyCard>();
        // handEnemyTrans = handEnemy.GetComponentsInChildren<LeftCardDropArea>();
        HandPositionTrans = HandPosition.GetComponentsInChildren<LeftCardDropArea>();
        

        //handManager = GetComponent<HandManager>();
       handManager = FindAnyObjectByType<HandManager>();
        stages = stage.first;
        TurnStage();
    }
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            TriggerCard();
        }
    }

    

    public void playerTurns()
    {
        playerTurn= true;
        enemyTurn= false;
        handManager.SpawnCard();
        EndTurnButton.SetActive(true);
        //enemyTurns();
    }
    public void enemyTurns()
    {
        EndTurnButton.SetActive(false);
        playerTurn = false;
        enemyTurn = true;
        handManager.SpawnEnemyCard();
        //playerTurns();
    }

    public void TriggerCard()
    {
        playerHandCard = HandPositions.GetComponentsInChildren<Card>();

        
        if (playerHandCard.Length == 0) { return; }
        int target = playerHandCard.Length;
        for (int i = 0; i < target; i++)
        {
            //bool fulls = playerHandCard[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            HandPositionTrans[i].GetComponent<LeftCardDropArea>().checkSlot();
            if (playerHandCard != null)
            {


                playerHandCard[i].GetComponentInChildren<Card>().ActionCard();
                
            }
            else
            {
                
            }
        }
    }

    public void TriggerEnemyCard()
    {
        if (handEnemyTrans.Length == 0) { return; }
        for (int i = 0; i < handEnemyTrans.Length; i++)
        {
            bool fulls = handEnemyTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == true)
            {
                

                enemyHand[i].GetComponentInChildren<EnemyCard>().ActionCard();

            }
            else
            {
                
            }
        }
        
    }

    public void preparePlayer()
    {
        GameObject[] kartu = GameObject.FindGameObjectsWithTag("PlayerHand");
        
        for (int i = 0;i < kartu.Length; i++)
        {
            kartu[i].GetComponent<Card>().sets();
        }
        /*foreach (GameObject p in kartu)
        {
            if (p != null)
            {
                p.GetComponent<Card>().sets();
            }
            
        }*/

    }

    public void unpreparePlayer()
    {
        GameObject[] kartu = GameObject.FindGameObjectsWithTag("PlayerHand");
        for (int i = 0; i < kartu.Length; i++)
        {
            kartu[i].GetComponent<Card>().unsets();
        }
        

    }

    public void getCard()
    {
        playerHandCard = HandPositions.GetComponentsInChildren<Card>();
        
    }
   
    private IEnumerator TungguCard(float waktu)
    {
        yield return new WaitForSeconds(waktu);
        Debug.Log("udah selesai");
    }

    public void TurnStage()
    {
        switch (stages) 
        {
            case stage.first:
                unpreparePlayer();
                //handManager.SpawnCard();
                StartCoroutine(firstStage(1.0f));
                
                break;
            case stage.firstMiddle:
                StartCoroutine(secondStage(5.0f));
                
                EndTurnButton.SetActive(true);
                
                break;
            case stage.middleEnd:
               // text.text = "Enemy Jalan";
                enemyObj.canMove = true;
                preparePlayer();
                //StartCoroutine(secondStage(5.0f));
                break;
            case stage.end:
                enemyObj.canMove = false;
                StartCoroutine(endStage(5.0f));
                unpreparePlayer();
                stages = stage.first;
                //StartCoroutine(endStage(3.0f));
                break;
        }

    }

    public void endButton()
    {
        stages = stage.middleEnd;
        TurnStage();
        EndTurnButton.SetActive(false);
    }

    private IEnumerator firstStage(float waktu)
    {
        handManager.SpawnCard();
        yield return new WaitForSeconds(waktu);
        stages = stage.firstMiddle;
        TurnStage();
       // Debug.Log("udah selesai");
    }

    private IEnumerator secondStage(float waktu)
    {
        //TriggerEnemyCard();
        //text.text = "Jalan";
        playerObj.canMove = true;
        yield return new WaitForSeconds(waktu);
        stages = stage.end;
        TurnStage();
        //Debug.Log("udah selesai");
    }

    private IEnumerator endStage(float waktu)
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Player");
        if (enemy == null)
        {
            panelMenang.SetActive(true);
        }
        playerObj.canMove = false;
        
        yield return new WaitForSeconds(waktu);
        //stages = stage.end;
        //TurnStage();
        Debug.Log("Game selesai");
    }
}
