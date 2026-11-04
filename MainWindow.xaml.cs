using System;
using System.Net;
using System.Net.Mail;
using System.Windows;

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

        private void MenuChrono_Click(object sender, RoutedEventArgs e)
        {
            ChronoWindow fenetreChrono = new ChronoWindow();
            fenetreChrono.Show();
        }

        private void MenuReseau_Click(object sender, RoutedEventArgs e)
        {
            CommunicationWindow fenetreReseau = new CommunicationWindow();
            fenetreReseau.Show();
        }
    }
}