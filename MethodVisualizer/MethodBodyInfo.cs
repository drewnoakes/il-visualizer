using System;
using System.Collections.Generic;

namespace DynamicMethodVisualizer
{
    [Serializable]
    public class MethodBodyInfo
    {
        private string m_typeName = string.Empty;
        private string m_methodToString = string.Empty;
        private List<string> m_instructions = new List<string>();
        private bool m_fixed = true;

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
            set { m_instructions = value; }
        }

        public bool FixupSuccess 
        {
            get { return m_fixed; }
            set { m_fixed = value; }
        }
    }
}
