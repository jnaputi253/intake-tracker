using System.Collections;
using System.Collections.Generic;

namespace IntakeTracker.Tests.Data
{
    public class NumericData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {-1},
            new object[] {10_001}
        };
        
        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}