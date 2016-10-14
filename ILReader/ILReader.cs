using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace ClrTest.Reflection
{
    // 
    // part of the following code is based on internal DynamicMethod implementation. 
    // We do not recommend use such approach in any production code; and 
    // it will most likley be broken in the future CLR release.
    // 

    public class ILReader : IEnumerable<ILInstruction>
    {
        Byte[] m_byteArray;
        Int32 m_position;
        ITokenResolver m_resolver;
        bool m_fixupSuccess;

        static OpCode[] s_OneByteOpCodes = new OpCode[0x100];
        static OpCode[] s_TwoByteOpCodes = new OpCode[0x100];

        static FieldInfo s_fiLen = typeof(ILGenerator).GetField("m_length", BindingFlags.NonPublic | BindingFlags.Instance);
        static FieldInfo s_fiStream = typeof(ILGenerator).GetField("m_ILStream", BindingFlags.NonPublic | BindingFlags.Instance);

        static MethodInfo s_miBakeByteArray = typeof(ILGenerator).GetMethod("BakeByteArray", BindingFlags.NonPublic | BindingFlags.Instance);

        static ILReader() {
            foreach (FieldInfo fi in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)) {
                OpCode opCode = (OpCode)fi.GetValue(null);
                UInt16 value = (UInt16)opCode.Value;
                if (value < 0x100) {
                    s_OneByteOpCodes[value] = opCode;
                } else if ((value & 0xff00) == 0xfe00) {
                    s_TwoByteOpCodes[value & 0xff] = opCode;
                }
            }
        }

        public bool FixupSuccess 
        {
            get { return this.m_fixupSuccess; }
        }

        public ILReader(DynamicMethod dynamicMethod) {
            this.m_resolver = new DynamicScopeTokenResolver(dynamicMethod);
            ILGenerator ilgen = dynamicMethod.GetILGenerator();

            this.m_fixupSuccess = false; 
            try {
                this.m_byteArray = (byte[])s_miBakeByteArray.Invoke(ilgen, null);
                if (this.m_byteArray == null) this.m_byteArray = new byte[0];
                m_fixupSuccess = true;
            } 
            catch (TargetInvocationException) 
            {
                int length = (int)s_fiLen.GetValue(ilgen);
                this.m_byteArray = new byte[length];
                Array.Copy((byte[])s_fiStream.GetValue(ilgen), this.m_byteArray, length);
            }
            this.m_position = 0;
        }

        public IEnumerator<ILInstruction> GetEnumerator()
        {
            while (m_position < m_byteArray.Length)
                yield return Next();

            m_position = 0;
            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        ILInstruction Next()
        {
            Int32 offset = m_position;
            OpCode opCode = OpCodes.Nop;
            Int32 token = 0;

            // read first 1 or 2 bytes as opCode
            Byte code = ReadByte();
            if (code != 0xFE)
            {
                opCode = s_OneByteOpCodes[code];
            }
            else
            {
                code = ReadByte();
                opCode = s_TwoByteOpCodes[code];
            }

            switch (opCode.OperandType)
            {
                case OperandType.InlineNone:
                    return new InlineNoneInstruction(m_resolver, offset, opCode);

                //The operand is an 8-bit integer branch target.
                case OperandType.ShortInlineBrTarget:
                    SByte shortDelta = ReadSByte();
                    return new ShortInlineBrTargetInstruction(m_resolver, offset, opCode, shortDelta);

                //The operand is a 32-bit integer branch target.
                case OperandType.InlineBrTarget:
                    Int32 delta = ReadInt32();
                    return new InlineBrTargetInstruction(m_resolver, offset, opCode, delta);

                //The operand is an 8-bit integer: 001F  ldc.i4.s, FE12  unaligned.
                case OperandType.ShortInlineI:
                    Byte int8 = ReadByte();
                    return new ShortInlineIInstruction(m_resolver, offset, opCode, int8);

                //The operand is a 32-bit integer.
                case OperandType.InlineI:
                    Int32 int32 = ReadInt32();
                    return new InlineIInstruction(m_resolver, offset, opCode, int32);

                //The operand is a 64-bit integer.
                case OperandType.InlineI8:
                    Int64 int64 = ReadInt64();
                    return new InlineI8Instruction(m_resolver, offset, opCode, int64);

                //The operand is a 32-bit IEEE floating point number.
                case OperandType.ShortInlineR:
                    Single float32 = ReadSingle();
                    return new ShortInlineRInstruction(m_resolver, offset, opCode, float32);

                //The operand is a 64-bit IEEE floating point number.
                case OperandType.InlineR:
                    Double float64 = ReadDouble();
                    return new InlineRInstruction(m_resolver, offset, opCode, float64);

                //The operand is an 8-bit integer containing the ordinal of a local variable or an argument
                case OperandType.ShortInlineVar:
                    Byte index8 = ReadByte();
                    return new ShortInlineVarInstruction(m_resolver, offset, opCode, index8);

                //The operand is 16-bit integer containing the ordinal of a local variable or an argument.
                case OperandType.InlineVar:
                    UInt16 index16 = ReadUInt16();
                    return new InlineVarInstruction(m_resolver, offset, opCode, index16);

                //The operand is a 32-bit metadata string token.
                case OperandType.InlineString:
                    token = ReadInt32();
                    return new InlineStringInstruction(m_resolver, offset, opCode, token);

                //The operand is a 32-bit metadata signature token.
                case OperandType.InlineSig:
                    token = ReadInt32();
                    return new InlineSigInstruction(m_resolver, offset, opCode, token);

                //The operand is a 32-bit metadata token.
                case OperandType.InlineMethod:
                    token = ReadInt32();
                    return new InlineMethodInstruction(m_resolver, offset, opCode, token);

                //The operand is a 32-bit metadata token.
                case OperandType.InlineField:
                    token = ReadInt32();
                    return new InlineFieldInstruction(m_resolver, offset, opCode, token);

                //The operand is a 32-bit metadata token.
                case OperandType.InlineType:
                    token = ReadInt32();
                    return new InlineTypeInstruction(m_resolver, offset, opCode, token);

                //The operand is a FieldRef, MethodRef, or TypeRef token.
                case OperandType.InlineTok:
                    token = ReadInt32();
                    return new InlineTokInstruction(m_resolver, offset, opCode, token);

                //The operand is the 32-bit integer argument to a switch instruction.
                case OperandType.InlineSwitch:
                    Int32 cases = ReadInt32();
                    Int32[] deltas = new Int32[cases];
                    for (Int32 i = 0; i < cases; i++) deltas[i] = ReadInt32();
                    return new InlineSwitchInstruction(m_resolver, offset, opCode, deltas);

                default:
                    throw new BadImageFormatException("unexpected OperandType " + opCode.OperandType);
            }
        }

        Byte ReadByte() { return (Byte)m_byteArray[m_position++]; }
        SByte ReadSByte() { return (SByte)ReadByte(); }

        UInt16 ReadUInt16() { m_position += 2; return BitConverter.ToUInt16(m_byteArray, m_position - 2); }
        UInt32 ReadUInt32() { m_position += 4; return BitConverter.ToUInt32(m_byteArray, m_position - 4); }
        UInt64 ReadUInt64() { m_position += 8; return BitConverter.ToUInt64(m_byteArray, m_position - 8); }

        Int32 ReadInt32() { m_position += 4; return BitConverter.ToInt32(m_byteArray, m_position - 4); }
        Int64 ReadInt64() { m_position += 8; return BitConverter.ToInt64(m_byteArray, m_position - 8); }

        Single ReadSingle() { m_position += 4; return BitConverter.ToSingle(m_byteArray, m_position - 4); }
        Double ReadDouble() { m_position += 8; return BitConverter.ToDouble(m_byteArray, m_position - 8); }
    }
}


