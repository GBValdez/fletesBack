using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.users;
using project.utils.catalogue;

namespace fletesProyect.driver.dto
{
    public class driverDto : driverDtoBase
    {
        public long Id { get; set; }
        public userEntity user { get; set; } = null!;
        public Catalogue brand { get; set; } = null!;
        public Catalogue model { get; set; } = null!;

    }
}