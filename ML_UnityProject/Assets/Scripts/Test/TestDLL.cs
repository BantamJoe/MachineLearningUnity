using System.Runtime.InteropServices;
using UnityEngine;

public class TestDLL : MonoBehaviour
{
    [DllImport("machineLearning", EntryPoint = "TestSort")]
    public static extern void TestSort(int[] a, int length);

    public int[] a;

    void Start()
    {
        TestSort(a, a.Length);
    }
}
