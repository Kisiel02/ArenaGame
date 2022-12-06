using UnityEngine;

namespace DefaultNamespace
{
    public class SurfaceAligner
    {
        

        private static Transform CalculatePosition(Transform gameObject, bool normal)
        {
            RaycastHit hit;
            Transform result = gameObject;

            if (Physics.Raycast(gameObject.position, Vector3.down, out hit, 500f))
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                if(normal == true) {
                result.rotation = rotation;
                } else {

                }
                result.position = hit.point;
            }
            
            return result;
        }

        public static Transform CalculatePositionAndAddHeight(Transform gameObject, float up, bool normal)
        {
            Transform result = CalculatePosition(gameObject, normal);
            result.position = new Vector3(result.position.x, result.position.y + up, result.position.z);
            return result;
        }

        public static float GroundDetector(Vector3 pos) {

            RaycastHit hit;
            
            float distance;

            if (Physics.Raycast(pos, Vector3.down, out hit, 500f)) {
                distance = hit.distance;

            }else {
                distance = -1;
            }
            Debug.Log(distance);

                return distance;
        }
    }
}