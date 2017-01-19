using UnityEngine;
using System.Collections;

public class Tools {

    public void combineVector3Arrays(ref Vector3[] array1, Vector3[] array2)
    {
        var array3 = new Vector3[array1.Length + array2.Length];
        Debug.Log("Array1 = " + array1.Length + " Array2 = " + array2.Length + " Array3 = " + array3.Length);
        System.Array.Copy(array1, array3, array1.Length);
        System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        
        System.Array.Resize(ref array1, array3.Length);
        System.Array.Copy(array3, array1, array3.Length);
    }

    public void combineVector2Arrays(ref Vector2[] array1, Vector2[] array2)
    {
        var array3 = new Vector2[array1.Length + array2.Length];
        System.Array.Copy(array1, array3, array1.Length);
        System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);

        System.Array.Resize(ref array1, array3.Length);
        System.Array.Copy(array3, array1, array3.Length);
    }

    public void combineIntArrays(ref int[] array1, int[] array2)
    {
        var array3 = new int[array1.Length + array2.Length];
        System.Array.Copy(array1, array3, array1.Length);
        System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);

        System.Array.Resize(ref array1, array3.Length);
        System.Array.Copy(array3, array1, array3.Length);
    }

}
