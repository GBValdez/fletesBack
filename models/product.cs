using project.utils;
using project.utils.catalogue;

namespace fletesProyect.models
{
    public class product : CommonsModel<long>
    {
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public long brandProductId { get; set; }
        public Catalogue brandProduct { get; set; } = null!;
        public long categoryId { get; set; }
        public Catalogue category { get; set; } = null!;
        public float weight { get; set; }
        public string imgUrl { get; set; } = null!;
    }
}