using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour {

    public Vector3[] combineVector3Arrays(Vector3[] array1, Vector3[] array2)
    {
        var array3 = new Vector3[array1.Length + array2.Length];
        System.Array.Copy(array1, array3, array1.Length);
        System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        return array3;
    }

    public Vector2[] combineVector2Arrays(Vector2[] array1, Vector2[] array2)
    {
        var array3 = new Vector2[array1.Length + array2.Length];
        System.Array.Copy(array1, array3, array1.Length);
        System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        return array3;
    }

    public int[] combineIntArrays(int[] array1, int[] array2)
    {
        var array3 = new int[array1.Length + array2.Length];
        System.Array.Copy(array1, array3, array1.Length);
        System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        return array3;
    }

}
