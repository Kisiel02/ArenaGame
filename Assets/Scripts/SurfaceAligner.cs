using UnityEngine;

namespace DefaultNamespace
{
    public class SurfaceAligner
    {
        public static Transform getTransform(Transform transformObject)
        {
            return CalculatePosition(transformObject);
        }

        private static Transform CalculatePosition(Transform gameObject)
        {
            RaycastHit hit;
            Transform result = gameObject;

            if (Physics.Raycast(gameObject.position, Vector3.down, out hit, 500f))
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                result.rotation = rotation;
                result.position = hit.point;
            }
            
            return result;
        }

        public static Transform CalculatePositionAndAddHeight(Transform gameObject, float up)
        {
            Transform result = CalculatePosition(gameObject);
            result.position = new Vector3(result.position.x, result.position.y + up, result.position.z);
            return result;
        }
    }
}