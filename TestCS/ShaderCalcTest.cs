using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ShaderCalcTest))]
public class ShaderCalcTestEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Dispatch Kernel"))
        {
            var inst = (ShaderCalcTest)target;
            inst.InvokeCS();
        }
    }
}


public class ShaderCalcTest : MonoBehaviour
{
    public ComputeShader cs;
    public string kernelName = "CSMain";
    public string resultBufName = "Result";
    public Vector3Int groupCounts = new Vector3Int(2, 2, 2);
    public int dataLength = 1;
    // Start is called before the first frame update
    public void InvokeCS()
    {
        var data = new Vector4[dataLength];
        var resultBuf = new ComputeBuffer(dataLength, 4 * 4 * dataLength);

        var kernelId = cs.FindKernel(kernelName);

        cs.SetBuffer(kernelId, resultBufName, resultBuf);
        cs.SetMatrix("unity_ObjectToWorld", transform.localToWorldMatrix);
        
        cs.Dispatch(kernelId, groupCounts.x, groupCounts.y, groupCounts.z);

        resultBuf.GetData(data);
        resultBuf.Release();

        var sb = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sb.AppendLine(data[i].ToString());
        }
        Debug.Log(sb);
    }

}
#endif