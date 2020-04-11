using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailComAnexo.Models
{
    public class ModeloEmail
    {
        public string Nome { get; set; }

        public IEnumerable<HttpPostedFileBase> Anexos { get; set; }
    }
}