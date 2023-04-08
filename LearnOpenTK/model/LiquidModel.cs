using static LearnOpenTK.model.BlockFace;

namespace LearnOpenTK.model
{
    public class LiquidModel : Model
    {
        static readonly float[] left_vertices =
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            1.0f, 0.9f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.9f, 0.0f, 1.0f, 1.0f,
            0.0f, 0.9f, 0.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        };

        static readonly float[] right_vertices =
        {
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            1.0f, 0.9f, 1.0f, 1.0f, 1.0f,
            1.0f, 0.9f, 1.0f, 1.0f, 1.0f,
            0.0f, 0.9f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
        };

        static readonly float[] back_vertices = {
            0.0f, 0.9f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.9f, 0.0f, 1.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.9f, 1.0f, 0.0f, 1.0f,
        };

        static readonly float[] front_vertices = {
            1.0f, 0.9f, 1.0f, 0.0f, 1.0f,
            1.0f, 0.9f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            1.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 0.9f, 1.0f, 0.0f, 1.0f,
        };

        static readonly float[] bottom_vertices = {
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        };

        static readonly float[] top_vertices = {
            0.0f, 0.9f, 0.0f, 0.0f, 1.0f,
            1.0f, 0.9f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.9f, 1.0f, 1.0f, 0.0f,
            1.0f, 0.9f, 1.0f, 1.0f, 0.0f,
            0.0f, 0.9f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.9f, 0.0f, 0.0f, 1.0f
        };


        public override float[] GetFace(Face face)
        {
            switch (face)
            {
                case Face.FRONT:
                    return front_vertices;
                case Face.BACK:
                    return back_vertices;
                case Face.LEFT:
                    return left_vertices;
                case Face.RIGHT:
                    return right_vertices;
                case Face.TOP:
                    return top_vertices;
                case Face.BOTTOM:
                    return bottom_vertices;
                default:
                    return null;
            }
        }
    }
}
