using OpenTK.Mathematics;

namespace LearnOpenTK
{
    public class Frustum
    {
        private Vector4[] planes;

        public void Generate()
        {
            Matrix4 viewProj = Game.GetInstance().GetPlayer().GetCamera().GetViewMatrix() * Game.GetInstance().GetPlayer().GetCamera().GetProjectionMatrix();
            planes = new Vector4[6];

            // Left clipping plane
            planes[0] = new Vector4(
                viewProj.M14 + viewProj.M11,
                viewProj.M24 + viewProj.M21,
                viewProj.M34 + viewProj.M31,
                viewProj.M44 + viewProj.M41
            );
            // Right clipping plane
            planes[1] = new Vector4(
                viewProj.M14 - viewProj.M11,
                viewProj.M24 - viewProj.M21,
                viewProj.M34 - viewProj.M31,
                viewProj.M44 - viewProj.M41
            );
            // Top clipping plane
            planes[2] = new Vector4(
                viewProj.M14 - viewProj.M12,
                viewProj.M24 - viewProj.M22,
                viewProj.M34 - viewProj.M32,
                viewProj.M44 - viewProj.M42
            );
            // Bottom clipping plane
            planes[3] = new Vector4(
                viewProj.M14 + viewProj.M12,
                viewProj.M24 + viewProj.M22,
                viewProj.M34 + viewProj.M32,
                viewProj.M44 + viewProj.M42
            );
            // Near clipping plane
            planes[4] = new Vector4(
                viewProj.M13,
                viewProj.M23,
                viewProj.M33,
                viewProj.M43
            );
            // Far clipping plane
            planes[5] = new Vector4(
                viewProj.M14 - viewProj.M13,
                viewProj.M24 - viewProj.M23,
                viewProj.M34 - viewProj.M33,
                viewProj.M44 - viewProj.M43
            );
        }

        public bool PointInFrustum(Vector4 point)
        {
            for (int i = 0; i < 6; i++)
            {
                if (planes[i].X * point.X + planes[i].Y * point.Y + planes[i].Z * point.Z + planes[i].W <= 0)
                {
                    // The point is outside the frustum
                    return false;
                }
            }
            // The point is inside the frustum
            return true;

        }

        public bool CubeInFrustum(Vector3 center, Vector3 size)
        {
            // Calculate the half-sizes of the cube's edges
            Vector3 halfSize = size / 2;

            // Calculate the corners of the cube
            Vector3[] corners = new Vector3[8];
            corners[0] = center + new Vector3(-halfSize.X, -halfSize.Y, -halfSize.Z);
            corners[1] = center + new Vector3(-halfSize.X, -halfSize.Y, halfSize.Z);
            corners[2] = center + new Vector3(-halfSize.X, halfSize.Y, -halfSize.Z);
            corners[3] = center + new Vector3(-halfSize.X, halfSize.Y, halfSize.Z);
            corners[4] = center + new Vector3(halfSize.X, -halfSize.Y, -halfSize.Z);
            corners[5] = center + new Vector3(halfSize.X, -halfSize.Y, halfSize.Z);
            corners[6] = center + new Vector3(halfSize.X, halfSize.Y, -halfSize.Z);
            corners[7] = center + new Vector3(halfSize.X, halfSize.Y, halfSize.Z);

            // Test each corner against the frustum planes
            for (int i = 0; i < 6; i++)
            {
                int insideCorners = 8;
                for (int j = 0; j < 8; j++)
                {
                    if (planes[i].X * corners[j].X + planes[i].Y * corners[j].Y + planes[i].Z * corners[j].Z + planes[i].W < 0)
                    {
                        insideCorners--;
                    }
                }
                if (insideCorners == 0)
                {
                    // All corners are outside this plane of the frustum
                    return false;
                }
            }

            // All corners are inside or on the frustum planes
            return true;
        }
    }
}
