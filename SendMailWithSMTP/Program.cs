using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace SendMailWithSMTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** | Email Gönder | *****");
            Console.WriteLine("----------------------------");
            Console.Write("| Emailinizi giriniz : ");
            string fromMail = Console.ReadLine();
            Console.Write("| Şifrenizi Giriniz : ");
            string fromPass = Orb.App.Console.ReadPassword();
            Console.Write("| Email Göndermek istediğiniz adresi giriniz : ");
            string toMail = Console.ReadLine();
            Console.Write("| Email Başlığını Giriniz : ");
            string mailBaslik = Console.ReadLine();
            Console.Write("| Email Yazısını Giriniz : ");
            string mailYazi = Console.ReadLine();

            EmailGonder(fromMail, fromPass, toMail, mailBaslik, mailYazi);
        }

        static void EmailGonder(string fromMail, string fromPass, string toMail, string baslik, string yazi)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 25)
                {
                    Port = 25,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(fromMail, fromPass),
                };

                using (var mail = new MailMessage(fromMail, toMail)
                {
                    Subject = baslik,
                    Body = yazi,
                })

                    client.Send(mail);
                Console.WriteLine("Mail Gönderme Başarılı");
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("Mail Gönderme Başarısız!");
                Console.ReadKey();

            }


        }

    }
}
namespace Orb.App
{

    static public class Console
    {

        public static string ReadPassword(char mask)
        {
            const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = { 0, 27, 9, 10 };

            var pass = new Stack<char>();
            char chr = (char)0;

            while ((chr = System.Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push((char)chr);
                    System.Console.Write(mask);
                }
            }

            System.Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

        public static string ReadPassword()
        {
            return Orb.App.Console.ReadPassword('*');
        }
    }
}