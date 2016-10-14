using System;
using System.Collections.Generic;
using System.Reflection;

namespace ClrTest.Reflection
{
    [Serializable]
    public class MethodBodyInfo
    {
        private int m_methodId;

        private string m_typeName;
        private string m_methodToString;

        private List<string> m_instructions = new List<string>();

        public int Identity
        {
            get { return m_methodId; }
            set { m_methodId = value; }
        }

        public string TypeName
        {
            get { return m_typeName; }
            set { m_typeName = value; }
        }

        public string MethodToString
        {
            get { return m_methodToString; }
            set { m_methodToString = value; }
        }

        public List<string> Instructions
        {
            get { return m_instructions; }
        }

        private void AddInstruction(string inst)
        {
            m_instructions.Add(inst);
        }

        public static MethodBodyInfo Create(MethodBase method)
        {
            var mbi = new MethodBodyInfo();

            mbi.Identity = method.GetHashCode();
            mbi.TypeName = method.GetType().Name;
            mbi.MethodToString = method.ToString();

            var visitor = new ReadableILStringVisitor(
                new MethodBodyInfoBuilder(mbi),
                DefaultFormatProvider.Instance);

            var reader = ILReaderFactory.Create(method);
            reader.Accept(visitor);

            return mbi;
        }

        private class MethodBodyInfoBuilder : IILStringCollector
        {
            private MethodBodyInfo m_mbi;

            public MethodBodyInfoBuilder(MethodBodyInfo mbi)
            {
                m_mbi = mbi;
            }

            public void Process(ILInstruction instruction, string operandString)
            {
                m_mbi.AddInstruction(string.Format("IL_{0:x4}: {1,-10} {2}",
                    instruction.Offset,
                    instruction.OpCode.Name,
                    operandString));
            }
        }
    }
}