using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace PrograMail_26_27
{
    public partial class CommunicationWindow : Window
    {
        private UdpClient udpServeur;
        private TcpListener tcpServeur;
        private Thread threadUdp;
        private Thread threadTcp;

        public CommunicationWindow()
        {
            InitializeComponent();
        }

        private void EcrireEchange(string texte)
        {
            Dispatcher.Invoke(() =>
            {
                txtEchanges.AppendText(texte + Environment.NewLine);
            });
        }

        private void MenuVerifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nomServeur = txtServeur.Text;
                IPAddress[] adresses = Dns.GetHostAddresses(nomServeur);
                IPAddress ipTrouvee = null;

                foreach (IPAddress ip in adresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipTrouvee = ip;
                        break;
                    }
                }

                if (ipTrouvee != null)
                {
                    Ping ping = new Ping();
                    PingReply reponse = ping.Send(ipTrouvee, 2000);

                    if (reponse.Status == IPStatus.Success)
                    {
                        txtIp.Text = ipTrouvee.ToString();
                        MessageBox.Show("Vérification réussie, le serveur répond au ping !");
                    }
                    else
                    {
                        MessageBox.Show("Erreur : le serveur ne répond pas au ping.");
                    }
                }
                else
                {
                    MessageBox.Show("Erreur : aucune adresse IPv4 trouvée.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void MenuUdpEcouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                udpServeur = new UdpClient(8080);
                threadUdp = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            IPEndPoint distant = new IPEndPoint(IPAddress.Any, 0);
                            byte[] data = udpServeur.Receive(ref distant);
                            string message = Encoding.UTF8.GetString(data);
                            EcrireEchange("Message reçu " + message);
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
                threadUdp.IsBackground = true;
                threadUdp.Start();
                EcrireEchange("Ecoute UDP démarrée sur le port 8080");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void MenuUdpConnecter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UdpClient client = new UdpClient();
                string message = txtMessage.Text;
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data, data.Length, txtServeur.Text, 8080);
                client.Close();

                EcrireEchange("Message envoyé " + message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void MenuTcpEcouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tcpServeur = new TcpListener(IPAddress.Any, 8000);
                tcpServeur.Start();

                threadTcp = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient client = tcpServeur.AcceptTcpClient();
                            NetworkStream stream = client.GetStream();
                            BinaryWriter writer = new BinaryWriter(stream);
                            BinaryReader reader = new BinaryReader(stream);

                            writer.Write("Connexion réussie");

                            string messageRecu = reader.ReadString();
                            EcrireEchange(messageRecu);

                            client.Close();
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
                threadTcp.IsBackground = true;
                threadTcp.Start();
                EcrireEchange("Ecoute TCP démarrée sur le port 8000");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void MenuTcpConnecter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(txtServeur.Text, 8000);

                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                BinaryReader reader = new BinaryReader(stream);

                string reponse = reader.ReadString();
                EcrireEchange(reponse);

                string nomMachine = Environment.MachineName;
                writer.Write("Machine " + nomMachine + " connectée");

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void BtnEnvoyer_Click(object sender, RoutedEventArgs e)
        {
            MenuUdpConnecter_Click(sender, e);
        }

        private void MenuSocketEcouter_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuSocketConnecter_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuSocketDeconnecter_Click(object sender, RoutedEventArgs e)
        {
            if (udpServeur != null)
            {
                udpServeur.Close();
                udpServeur = null;
            }
            if (tcpServeur != null)
            {
                tcpServeur.Stop();
                tcpServeur = null;
            }
            EcrireEchange("Déconnexion effectuée.");
        }
    }
}