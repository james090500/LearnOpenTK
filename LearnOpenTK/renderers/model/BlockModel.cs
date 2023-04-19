using static LearnOpenTK.renderers.model.BlockFace;

namespace LearnOpenTK.renderers.model
{
    public class BlockModel : Model
    {
        static readonly float[] left_vertices =
        {
            // Position       // Texture  // Light
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.8f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.8f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.8f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.8f,
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.8f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.8f,
        };

        static readonly float[] right_vertices =
        {
            // Position       // Texture  // Light
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.8f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.8f,
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f,
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f,
            0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.8f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.8f,
        };

        static readonly float[] back_vertices = 
        {
            // Position       // Texture  // Light
            0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.8f,
            0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.8f,
            0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.8f,
            0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.8f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.8f,
            0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.8f,
        };

        static readonly float[] front_vertices = 
        {
            // Position       // Texture  // Light
            1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.8f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.8f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.8f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.8f,
            1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.8f,
            1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.8f,
        };

        static readonly float[] bottom_vertices = 
        {
            // Position       // Texture  // Light
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.5f,
            1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.5f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.5f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.5f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.5f,
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.5f,
        };

        static readonly float[] top_vertices = 
        {
            // Position       // Texture  // Light
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
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