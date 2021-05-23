using AutoMapper;

namespace InterviewExamWebApi.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TProduct, TProductDTO>()
                .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory.Name))
                .AfterMap((src, dest) => {

                    foreach (var a in src.TProductInfos)
                    {
                        dest.EffectDate = a.EffectDate;
                        dest.Image1400x400 = a.Image1400x400;
                        dest.Image2400x400 = a.Image2400x400;
                        dest.Image3400x400 = a.Image3400x400;
                        dest.Image4400x400 = a.Image4400x400;
                        dest.Detail = a.Detail;
                        dest.Title = a.Title;
                    }

                    foreach (var a in src.TProductItems)
                    {
                        dest.Price = a.Price;
                        dest.QuantityRemain = a.QuantityRemain;
                        dest.Barcode = a.Barcode;
                        dest.DateIn = a.DateIn;
                        dest.QuantityMaximum = a.QuantityMaximum;
                        dest.QuantityMinimum = a.QuantityMinimum;
                    }
                });

            CreateMap<TProductDTO, TProduct>()
                .AfterMap((src, dest) => {

                    foreach (var a in dest.TProductInfos)
                    {
                        a.EffectDate = src.EffectDate;
                        a.Image1400x400 = src.Image1400x400;
                        a.Image2400x400 = src.Image2400x400;
                        a.Image3400x400 = src.Image3400x400;
                        a.Image4400x400 = src.Image4400x400;
                        a.Detail = src.Detail;
                        a.Title = src.Title;
                    }

                    foreach (var a in dest.TProductItems)
                    {
                        a.Price = src.Price;
                        a.QuantityRemain = src.QuantityRemain;
                        a.Barcode = src.Barcode;
                        a.DateIn = src.DateIn;
                        a.QuantityMaximum = src.QuantityMaximum;
                        a.QuantityMinimum = src.QuantityMinimum;
                    }
                });

            CreateMap<TOrder, TOrderDTO>()
                .ForMember(dest => dest.TOrderItems, opt => opt.MapFrom(x => x.TOrderItems));

            CreateMap<TOrderDTO, TOrder>()
                .ForMember(dest => dest.TOrderItems, opt => opt.MapFrom(x => x.TOrderItems));

            CreateMap<TOrderItem, TOrderItemDTO>();
            CreateMap<TOrderItemDTO, TOrderItem>();

            CreateMap<TShoppingCart, TShoppingCartDTO>()
                .ForMember(dest => dest.TShoppingItems, opt => opt.MapFrom(x => x.TShoppingItems));

            CreateMap<TShoppingCartDTO, TShoppingCart>()
                .ForMember(dest => dest.TShoppingItems, opt => opt.MapFrom(x => x.TShoppingItems));


            CreateMap<TShoppingItem, TShoppingItemDTO>();
            CreateMap<TShoppingItemDTO, TShoppingItem>();
        }
    }
}
