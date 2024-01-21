using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detective : MonoBehaviour
{
    public float maxDistance;
    public float radius;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private LayerMask igLayerMask;
    
    private bool isRangeDetection;
    private bool isRayDetection;
    public Transform targetTransform;
    public bool IsDetect
    {
        get
        {
            return isRayDetection && isRangeDetection;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        maxDistance = 10f;
        radius = 10f;
        isRangeDetection = false;
        isRayDetection = false;
    }
    // Update is called once per frame
    void Update()
    {
        CircleRay();
    }
    void CircleRay()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius, layerMask);
        isRangeDetection = (bool)(cols.Length > 0);
        if (isRangeDetection)
        {
            Collider tempCol = cols[0];
            
            for(int i=0; i<cols.Length; i++)
            {
                if (cols[i].transform.TryGetComponent<Enemy>(out Enemy enemyTemp)){
                    if (enemyTemp.enemyRoomNum == GameManager.Instance.player.dungeonNum)
                    {
                        tempCol = cols[i];
                    }
                }
            }
            Vector3 direction = (tempCol.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, maxDistance, ~igLayerMask))//igmask¿Ü¿¡°Å
            {
                Debug.DrawLine(transform.position, (transform.position + direction * maxDistance) , Color.blue);
                if (isRayDetection = (layerMask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    targetTransform = hit.transform;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
