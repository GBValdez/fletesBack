using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.models;
using fletesProyect.products;
using fletesProyect.providersModule.dto;

namespace fletesProyect.providersModule.providerProducts.dtos
{
    public class productProviderDto : productProviderDtoBase
    {
        public productDto product { get; set; } = null!;
        public providerDto provider { get; set; } = null!;
        public long providerId { get; set; }

    }
}