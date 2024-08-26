using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils.catalogues.dto;

namespace fletesProyect.driver.modelGasolineModule.dto
{
    public class modelGasolineDto : modelGasolineDtoBase
    {
        public long Id { get; set; }

        public catalogueDto gasolineType { get; set; } = null!;
        public catalogueDto model { get; set; } = null!;

    }
}