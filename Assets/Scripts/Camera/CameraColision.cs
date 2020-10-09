using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColision : MonoBehaviour
{
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float smooth;
    Vector3 dollyDir;
    private Vector3 dollyDirAdjusted;
    private float distance;
    int layerMask;
    [SerializeField]
    private float hitDistance;
    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        // .normalized = vetor mantém a mesma direção, mas seu comprimento é 1,0.
        dollyDir = transform.localPosition.normalized;
        // .magnitude = raiz quadrada do Vector3
        distance = transform.localPosition.magnitude;
      //  Physics.IgnoreLayerCollision(8,8);
    }

    // Update is called once per frame
    void Update()
    {
        
        //faz um teste de colisão para ver se não tem nada no caminho da câmera, se não tiver mantem posicao, mas se tiver ele transforma a posicao ate ficar livre da colisão.
        Vector3 posCamDesejada = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position, posCamDesejada, out hit, layerMask))
        {
            distance = Mathf.Clamp((hit.distance * hitDistance), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
