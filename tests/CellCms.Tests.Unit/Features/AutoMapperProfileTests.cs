
using AutoMapper;

using CellCms.Tests.Unit.Utils;

using Xunit;

namespace CellCms.Tests.Unit.Features
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public class AutoMapperProfileTests
    {
        [Theory]
        [CreateData]
        public void IMapper_AllProfiles_ValidConfiguration(MapperConfiguration subject)
        {
            subject.AssertConfigurationIsValid();
        }
    }
}
