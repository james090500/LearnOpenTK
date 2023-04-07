namespace LearnOpenTK.renderers
{
    internal class Mesh
    {
        private List<float> vertices = new List<float>();
        private List<float> texCoords = new List<float>();
        private List<int> indices = new List<int>();

        public void Update(float[] temp_vertices, int x, int y, int z, int texturePosition)
        {
            int baseIndex = this.vertices.Count;
            for (int i = 0; i < temp_vertices.Length / 5; i++)
            {
                this.vertices.Add(temp_vertices[0 + i * 5] + x);
                this.vertices.Add(temp_vertices[1 + i * 5] + y);
                this.vertices.Add(temp_vertices[2 + i * 5] + z);


                //OpenGL DOESN'T start at 0,0. It reads images like a graph so it's starts at 0,1. 
                //This means if textures are at the top of the image, i need to read from the bottom
                //Left first!
                float texSize = (float)1 / (float)8;
                this.vertices.Add(temp_vertices[3 + i * 5] == 0 ? texSize * (float)texturePosition : (texSize * (float)texturePosition) + texSize);
                this.vertices.Add(temp_vertices[4 + i * 5] == 0 ? texSize * (float)7 : 1);
            }
            //0, 0
            //0.125, 0
            //0.125, 0.125
            //0.125, 0.125
            //0, 0.125
            //0, 0

            this.indices.Add(baseIndex + 0);
            this.indices.Add(baseIndex + 1);
            this.indices.Add(baseIndex + 3);
            this.indices.Add(baseIndex + 1);
            this.indices.Add(baseIndex + 2);
            this.indices.Add(baseIndex + 3);
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
