using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb2d;
    private Vector2 moveInput;
    public Transform movepoint;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        movepoint.parent = null;
    }

    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, movepoint.position, moveSpeed * Time.deltaTime);


        if (Vector3.Distance(transform.position, movepoint.position) <= 0.5f)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movepoint.position += new Vector3(1f, 0f, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movepoint.position += new Vector3(-1f, 0f, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movepoint.position += new Vector3(0f, 1f, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movepoint.position += new Vector3(0f, -1f, 0f);
            }
        }

    }
}
