

using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Altoea.Framework.Helpers
{
    public class QueryHelper
    {
        public enum Operators
        {
            None = 0,
            Equal = 1,
            GreaterThan = 2,
            GreaterThanOrEqual = 3,
            LessThan = 4,
            LessThanOrEqual = 5,
            Contains = 6,
            StartWith = 7,
            EndWidth = 8,
            Range = 9
        }
        public enum Condition
        {
            OrElse = 1,
            AndAlso = 2
        }
        public string Name { get; set; }
        public Operators Operator { get; set; }
        public object Value { get; set; }
        public object ValueMin { get; set; }
        public object ValueMax { get; set; }
    }

    public class QueryCollection : Collection<QueryHelper>
    {
        public Expression<Func<T, bool>> AsExpression<T>(QueryHelper.Condition? condition = QueryHelper.Condition.OrElse) where T : class
        {
            Type targetType = typeof(T);
            TypeInfo typeInfo = targetType.GetTypeInfo();
            var parameter = Expression.Parameter(targetType, "m");
            Expression expression = null;
            Func<Expression, Expression, Expression> Append = (exp1, exp2) =>
            {
                if (exp1 == null)
                {
                    return exp2;
                }
                return (condition ?? QueryHelper.Condition.OrElse) == QueryHelper.Condition.OrElse ? Expression.OrElse(exp1, exp2) : Expression.AndAlso(exp1, exp2);
            };
            foreach (var item in this)
            {
                var property = typeInfo.GetProperty(item.Name);
                if (property == null ||
                    !property.CanRead ||
                    (item.Operator != QueryHelper.Operators.Range && item.Value == null) ||
                    (item.Operator == QueryHelper.Operators.Range && item.ValueMin == null && item.ValueMax == null))
                {
                    continue;
                }
                Type realType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                switch (item.Operator)
                {
                    case QueryHelper.Operators.Equal:
                        {
                            expression = Append(expression, Expression.Equal(Expression.Property(parameter, item.Name), 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType)));
                            break;
                        }
                    case QueryHelper.Operators.GreaterThan:
                        {
                            expression = Append(expression, Expression.GreaterThan(Expression.Property(parameter, item.Name), 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType)));
                            break;
                        }
                    case QueryHelper.Operators.GreaterThanOrEqual:
                        {
                            expression = Append(expression, Expression.GreaterThanOrEqual(Expression.Property(parameter, item.Name), 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType)));
                            break;
                        }
                    case QueryHelper.Operators.LessThan:
                        {
                            expression = Append(expression, Expression.LessThan(Expression.Property(parameter, item.Name), 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType)));
                            break;
                        }
                    case QueryHelper.Operators.LessThanOrEqual:
                        {
                            expression = Append(expression, Expression.LessThanOrEqual(Expression.Property(parameter, item.Name), 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType)));
                            break;
                        }
                    case QueryHelper.Operators.Contains:
                        {
                            var nullCheck = Expression.Not(Expression.Call(typeof(string), "IsNullOrEmpty", null, Expression.Property(parameter, item.Name)));
                            var contains = Expression.Call(Expression.Property(parameter, item.Name), "Contains", null, 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType));
                            expression = Append(expression, Expression.AndAlso(nullCheck, contains));
                            break;
                        }
                    case QueryHelper.Operators.StartWith:
                        {
                            var nullCheck = Expression.Not(Expression.Call(typeof(string), "IsNullOrEmpty", null, Expression.Property(parameter, item.Name)));
                            var startsWith = Expression.Call(Expression.Property(parameter, item.Name), "StartsWith", null, 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType));
                            expression = Append(expression, Expression.AndAlso(nullCheck, startsWith));
                            break;
                        }
                    case QueryHelper.Operators.EndWidth:
                        {
                            var nullCheck = Expression.Not(Expression.Call(typeof(string), "IsNullOrEmpty", null, Expression.Property(parameter, item.Name)));
                            var endsWith = Expression.Call(Expression.Property(parameter, item.Name), "EndsWith", null, 
                                Expression.Constant(Convert.ChangeType(item.Value, realType), property.PropertyType));
                            expression = Append(expression, Expression.AndAlso(nullCheck, endsWith));
                            break;
                        }
                    case QueryHelper.Operators.Range:
                        {
                            Expression minExp = null, maxExp = null;
                            if (item.ValueMin != null)
                            {
                                var minValue = Convert.ChangeType(item.ValueMin, realType);
                                minExp = Expression.GreaterThanOrEqual(Expression.Property(parameter, item.Name), Expression.Constant(minValue, property.PropertyType));
                            }
                            if (item.ValueMax != null)
                            {
                                var maxValue = Convert.ChangeType(item.ValueMax, realType);
                                maxExp = Expression.LessThanOrEqual(Expression.Property(parameter, item.Name), Expression.Constant(maxValue, property.PropertyType));
                            }

                            if (minExp != null && maxExp != null)
                            {
                                expression = Append(expression, Expression.AndAlso(minExp, maxExp));
                            }
                            else if (minExp != null)
                            {
                                expression = Append(expression, minExp);
                            }
                            else if (maxExp != null)
                            {
                                expression = Append(expression, maxExp);
                            }

                            break;
                        }
                }
            }
            if (expression == null)
            {
                return null;
            }
            return ((Expression<Func<T, bool>>)Expression.Lambda(expression, parameter));
        }
    }
}