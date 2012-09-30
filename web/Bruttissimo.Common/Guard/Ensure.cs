using System;
using System.Linq.Expressions;

namespace Bruttissimo.Common.Guard
{
    public static class Ensure
    {
        public static Param<T> That<T>(T value, string name = Param.DefaultName)
        {
            return new Param<T>(name, value);
        }

        public static Param<T> That<T>(Expression<Func<T>> expression)
        {
            MemberExpression memberExpression = expression.GetRightMostMember();

            return new Param<T>(memberExpression.ToPath(), expression.Compile().Invoke());
        }

        public static Param<T> That<T>(Func<T> function, string name = Param.DefaultName)
        {
            return new Param<T>(name, function.Invoke());
        }

        public static TypeParam ThatTypeFor<T>(T value, string name = Param.DefaultName)
        {
            return new TypeParam(name, value.GetType());
        }
    }
}