namespace NServiceBus
{
    using System;
    using System.Linq;
    using NServiceBus.Pipeline;

    static class BehaviorTypeChecker
    {
        public static void ThrowIfInvalid(Type behavior, string paramName)
        {
            Guard.AgainstNull(nameof(behavior), behavior);
            if (behavior.IsAbstract)
            {
                throw new ArgumentException($"The behavior '{behavior.Name}' is invalid since it is abstract.", paramName);
            }
            if (behavior.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"The behavior '{behavior.Name}' is invalid since it is an open generic.", paramName);
            }
            if (!IsAssignableToIBehavior(behavior))
            {
                throw new ArgumentException($@"The behavior '{behavior.Name}' is invalid since it does not implement IBehavior<TFrom, TTo>.", paramName);
            }
        }

        static Type iBehaviorType = typeof(IBehavior<,>);

        static bool IsAssignableToIBehavior(Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == iBehaviorType))
            {
                return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == iBehaviorType)
            {
                return true;
            }

            return false;
        }
    }
}