using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform camPosition;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Billboard>().camPosition = mainCamera.transform;
        camPosition = GameManager.Instance.mainCamera.transform;
        //TODO: require camera component and assign camera position without requiring manual assignment in editor
        transform.LookAt(transform.position + camPosition.forward);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + camPosition.forward);
    }
}
