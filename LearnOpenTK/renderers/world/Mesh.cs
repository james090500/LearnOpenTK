namespace LearnOpenTK.renderers.world
{
    internal class Mesh
    {
        private List<float> vertices = new List<float>();
        private List<float> texCoords = new List<float>();
        private List<int> indices = new List<int>();

        public void Update(float[] temp_vertices, int x, int y, int z, int texturePosition)
        {
            int baseIndex = vertices.Count;
            for (int i = 0; i < temp_vertices.Length / 5; i++)
            {
                vertices.Add(temp_vertices[0 + i * 5] + x);
                vertices.Add(temp_vertices[1 + i * 5] + y);
                vertices.Add(temp_vertices[2 + i * 5] + z);


                //OpenGL DOESN'T start at 0,0. It reads images like a graph so it's starts at 0,1. 
                //This means if textures are at the top of the image, i need to read from the bottom
                //Left first!
                float texSize = 1 / (float)8;
                vertices.Add(temp_vertices[3 + i * 5] == 0 ? texSize * texturePosition : texSize * texturePosition + texSize);
                vertices.Add(temp_vertices[4 + i * 5] == 0 ? texSize * 7 : 1);
            }
            //0, 0
            //0.125, 0
            //0.125, 0.125
            //0.125, 0.125
            //0, 0.125
            //0, 0

            indices.Add(baseIndex + 0);
            indices.Add(baseIndex + 1);
            indices.Add(baseIndex + 3);
            indices.Add(baseIndex + 1);
            indices.Add(baseIndex + 2);
            indices.Add(baseIndex + 3);
        }

        public float[] getVertices()
        {
            return vertices.ToArray();
        }

        public float[] getTexCoords()
        {
            return texCoords.ToArray();
        }

        public int[] getIndices()
        {
            return indices.ToArray();
        }

        public int getTriangleCount()
        {
            return vertices.Count() / 3;
        }
    }
}
