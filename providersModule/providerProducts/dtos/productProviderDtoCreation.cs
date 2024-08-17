using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.providersModule.providerProducts.dtos
{
    public class productProviderDtoCreation : productProviderDtoBase
    {
        public long productId { get; set; }
        public long providerId { get; set; }

    }
}