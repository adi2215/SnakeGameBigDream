using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerCharacter : MonoBehaviour
{
    private List<GameObject> BodyPart = new List<GameObject>();
    private List<Vector3> Positions = new List<Vector3>();
    public GameObject bodyPref;
    public GameObject foodPref;
    public GameObject snake;
    public GameObject Bodysnake;
    public GameObject ParticleSystem;
    public GameObject DeathUI;
    public Text points;
    public AudioSource soundEat;
    private ParticleSystem particleEffect;

    private float direction;
    private int scorePoint = 0;
    public float speed = 5f;
    public float turnSpeed = 180f;
    public int offset = 10;

    private void Start()
    {
        particleEffect = ParticleSystem.GetComponent<ParticleSystem>();
        GameObject firstBody = Instantiate(bodyPref, Bodysnake.transform);
        BodyPart.Add(firstBody);
    }

    private void FixedUpdate()
    {
        direction = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * direction * turnSpeed * Time.fixedDeltaTime);

        transform.position += transform.forward * speed * Time.fixedDeltaTime;

        Positions.Insert(0, new Vector3(transform.position.x, 0, transform.position.z));

        MovingBody();
    }

    private void MovingBody()
    {
        int index = 0;
        foreach (var body in BodyPart)
        {
            if (index * offset > Positions.Count - 1)
                return;
            Vector3 point = Positions[index * offset];
            Vector3 bodyDirection = point - body.transform.position;
            body.transform.position += bodyDirection * speed * Time.fixedDeltaTime;
            body.transform.LookAt(point);
            index++;
        }
        Positions.RemoveAt(Positions.Count - 1);
    }

    private void OnTriggerEnter(Collider other) 
    {
        switch(other.tag)
        {
            case "Food":
                TakeFood(other.gameObject);
                break;

            case "Wall":
                Invoke(nameof(GameOver), 0.15f);
                break;

            case "Body":
                Invoke(nameof(GameOver), 0.15f);
                break;
        }
    }

    private void TakeFood(GameObject item)
    {
        ParticleSystem.transform.position = item.transform.position;
        Destroy(item);
        particleEffect.Play();
        IncreaseSnake();
        var positionFood = new Vector3(Random.Range(-6.0f, 6.0f), 0.75f, Random.Range(-6.0f, 6.0f));
        Instantiate(foodPref, positionFood, foodPref.transform.rotation, transform.parent.parent);
        scorePoint++;
        points.text = "Points: " + scorePoint.ToString();
        soundEat.Play();
    } 

    private void GameOver()
    {
        DeathUI.SetActive(true);
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<ControllerCharacter>().enabled = false;
    } 

    private void IncreaseSnake()
    {
        GameObject nextBody = Instantiate(bodyPref, BodyPart[BodyPart.Count - 1].transform.position, BodyPart[BodyPart.Count - 1].transform.rotation, Bodysnake.transform);
        nextBody.transform.GetChild(0).tag = "Body";
        BodyPart.Add(nextBody);
    }
}
