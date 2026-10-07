using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace PrograMail_26_27
{
    public class ChronoViewModel : INotifyPropertyChanged
    {
        private readonly ChronoModel _model;
        private readonly DispatcherTimer _timer;
        private bool _enCours;

        public ChronoViewModel()
        {
            _model = new ChronoModel();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            DemarrerCommand = new RelayCommand(p => Demarrer(), p => !_enCours);
            ArreterCommand = new RelayCommand(p => Arreter(), p => _enCours);
            ReinitialiserCommand = new RelayCommand(p => Reinitialiser(), p => _model.SecondesTotales > 0 || _enCours);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.Incrementer();
            NotifierChangements();
        }

        public double AngleAiguille => (_model.SecondesTotales % 60) * 6.0;

        public string TempsTexte
        {
            get
            {
                TimeSpan t = TimeSpan.FromSeconds(_model.SecondesTotales);
                return t.ToString(@"mm\:ss");
            }
        }

        public ICommand DemarrerCommand { get; }
        public ICommand ArreterCommand { get; }
        public ICommand ReinitialiserCommand { get; }

        private void Demarrer()
        {
            _timer.Start();
            _enCours = true;
            CommandManager.InvalidateRequerySuggested();
        }

        private void Arreter()
        {
            _timer.Stop();
            _enCours = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private void Reinitialiser()
        {
            _timer.Stop();
            _enCours = false;
            _model.Reinitialiser();
            NotifierChangements();
            CommandManager.InvalidateRequerySuggested();
        }

        private void NotifierChangements()
        {
            OnPropertyChanged(nameof(AngleAiguille));
            OnPropertyChanged(nameof(TempsTexte));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}