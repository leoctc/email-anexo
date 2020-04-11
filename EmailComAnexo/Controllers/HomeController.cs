using EmailComAnexo.Models;
using EmailComAnexo.Utils;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace EmailComAnexo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviarEmail(ModeloEmail m)
        {

            var sender = new MailSender();

            try
            {
                //pegando os anexos e enviando junto no email
                var anexos = new List<Anexo>();

                foreach (var itemLista in m.Anexos)
                {
                    var a = new Anexo
                    {
                        Arquivo = itemLista.InputStream,
                        MediaType = itemLista.ContentType,
                        Nome = itemLista.FileName
                    };

                    anexos.Add(a);
                }

                var mail = new Email
                {
                    Assunto = "Teste",
                    De = "leonardothebald@gmail.com",
                    Para = "leonardothebald@gmail.com",
                    Corpo = $"Nome: {m.Nome}",
                    Anexos = anexos
                };

                sender.Send(mail);

                return View("Index",m);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}