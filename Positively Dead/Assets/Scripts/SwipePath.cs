﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipePath : MonoBehaviour
{
    public List<GameObject> hitObjects;
    GameObject player;

    public GameObject[] fogTypes;
    public Material defaultMaterial;
    public Material highlighter;
    Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = GameObject.FindGameObjectWithTag("Start Tile").transform.position;
        startPosition = player.transform.position;
        SetFog();
    }

    // Update is called once per frame
    void Update()
    {
        DrawPath();
    }

    /// <summary>
    /// Draws a path along the player's cursor/finger and moves the player along that path.
    /// </summary>
    void DrawPath()
    {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            this.GetComponent<TrailRenderer>().time = Mathf.Infinity;   // Keep the Trail on the screen for as long as the mouse/finger is clicking/touching the screen

            // Draw the trail of the click/touch
            // Code given by tutorial video for drawing lines to the screen in unity
            // Link: https://www.youtube.com/watch?v=cHVZ0SYIHkI
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
                this.transform.position = mRay.GetPoint(rayDistance);

            // Raycast to collide with the tiles and create a list of tiles that the player has chosen to go over
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                // If this is the first tile in the list, check to see that it is where the player is
                if (hitObjects.Count == 0)
                {
                    if (Mathf.Abs(hit.collider.gameObject.transform.position.y - player.transform.position.y) <= 0.1f)
                    {
                        if (Mathf.Abs(hit.collider.gameObject.transform.position.x - player.transform.position.x) <= 0.1f)
                        {
                            if (!hitObjects.Contains(hit.collider.gameObject))
                            {
                                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
                                hit.collider.gameObject.GetComponent<SpriteRenderer>().material = highlighter;
                                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                                hitObjects.Add(hit.collider.gameObject);
                            }
                        }
                    }
                }
                else if (hitObjects.Count >= 1)// If there are other tiles already existing in the list, check to see if the next tile is adjacent to the previous
                {
                    // If they have the same Y (same row)
                    if (Mathf.Abs(hit.collider.gameObject.transform.position.y - hitObjects[hitObjects.Count - 1].transform.position.y) <= 0.1f)
                    {
                        if (Mathf.Abs(Mathf.Abs(hit.collider.gameObject.transform.position.x) - Mathf.Abs(hitObjects[hitObjects.Count - 1].transform.position.x)) <= 3.3f)
                        {
                            // If there are other tiles already existing in the list, check to see if the next tile is adjacent to the previous
                            if (!hitObjects.Contains(hit.collider.gameObject))
                            {
                                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
                                hit.collider.gameObject.GetComponent<SpriteRenderer>().material = highlighter;
                                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                                hitObjects.Add(hit.collider.gameObject);
                            }
                        }
                    }
                    // If they have the same X (same column)
                    else if (Mathf.Abs(hit.collider.gameObject.transform.position.x - hitObjects[hitObjects.Count - 1].transform.position.x) <= 0.1f)
                    {
                        if (Mathf.Abs(Mathf.Abs(hit.collider.gameObject.transform.position.y) - Mathf.Abs(hitObjects[hitObjects.Count - 1].transform.position.y)) <= 3.3f)
                        {
                            // If there are other tiles already existing in the list, check to see if the next tile is adjacent to the previous
                            if (!hitObjects.Contains(hit.collider.gameObject))
                            {
                                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
                                hit.collider.gameObject.GetComponent<SpriteRenderer>().material = highlighter;
                                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                                hitObjects.Add(hit.collider.gameObject);
                            }
                        }
                    }
                }

            }
        }
        else
        {
            // Clear the screen of the path, reset the hitObjects List and mvoe the player to the last valid tile they were holding their click on
            this.GetComponent<TrailRenderer>().time = 0;
            MovePlayer();
        }
    }

    /// <summary>
    /// Moves the player smoothly along the tiles that the player selected with their swipe
    /// </summary>
    void MovePlayer()
    {
        // Check if the player is not currently on the tile at the front of the list
        if (hitObjects.Count > 0)
        {
            if (Vector3.Distance(player.transform.position, hitObjects[0].transform.position) >= 0.1f)
            {
                if (player.transform.position.x != hitObjects[0].transform.position.x)
                {
                    if (player.transform.position.x > hitObjects[0].transform.position.x)
                    {
                        player.transform.position -= new Vector3(2f * Time.deltaTime, 0.0f);
                    }
                    else
                    {
                        player.transform.position += new Vector3(2f * Time.deltaTime, 0.0f);
                    }
                }
                if (player.transform.position.y != hitObjects[0].transform.position.y)
                {
                    if (player.transform.position.y > hitObjects[0].transform.position.y)
                    {
                        player.transform.position -= new Vector3(0.0f, 2f * Time.deltaTime);
                    }
                    else
                    {
                        player.transform.position += new Vector3(0.0f, 2f * Time.deltaTime);
                    }
                }
            }
            else
            {
                player.transform.position = hitObjects[0].transform.position; // used to smooth the movement
            }

            // Brighten the tile before walking onto it so the player sees what they are walking into
            if (Vector3.Distance(player.transform.position, hitObjects[0].transform.position) <= 3.0f)
            {
                hitObjects[0].GetComponent<SpriteRenderer>().material = defaultMaterial;
                hitObjects[0].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }

            // Remove the current front of the list whenever the player has reached that position.
            if (Vector3.Distance(player.transform.position, hitObjects[0].transform.position) <= 0.1f)
            {
                // "Kill" the player by resetting them to the start if they walk over a trap tile
                if (hitObjects[0].tag.Equals("Trap Tile"))
                {
                    // Move player
                    player.transform.position = startPosition;

                    // Clean up fog and reset tile opacity
                    SetFog();

                    // Clean out any objects remaining that were hit by the swipe
                    hitObjects.Clear();

                    // Move swipe object back to player
                    this.transform.position = player.transform.position;
                }
                else
                {
                    hitObjects[0].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); // brighten the tile
                    hitObjects.RemoveAt(0);
                }
            }
        }
        else
        {
            this.transform.position = player.transform.position;
        }
    }

    /// <summary>
    /// Sets up the fog and hides tiles by lowering their visibility.
    /// </summary>
    void SetFog()
    {
        // Clear out existing fog
        GameObject[] fogTiles = GameObject.FindGameObjectsWithTag("Fog");
        for (int i = 0; i < fogTiles.Length; i++)
        {
            Destroy(fogTiles[i]);
        }

        GameObject[] walkTiles = GameObject.FindGameObjectsWithTag("Walk Tile");
        GameObject[] trapTiles = GameObject.FindGameObjectsWithTag("Trap Tile");

        // Give all walk tiles fog and lower their opacity
        for (int i = 0; i < walkTiles.Length; i++)
        {
            Vector3 tilePosition = walkTiles[i].transform.position;
            int randNum = Random.Range(0, 10);
            if (randNum < 5)
            {
                Instantiate(fogTypes[0], new Vector3(tilePosition.x + 0.65f, tilePosition.y - 0.64f), fogTypes[0].transform.rotation);
                Instantiate(fogTypes[1], new Vector3(tilePosition.x - 0.65f, tilePosition.y + 0.75f), fogTypes[1].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x - 0.8f, tilePosition.y - 0.69f), fogTypes[2].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x + 0.8f, tilePosition.y + 1f), fogTypes[2].transform.rotation);
            }
            else
            {
                Instantiate(fogTypes[0], new Vector3(tilePosition.x - 0.65f, tilePosition.y - 0.64f), fogTypes[0].transform.rotation);
                Instantiate(fogTypes[1], new Vector3(tilePosition.x + 0.75f, tilePosition.y + 0.75f), fogTypes[1].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x + 0.8f, tilePosition.y - 0.69f), fogTypes[2].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x - 0.8f, tilePosition.y + 1f), fogTypes[2].transform.rotation);
            }

            walkTiles[i].GetComponent<SpriteRenderer>().material = defaultMaterial;
            walkTiles[i].GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 0.1f);
        }

        // Give all trap tiles fog and lower their opacity
        for (int i = 0; i < trapTiles.Length; i++)
        {
            Vector3 tilePosition = trapTiles[i].transform.position;
            int randNum = Random.Range(0, 10);
            if (randNum < 5)
            {
                Instantiate(fogTypes[0], new Vector3(tilePosition.x + 0.57f, tilePosition.y - 0.64f), fogTypes[0].transform.rotation);
                Instantiate(fogTypes[1], new Vector3(tilePosition.x - 0.57f, tilePosition.y + 0.66f), fogTypes[1].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x - 0.75f, tilePosition.y - 0.69f), fogTypes[2].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x + 0.75f, tilePosition.y + 1f), fogTypes[2].transform.rotation);
            }
            else
            {
                Instantiate(fogTypes[0], new Vector3(tilePosition.x - 0.57f, tilePosition.y - 0.64f), fogTypes[0].transform.rotation);
                Instantiate(fogTypes[1], new Vector3(tilePosition.x + 0.57f, tilePosition.y + 0.66f), fogTypes[1].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x + 0.75f, tilePosition.y - 0.69f), fogTypes[2].transform.rotation);
                Instantiate(fogTypes[2], new Vector3(tilePosition.x - 0.75f, tilePosition.y + 1f), fogTypes[2].transform.rotation);
            }

            trapTiles[i].GetComponent<SpriteRenderer>().material = defaultMaterial;
            trapTiles[i].GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 0.1f);
        }
    }
}