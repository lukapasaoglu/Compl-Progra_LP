using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace PrograMail_26_27
{
    public partial class TodoWindow : Window
    {
        public ObservableCollection<TodoItem> Taches { get; set; } = new ObservableCollection<TodoItem>();

        private readonly string _cheminFichier = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "todolist_sauvegarde.xml"
        );

        public TodoWindow()
        {
            InitializeComponent();
            LstTaches.ItemsSource = Taches;

            ChargerTaches();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            string titre = TxtNouvelleTache.Text.Trim();
            if (!string.IsNullOrEmpty(titre))
            {
                Taches.Add(new TodoItem(titre, false));
                TxtNouvelleTache.Clear();
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (LstTaches.SelectedItem is TodoItem itemSelectionne)
            {
                Taches.Remove(itemSelectionne);
            }
            else
            {
                MessageBox.Show("Sélectionnez d'abord une tâche à supprimer.");
            }
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TodoItem>));
                using (StreamWriter writer = new StreamWriter(_cheminFichier))
                {
                    serializer.Serialize(writer, new List<TodoItem>(Taches));
                }
                MessageBox.Show("To-do list enregistrée avec succès !");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'enregistrement : " + ex.Message);
            }
        }

        private void ChargerTaches()
        {
            if (File.Exists(_cheminFichier))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<TodoItem>));
                    using (StreamReader reader = new StreamReader(_cheminFichier))
                    {
                        var liste = (List<TodoItem>)serializer.Deserialize(reader);
                        if (liste != null)
                        {
                            Taches.Clear();
                            foreach (var item in liste)
                            {
                                Taches.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors du chargement : " + ex.Message);
                }
            }
        }
    }
}