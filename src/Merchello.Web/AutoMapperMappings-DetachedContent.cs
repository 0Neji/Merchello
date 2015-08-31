﻿namespace Merchello.Web
{
    using System.Diagnostics;

    using Merchello.Core.Models.DetachedContent;
    using Merchello.Web.Models.ContentEditing.Content;
    using Merchello.Web.Models.MapperResolvers;
    using Merchello.Web.Models.MapperResolvers.DetachedContent;

    using Umbraco.Core.Models;

    /// <summary>
    /// The auto mapper mappings.
    /// </summary>
    internal static partial class AutoMapperMappings
    {
        /// <summary>
        /// The create detached content mappings.
        /// </summary>
        private static void CreateDetachedContentMappings()
        {
            AutoMapper.Mapper.CreateMap<IDetachedContentType, DetachedContentTypeDisplay>()
                .ForMember(
                    dest => dest.EntityTypeField,
                    opt =>
                    opt.ResolveUsing<EntityTypeFieldResolver>().ConstructedBy(() => new EntityTypeFieldResolver()))
                .ForMember(
                    dest => dest.UmbContentType, 
                    opt 
                    => 
                    opt.ResolveUsing<UmbContentTypeResolver>().ConstructedBy(() => new UmbContentTypeResolver()));

            AutoMapper.Mapper.CreateMap<IContentType, UmbContentTypeDisplay>()
                .ForMember(
                    dest => dest.Tabs,
                    opt =>
                    opt.ResolveUsing<EmbeddedContentTabsResolver>()
                        .ConstructedBy(() => new EmbeddedContentTabsResolver()));

            AutoMapper.Mapper.CreateMap<IProductVariantDetachedContent, ProductVariantDetachedContentDisplay>()
                .ForMember(
                    dest => dest.DetachedDataValues,
                    opt =>
                    opt.ResolveUsing<DetachedDataValuesResolver>().ConstructedBy(() => new DetachedDataValuesResolver()));
        }
    }
}