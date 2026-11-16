using UnityEngine;
using UnityEngine.InputSystem;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength = 15;
    public LogicScript logic;
    [HideInInspector] public bool birdIsAlive;
    public float margin = 2;

    private Camera cam;

    void Start()
    {
        birdIsAlive = true;
        cam = Camera.main;
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    void Update()
    {
        bool flap = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                 || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (flap && birdIsAlive)
        {
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
        }

        KeepInsideScreen();
    }

    void KeepInsideScreen()
    {
        float top = cam.orthographicSize - margin;
        float bottom = -cam.orthographicSize + margin;
        Vector3 pos = transform.position;

        if (pos.y > top)
        {
            pos.y = top;
            transform.position = pos;
            if (myRigidbody.linearVelocity.y > 0)
                myRigidbody.linearVelocity = Vector2.zero;
        }
        else if (pos.y < bottom)
        {
            pos.y = bottom;
            transform.position = pos;
            if (birdIsAlive)
            {
                logic.gameOver();
                birdIsAlive = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!birdIsAlive) return;
        logic.gameOver();
        birdIsAlive = false;
    }
}