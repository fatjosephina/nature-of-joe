using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffects : MonoBehaviour
{
    [SerializeField]
    RenderTexture rt;
    public Transform target;

    private void Awake()
    {
        Shader.SetGlobalTexture("_GlobalEffectRT", rt);
        Shader.SetGlobalFloat("_OrthographicCamSize", GetComponent<Camera>().orthographicSize);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, transform.transform.position.y, target.transform.position.z);
            Shader.SetGlobalVector("_Position", transform.position);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
