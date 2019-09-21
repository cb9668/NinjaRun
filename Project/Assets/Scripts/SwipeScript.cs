using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwipeScript : MonoBehaviour
{
	private const float MIN_SWIPE_LENGTH = 20f;
	private const float MAX_SWIPE_TIME = 1f;

    
	public Rigidbody2D rb;
	private Vector3 _mouseStartPos;
	private float _elapsedTime;
	private bool _startTimer;
    private bool hit=false;

    private Transform spawnLocation;
    public float speed;
    public AudioClip enemySnd;

    public Transform spawnEndLocation;

    private Camera main;
    //public AudioSource aSource;

    void Start()
	{
		//_rb = GetComponent<Rigidbody2D>();
		_mouseStartPos = new Vector3(0f, 0f, 0f);
		_elapsedTime = 0f;
        speed = 3f;

        spawnLocation = GameObject.Find("EnemySpawnPoint").transform;
        spawnEndLocation = GameObject.Find("SpawnEndPoint").transform;
        main = Camera.main;
     
	}

	void Update()
	{

        if (transform.position.x < spawnEndLocation.position.x || transform.position.x > spawnEndLocation.position.x+50)
        {
            if (!RendererExtensions.IsVisibleFrom(gameObject.GetComponentInChildren<Renderer>(), main))
            {
                transform.position = new Vector3(spawnLocation.position.x, Random.Range((float)spawnLocation.position.y - 1.0f, (float)spawnLocation.position.y + 1.0f), spawnLocation.position.z);
                rb.velocity = Vector3.zero;
            }
        }


        //if (RendererExtensions.IsVisibleFrom(GetComponentInChildren<Renderer>() , Camera.main))
        // {
#if UNITY_EDITOR
        //SimulateMouseSwipe();
#endif

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 1)
            {

                //_rb.gravityScale = 0;
                Touch touch = Input.GetTouch(0);                
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                    //Debug.Log(ray);
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    //Debug.Log("touch pos " + pos);
                    //Collider2D[] overlap = Physics2D.OverlapPointAll(pos);

                    RaycastHit2D raycastHit = Physics2D.Raycast(pos, Input.GetTouch(0).position, 100f);

                    if (raycastHit)
                    {
                        //Debug.Log("Something Hit");
                        //Debug.Log(raycastHit.collider.name);
                        if (raycastHit.collider.CompareTag("EnemyCollider") || raycastHit.collider.CompareTag("Enemy"))
                        {
                            //Debug.Log("Enemy hit");
                            hit = true;
                            rb = raycastHit.transform.gameObject.GetComponentInParent<Rigidbody2D>();
                            //Debug.Log("name" + rb.name);
                        }
                    }

                    _startTimer = true;
                    _elapsedTime = 0f;
                }

                if (touch.phase == TouchPhase.Ended && _elapsedTime < MAX_SWIPE_TIME && hit)
                {
                    //Debug.Log("swipe");
                    //Debug.Log("name" + rb.name);
                    Swipe(touch.deltaPosition, rb);
                    SoundManager.instance.PlaySingleSound(enemySnd, SoundManager.instance.sfxVolume);

            }
            //}
#endif

            if (_startTimer)
            {
                if (_elapsedTime < MAX_SWIPE_TIME)
                {
                    _elapsedTime += Time.deltaTime;

                }

                if (_elapsedTime >= MAX_SWIPE_TIME)
                {
                    _startTimer = false;
                }
            }
        }


        //if (RendererExtensions.IsVisibleFrom(GetComponentInChildren<Renderer>(), Camera.main) == false && hit)
        //{
        //    transform.position = spawnLocation.position;
        //    hit = false;
        //}

    

	/// <summary>
	/// This will simulate both directional and free swipe using the Left mouse button.
	/// </summary>
	//private void SimulateMouseSwipe()
	//{
	//	if (Input.GetMouseButtonDown(0))
	//	{
	//		_mouseStartPos = Input.mousePosition;
	//		_startTimer = true;
	//		_elapsedTime = 0f;
	//	}

	//	if (Input.GetMouseButtonUp(0) && _elapsedTime < MAX_SWIPE_TIME)
	//	{
	//		_startTimer = false;

	//		Vector3 mouseEndPos = Input.mousePosition;
	//		Vector3 direction = (mouseEndPos - _mouseStartPos);

	//		Swipe(direction);
	//	}
	//}

	private void Swipe(Vector3 pDirection, Rigidbody2D rb)
	{
        // The greater your swipe the faster the cube will spin. This can also
        // represent the distance of the swipe as the magnitude is just the length
        // of the vector.
        
		float force = pDirection.magnitude;
        // Debug.Log("Direction Vector: " + pDirection.ToString());
        // Debug.Log("Swipe Distance: " + force);
        // Debug.Log("Valid Swipe - Elapsed Time: " + _elapsedTime);

        //Debug.Log(pDirection.magnitude);
		if (pDirection.magnitude > MIN_SWIPE_LENGTH)
		{
			// Normalize to get a unit vector.
			pDirection.Normalize();

			rb.AddForce(new Vector2(0f, 1f) * pDirection.x * force*speed, ForceMode2D.Impulse);
			rb.AddForce(new Vector2(1f, 0f) * pDirection.y * force*speed, ForceMode2D.Impulse);

		}
	}

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(spawnLocation.position.x, Random.Range((float)spawnLocation.position.y - 1.0f, (float)spawnLocation.position.y + 1.0f), spawnLocation.position.z);
        hit = false;
    }

}
