using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Compute.OpenCL.CLGL;

namespace LearnOpenTK.blocks
{
    public class Blocks
    {
        Dictionary<byte, Type> IdsToBlock = new Dictionary<byte, Type>();
        Dictionary<Type, byte> BlockToIds = new Dictionary<Type, byte>();

        public void Init()
        {
            IdsToBlock.Add(1, typeof(StoneBlock));
            IdsToBlock.Add(2, typeof(DirtBlock));
            IdsToBlock.Add(3, typeof(GrassBlock));
            IdsToBlock.Add(4, typeof(SandBlock));
            IdsToBlock.Add(5, typeof(WaterBlock));
            IdsToBlock.Add(6, typeof(LogBlock));
            IdsToBlock.Add(7, typeof(LeafBlock));

            IdsToBlock.ToList().ForEach(action =>
            {
                BlockToIds.Add(action.Value, action.Key);
            });
        }

        public Block? GetBlockFromId(byte id)
        {
            Type? block;
            if(!IdsToBlock.TryGetValue(id, out block))
            {
                return null;
            }

            return (Block?) Activator.CreateInstance(block);
        }

        public byte GetIdFromBlock(Block? block) {
            if(block == null)
            {
                return 0;
            }

            byte blockId;
            BlockToIds.TryGetValue(block.GetType(), out blockId);
            return blockId;
        }
    }
}
