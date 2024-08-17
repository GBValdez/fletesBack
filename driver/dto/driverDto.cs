using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.users;
using project.users.dto;
using project.utils.catalogue;
using project.utils.catalogues.dto;

namespace fletesProyect.driver.dto
{
    public class driverDto : driverDtoBase
    {
        public long Id { get; set; }
        public userDto user { get; set; } = null!;
        public catalogueDto brand { get; set; } = null!;
        public catalogueDto model { get; set; } = null!;

    }
}