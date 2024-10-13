using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool Is_Running = false;
    public GameObject Player;
    public Animator player_animator;
    public bool Game_started = false;
    public bool Game_stopped = false;
    public GameObject Camera;
    public GameObject toy;
    public GameObject Laser;
    public Animator toy_animator;
    public ParticleSystem blood_splash;
    public GameObject blood;
    AudioSource source;
    public AudioClip step;
    public AudioClip hit;
    public AudioClip shooting;
    public AudioClip fall;
    public GameObject ui_start;
    public GameObject ui_GameOver;
    public GameObject ui_Win;
    public TextMeshProUGUI ui_guide;
    KeyCode key1, key2, key3;
    float speed=1,p_speed;
    float steps_counter;
    private object ScebeManager;
   
    void Start()
    {
        source = GetComponent<AudioSource>();
       
        ui_start.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Game_stopped)
        {
            SceneManager.LoadScene("SampleScene");
        }
            if (Input.GetKeyDown(KeyCode.Space) && !Game_started)
        {
            ui_start.SetActive(false);
            Is_Running = true;
            player_animator.SetTrigger("run");
            Game_started = true;
            StartCoroutine(Sing());
        }

        if (Is_Running)
        {
            steps_counter += Time.deltaTime;
            if (steps_counter > .25f)
            {
                steps_counter = 0;
                source.PlayOneShot(step);
            }

            Camera.transform.position -= new Vector3(0, 0, .5f * Time.deltaTime);
            Player.transform.position -= new Vector3(0, 0, .5f * Time.deltaTime);
            Debug.Log("Moving Player and Camera");
        }
        
        
        

        // Move key checking outside of the game start logic
        if (Input.GetKey(key1) && Input.GetKey(key2) && Input.GetKey(key3) && !Game_stopped)
        {
            Is_Running = false;
            // Stop the player animation or perform other actions
            player_animator.speed = 0;
        }
        else if ((Input.GetKeyUp(key1) || Input.GetKeyUp(key2) || Input.GetKeyUp(key3)) && !Game_stopped)
        {
            Is_Running = true;
            // Resume the player animation or perform other actions
            player_animator.speed = 1;
        }
    }

    IEnumerator Sing()
    {
       
        toy.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(4.5f  );

        key1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), ((char)('A' + Random.Range(0, 26))).ToString());
        key2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), ((char)('A' + Random.Range(0, 26))).ToString());
        key3 = (KeyCode)System.Enum.Parse(typeof(KeyCode), ((char)('A' + Random.Range(0, 26))).ToString());

        ui_guide.text = "Press " + key1 + " + " + key2 + " + " + key3 + " to Stop";

        toy_animator.SetTrigger("look");
        yield return new WaitForSeconds(3 /speed);

        if (Is_Running)
        {
            Debug.Log("Shoot The Player");
            GameObject New_Laser = Instantiate(Laser);
            New_Laser.transform.position = toy.transform.GetChild(0).transform.position;
            Game_stopped = true;
            source.PlayOneShot(shooting);
        }
        ui_guide.text = " ";
        yield return new WaitForSeconds(2 / speed  );
        toy_animator.SetTrigger("idle");
        yield return new WaitForSeconds(1 / speed);
        speed = speed * 1.15f;
        p_speed = speed;
        toy.GetComponent<AudioSource>().pitch = p_speed;
        toy.GetComponent<AudioSource>().Stop();
        
        
        //Debug.Log(speed/speed);
        if (!Game_stopped)
        {
            StartCoroutine(Sing());
        }
    }

    public void Hit_Player()
    {
        Is_Running = false;
        player_animator.SetTrigger("idle");
        Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 2, 2);
        Player.GetComponent<Rigidbody>().angularVelocity = new Vector3(3, 0, 0);
        Camera.GetComponent<Animator>().Play("Camera_lose");
        blood_splash.Play();
        StartCoroutine(ShowBlood());
        source.PlayOneShot(hit);
    }

    IEnumerator ShowBlood()
    {
        yield return new WaitForSeconds(.5f);
        ui_GameOver.SetActive(true);
        source.PlayOneShot(fall);
        blood.SetActive(true);
        blood.transform.position = new Vector3(Player.transform.position.x, 0.006f, Player.transform.position.z);
    }

    public IEnumerator PlayerWin()
    {
        Game_stopped = true;
        yield return new WaitForSeconds(1f);
        Is_Running = false;
        player_animator.SetTrigger("idle");
        ui_Win.SetActive(true);
    }
}