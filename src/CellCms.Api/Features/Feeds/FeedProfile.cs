using System.Linq;
using AutoMapper;

using CellCms.Api.Models;

namespace CellCms.Api.Features.Feeds
{
    /// <summary>
    /// Mapeamentos para os commands e queries de Feeds.
    /// </summary>
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            // Aqui indicamos para o AutoMapper criar, por inferência, o mapeamento entre o CreateFeed e o Feed
            CreateMap<CreateFeed, Feed>();
            // Aqui indicamos para o AutoMapper criar, por inferência, o mapeamento entre o UpdateFeed e o Feed
            CreateMap<UpdateFeed, Feed>();
            // Aqui indicamos que o AutoMapepr poderá mapear entre dois objetos do mesmo tipo
            CreateMap<Feed, Feed>();
            // Para Mapearmos de um Feed para um ListFeed precisamos configurar um mapeamento de Tags para IEnumerable<string> e Contents para IEnumerable<string>
            // O método ForMember nos permite configurar este mapeando e utilizarmos LINQ para escolher qual campo queremos traze do Content/Tag para o ListFeed
            CreateMap<Feed, ListFeed>()
                .ForMember(d => d.ContentsTitulos, opt => opt.MapFrom(s => s.Contents.Select(c => c.Titulo)))
                .ForMember(d => d.TagsNomes, opt => opt.MapFrom(s => s.Tags.Select(t => t.Nome)));
        }
    }
}
