using Assets.Scripts.MazeParts.Cells;
using Assets.Scripts.MazeParts.Grids;
using Assets.Scripts.MazeParts.Path;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplaySquareMaze : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject startImage;
    [SerializeField]
    private GameObject endImage;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]

    private Canvas canvas;
    private GameObject wall;
    private GameObject player;

    private float cellHeight;
    private float cellWidth;
    private float wallScale;


    public float CellHeight { get => cellHeight; set => cellHeight = value; }
    public float CellWidth { get => cellWidth; set => cellWidth = value; }
    public float WallScale { get => wallScale; set => wallScale = value; }

    protected void SetCellSize(int row, int col, float heightScale, float widthScale)
    {
        float backgroundHeight = canvas.GetComponent<RectTransform>().rect.height;
        float backgroundWidth = canvas.GetComponent<RectTransform>().rect.width;
        CellHeight = ((backgroundHeight * 0.9f) / row * canvas.GetComponent<RectTransform>().localScale.y);
        CellWidth = ((backgroundWidth * 0.75f) / col * canvas.GetComponent<RectTransform>().localScale.x);
    }

    public virtual void DisplayMaze(MazeGrid mazeGrid)
    {
        wallScale = 0.08f;
        //SetCellSize(mazeGrid.Row, mazeGrid.Column, 0.9f, 0.79f);
        SetCellSize(mazeGrid.Row, mazeGrid.Column, 1, 1);
        float x1, x2, y1, y2;
        for (int i = 0; i < mazeGrid.Grid.Length; i++)
        {
            for (int j = 0; j < mazeGrid.Grid[i].Length; j++)
            {
                Cell cell = mazeGrid.Grid[i][j];
                x1 = (cell.Column * CellWidth) - ((mazeGrid.Grid[0].Length / 2.0f) * CellWidth);
                y1 = (cell.Row * CellHeight) - ((mazeGrid.Grid.Length / 2.0f) * CellHeight);
                x2 = ((cell.Column + 1) * CellWidth) - ((mazeGrid.Grid[0].Length / 2.0f) * CellWidth);
                y2 = ((cell.Row + 1) * CellHeight) - ((mazeGrid.Grid.Length / 2.0f) * CellHeight);

                if (i == 0 && j == 0)
                {
                    CreateStartImage(x1 - CellWidth / 1.5f, y1 + CellHeight / 2);
                }
                if (i == mazeGrid.Row - 1 && j == mazeGrid.Column - 1)
                {
                    CreateEndImage(x2 + CellWidth / 2, y2 - CellHeight / 2);
                }



                if (!cell.Neighbours.ContainsKey("South") || (cell.Neighbours.ContainsKey("South") && !(cell.Linked(cell.Neighbours["South"]))))
                {
                    CreateWall(new Vector3(x1, y1, -1), new Vector3(x2, y1, -1));
                }
                if (!cell.Neighbours.ContainsKey("West"))
                {

                    if (i == 0 && j == 0)
                    {
                        CreateWall(new Vector3(x1, y1, -1), new Vector3(x1, y2, -1), "Start");
                        CreatePlayer(new Vector3((x1 + CellWidth / 2), (y1 + CellHeight / 2), -2));
                    }
                    else
                    {
                        CreateWall(new Vector3(x1, y1, -1), new Vector3(x1, y2, -1));
                    }

                }
                if (!cell.Neighbours.ContainsKey("East") || (cell.Neighbours.ContainsKey("East") && !(cell.Linked(cell.Neighbours["East"]))))
                {
                    if (i == mazeGrid.Row - 1 && j == mazeGrid.Column - 1)
                    {
                        CreateWall(new Vector3(x2, y1, -1), new Vector3(x2, y2, -1), "End");
                    }
                    else
                        CreateWall(new Vector3(x2, y1, -1), new Vector3(x2, y2, -1));
                }
                if (!(cell.Neighbours.ContainsKey("North")))
                {
                    CreateWall(new Vector3(x1, y2, -1), new Vector3(x2, y2, -1));
                }

            }
        }
    }

    protected void CreateWall(Vector3 startPosition, Vector3 endPosition, string startOrEnd = null)
    {
        Vector3 notTar = (endPosition - startPosition).normalized;
        float angle = Mathf.Atan2(notTar.y, notTar.x) * Mathf.Rad2Deg;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0, angle - 90);
        float distance = Vector3.Distance(startPosition, endPosition);
        wall = Instantiate(wallPrefab, startPosition, Quaternion.identity);
        wall.transform.position = (startPosition + endPosition) / 2;
        wall.transform.rotation = rotation;
        wall.transform.SetParent(GameObject.FindGameObjectWithTag("Maze").transform, false);
        wall.transform.localScale = new Vector3(CellHeight * wallScale, distance + (CellHeight * wallScale)/2, 0);


        if (startOrEnd != null)
        {
            wall.GetComponent<SpriteRenderer>().color = new Color(wall.GetComponent<SpriteRenderer>().color.r, wall.GetComponent<SpriteRenderer>().color.g, wall.GetComponent<SpriteRenderer>().color.b, 0);
            if (startOrEnd.Equals("End"))
                wall.tag = "End";
        }
    }

    public virtual void CreateStartImage(float x, float y)
    {
        startImage.transform.localPosition = new Vector3(x, y, -1f);
        startImage.transform.localScale = new Vector2(CellWidth, CellHeight);
    }
    public virtual void CreateEndImage(float x, float y, bool end = false)
    {
        endImage.transform.localPosition = new Vector3(x, y, -1f);
        endImage.transform.localScale = new Vector2(CellWidth, CellHeight);
        if (end)
        {
            endImage.tag = "End";
        }
    }

    public virtual void CreatePlayer(Vector3 position)
    {
        player = Instantiate(playerPrefab, position, Quaternion.identity) as GameObject;
        player.transform.SetParent(GameObject.FindGameObjectWithTag("Maze").transform, false);
        player.transform.localScale = new Vector3(CellHeight, CellHeight, 0);

    }
}
