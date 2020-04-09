using AutoMapper;

using CellCms.Api.Models;

namespace CellCms.Api.Features.Tags
{
    /// <summary>
    /// Mapeamento para os Commands e Queries de Tags.
    /// </summary>
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<CreateTag, Tag>();
            CreateMap<UpdateTag, Tag>();
            CreateMap<Tag, Tag>()
                .ForMember(s => s.ContentTags, opt => opt.Ignore())
                .ForMember(s => s.Feed, opt => opt.Ignore())
                .ForMember(s => s.FeedId, opt => opt.Ignore());
        }
    }
}
