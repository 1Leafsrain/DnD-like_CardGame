using Unity.Mathematics;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public int action = 3;

    public bool canMove;

    public LayerMask stop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canMove = false;
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (action <= 0)
        {
            canMove = false;
        }
        transform.position =Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime); //ngikutin transform point
        if(Vector3.Distance(transform.position, movePoint.position) <= 0.5f) // kalo jarak nya pas baru bisa maju lagi biar ngga melesat jauh
        {
           

            if (math.abs(Input.GetAxisRaw("Horizontal")) == 1f && action > 0) // kalo ada input, kenapa ada math abs, kan ada negatif dan positif nah jadi ngga diskriminasi
            {
                /*penjelasan buat if ini : Physics2D.OverlapCircle = buat cicle imaginer dengan posisi dan ukuran yang udah ditentukan ke layermask stop, 
            * jadi kalo cicle nya kena layer mask gitu, negasi diawal karna kita ngga mau jalan ke layer stop*/
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, stop))
                {
                    action -= 1;
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f); // nah ini baru jalan
                }
            }

            if (math.abs(Input.GetAxisRaw("Vertical")) == 1f && action > 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, stop))
                {
                    action -= 1;
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
                
            }

            
                
        }

        
    }
}
