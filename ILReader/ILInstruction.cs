using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;

namespace ClrTest.Reflection
{
    public abstract class ILInstruction
    {
        internal ITokenResolver m_resolver;
        internal Int32 m_offset;
        internal OpCode m_opCode;

        internal ILInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode)
        {
            this.m_resolver = resolver;
            this.m_offset = offset;
            this.m_opCode = opCode;
        }

        public Int32 Offset { get { return m_offset; } }
        public OpCode OpCode { get { return m_opCode; } }
        public virtual string RawOperand { get { return string.Empty; } }
        public virtual string ProcessedOperand { get { return RawOperand; } }

        internal static IFormatProvider s_formatProvider = new DefaultFormatProvider();
        public static void SetFormatProvider(IFormatProvider formatProvider) { s_formatProvider = formatProvider; }

        public override string ToString() { return s_formatProvider.Format(this); }
    }

    public class InlineNoneInstruction : ILInstruction
    {
        public InlineNoneInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode)
            : base(resolver, offset, opCode) { }
    }

    public class InlineBrTargetInstruction : ILInstruction
    {
        public InlineBrTargetInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 delta)
            : base(resolver, offset, opCode)
        {
            this.m_delta = delta;
        }

        public Int32 Delta { get { return m_delta; } }
        public Int32 TargetOffset { get { return m_offset + m_delta + 1 + 4; } }
        public override String RawOperand { get { return s_formatProvider.Int32ToHex(m_delta); } }
        public override String ProcessedOperand { get { return s_formatProvider.Label(TargetOffset); } }

        Int32 m_delta;
    }

    public class ShortInlineBrTargetInstruction : ILInstruction
    {
        public ShortInlineBrTargetInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, SByte delta)
            : base(resolver, offset, opCode)
        {
            this.m_delta = delta;
        }

        public SByte Delta { get { return m_delta; } }
        public Int32 TargetOffset { get { return m_offset + m_delta + 1 + 1; } }
        public override string RawOperand { get { return s_formatProvider.Int8ToHex(m_delta); } }
        public override string ProcessedOperand { get { return s_formatProvider.Label(TargetOffset); } }

        SByte m_delta;
    }

    public class InlineSwitchInstruction : ILInstruction
    {
        public InlineSwitchInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32[] deltas)
            : base(resolver, offset, opCode)
        {
            this.m_deltas = deltas;
        }

        public Int32[] TargetOffsets
        {
            get
            {
                if (m_targetOffsets == null)
                {
                    int cases = m_deltas.Length;
                    int itself = 1 + 4 + 4 * cases;
                    m_targetOffsets = new Int32[cases];
                    for (Int32 i = 0; i < cases; i++)
                        m_targetOffsets[i] = m_offset + m_deltas[i] + itself;
                }
                return m_targetOffsets;
            }
        }

        public override string RawOperand { get { return "..."; } }
        public override string ProcessedOperand { get { return s_formatProvider.MultipleLabels(TargetOffsets); } }

        Int32[] m_deltas;
        Int32[] m_targetOffsets;
    }

    public class InlineIInstruction : ILInstruction
    {
        public InlineIInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 value)
            : base(resolver, offset, opCode)
        {
            this.m_int32 = value;
        }

        public Int32 Int32 { get { return m_int32; } }
        public override String RawOperand { get { return s_formatProvider.Int32ToHex(m_int32); } }
        public override string ProcessedOperand { get { return m_int32.ToString(); } }

        Int32 m_int32;
    }
    public class InlineI8Instruction : ILInstruction
    {
        public InlineI8Instruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int64 value)
            : base(resolver, offset, opCode)
        {
            this.m_int64 = value;
        }

        public Int64 Int64 { get { return m_int64; } }
        public override string RawOperand { get { return m_int64.ToString("x16"); } }
        public override string ProcessedOperand { get { return m_int64.ToString(); } }

        Int64 m_int64;
    }
    public class ShortInlineIInstruction : ILInstruction
    {
        public ShortInlineIInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Byte value)
            : base(resolver, offset, opCode)
        {
            this.m_int8 = value;
        }

        public Byte Byte { get { return m_int8; } }
        public override string RawOperand { get { return s_formatProvider.Int8ToHex(m_int8); } }
        public override string ProcessedOperand { get { return m_int8.ToString(); } }

        Byte m_int8;
    }

    public class InlineRInstruction : ILInstruction
    {
        public InlineRInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Double value)
            : base(resolver, offset, opCode)
        {
            this.m_value = value;
        }

        public Double Double { get { return m_value; } }
        public override string RawOperand { get { return m_value.ToString(); } }

        Double m_value;
    }
    public class ShortInlineRInstruction : ILInstruction
    {
        public ShortInlineRInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Single value)
            : base(resolver, offset, opCode)
        {
            this.m_value = value;
        }

        public Single Single { get { return m_value; } }
        public override string RawOperand { get { return m_value.ToString(); } }

        Single m_value;
    }

    public class InlineFieldInstruction : ILInstruction
    {
        public InlineFieldInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 token)
            : base(resolver, offset, opCode)
        {
            this.m_token = token;
        }

        public FieldInfo Field
        {
            get
            {
                if (m_field == null) m_field = m_resolver.AsField(m_token);
                return m_field;
            }
        }
        public Int32 Token { get { return m_token; } }
        public override String RawOperand { get { return s_formatProvider.Int32ToHex(m_token); } }
        public override String ProcessedOperand { get { return Field + "/" + Field.DeclaringType; } }

        Int32 m_token;
        FieldInfo m_field;
    }
    public class InlineMethodInstruction : ILInstruction
    {
        public InlineMethodInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 token)
            : base(resolver, offset, opCode)
        {
            this.m_token = token;
        }

        public MethodBase Method
        {
            get
            {
                if (m_method == null) m_method = m_resolver.AsMethod(m_token);
                return m_method;
            }
        }
        public Int32 Token { get { return m_token; } }
        public override string RawOperand { get { return s_formatProvider.Int32ToHex(m_token); } }
        public override string ProcessedOperand { get { return Method + "/" + Method.DeclaringType; } }

        Int32 m_token;
        MethodBase m_method;
    }
    public class InlineTypeInstruction : ILInstruction
    {
        public InlineTypeInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 token)
            : base(resolver, offset, opCode)
        {
            this.m_token = token;
        }

        public Type Type
        {
            get
            {
                if (m_type == null) m_type = m_resolver.AsType(m_token);
                return m_type;
            }
        }
        public Int32 Token { get { return m_token; } }
        public override string RawOperand { get { return s_formatProvider.Int32ToHex(m_token); } }
        public override string ProcessedOperand { get { return Type.ToString(); } }

        Int32 m_token;
        Type m_type;
    }
    public class InlineSigInstruction : ILInstruction
    {
        public InlineSigInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 token)
            : base(resolver, offset, opCode)
        {
            this.m_token = token;
        }

        public byte[] Signature
        {
            get
            {
                if (m_signature == null) m_signature = m_resolver.AsSignature(m_token);
                return m_signature;
            }
        }
        public Int32 Token { get { return m_token; } }

        public override string RawOperand { get { return s_formatProvider.Int32ToHex(m_token); } }
        public override string ProcessedOperand { get { return s_formatProvider.SigByteArrayToString(Signature); } }

        Int32 m_token;
        byte[] m_signature;
    }
    public class InlineTokInstruction : ILInstruction
    {
        public InlineTokInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 token)
            : base(resolver, offset, opCode)
        {
            this.m_token = token;
        }

        public MemberInfo Member
        {
            get
            {
                if (m_member == null) { m_member = m_resolver.AsMember(Token); }
                return m_member;
            }
        }
        public Int32 Token { get { return m_token; } }
        public override string RawOperand { get { return s_formatProvider.Int32ToHex(m_token); } }
        public override string ProcessedOperand { get { return Member + "/" + Member.DeclaringType; } }

        Int32 m_token;
        MemberInfo m_member;
    }

    public class InlineStringInstruction : ILInstruction
    {
        public InlineStringInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Int32 token)
            : base(resolver, offset, opCode)
        {
            this.m_token = token;
        }

        public String String
        {
            get
            {
                if (m_string == null) m_string = m_resolver.AsString(Token);
                return m_string;
            }
        }
        public Int32 Token { get { return m_token; } }
        public override string RawOperand { get { return s_formatProvider.Int32ToHex(Token); } }
        public override string ProcessedOperand { get { return s_formatProvider.EscapedString(String); } }

        Int32 m_token;
        String m_string;
    }

    public class InlineVarInstruction : ILInstruction
    {
        public InlineVarInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, UInt16 ordinal)
            : base(resolver, offset, opCode)
        {
            this.m_ordinal = ordinal;
        }

        public UInt16 Ordinal { get { return m_ordinal; } }
        public override string RawOperand { get { return s_formatProvider.Int16ToHex(m_ordinal); } }
        public override string ProcessedOperand { get { return s_formatProvider.Argument(m_ordinal); } }
        UInt16 m_ordinal;
    }
    public class ShortInlineVarInstruction : ILInstruction
    {
        public ShortInlineVarInstruction(ITokenResolver resolver, Int32 offset, OpCode opCode, Byte ordinal)
            : base(resolver, offset, opCode)
        {
            this.m_ordinal = ordinal;
        }

        public Byte Ordinal { get { return m_ordinal; } }
        public override string RawOperand { get { return s_formatProvider.Int8ToHex(m_ordinal); } }
        public override string ProcessedOperand { get { return s_formatProvider.Argument(m_ordinal); } }
        Byte m_ordinal;
    }

}
