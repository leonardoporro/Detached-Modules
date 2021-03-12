﻿using Detached.Mappers;
using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class DetacheModulesMapperCustomizer : IMapperCustomizer
    {
        readonly Application _app;

        public DetacheModulesMapperCustomizer(Application app)
        {
            _app = app;
        }

        public void Customize(DbContext dbContext, MapperOptions mapperOptions)
        {
            foreach (Module module in _app.Modules)
            {
                foreach (EFRepositoryComponent component in module.Components.OfType<EFRepositoryComponent>())
                {
                    component.ConfigureMapper(dbContext, mapperOptions);
                }
            }
        }
    }
}
