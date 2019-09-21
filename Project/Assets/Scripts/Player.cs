using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private Rigidbody2D rb2d;
    private Collider2D col2d;

    public float gravityMult;
    public float moveSpeed;
    public float jumpSpeed;
    public bool isGrounded;
    public LayerMask ground;
    public float gravityScale;

    public AudioSource aSource;

    public AudioClip jumpSnd;
    public AudioClip deathSnd;
    public AudioClip collectibleSnd;

    public int lives;
    public float score;

    private Text highscoreText;
    private Text scoreText;
    private Text livesText;

    private float _elapsedTime;
    private bool _startTimer;
    private const float MAX_TAP_TIME = 0.15f;
    private Animator anim;

    private bool hit = false;
    public Camera main;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D attached to this GameObject.
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        highscoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
        scoreText = GameObject.Find("CurrentScoreText").GetComponent<Text>();
        livesText = GameObject.Find("LivesText").GetComponent<Text>();

        moveSpeed = 0f;
        jumpSpeed = 40f;
        gravityMult = 5f;

        score = 0;
        lives = 3;
        scoreText.text = score.ToString();
        highscoreText.text = GameManager.instance.highscore.ToString();

        if (lives==0)
        {
            Time.timeScale = 0;
            Debug.Log("You lost");
        }

        aSource.enabled=true;

        gravityScale = 1f;
        rb2d.gravityScale = gravityScale;

        main = Camera.main;
    }

    void Update()
    {

        if (Time.timeScale == 1)
        {
 
            if (rb2d.velocity.y < 0)    
            {
                // a heavier gravity when landing the jump to create a more crisp jump
                rb2d.velocity += Vector2.up * Physics2D.gravity.y * rb2d.gravityScale * (gravityMult - 1) * Time.deltaTime;
            }

            if (!RendererExtensions.IsVisibleFrom(gameObject.GetComponent<Renderer>(), main))
            {
                GameOver();
            }

            scoreText.text = score.ToString();
            

            if (score > GameManager.instance.highscore)
            {
                GameManager.instance.highscore = score;
                highscoreText.text = score.ToString();
            }


            isGrounded = Physics2D.IsTouchingLayers(col2d, ground);


#if UNITY_IOS || UNITY_ANDROID

            if(isGrounded)
            {

                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                if (Input.touchCount == 1)
                {

                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));

                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                        RaycastHit2D raycastHit = Physics2D.Raycast(pos, Input.GetTouch(0).position, 100f);

                        // as long as player is not hitting enemy, jump
                        // jump is much smoother this way, otherwise waiting till TouchPhase.end has a slight lag in jumping
                        if (!(raycastHit && raycastHit.collider.CompareTag("EnemyCollider")))
                        {

                            if (Input.GetTouch(0).tapCount == 1)
                            {

                                rb2d.gravityScale = gravityScale;

                                rb2d.velocity = new Vector2(2f, jumpSpeed);


                                PlaySoundEffect(jumpSnd, SoundManager.instance.sfxVolume);
                                anim.SetTrigger("Jump");
                            }

                        }
                        _startTimer = true;
                        _elapsedTime = 0f;


                    }
                }


                if (_startTimer)
                {
                    if (_elapsedTime < MAX_TAP_TIME)
                    {
                        _elapsedTime += Time.deltaTime;

                    }

                    if (_elapsedTime >= MAX_TAP_TIME)
                    {
                        _startTimer = false;
                    }
                }

            }

#endif
        }

    }


    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Enemy")
        {

            lives -= 1;
            livesText.text = lives.ToString();
            

            if (lives == 0)
            {
                GameOver();
            }
        }

        if(c.gameObject.tag=="Collectible")
        {
            score += 1;
            PlaySoundEffect(collectibleSnd, SoundManager.instance.sfxVolume);
        }
    }


    private void PlaySoundEffect(AudioClip clip, float volume)
    {
        if (!aSource.isPlaying && SoundManager.instance.sfxToggle ==1)      //only play sound effect if toggle is on
        {
            SoundManager.instance.PlaySingleSound(clip, volume);
        }
    }

    private void GameOver()
    {
        Debug.Log("You died");
        PlaySoundEffect(deathSnd, SoundManager.instance.sfxVolume);
        Time.timeScale = 0;
        aSource.enabled = false;
        GameManager.instance.Load("End");
    }

}



