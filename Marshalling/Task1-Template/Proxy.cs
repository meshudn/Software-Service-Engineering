using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Task1
{
    public class Proxy
    {
        enum OperationType
        {
            Multiplication, Division
        }

        struct Calculation
        {
            public OperationType Type;
            public double Lhs;
            public double Rhs;
            public double Result;
        }

        private Queue<Calculation> _queue = new Queue<Calculation>(10);

        public Proxy()
        {
        }

        private double? FromCache(OperationType op, double lhs, double rhs)
        {
            foreach (var r in _queue)
            {
                if (r.Type == op && r.Lhs == lhs && r.Rhs == rhs)
                {
                    return r.Result;
                }
            }

            return null;
        }

        [Fact]
        public void TestProxy()
        {
            var result = Multiply(3, 2);
            var head = _queue.First();
            Assert.Equal(OperationType.Multiplication, head.Type);
            Assert.Equal(3, head.Lhs);
            Assert.Equal(2, head.Rhs);
            Assert.Equal(6, head.Result);

            head.Result = 42;
            _queue.Clear();
            _queue.Enqueue(head);
            result = Multiply(3, 2);
            Assert.Equal(42, result);
        }
    }
}
