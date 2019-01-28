using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using _04_periode3_opdracht1_microwave.Annotations;
using _04_periode3_opdracht1_microwave.Enums;

namespace _04_periode3_opdracht1_microwave
{
    public class Microwave : INotifyPropertyChanged
    {
        // Default State of the Microwave
        private string light = Constants.Light.Off;
        private string door = Constants.Door.Closed;
        private string microwaving = Constants.Microwaving.No;

        public event PropertyChangedEventHandler PropertyChanged;

        public Microwave()
        {
        }

        /// <summary>
        /// Updates microwave state
        /// </summary>
        /// <param name="value"></param>
        /// <param name="methodName"></param>
        public void UpdateState(string value, string methodName)
        {
            switch (methodName)
            {
                case nameof(MicrowaveDoor):
                    door = value;

                    break;
                case nameof(Microwaving):
                    microwaving = value;

                    break;
                case nameof(MicrowaveLight):
                    light = value;

                    break;
            }

            // At the moment this causes unneeded re-renders of the UI. Even when something might not be updated we still say it has.
            this.OnPropertyChanged(methodName);
        }

        /// <summary>
        /// Data binding for the door status
        /// </summary>
        public string MicrowaveDoor
        {
            get => door == Constants.Door.Open ? Constants.Door.Open : Constants.Door.Closed;
            set => UpdateState(value, nameof(MicrowaveDoor));
        }

        /// <summary>
        /// Data binding for the microwaving status
        /// </summary>
        public string Microwaving
        {
            get => microwaving == Constants.Microwaving.Yes ? Constants.Microwaving.Yes : Constants.Microwaving.No;
            set => UpdateState(value, nameof(Microwaving));
        }

        /// <summary>
        /// Data binding for the light status
        /// </summary>
        public string MicrowaveLight
        {
            // Convert from enum to string
            get => light == Constants.Light.On ? Constants.Light.On : Constants.Light.Off;
            set => UpdateState(value, nameof(MicrowaveLight));
        }


        /// <summary>
        /// Method implemented from INotifyPropertyChanged.
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}