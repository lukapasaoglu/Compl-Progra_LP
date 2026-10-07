using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;

namespace PrograMail_26_27
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEnvoyer_Click(object sender, RoutedEventArgs e)
        {
            string expediteur = TxtExpediteur.Text.Trim();
            string motDePasse = TxtPassword.Password.Trim();
            string destinataire = TxtDestinataire.Text.Trim();
            string sujet = TxtSujet.Text.Trim();
            string message = TxtMessage.Text;

            string serveurSmtp;

            if (expediteur.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                serveurSmtp = "smtp.gmail.com";
            }
            else if (expediteur.EndsWith("@outlook.com", StringComparison.OrdinalIgnoreCase) ||
                     expediteur.EndsWith("@hotmail.com", StringComparison.OrdinalIgnoreCase))
            {
                serveurSmtp = "smtp.office365.com";
            }
            else
            {
                MessageBox.Show("Veuillez utiliser une adresse Gmail ou Outlook/Hotmail.");
                return;
            }

            try
            {
                using (MailMessage monMail = new MailMessage())
                {
                    monMail.From = new MailAddress(expediteur);
                    monMail.To.Add(destinataire);
                    monMail.Subject = sujet;
                    monMail.Body = message;

                    using (SmtpClient client = new SmtpClient(serveurSmtp, 587))
                    {
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(expediteur, motDePasse);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;

                        client.Send(monMail);
                    }
                }

                MessageBox.Show("Super ! Le mail a bien été envoyé !");
            }
            catch (Exception erreur)
            {
                MessageBox.Show("Erreur lors de l'envoi : " + erreur.Message);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TodoWindow fenetreTodo = new TodoWindow();
            fenetreTodo.Show();
        }
    }
}
