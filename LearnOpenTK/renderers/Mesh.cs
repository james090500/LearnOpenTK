namespace LearnOpenTK.renderers
{
    internal class Mesh
    {
        private List<float> vertices = new List<float>();
        private List<float> texCoords = new List<float>();
        private List<int> indices = new List<int>();

        public void Update(float[] temp_vertices, int x, int y, int z, int texturePosition)
        {           
            //Times/Divide by 5 because we have 3 coords and 2 textures
            float textureOffset = texturePosition * ((float)16 / (float)128);

            int baseIndex = this.vertices.Count;
            for (int i = 0; i < temp_vertices.Length / 5; i++)
            {
                this.vertices.Add(temp_vertices[0 + i * 5] + x);
                this.vertices.Add(temp_vertices[1 + i * 5] + y);
                this.vertices.Add(temp_vertices[2 + i * 5] + z);

                //Atlast is 128, textures are 16x                
                if(textureOffset > 0)
                {
                    Console.WriteLine("sPRITE AT - " + (((temp_vertices[3 + i * 5] * 16) / 128) + textureOffset) + ", " + (((temp_vertices[4 + i * 5] * 16) / 128) + textureOffset));
                }

                //Coordinates probably need normalising? 
                //Image seems to be inverted or somethinfg!!!
                this.vertices.Add(((temp_vertices[3 + i * 5] * 16) / 128) + textureOffset);
                this.vertices.Add(((temp_vertices[4 + i * 5] * 16) / 128) + textureOffset);
            }

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
