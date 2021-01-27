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
            CreateMap<CreateTag, Tag>()
                .ForMember(d => d.FeedId, opt => opt.MapFrom(o => o.FeedId))
                .ForMember(d => d.Nome, opt => opt.MapFrom(o => o.Nome))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<UpdateTag, Tag>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, opt => opt.MapFrom(s => s.Nome))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<Tag, Tag>()
                .ForMember(s => s.ContentTags, opt => opt.Ignore())
                .ForMember(s => s.Feed, opt => opt.Ignore())
                .ForMember(s => s.FeedId, opt => opt.Ignore());
        }
    }
}
