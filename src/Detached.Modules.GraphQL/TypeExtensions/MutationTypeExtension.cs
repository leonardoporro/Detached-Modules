﻿using HotChocolate.Types;

namespace Detached.Modules.GraphQL.TypeExtensions
{
    public class MutationTypeExtension<T> : ObjectTypeExtension<T>
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            descriptor.Name("Mutation");
        }
    }
}