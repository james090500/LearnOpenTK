using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnOpenTK.textures
{
    public class TextureManager
    {
        private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public Texture loadTexture(string name, string path)
        {
            textures[name] = new Texture(path);
            return textures[name];
        }

        public Texture getTexture(String name)
        {
            return textures[name];
        }
    }
}
