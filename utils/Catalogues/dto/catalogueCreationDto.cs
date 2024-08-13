using System.ComponentModel.DataAnnotations;
using fletesProyect.utils.Catalogues.dto;

namespace project.utils.catalogues.dto
{
    public class catalogueCreationDto : catalogueDtoBse
    {
        public long? catalogueParentId { get; set; }
    }
}
