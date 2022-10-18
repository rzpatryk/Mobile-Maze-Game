using Assets.Scripts.MazeParts.Cells;
using Assets.Scripts.MazeParts.Grids;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCircleMaze : DisplaySquareMaze
{
    public override void DisplayMaze(MazeGrid mazeGrid)
    {
        WallScale = 0.04f;
        float theta, inner_radius, outer_radius, theta_ccw, theta_cw;
        float ax, ay, bx, by, cx, cy, dx, dy;
        SetCellSize(mazeGrid.Row+1, mazeGrid.Column, 0.95f, 0);
        for (int i = 0; i < mazeGrid.Grid.Length; i++)
        {
            for (int j = 0; j < mazeGrid.Grid[i].Length; j++)
            {
                PolarCell cell = (PolarCell)mazeGrid.Grid[i][j];
                int row1 = cell.Row;
                theta = (float)(2 * Math.PI / mazeGrid.Grid[row1].Length);
                inner_radius = row1 * (CellHeight/ 2); 

                outer_radius = ((row1 + 1) * CellHeight / 2);

                theta_ccw = cell.Column * theta;
                theta_cw = (cell.Column + 1) * theta;

                ax = (float)(inner_radius * Math.Cos(theta_ccw));
                ay = (float)(inner_radius * Math.Sin(theta_ccw));
                bx = (float)(outer_radius * Math.Cos(theta_ccw));
                by = (float)(outer_radius * Math.Sin(theta_ccw));
                cx = (float)(inner_radius * Math.Cos(theta_cw));
                cy = (float)(inner_radius * Math.Sin(theta_cw));
                dx = (float)(outer_radius * Math.Cos(theta_cw));
                dy = (float)(outer_radius * Math.Sin(theta_cw));

                if (cell.Row > 0)
                {
                    Vector3 positionA = new Vector3(ax, ay, -1);
                    Vector3 positionB = new Vector3(bx, by, -1);
                    Vector3 positionC = new Vector3(cx, cy, -1);
                    Vector3 positionD = new Vector3(dx, dy, -1);
                    
                    if (i == mazeGrid.Grid.Length - 1)
                    {
                        CreateWall(positionB, positionD);
                    }
                    if (!cell.Linked(cell.Inward))
                    {

                        CreateWall(positionA, positionC);


                    }
                    if (cell.Neighbours.ContainsKey("Cw") && !cell.Linked(cell.Neighbours["Cw"]))
                    {
                        CreateWall(positionC, positionD);
                    }

                }

            }
        }
    }
}
