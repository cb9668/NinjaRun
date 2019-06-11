using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer
/// </summary>
public class RepeatingBackground : MonoBehaviour
{
    /// Scrolling speed
    public Vector2 speed = new Vector2(0.5f, 0);

    /// Moving direction
    public Vector2 direction = new Vector2(-1, 0);

    /// Movement should be applied to camera
    public bool isLinkedToCamera = false;

    /// 2 - List of children with a renderer.
    private List<SpriteRenderer> backgroundPart;

    public GameObject background;
    private float backgroundWidth;

    void Start()
    {

            // Get all the children of the layer with a renderer
        backgroundPart = new List<SpriteRenderer>();

        backgroundWidth = background.GetComponent<SpriteRenderer>().bounds.size.x * transform.localScale.x;

        for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                SpriteRenderer r = child.GetComponent<SpriteRenderer>();

                // Add only the visible children
                if (r != null)
                {
                    backgroundPart.Add(r);
                }
           
            // Sort by position.
            backgroundPart = backgroundPart.OrderBy( t => t.transform.position.x).ToList();
        }
    }

    void Update()
    {
        // Movement
        Vector3 movement = new Vector3( speed.x * direction.x,speed.y * direction.y, 0)* Time.deltaTime;

        transform.Translate(movement);

        // Move the camera
        if (isLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

            // Get the first object.
            // The list is ordered from left (x position) to right.
            SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

            if (firstChild != null)
            {
                // Check if the child is already (partly) before the camera.
                // We test the position first because the IsVisibleFrom
                // method is a bit heavier to execute.
                if (firstChild.transform.position.x < Camera.main.transform.position.x)
                {
                    // If the child is already on the left of the camera,
                    // we test if it's completely outside and needs to be
                    // recycled.
                    if (firstChild.IsVisibleFrom(Camera.main) == false)
                    {
                        // Get the last child position.
                        SpriteRenderer lastChild = backgroundPart.LastOrDefault();

                        Vector3 lastPosition = lastChild.transform.position;
                        Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

                        firstChild.transform.position = new Vector3(firstChild.transform.position.x+backgroundWidth*2, firstChild.transform.position.y, firstChild.transform.position.z);

                        // Set the recycled child to the last position of the backgroundPart list.
                        backgroundPart.Remove(firstChild);
                        backgroundPart.Add(firstChild);
                    }
                }
            }
        
    }
}


