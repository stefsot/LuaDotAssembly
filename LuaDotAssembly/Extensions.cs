using System;
using System.Linq;

namespace LuaDotAssembly
{
    public static class Extensions
    {
        public static LuaOpcode ToLuaOpcode(this string s)
        {
            var args = s.Split(' ');
            Enum.TryParse("OP_" + args[0], out LuaOpcode.Opcode op);

            var opcode = new LuaOpcode(op);

            for (var i = 1; i < args.Length; i++)
                opcode.SetOperandValue(i - 1, int.Parse(args[i]));

            return opcode;
        }

        public static LuaConstant ToLuaConstant(this string s)
        {
            var args = s.Split(' ').ToList();
            Enum.TryParse(args[0], out LuaConstant.ConstantType type);

            var constant = new LuaConstant(type);

            switch (type)
            {
                case LuaConstant.ConstantType.Boolean:
                    constant.StringValue = args[1];
                    break;
                case LuaConstant.ConstantType.Number:
                    constant.StringValue = args[1];
                    break;
                case LuaConstant.ConstantType.String:
                    args.RemoveAt(0);
                    constant.StringValue = string.Concat(args);
                    break;
            }

            return constant;
        }
    }
}
