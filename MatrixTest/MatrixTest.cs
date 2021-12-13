using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MatrixTest: MonoBehaviour
{
    public ComputeShader shader;
    // Start is called before the first frame update
    void OnEnable()
    {
        var datas = new Vector3[1];
        var resultBuf = new ComputeBuffer(datas.Length, 4*3);

        var mainId = shader.FindKernel("CSMain");
        var resultId = Shader.PropertyToID("Result");
        shader.SetBuffer(mainId, "Result", resultBuf);
        
        shader.Dispatch(mainId, 1,1,1);

        resultBuf.GetData(datas);
        resultBuf.Release();

        var sb = new StringBuilder();
        foreach (var item in datas)
        {
            sb.AppendLine(item.ToString());
        }
        Debug.Log(sb);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
