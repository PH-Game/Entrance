using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public string sceneName;
    public GameObject player;
    private BoxCollider2D playerBoxCollider2D;
    private BoxCollider2D doorBoxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        playerBoxCollider2D = player.GetComponent<BoxCollider2D>();
        doorBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doorBoxCollider2D.IsTouching(playerBoxCollider2D))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
