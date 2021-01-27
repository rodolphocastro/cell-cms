using System;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

using AutoMapper;

using CellCms.Api;

using Microsoft.EntityFrameworkCore;

namespace CellCms.Tests.Unit.Utils
{
    /// <summary>
    /// Atributo para configurar automaticamente os dados de um test case.
    /// </summary>
    public class CreateDataAttribute : AutoDataAttribute
    {
        public CreateDataAttribute() : base(SetupCellCmsFixture)
        {

        }

        /// <summary>
        /// Configura uma fixture com todos os objetos necessários para
        /// testar o CellCMS.
        /// </summary>
        /// <returns></returns>
        private static IFixture SetupCellCmsFixture()
        {
            var fix = new Fixture();
            fix.Customize(new AutoNSubstituteCustomization());
            SetupRecursionBehaviors(fix);
            SetupCellContext(fix);
            SetupAutoMapper(fix);
            return fix;
        }

        /// <summary>
        /// Configura e Injeta uma instância do AutoMapper.
        /// </summary>
        /// <param name="fix"></param>
        private static void SetupAutoMapper(Fixture fix)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Startup));
            });
            fix.Inject(autoMapperConfig);
            fix.Inject(autoMapperConfig.CreateMapper());
        }

        /// <summary>
        /// Configura e Injeta uma instância do CellContext.
        /// </summary>
        /// <param name="fix"></param>
        private static void SetupCellContext(Fixture fix)
        {
            var dbOptions = new DbContextOptionsBuilder()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString());
            fix.Inject(new CellContext(dbOptions.Options));
        }

        /// <summary>
        /// Configura o comportamento da Fixture durante
        /// chamadas recursivas.
        /// </summary>
        /// <param name="fix"></param>
        private static void SetupRecursionBehaviors(Fixture fix)
        {
            fix.Behaviors.Remove(new ThrowingRecursionBehavior());
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}
