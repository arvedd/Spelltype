using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    [Header("Move Settings")]
    [SerializeField] public float MoveRate = 0f;
    [SerializeField] private float movetimer = 0f;
    [SerializeField] public float moveSpeed = 1;
    public Transform movepoint;

    [Header("Move Percentage")]
    [SerializeField] public float UpPercent = 55f;
    [SerializeField] public float LeftPercent = 55f;
    [SerializeField] public float DownPercent = 55f;
    [SerializeField] public float RightPercent = 55f;
    void Start()
    {
        movetimer = MoveRate;
        movepoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        movetimer -= Time.deltaTime;

        if (movetimer < 0f)
        {
            MoveRandom();
        }

        transform.position = Vector3.MoveTowards(transform.position, movepoint.position, moveSpeed * Time.deltaTime);
    }

    void MoveRandom()
    {

        float total = UpPercent + LeftPercent + DownPercent + RightPercent;
        float roll = Random.Range(0f, total); // pick 0 - total percentage

        Vector3 moveDir = Vector3.zero;

        if (roll < RightPercent)               
            movepoint.position += new Vector3(1f, 0f, 0f); //Right
        else if (roll < RightPercent + LeftPercent)         
            movepoint.position += new Vector3(-1f, 0f, 0f); //Left
        else if (roll < RightPercent + LeftPercent + UpPercent) 
            movepoint.position += new Vector3(0f, 1f, 0f); //Up
        else      
            movepoint.position += new Vector3(0f, -1f, 0f); //Down  

        movetimer = MoveRate;
    }

}
