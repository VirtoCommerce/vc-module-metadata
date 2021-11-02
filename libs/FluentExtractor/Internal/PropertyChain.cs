namespace FluentExtractor.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class PropertyChain
    {
        private readonly List<string> _memberNames = new List<string>(2);

        public PropertyChain(PropertyChain parent)
        {
            if (parent != null
                && parent._memberNames.Count > 0)
            {
                _memberNames.AddRange(parent._memberNames);
            }
        }

        public PropertyChain(IEnumerable<string> memberNames)
        {
            _memberNames.AddRange(memberNames);
        }

        public static PropertyChain FromExpression(LambdaExpression expression)
        {
            var memberNames = new Stack<string>();

            var getMemberExp = new Func<Expression, MemberExpression>(toUnwrap =>
            {
                if (toUnwrap is UnaryExpression unaryExpression)
                {
                    return unaryExpression.Operand as MemberExpression;
                }

                return toUnwrap as MemberExpression;
            });

            var memberExp = getMemberExp(expression.Body);

            while (memberExp != null)
            {
                memberNames.Push(memberExp.Member.Name);
                memberExp = getMemberExp(memberExp.Expression);
            }

            return new PropertyChain(memberNames);
        }

        public void Add(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
                _memberNames.Add(propertyName);
        }

        public override string ToString() => _memberNames.Count switch
        {
            0 => string.Empty,
            1 => _memberNames[0],
            _ => string.Join(".", _memberNames),
        };

        public string BuildPropertyName(string propertyName)
        {
            if (_memberNames.Count == 0)
            {
                return propertyName;
            }

            var chain = new PropertyChain(this);
            chain.Add(propertyName);
            return chain.ToString();
        }

        public int Count => _memberNames.Count;
    }
}
