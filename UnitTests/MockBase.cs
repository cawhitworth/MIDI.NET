using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    internal class MockBase
    {
        Dictionary<string, int> m_functionCount = new Dictionary<string, int>();

        protected void noteCall(string function)
        {
            if (m_functionCount.ContainsKey(function))
            {
                m_functionCount[function] ++;
            }
            else 
            {
                m_functionCount[function] = 1;
            }
        }

        public void resetCalls()
        {
            m_functionCount.Clear();
        }

        public void resetCallsTo(string function)
        {
            if (m_functionCount.ContainsKey(function))
            {
                m_functionCount.Remove(function);
            }
        }

        public int callsTo(string function)
        {
            if (m_functionCount.ContainsKey(function))
            {
                return m_functionCount[function];
            }
            else
            {
                return 0;
            }
        }
    }
}
