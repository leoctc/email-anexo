using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace EmailComAnexo.Utils
{
    /// <summary>
    /// Classe de envio de e-mails.
    /// </summary>
    public class MailSender
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="MailSender"/>.
        /// </summary>
        public MailSender() { }

        /// <summary>
        /// Envia e-mails.
        /// </summary>
        /// <param name="email"><see cref="Email"/>.</param>
        /// <returns><see cref="Boolean"/>.</returns>
        public bool Send(Email email)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(email.De);
                mail.To.Add(email.Para);
                if (!string.IsNullOrEmpty(email.Cc)) mail.CC.Add(email.Cc);
                mail.Subject = email.Assunto;
                mail.Body = email.Corpo;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                #region Template

                StringBuilder sb = new StringBuilder();

                string path = (string.IsNullOrEmpty(email.Logo)) ?
                   HttpContext.Current.Server.MapPath(@"~/Images/logo-principal.png") :
                   email.Logo; // pode vir como HttpContext.Current.Server.MapPath(@"~/images/logo_principal.png"), por ex.

                LinkedResource logo = new LinkedResource(path, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "LogoID",
                    TransferEncoding = TransferEncoding.Base64
                };

                if (email.UseTemplate)
                {
                    sb.AppendLine("<html dir='ltr'>");
                    sb.AppendLine("    <head><title>Empresa XPTO</title></head>");
                    sb.AppendLine("    <body leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0' bgcolor='#f5f5f5' style='mso-fareast-language: PT-BR;'>");
                    sb.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' bgcolor='#f5f5f5' style='font-family: Segoe UI, Arial, Verdana, Tahoma;'>");
                    sb.AppendLine("            <tr valign='top'>");
                    sb.AppendLine("                <td>");
                    sb.AppendLine("                    <p style='text-align: center; padding: 0; margin: 0; line-height: 30px; color: #777; font-size: 10px;'>");
                    sb.AppendLine("                        <span style='text-decoration:none; color: #505151;font-size: 10px;'>ESTE É UM E-MAIL AUTOMÁTICO. POR FAVOR, NÃO RESPONDA.</span>");
                    sb.AppendLine("                    </p><br />");
                    sb.AppendLine("                    <table cellpadding='0' cellspacing='0' width='700' bgcolor='#FFFFFF' align='center' style='box-shadow: 0 3px 5px #bbb;background-color: #FFFFFF; border: 1px solid #ddd;font-family: Segoe UI, Arial, Verdana, Tahoma;'>");
                    sb.AppendLine("                        <tr>");
                    sb.AppendLine("                            <td style='padding-bottom: 15px; padding-top: 15px; border-bottom: 1px solid #ddd;'>");
                    sb.AppendLine("                                <table width='100%' style='font-family: Segoe UI, Arial, sans-serif; font-size: 12px;'>");
                    sb.AppendLine("                                    <tr valign='top'>");
                    sb.AppendLine("                                        <td align='left' style='font-size: 12px; color: #505151; padding-left: 10px;  padding-top: 10px;'>");
                    sb.AppendLine("                                            <h1><img src='cid:LogoID' style='border: none;'></img></h1>");
                    sb.AppendLine("                                        </td>");
                    sb.AppendLine("                                        <td style='padding-left: 25px;'><br/></td>");
                    sb.AppendLine("                                        <td style='font-size: 12px; color: #505151; text-align: right; padding: 20px;' align='right'>");
                    sb.AppendLine("                                            <br /><span>" + DateTime.Now.ToString("dddd',' d 'de' MMMM 'de' yyyy 'às' HH:mm'h.'") + "</span><br />");
                    sb.AppendLine("                                            <span>" + email.Assunto.ToUpper() + "</span>");
                    sb.AppendLine("                                        </td>");
                    sb.AppendLine("                                    </tr>");
                    sb.AppendLine("                                </table>");
                    sb.AppendLine("                            </td>");
                    sb.AppendLine("                        </tr>");
                    sb.AppendLine("                        <tr><td style='padding: 24px;'>");
                    sb.AppendLine("                                            " + email.Corpo);
                    sb.AppendLine("                        </td></tr>");
                    sb.AppendLine("                        <tr valign='center'>");
                    sb.AppendLine("                            <td style='background-color: #242424; color: #242424; font-size: 11px; padding: 5px; line-height: 40px;'>");
                    sb.AppendLine("                                <table cellpadding='0' cellspacing='0' width='640' bgcolor='#242424' align='center' style='font-size: 11px; line-height: 16px;color: #959595;background-color: #242424; font-family: Arial, Verdana, Tahoma;'>");
                    sb.AppendLine("                                    <tr>");
                    sb.AppendLine("                                        <td>");
                    sb.AppendLine("                                            <a style='color: #FFFFFF; text-decoration: none' href='#'>Entre em contato</a> | <a style='color: #FFFFFF; text-decoration: none' href='http://www.XPTO.org.br/'>Acesse o site</a><br />");
                    sb.AppendLine("                                            Copyright &copy; " + DateTime.Now.Year + " XPTO. All rights reserved.<br />");
                    sb.AppendLine("                                            Rua Visconde de Inhaúma, 107, Rio de Janeiro - RJ<br /><br />");
                    sb.AppendLine("                                            Este é um e-mail oficial da XPTO e todos os links levam para canais oficiais da empresa.<br />");
                    sb.AppendLine("                                            Você está recebendo este e-mail porque possui um cadastro vinculado ou solicitou um contato à XPTO.<br />");
                    sb.AppendLine("                                        </td>");
                    sb.AppendLine("                                    </tr>");
                    sb.AppendLine("                                </table>");
                    sb.AppendLine("                            </td>");
                    sb.AppendLine("                        </tr>");
                    sb.AppendLine("                    </table>");
                    sb.AppendLine("                    <br/><br/><br/>");
                    sb.AppendLine("                </td>");
                    sb.AppendLine("            </tr>");
                    sb.AppendLine("        </table>");
                    sb.AppendLine("    </body>");
                    sb.AppendLine("</html>");

                    email.Corpo = sb.ToString();
                }

                AlternateView av = AlternateView.CreateAlternateViewFromString(email.Corpo, Encoding.UTF8, MediaTypeNames.Text.Html);
                av.LinkedResources.Add(logo);

                mail.AlternateViews.Add(av);
                #endregion

                #region Cc/Bcc

                if (!string.IsNullOrEmpty(email.Cc))
                {
                    if (email.Cc.Contains(";"))
                    {
                        string[] itens = email.Cc.Split(';');

                        foreach (string item in itens)
                            if (!String.IsNullOrEmpty(item))
                                mail.Bcc.Add(item);
                    }
                    else { mail.Bcc.Add(email.Cc); }
                }
                #endregion

                #region Anexos
                // Se quiser adicionar anexos
                if (email.Anexos.Count() > 0)
                {
                    foreach (var anexo in email.Anexos)
                    {
                        mail.Attachments.Add(new Attachment(anexo.Arquivo, anexo.Nome, anexo.MediaType));
                    }
                }
                #endregion
                
                try
                {
                    using (SmtpClient smtp = new SmtpClient(email.Smtp, email.Porta))
                    {
                        if (!string.IsNullOrEmpty(email.Senha)) smtp.Credentials = new NetworkCredential(email.Usuario, email.Senha);
                        else smtp.Credentials = new NetworkCredential();

                        smtp.EnableSsl = email.Ssl;
                        smtp.Send(mail);
                    }

                    return true;
                }
                catch (Exception) { return false; }
            }
        }
    }

    /// <summary>
    /// Modelo de dados para envio de e-mails.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        public Email()
        {

            if (string.IsNullOrEmpty(this.De)) this.De = "leonardothebald@gmail.com";
            if (string.IsNullOrEmpty(this.Smtp)) this.Smtp = "smtp.gmail.com";
            if (this.Porta < 1) this.Porta = 587;
            Anexos = new List<Anexo>();

            //se utiliza ssl credenciais
            this.Ssl = true;
            this.Usuario = "leonardothebald@gmail.com";
            this.Senha = "1qaz234wer";
        }

        #region Conteúdo
        public string De { get; set; }
        public string Para { get; set; }
        public string Cc { get; set; }
        public string Assunto { get; set; }
        public string Corpo { get; set; }
        public IEnumerable<Anexo> Anexos { get; set; }
        public string Logo { get; set; }
        public bool UseTemplate { get; set; }
        #endregion

        #region Autenticação
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Smtp { get; set; }
        public int Porta { get; set; }
        public bool Ssl { get; set; }
        #endregion
    }

    /// <summary>
    /// Anexos dos e-mails.
    /// </summary>
    public class Anexo
    {
        public string Nome { get; set; }
        public string MediaType { get; set; }
        public Stream Arquivo { get; set; }
    }
}