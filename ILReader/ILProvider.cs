using System;
using System.Reflection;

namespace ClrTest.Reflection {
    public interface IILProvider {
        Byte[] GetByteArray();
    }

    public class MethodBaseILProvider : IILProvider {
        private MethodBase m_method;
        private byte[] m_byteArray;

        public MethodBaseILProvider(MethodBase method) {
            m_method = method;
        }

        public byte[] GetByteArray() {
            if (m_byteArray == null) {
                var methodBody = m_method.GetMethodBody();
                m_byteArray = (methodBody == null) ? new Byte[0] : methodBody.GetILAsByteArray();
            }
            return m_byteArray;
        }
    }
}