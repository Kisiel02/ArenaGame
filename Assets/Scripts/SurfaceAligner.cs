using UnityEngine;

namespace DefaultNamespace
{
    public class SurfaceAligner
    {
        
        public static Vector3 CalculatePosition(Vector3 position)
        {
            RaycastHit hit;

            if (Physics.Raycast(position, Vector3.down, out hit, 500f) && hit.transform.tag == "Mesh")
            {
                return hit.point;
            }

            return position;
        }
        
        public static Transform CalculatePosition(Transform gameObject, bool rotateObject)
        {
            RaycastHit hit;
            Transform result = gameObject;

            if (Physics.Raycast(gameObject.position, Vector3.down, out hit, 500f) && hit.transform.tag == "Mesh")
            {
                if (rotateObject == true)
                {
                    result.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
                
                result.position = hit.point;
            }

            return result;
        }

        public static Transform CalculatePositionAndAddHeight(Transform gameObject, float up, bool rotateObject)
        {
            Transform result = CalculatePosition(gameObject, rotateObject);
            result.position = new Vector3(result.position.x, result.position.y + up, result.position.z);
            return result;
        }
    }
}