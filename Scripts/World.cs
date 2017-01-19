using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]

public class World : MonoBehaviour {

    private string[,,] world = new string[100,100,100];
    private Vector3[] world_vertices;
    private Vector3[] world_normals;
    private Vector2[] world_uv;
    private int[] world_triangles;
    private Mesh mesh;
    private Tools tools;

    // Use this for initialization
    void Start() {
        mesh = GetComponent<MeshFilter>().mesh; // initialize
        tools = new Tools(); // initialize tools object

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
    }

    #region World Generation
    private void generateWorldArray() // Temporary world generation for testing to create a 10 x 10 x 10 block layout
    {
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                for (int z = 0; z < 100; z++)
                {
                    world[x, y, z] = "Dirt";
                }
            }
        }
    }

    private Hashtable checkCubeSides(int x, int y, int z)
    {
        Hashtable sides = new Hashtable();

        if (world[x + 1, y, z] != "")
        {
            sides.Add("Right", true); // right side
        } else
        {
            sides.Add("Right", false);
        }

        if (x > 0)
        {
            if (world[x - 1, y, z] != "")
            {
                sides.Add("Left", true); // left side
            }
            else
            {
                sides.Add("Left", false);
            }
        } else
        {
            sides.Add("Left", false);
        }

        if (world[x, y + 1, z] != "")
        {
            sides.Add("Front", true); // front side
        }
        else
        {
            sides.Add("Front", false);
        }

        if (y > 0)
        {
            if (world[x, y - 1, z] != "")
            {
                sides.Add("Back", true); // back side
            }
            else
            {
                sides.Add("Back", false);
            }
        } else
        {
            sides.Add("Back", false);
        }

        if (world[x, y, z + 1] != "")
        {
            sides.Add("Top", true); // top side
        }
        else
        {
            sides.Add("Left", false);
        }

        if (z > 0)
        {
            if (world[x, y, z - 1] != "")
            {
                sides.Add("Bottom", true); // bottom side
            }
            else
            {
                sides.Add("Bottom", false);
            }
        } else
        {
            sides.Add("Bottom", false);
        }

        return sides;
    }
    #endregion

    private void generateCube(int x, int y, int z)
    {
        // Skip generation if all sides are hidden
        Hashtable sides = checkCubeSides(x, y, z);
        if (sides["Right"].Equals(true) && sides["Left"].Equals(true) && sides["Front"].Equals(true) && sides["Back"].Equals(true) && sides["Top"].Equals(true) && sides["Bottom"].Equals(true))
        {
            return; // Doesn't generate anything when all sides are hidden from the player
        }

        float length = 1f;
        float width = 1f;
        float height = 1f;

        #region Vertices
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
	        // Bottom
	        p0, p1, p2, p3,
 
	        // Left
	        p7, p4, p0, p3,
 
	        // Front
	        p4, p5, p1, p0,
 
	        // Back
	        p6, p7, p3, p2,
 
	        // Right
	        p5, p6, p2, p1,
 
	        // Top
	        p7, p6, p5, p4
        };
        #endregion

        #region Normales
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Bottom
	        down, down, down, down,
 
	        // Left
	        left, left, left, left,
 
	        // Front
	        front, front, front, front,
 
	        // Back
	        back, back, back, back,
 
	        // Right
	        right, right, right, right,
 
	        // Top
	        up, up, up, up
        };
        #endregion

        #region UVs
        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Bottom
	        _11, _01, _00, _10,
 
	        // Left
	        _11, _01, _00, _10,
 
	        // Front
	        _11, _01, _00, _10,
 
	        // Back
	        _11, _01, _00, _10,
 
	        // Right
	        _11, _01, _00, _10,
 
	        // Top
	        _11, _01, _00, _10,
        };
        #endregion

        #region Triangles
        int[] triangles = new int[]
        {
	        // Bottom
	        3, 1, 0,
            3, 2, 1,

            // Left
            3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
 
	        // Front
	        3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
 
	        // Back
	        3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
 
	        // Right
	        3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
 
	        // Top
	        3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

        };

        // Remove triangles from unseen sides
        if (sides["Right"].Equals(true))
        {
            triangles[24] = 0;
            triangles[25] = 0;
            triangles[26] = 0;
            triangles[27] = 0;
            triangles[28] = 0;
            triangles[29] = 0;
        }
        if (sides["Left"].Equals(true))
        {
            triangles[6] = 0;
            triangles[7] = 0;
            triangles[8] = 0;
            triangles[9] = 0;
            triangles[10] = 0;
            triangles[11] = 0;
        }
        if (sides["Front"].Equals(true))
        {
            triangles[12] = 0;
            triangles[13] = 0;
            triangles[14] = 0;
            triangles[15] = 0;
            triangles[16] = 0;
            triangles[17] = 0;
        }
        if (sides["Back"].Equals(true))
        {
            triangles[18] = 0;
            triangles[19] = 0;
            triangles[20] = 0;
            triangles[21] = 0;
            triangles[22] = 0;
            triangles[23] = 0;
        }
        if (sides["Top"].Equals(true))
        {
            triangles[30] = 0;
            triangles[31] = 0;
            triangles[32] = 0;
            triangles[33] = 0;
            triangles[34] = 0;
            triangles[35] = 0;
        }
        if (sides["Bottom"].Equals(true))
        {
            triangles[0] = 0;
            triangles[1] = 0;
            triangles[2] = 0;
            triangles[3] = 0;
            triangles[4] = 0;
            triangles[5] = 0;
        }
        #endregion

        world_vertices = tools.combineVector3Arrays(world_vertices, vertices);
        world_normals = tools.combineVector3Arrays(world_normals, normales);
        world_uv = tools.combineVector2Arrays(world_uv, uvs);
        world_triangles = tools.combineIntArrays(world_triangles, triangles);
    }

}
