using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]

public class World : MonoBehaviour {

    private Cube[,,] world = new Cube[100,100,100];
    private List<string> block_types = new List<string> { "Dirt" };
    private Vector3[] world_vertices = new Vector3[0];
    private Vector3[] world_normals = new Vector3[0];
    private Vector2[] world_uv = new Vector2[0];
    private int[] world_triangles = new int[0];
    private int cubeCount = 0;
    private Mesh mesh;
    private Tools tools;

    // Use this for initialization
    void Start() {
        mesh = GetComponent<MeshFilter>().mesh;
        Renderer renderer = GetComponent<Renderer>();
        tools = new Tools();
        generateWorld();
    }

    // Update is called once per frame
    void Update() {

    }

    private void generateWorld() // Temporary world generation for testing to create a 10 x 10 x 10 block layout
    {
        mesh.Clear();

        generateWorldArray();

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    generateCube(x, y, z);
                }
            }
        }

        mesh.vertices = world_vertices;
        mesh.normals = world_normals;
        mesh.uv = world_uv;
        mesh.triangles = world_triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();
        mesh.subMeshCount = 5;
        Debug.Log(mesh.subMeshCount);
    }

    #region World Generation
    private void generateWorldArray() // Temporary world generation for testing to create a 10 x 10 x 10 block layout
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    world[x, y, z] = new Cube("Dirt");
                }
            }
        }
    }

    private Hashtable checkCubeSides(int x, int y, int z)
    {
        Hashtable sides = new Hashtable();


        if (cube_type_bool(x + 1, y, z) && x != world.GetLength(0) - 1)
        {
            sides.Add("Right", true); // right side
        }
        else
        {
            sides.Add("Right", false);
        }

        if (cube_type_bool(x - 1, y, z) && x > 0)
        {
            sides.Add("Left", true); // left side
        }
        else
        {
            sides.Add("Left", false);
        }

        if (cube_type_bool(x, y, z + 1) && z != world.GetLength(1) - 1)
        {
            sides.Add("Front", true); // front side
        }
        else
        {
            sides.Add("Front", false);
        }

        if (cube_type_bool(x, y, z - 1) && z > 0)
        {
            sides.Add("Back", true); // back side
        }
        else
        {
            sides.Add("Back", false);
        }
        
        if (cube_type_bool(x, y, y + 1) && y != world.GetLength(1) - 1)
        {
            sides.Add("Top", true); // top side
        }
        else
        {
            sides.Add("Top", false);
        }

        if (cube_type_bool(x, y - 1, z) && y > 0)
        {
            sides.Add("Bottom", true); // bottom side
        }
        else
        {
            sides.Add("Bottom", false);
        }

        return sides;
    }
    #endregion

    private bool cube_type_bool(int x, int y, int z)
    {
        try
        {
            if (block_types.Contains(world[x, y, z].type))
            {
                return true;
            }
            else
            {
                return false;
            }
        } catch (Exception)
        {
            return false;
        }
    }

    private void generateCube(int x, int y, int z)
    {
        Hashtable sides = checkCubeSides(x, y, z);

        if (sides["Right"].Equals(true) && sides["Left"].Equals(true) && sides["Front"].Equals(true) && sides["Back"].Equals(true) && sides["Top"].Equals(true) && sides["Bottom"].Equals(true))
        {
            return; // Doesn't generate anything when all sides are hidden from the player
        }

        float length = 1f;
        float width = 1f;
        float height = 1f;

         if (sides["Right"].Equals(false))
         {
             cubeRight(x, y, z, length, width, height);
         } 
         if (sides["Left"].Equals(false))
         {
             cubeLeft(x, y, z, length, width, height);
         }
         if (sides["Front"].Equals(false))
         {
             cubeFront(x, y, z, length, width, height);
         }
         if (sides["Back"].Equals(false))
         {
             cubeBack(x, y, z, length, width, height);
         }
         if (sides["Top"].Equals(false))
         {
             cubeTop(x, y, z, length, width, height);
         }
         if (sides["Bottom"].Equals(false))
         {
             cubeBottom(x, y, z, length, width, height);
         }

    }

    #region Cube Bottom
    private void cubeBottom(int x, int y, int z, float length, float width, float height)
    {
        
        Vector3 p0 = new Vector3(x + -length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p1 = new Vector3(x + length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p2 = new Vector3(x + length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p3 = new Vector3(x + -length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p4 = new Vector3(x + -length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p5 = new Vector3(x + length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p6 = new Vector3(x + length * .5f, y + width * .5f, z + -height * .5f);
        Vector3 p7 = new Vector3(x + -length * .5f, y + width * .5f, z + -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
	        p0, p1, p2, p3
        };

        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Bottom
	        down, down, down, down
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Bottom
	        _11, _01, _00, _10
        };

        int[] triangles = new int[]
        {
	        // Bottom
	        cubeCount * 4 + 2, cubeCount * 4 +  1, cubeCount * 4 + 0,
            cubeCount * 4 + 3, cubeCount * 4 + 2, cubeCount * 4 + 0
        };
        
        cubeCount++;
        
        tools.combineVector3Arrays(ref world_vertices, vertices);
        tools.combineVector3Arrays(ref world_normals, normales);
        tools.combineVector2Arrays(ref world_uv, uvs);
        tools.combineIntArrays(ref world_triangles, triangles);
    }
    #endregion
    #region Cube Left
    private void cubeLeft(int x, int y, int z, float length, float width, float height)
    {

        Vector3 p0 = new Vector3(x + -length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p1 = new Vector3(x + length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p2 = new Vector3(x + length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p3 = new Vector3(x + -length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p4 = new Vector3(x + -length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p5 = new Vector3(x + length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p6 = new Vector3(x + length * .5f, y + width * .5f, z + -height * .5f);
        Vector3 p7 = new Vector3(x + -length * .5f, y + width * .5f, z + -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
            p7, p4, p0, p3
        };

        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Left
	        left, left, left, left
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Left
	        _11, _01, _00, _10
        };

        int[] triangles = new int[]
        {
	        // Left
	        cubeCount * 4 + 2, cubeCount * 4 +  1, cubeCount * 4 + 0,
            cubeCount * 4 + 3, cubeCount * 4 + 2, cubeCount * 4 + 0
        };

        cubeCount++;

        tools.combineVector3Arrays(ref world_vertices, vertices);
        tools.combineVector3Arrays(ref world_normals, normales);
        tools.combineVector2Arrays(ref world_uv, uvs);
        tools.combineIntArrays(ref world_triangles, triangles);
    }
    #endregion
    #region Cube Front
    private void cubeFront(int x, int y, int z, float length, float width, float height)
    {

        Vector3 p0 = new Vector3(x + -length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p1 = new Vector3(x + length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p2 = new Vector3(x + length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p3 = new Vector3(x + -length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p4 = new Vector3(x + -length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p5 = new Vector3(x + length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p6 = new Vector3(x + length * .5f, y + width * .5f, z + -height * .5f);
        Vector3 p7 = new Vector3(x + -length * .5f, y + width * .5f, z + -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
            // Front
	        p4, p5, p1, p0
        };

        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Front
	        front, front, front, front
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Front
	        _11, _01, _00, _10
        };

        int[] triangles = new int[]
        {
	        // Front
	        cubeCount * 4 + 2, cubeCount * 4 +  1, cubeCount * 4 + 0,
            cubeCount * 4 + 3, cubeCount * 4 + 2, cubeCount * 4 + 0
        };

        cubeCount++;

        tools.combineVector3Arrays(ref world_vertices, vertices);
        tools.combineVector3Arrays(ref world_normals, normales);
        tools.combineVector2Arrays(ref world_uv, uvs);
        tools.combineIntArrays(ref world_triangles, triangles);
    }
    #endregion
    #region Cube Back
    private void cubeBack(int x, int y, int z, float length, float width, float height)
    {

        Vector3 p0 = new Vector3(x + -length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p1 = new Vector3(x + length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p2 = new Vector3(x + length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p3 = new Vector3(x + -length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p4 = new Vector3(x + -length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p5 = new Vector3(x + length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p6 = new Vector3(x + length * .5f, y + width * .5f, z + -height * .5f);
        Vector3 p7 = new Vector3(x + -length * .5f, y + width * .5f, z + -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
            // Back
	        p6, p7, p3, p2
        };

        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Back
	        back, back, back, back
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Back
	        _11, _01, _00, _10
        };

        int[] triangles = new int[]
        {
	        // Back
	        cubeCount * 4 + 2, cubeCount * 4 +  1, cubeCount * 4 + 0,
            cubeCount * 4 + 3, cubeCount * 4 + 2, cubeCount * 4 + 0
        };

        cubeCount++;

        tools.combineVector3Arrays(ref world_vertices, vertices);
        tools.combineVector3Arrays(ref world_normals, normales);
        tools.combineVector2Arrays(ref world_uv, uvs);
        tools.combineIntArrays(ref world_triangles, triangles);
    }
    #endregion
    #region Cube Right
    private void cubeRight(int x, int y, int z, float length, float width, float height)
    {

        Vector3 p0 = new Vector3(x + -length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p1 = new Vector3(x + length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p2 = new Vector3(x + length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p3 = new Vector3(x + -length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p4 = new Vector3(x + -length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p5 = new Vector3(x + length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p6 = new Vector3(x + length * .5f, y + width * .5f, z + -height * .5f);
        Vector3 p7 = new Vector3(x + -length * .5f, y + width * .5f, z + -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
            // Right
	        p5, p6, p2, p1
        };

        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Right
	        right, right, right, right
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Right
	        _11, _01, _00, _10
        };

        int[] triangles = new int[]
        {
	        // Right
	        cubeCount * 4 + 2, cubeCount * 4 +  1, cubeCount * 4 + 0,
            cubeCount * 4 + 3, cubeCount * 4 + 2, cubeCount * 4 + 0
        };
        
        cubeCount++;

        tools.combineVector3Arrays(ref world_vertices, vertices);
        tools.combineVector3Arrays(ref world_normals, normales);
        tools.combineVector2Arrays(ref world_uv, uvs);
        tools.combineIntArrays(ref world_triangles, triangles);
    }
    #endregion
    #region Cube Top
    private void cubeTop(int x, int y, int z, float length, float width, float height)
    {

        Vector3 p0 = new Vector3(x + -length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p1 = new Vector3(x + length * .5f, y + -width * .5f, z + height * .5f);
        Vector3 p2 = new Vector3(x + length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p3 = new Vector3(x + -length * .5f, y + -width * .5f, z + -height * .5f);
        Vector3 p4 = new Vector3(x + -length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p5 = new Vector3(x + length * .5f, y + width * .5f, z + height * .5f);
        Vector3 p6 = new Vector3(x + length * .5f, y + width * .5f, z + -height * .5f);
        Vector3 p7 = new Vector3(x + -length * .5f, y + width * .5f, z + -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
            // Top
	        p7, p6, p5, p4
        };

        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Top
	        up, up, up, up
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Top
	        _11, _01, _00, _10
        };

        int[] triangles = new int[]
        {
	        // Top
	        cubeCount * 4 + 2, cubeCount * 4 +  1, cubeCount * 4 + 0,
            cubeCount * 4 + 3, cubeCount * 4 + 2, cubeCount * 4 + 0
        };

        cubeCount++;

        tools.combineVector3Arrays(ref world_vertices, vertices);
        tools.combineVector3Arrays(ref world_normals, normales);
        tools.combineVector2Arrays(ref world_uv, uvs);
        tools.combineIntArrays(ref world_triangles, triangles);
    }
    #endregion

}
