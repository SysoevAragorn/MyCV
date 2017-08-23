using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;

namespace VrachMedcentr
{
    class EditDayViewModel : INotifyPropertyChanged
    {
        SelectedDatesCollection tempSelected = new SelectedDatesCollection(new Calendar());
        #region Constructor
        public EditDayViewModel(DocNames Doc)
        {
            WorkDays = con.GetListOfWorkingDays(Convert.ToInt32(Doc.docID));
            docSelected = Doc;
            selectedDays = DateTime.Now;
            tempSelected.Add(DateTime.Now);
        }
        #endregion

        #region Helpers object
        conBD con = new conBD("shostka.mysql.ukraine.com.ua", "shostka_crl", "shostka_crl", "Cpu25Pro");
        SynhronyzeClass sync = new SynhronyzeClass();
        #endregion

        #region Publick Variables

        List<DateTime> selectedDaysCal = new List<DateTime>();
        private DateTime _selectedDays;
        public DateTime selectedDays
        {
            get
            {
                return _selectedDays;
            }
            set
            {
                try
                {
                    _selectedDays = value;
                    docTimes = new ObservableCollection<Times>();
                    if (WorkDays.Contains(value) == true)
                    {
                        List<Times> temp = con.getDocTimes(docSelected.docID, value);
                        foreach (var a in temp)
                        {
                            docTimes.Add(new Times { Time = a.Time, Label = a.Label, Status = a.Status, PublickPrivate=a.PublickPrivate });
                        }
                        //docTimes = docTimes.OrderBy(p => p.Time) as ObservableCollection<Times>;


                    }
                    else
                    {
                        docTimes = new ObservableCollection<Times>();
                        //docTimes.Add(new Times { Label = "Не робочій день", Status = "Red" });
                    }




                }
                catch (Exception e)
                {
                   // MessageBox.Show(e.Message.ToString());
                   
                }

            }
        }
        public DocNames docSelected { get; set; }
        public ObservableCollection<Times> docTimes { get; set; }
        public ObservableCollection<DateTime> WorkDays { get; set; }





        public ObservableCollection<DocNames> docBool { get; set; }

        public Times SelectedTime { get; set; }

        #endregion


        #region Commands
        private RelayCommand _addTimes;
        public RelayCommand addTimes
        {
            get
            {

                return _addTimes ??
                  (_addTimes = new RelayCommand(obj =>
                  {

                      docTimes.Add(new Times { Time = "12:00", Label = "12:00" });
                      // NotifyPropertyChanged("docTimes");



                  }));
            }

        }

        /// <summary>
        /// команда удаления записи в качесте obj передаеться пораметр команды с общим Binding
        /// </summary>
        private RelayCommand _remTimes;

        public RelayCommand remTimes
        {
            get
            {

                return _remTimes ??
                  (_remTimes = new RelayCommand(obj =>
                  {

                      try
                      {

                          docTimes.Remove(obj as Times);
                          foreach (var a in tempSelected as SelectedDatesCollection)
                          {
                              Times temp = obj as Times;
                              string[] parTime = temp.Time.Split(new char[] { ':' });
                              con.RemTimeInWorkDay(docSelected.docID, a, parTime[0], parTime[1]);
                              //docTimes = new ObservableCollection<Times>();
                              //List < Times > temp1 = new List<Times>();
                              //temp1 = con.getDocTimes(docSelected.docID, selectedDays);
                              //foreach (var a1 in temp1)
                              //{
                              //    docTimes.Add(new Times { Time = a1.Time, Label = a1.Label, Status = a1.Status });
                              //}

                          }


                      }
                      catch (Exception) { }



                  }));
            }

        }




        private RelayCommand _setSelectedDays;

        public RelayCommand setSelectedDays
        {
            get
            {

                return _setSelectedDays ??
                  (_setSelectedDays = new RelayCommand(obj =>
                  {
                      //tempSelected.Clear();
                      tempSelected = obj as SelectedDatesCollection;

                  }));
            }

        }
       

