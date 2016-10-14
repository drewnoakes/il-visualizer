using System;
using System.Reflection;

namespace ClrTest.Reflection
{
    public interface IILProvider
    {
        byte[] GetByteArray();
    }

    public class MethodBaseILProvider : IILProvider
    {
        private readonly MethodBase m_method;
        private byte[] m_byteArray;

        public MethodBaseILProvider(MethodBase method)
        {
            m_method = method;
        }

        public byte[] GetByteArray()
        {
            if (m_byteArray == null)
            {
                var methodBody = m_method.GetMethodBody();
                m_byteArray = (methodBody == null) ? new byte[0] : methodBody.GetILAsByteArray();
            }
            return m_byteArray;
        }
    }
}