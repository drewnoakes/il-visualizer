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
            return m_byteArray ?? (m_byteArray = m_method.GetMethodBody()?.GetILAsByteArray() ?? new byte[0]);
        }
    }
}