        private RelayCommand _checkDays;

        public RelayCommand checkDays
        {
            get
            {

                return _checkDays ??
                  (_checkDays = new RelayCommand(obj =>
                  {


                      string datestring = "";
                      List<DateTime> DateWithoutWorkingDays = new List<DateTime>();
                      try
                      {
                          //создаем стирнг из дат которіе пересекаються с робочими днями, не робочие дни добавляем в отдельный лист
                          foreach (var a in tempSelected )
                          {
                              if (WorkDays.Contains(a) == true)
                              {
                                  datestring = datestring + a.ToShortDateString() + ", ";
                              }
                              else
                              {
                                  DateWithoutWorkingDays.Add(a);
                              }
                          }

                          if (datestring != "")
                          {
                              datestring = datestring.Remove(datestring.Length - 2);
                          }
                          

                          //если среди выбраных дней есть робочие выводим сообщение с опрос пользователя о дальнейших действиях
                          if (datestring != "")
                          {
                              var result = MessageBox.Show("На обрану дати: " + datestring + " вже існує розклад.\nПерезаписати розклад на ці дні?", "Повідомлення", MessageBoxButton.YesNo, MessageBoxImage.Question);
                              if (result == MessageBoxResult.Yes)
                              {
                                  foreach (var a in tempSelected as SelectedDatesCollection)
                                  {
                                      con.remWorkDays(docSelected.docID, a);
                                      foreach (var t in docTimes)
                                      {
                                          string[] parTime = t.Time.Split(new char[] { ':' });

                                          con.addWorkDays(docSelected.docID, "0", false, t.PublickPrivate, a, parTime[0], parTime[1], "0", "0");
                                      }
                                  }
                                  datestring = "";
                                  DateWithoutWorkingDays = new List<DateTime>();
                              }
                              if (result == MessageBoxResult.No)
                              {
                                  foreach (var a in DateWithoutWorkingDays)
                                  {

                                      foreach (var t in docTimes)
                                      {
                                          string[] parTime = t.Time.Split(new char[] { ':' });

                                          con.addWorkDays(docSelected.docID, "0", false, t.PublickPrivate, a, parTime[0], parTime[1], "0", "0");
                                      }

                                  }
                                  datestring = "";
                                  DateWithoutWorkingDays = new List<DateTime>();
                              }
                          }
                          //если нет просто публикуем росписание
                          else
                          {
                              foreach (var a in tempSelected as SelectedDatesCollection)
                              {

                                  foreach (var t in docTimes)
                                  {
                                      string[] parTime = t.Time.Split(new char[] { ':' });

                                      con.addWorkDays(docSelected.docID, "0", false, t.PublickPrivate, a, parTime[0], parTime[1], "0", "0");
                                  }

                              }
                          }
                          

                          WorkDays = con.GetListOfWorkingDays(Convert.ToInt32(docSelected.docID));
                      }
                      catch (Exception e)
                      {
                      //    MessageBox.Show(e.ToString());
                      }

                      //editDays edDays = new editDays();
                      // sync.SynhronyzeTable("enx4w_ttfsp", 2);

                      // edDays.Close();

                  }));
            }

        }

        private RelayCommand _uncheckDays;

        public RelayCommand uncheckDays
        {
            get
            {

                return _uncheckDays ??
                  (_uncheckDays = new RelayCommand(obj =>
                  {



                      try
                      {
                          foreach (var a in tempSelected as SelectedDatesCollection)
                          {


                              con.remWorkDays(docSelected.docID, a);

                          }
                          WorkDays = con.GetListOfWorkingDays(Convert.ToInt32(docSelected.docID));
                          docTimes = new ObservableCollection<Times>();
                          // sync.SynhronyzeTable("ekfgq_ttfsp", 2);
                      }
                      catch (Exception) { }


                  }));
            }

        }
        #endregion

        #region Interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion
}
