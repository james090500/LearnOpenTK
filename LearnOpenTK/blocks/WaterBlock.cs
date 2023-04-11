using OpenTK.Mathematics;
using System.Security;
using static LearnOpenTK.renderers.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class WaterBlock : Block
    {
        public WaterBlock() : base()
        {
            this.Transparent = true;
            this.Breakable = false;
            this.Liquid = true;
        }

        public override int GetTexturePosition(Face face)
        {
            return 5;
        }
    }
}
