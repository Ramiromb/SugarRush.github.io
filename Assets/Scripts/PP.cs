using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP : MonoBehaviour
{
    public Shader shad;
    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        mat = new Material(shad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(ObjectManager.om.iswin)
        Graphics.Blit(source, destination, mat);
        else
            Graphics.Blit(source, destination);

    }
}
