using UnityEngine;

public class CharaterMovement : MonoBehaviour
{   
    [SerializeField]
    private float speed = 5f;

    [SerializeField]

    GameObject model;

    CharacterController controller;
    Vector3 velocity;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        LookAtMouse(model);
    }

     public void Move()
    {
        // Get input from player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move player
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    public void LookAtMouse(GameObject model)
    {
        // Get mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            model.transform.LookAt(new Vector3(point.x, model.transform.position.y, point.z));
        }
    }
}