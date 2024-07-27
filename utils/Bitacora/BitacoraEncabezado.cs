using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using project.users;
using project.utils;

namespace AvionesBackNet.Models;

public partial class BitacoraEncabezado : CommonsModel<long>
{
    public string Tabla { get; set; } = null!;

    public string IdRegistro { get; set; } = null!;

    public string TipoTransaccion { get; set; } = null!;

    public string UserId { get; set; }
    public ICollection<BitacoraCuerpo> BitacoraCuerpos { get; set; } = new List<BitacoraCuerpo>();
    [ForeignKey("UserId")]
    public userEntity User { get; set; } = null!;
}
