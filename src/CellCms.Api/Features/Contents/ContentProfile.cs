using System.Linq;

using AutoMapper;

using CellCms.Api.Models;

namespace CellCms.Api.Features.Contents
{
    /// <summary>
    /// Profile para mapear Contents e seus Commands e Queries.
    /// </summary>
    public class ContentProfile : Profile
    {
        public ContentProfile()
        {
            CreateMap<CreateContent, Content>()
                .ForMember(d => d.ContentTags, opt => opt.MapFrom(s => s.TagsId.Select(t => new ContentTag { TagId = t })));
            CreateMap<UpdateContent, Content>()
                .ForMember(d => d.ContentTags, opt => opt.MapFrom(s => s.TagsId.Select(t => new ContentTag { TagId = t })));
            CreateMap<Content, Content>()
                .ForMember(d => d.Feed, opt => opt.Ignore())
                .ForMember(d => d.FeedId, opt => opt.Ignore());
        }
    }
}
