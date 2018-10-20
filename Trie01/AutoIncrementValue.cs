using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trie01
{
    public class AutoIncrementValue
    {
        private long _value;
        public AutoIncrementValue(long initValue = 0)
        {
            _value = initValue;
        }

        public long GetNext
        {
            get
            {
                Increment();
                return _value;
            }
        }

        public void Increment()
        {
            _value++;
        }

        public long GetLastAdded
        {
            get
            {
                return _value;
            }
        }

        public void ReInit(long initValue = 0)
        {
            _value = initValue;
        }
    }
}
