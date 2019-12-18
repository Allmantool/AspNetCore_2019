using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;
using Northwind.Web.MVC.Models.Categories;

namespace Northwind.Web.MVC.Models.MappingProfiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            this.CreateMap<CategoryEditViewModel, CategoryUpdate>()
                .ForMember(ce => ce.Picture, opts => opts.ConvertUsing<IFormFile>(new IFormFileToByteConverter(), cevm => cevm.Picture));

            this.CreateMap<CategoryUpdate, CategoryEditViewModel>()
                .ForMember(cevm => cevm.Picture,
                    opts => opts.ConvertUsing<byte[]>(new ByteToIFormFileConverter(), ce => ce.Picture));
        }

        internal class IFormFileToByteConverter : IValueConverter<IFormFile, byte[]>
        {
            public byte[] Convert(IFormFile sourceMember, ResolutionContext context)
            {
                using var memoryStream = new MemoryStream();
                
                sourceMember.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var result = memoryStream.ToArray();
                return result;
            }
        }

        internal class ByteToIFormFileConverter : IValueConverter<byte[],IFormFile>
        {
            public IFormFile Convert(byte[] sourceMember, ResolutionContext context)
            {
                using var memoryStream = new MemoryStream(sourceMember);

                var result = new FormFile(memoryStream, 0, memoryStream.Length, string.Empty, string.Empty);

                return result;
            }
        }
    }

}
