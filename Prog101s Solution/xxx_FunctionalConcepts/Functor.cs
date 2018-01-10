using System;
using System.Collections.Generic;
using System.Text;
#pragma warning disable 659 //this ignores warnings that we have override Equals(), but not GetHashCode()

namespace Prog101s.Functor
{
    public interface IFunctor<TA>
    {
        IFunctor<TB> Fmap<TB>(Func<TA, TB> function);
    }

    // do a stringybag which converts to string to store?
    // do a "trinity" which always hold 3 of TA?
    public class Bag<TA> : IFunctor<TA>
    {
        private TA _bagged;

        public Bag(TA content)
        {
            _bagged = content;
        }

        public IFunctor<TB> Fmap<TB>(Func<TA, TB> function)
        {
            return new Bag<TB>(function(this._bagged));
        }

        public override bool Equals(object obj)
        {
            if ((obj as Bag<TA>) == null) return false;
            return ((obj as Bag<TA>)._bagged.Equals(_bagged));
        }
    }

    public class Box<TA> : IFunctor<TA>
    {
        private TA _boxed;

        public Box(TA content)
        {
            _boxed = content;
        }

        public IFunctor<TB> Fmap<TB>(Func<TA, TB> function)
        {
            return new Box<TB>(function(this._boxed));
        }

        public override bool Equals(object obj)
        {
            if ((obj as Box<TA>) == null) return false;
            return ((obj as Box<TA>)._boxed.Equals(_boxed));
        }
    }
}
#pragma warning restore 659
