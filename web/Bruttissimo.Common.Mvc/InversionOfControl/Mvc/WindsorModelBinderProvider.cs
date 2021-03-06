using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Bruttissimo.Common.Guard;
using Castle.MicroKernel;

namespace Bruttissimo.Common.Mvc.InversionOfControl.Mvc
{
    internal sealed class WindsorModelBinderProvider : IModelBinderProvider
    {
        private readonly IKernel kernel;
        private readonly IDictionary<Type, Type> modelBinderTypes;

        public WindsorModelBinderProvider(IKernel kernel, IDictionary<Type, Type> modelBinderTypes)
        {
            Ensure.That(() => kernel).IsNotNull();
            Ensure.That(() => modelBinderTypes).IsNotNull();

            this.kernel = kernel;
            this.modelBinderTypes = modelBinderTypes;
        }

        public IModelBinder GetBinder(Type modelType)
        {
            Ensure.That(() => modelType).IsNotNull();

            if (modelBinderTypes.ContainsKey(modelType))
            {
                Type modelBinder = modelBinderTypes[modelType];
                return (IModelBinder)kernel.Resolve(modelBinder);
            }
            return null;
        }
    }
}
