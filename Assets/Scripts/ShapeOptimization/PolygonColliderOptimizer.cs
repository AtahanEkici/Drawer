using UnityEngine;
using System.Collections.Generic;
namespace Collider2DOptimization
{
	[AddComponentMenu("2D Collider Optimization/ Polygon Collider Optimizer")]
	[RequireComponent(typeof(PolygonCollider2D))]
	public class PolygonColliderOptimizer : MonoBehaviour 
	{
		public double tolerance = 0;
		private PolygonCollider2D coll;
		private List<List<Vector2>> originalPaths = new List<List<Vector2>>();

		private void OnValidate()
		{
			if(coll == null)
			{
				coll = GetComponent<PolygonCollider2D>();
				for(int i = 0; i < coll.pathCount; i++)
				{
					List<Vector2> path = new List<Vector2>(coll.GetPath(i));
					originalPaths.Add(path);
				}
			}
			if(tolerance <= 0)
			{
				for(int i = 0; i < originalPaths.Count; i++)
				{
					List<Vector2> path = originalPaths[i];
					coll.SetPath(i, path.ToArray());
				}
				return;
			}
			for(int i = 0; i < originalPaths.Count; i++)
			{
				List<Vector2> path = originalPaths[i];
				path = ShapeOptimizationHelper.DouglasPeuckerReduction(path, tolerance);
				coll.SetPath(i, path.ToArray());
			}
		}
		public static void OptimizePoligonCollider(PolygonCollider2D pol)
		{
			List<List<Vector2>> original_Paths = new();
            double _tolerance = 0;

            for (int i = 0; i < pol.pathCount; i++)
            {
                List<Vector2> path = new(pol.GetPath(i));
                original_Paths.Add(path);
            }

            if (_tolerance <= 0)
            {
                for (int i = 0; i < original_Paths.Count; i++)
                {
                    List<Vector2> path = original_Paths[i];
                    pol.SetPath(i, path.ToArray());
                }
                return;
            }

            for (int i = 0; i < original_Paths.Count; i++)
            {
                List<Vector2> path = original_Paths[i];
                path = ShapeOptimizationHelper.DouglasPeuckerReduction(path, _tolerance);
                pol.SetPath(i, path.ToArray());
            }
        }
	}
}
