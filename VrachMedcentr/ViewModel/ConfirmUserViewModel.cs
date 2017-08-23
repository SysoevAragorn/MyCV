using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrachMedcentr.View;

namespace VrachMedcentr
{
    class ConfirmUserViewModel : INotifyPropertyChanged
    {
        conBD con = new conBD();
        public ObservableCollection<Users> allUsers { get; set; }
        public Users SelectedUser { get; set; }

        public ConfirmUserViewModel()
        {
            allUsers = con.GetUnConfirmedUsers();
        }
        public void confUserRealization(object obj)
        {
            var selUser = obj as Users;
            con.ConfirmUser(selUser.userId);
          
            allUsers = con.GetUnConfirmedUsers();
            


        }
        private RelayCommand _confUserCommand;
        public RelayCommand confUserCommand
        {
            get
            {
                return _confUserCommand ??
                       (_confUserCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               //confUserRealization(obj);
                               var selUser = obj as Users;
                               //var selUser = SelectedUser;
                               con.ConfirmUser(selUser.userId);
                               allUsers = new ObservableCollection<Users>();
                               allUsers = con.GetUnConfirmedUsers();
                           }
                           catch
                           {

                           }


                       }));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
