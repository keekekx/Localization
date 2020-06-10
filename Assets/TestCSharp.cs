using AOT;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestCSharp : MonoBehaviour
{
    struct Param
    {
        public int id;
        public string name;
    }
    delegate void TestCallBack(Param data);


    [DllImport("__Internal")]
    static extern void TestFunc(TestCallBack cb);

    [MonoPInvokeCallback(typeof(TestCallBack))]
    static void OnCallBack(Param data)
    {
        Debug.Log($"id:{data.id},name:{data.name}");
    }

    private void Start()
    {
        TestFunc(OnCallBack);
    }
}
