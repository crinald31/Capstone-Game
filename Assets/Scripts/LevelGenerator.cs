using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layout;
    private GameObject endRoom;
    public Color start;
    public Color end;
    public Transform generatorPoint;
    public LayerMask whatIsRoom;
    public RoomPrefabs rooms;
    public enum Direction {up, right, down, left};
    public Direction direction;

    public int distanceToEnd;
    public float xOffset = 18f;
    public float yOffset = 10f;

    public CenterRoom centerStart;
    public CenterRoom centerEnd;
    public CenterRoom[] centers;

    private List<GameObject> layoutRoomList = new List<GameObject>();
    private List<GameObject> outlines = new List<GameObject>();

    void Start()
    {
        Instantiate(layout, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = start;
        direction = (Direction) Random.Range(0, 4);
        MoveGeneration();

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layout, generatorPoint.position, generatorPoint.rotation);

            layoutRoomList.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = end;
                layoutRoomList.RemoveAt(layoutRoomList.Count - 1);
                endRoom = newRoom;
            }

            direction = (Direction)Random.Range(0, 4);
            MoveGeneration();

            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                MoveGeneration();
            }
        }

        CreateOutline(Vector3.zero);

        foreach (GameObject room in layoutRoomList)
        {
            CreateOutline(room.transform.position);
        }

        CreateOutline(endRoom.transform.position);

        foreach (GameObject outline in outlines)
        {
            bool generateCenter = true;

            if (outline.transform.position == new Vector3(0f, 0f, 0f))
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (generateCenter)
            {
                int selectedCenter = Random.Range(0, centers.Length);
                Instantiate(centers[selectedCenter], outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void MoveGeneration()
    {
        switch(direction)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateOutline(Vector3 position)
    {
        bool isRoomAbove = Physics2D.OverlapCircle(position + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
        bool isRoomBelow = Physics2D.OverlapCircle(position + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool isRoomRight = Physics2D.OverlapCircle(position + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);
        bool isRoomLeft = Physics2D.OverlapCircle(position + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        int directionCount = 0;

        if (isRoomAbove)
        {
            directionCount++;
        }
        if (isRoomBelow)
        {
            directionCount++;
        }
        if (isRoomRight)
        {
            directionCount++;
        }
        if (isRoomLeft)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("No Room Exists");
                break;
            case 1:
                if (isRoomAbove)
                {
                    outlines.Add(Instantiate(rooms.singleUp, position, transform.rotation));
                }
                if (isRoomBelow)
                {
                    outlines.Add(Instantiate(rooms.singleDown, position, transform.rotation));
                }
                if (isRoomRight)
                {
                    outlines.Add(Instantiate(rooms.singleRight, position, transform.rotation));
                }
                if (isRoomLeft)
                {
                    outlines.Add(Instantiate(rooms.singleLeft, position, transform.rotation));
                }
                break;
            case 2:
                if (isRoomAbove && isRoomBelow)
                {
                    outlines.Add(Instantiate(rooms.doubleUpDown, position, transform.rotation));
                }
                if (isRoomRight && isRoomLeft)
                {
                    outlines.Add(Instantiate(rooms.doubleLeftRight, position, transform.rotation));
                }
                if (isRoomAbove && isRoomRight)
                {
                    outlines.Add(Instantiate(rooms.doubleUpRight, position, transform.rotation));
                }
                if (isRoomRight && isRoomBelow)
                {
                    outlines.Add(Instantiate(rooms.doubleRightDown, position, transform.rotation));
                }
                if (isRoomAbove && isRoomBelow)
                {
                    outlines.Add(Instantiate(rooms.doubleUpDown, position, transform.rotation));
                }
                if (isRoomBelow && isRoomLeft)
                {
                    outlines.Add(Instantiate(rooms.doubleDownLeft, position, transform.rotation));
                }
                if (isRoomLeft && isRoomAbove)
                {
                    outlines.Add(Instantiate(rooms.doubleLeftUp, position, transform.rotation));
                }
                break;
            case 3:
                if (isRoomAbove && isRoomBelow && isRoomRight)
                {
                    outlines.Add(Instantiate(rooms.tripleUpRightDown, position, transform.rotation));
                }
                if (isRoomLeft && isRoomBelow && isRoomRight)
                {
                    outlines.Add(Instantiate(rooms.tripleRightDownLeft, position, transform.rotation));
                }
                if (isRoomAbove && isRoomBelow && isRoomLeft)
                {
                    outlines.Add(Instantiate(rooms.tripleDownLeftUp, position, transform.rotation));
                }
                if (isRoomAbove && isRoomLeft && isRoomRight)
                {
                    outlines.Add(Instantiate(rooms.tripleLeftUpRight, position, transform.rotation));
                }
                break;
            case 4:
                if (isRoomAbove && isRoomBelow && isRoomRight && isRoomLeft)
                {
                    outlines.Add(Instantiate(rooms.fourway, position, transform.rotation));
                }
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp;
    public GameObject singleDown;
    public GameObject singleRight;
    public GameObject singleLeft;
    public GameObject doubleUpDown;
    public GameObject doubleLeftRight;
    public GameObject doubleUpRight;
    public GameObject doubleRightDown;
    public GameObject doubleDownLeft;
    public GameObject doubleLeftUp;
    public GameObject tripleUpRightDown;
    public GameObject tripleRightDownLeft;
    public GameObject tripleDownLeftUp;
    public GameObject tripleLeftUpRight;
    public GameObject fourway;
}
