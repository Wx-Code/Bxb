using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Test.Common.TestPriority
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; private set; }
    }
}
