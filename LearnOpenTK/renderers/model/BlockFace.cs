using LearnOpenTK.blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnOpenTK.renderers.model
{
    public class BlockFace
    {
        private static BlockModel BlockModel = new BlockModel();
        private static LiquidModel LiquidModel = new LiquidModel();

        public static BlockModel GetBlockModel()
        {
            return BlockModel;
        }

        public static LiquidModel GetLiquidModel()
        {
            return LiquidModel;
        }

        public enum Face
        {
            FRONT,
            BACK,
            LEFT,
            RIGHT,
            TOP,
            BOTTOM,
        }
    }
}
