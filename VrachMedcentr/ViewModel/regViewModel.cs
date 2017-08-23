using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VrachMedcentr.HelpersClass.MyHalpers;
using VrachMedcentr.View;
using WPF_Hospital;

namespace VrachMedcentr
{
    class regViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Здесь лежат все приватные переменные для роскрития сетеров 
        /// </summary>
        #region Private Variables


        private string comboboxtext;
        private string selectedFIO;
        private bool timehour;
        private DateTime dateDoctorAcepting;
        private ObservableCollection<string> OneTimeUsers = new ObservableCollection<string>();// переменная для представления ФИО юзверей в комбобоксе  
        private List<Times> OneTimeDoctorTimes = new List<Times>();
        private ObservableCollection<Users> ListOfUsers;//переменная для считыванья списка юзверей единожди при запуске програмы
        public ObservableCollection<DateTime> Otemp { get; set; }
        private Users SelectedUser;


        #endregion

        //  conBD con = new conBD(@"localhost", "shostka_crl", "root", "monteshot"); // одновременно с этим задаются свойства в Synhronyze
        public conBD con { get; set; } = new conBD("shostka.mysql.ukraine.com.ua", "shostka_crl", "shostka_crl", "Cpu25Pro");
        public SynhronyzeClass synhronyze { get; set; } = new SynhronyzeClass();
        #region Constructor
        //
        DataTable azaza = new DataTable();
        public regViewModel()
        {
            // KARTA = new CardPageOne { Name = "aaaaaaaaaa", Sername = "bbbbbbbbbbb" };

            //CheckConnection();
            //synhronyze.SynhronyzeAll();
            if (synhronyze.CheckConnection())
            {
                synhronyze.conLocal = con;
                DateDoctorAcepting = DateTime.Today;
                ListOfSpecf = con.GetDocSpecification();
                ListOfUsers = con.GetUsers();

                Users = OneTimeUsers;

                //synhronyze.SynhronyzeTable("talon_time", 1);



                // MessageBox.Show(con.getHash().ToString());
                foreach (var a in ListOfUsers)
                {
                    OneTimeUsers.Add(a.userFIO);
                }
                DoctorTimes = new List<Times>();
                //try
                //{
                //    DoctorTimes = con.getDocTimes(SelectedDocNames.docID, SelectedDocNames.docTimeId, DateDoctorAcepting);
                //    OneTimeDoctorTimes = DoctorTimes;
                //}
                //catch { }
                //  localDB.save2("473", "SUG+", "AL+", "Inf+");
            }
            else
            {
                MessageBox.Show("Проблеми з інтернет з'єднанням.\nСпробуйте будь-ласка пізніше.");
            }

        }

        #endregion
        #region Public Variables


        public DoctorsList SelectedDoc { get; set; }
        //   public Times SelectedTime { get; set; }
        public ObservableCollection<Appointments> Appointments { get; set; }
        public List<DoctorsList> ListOfSpecf { get; set; }
        public DataTable localBD { get; set; }
        public DataTable siteBD { get; set; }
        public DataTable resultBD { get; set; }
        public ObservableCollection<DocNames> ListOfDocNames { get; set; }

        public List<Times> DoctorTimes { get; set; }
        // public ObservableCollection<Times> DoctorTimes { get; set; }
        public ObservableCollection<string> Users { get; set; }
        public ObservableCollection<DateTime> WorkingDays { get; set; }
        /// <summary>
        /// создал обект который бинтидься к дата контексту карточки в
        /// который влажываем все остальные дата контектсты и биндим их 
        ///  в тоже время все команды и функции с вложеных дата контекство вроде как работают проверил на кнопке 
        ///  **Файл CardPages строка: 218
        /// </summary>
        /// 

        private RelayCommand _printTalon;

        public RelayCommand printTalon
        {
            get
            {

                return _printTalon ??
                       (_printTalon = new RelayCommand(obj =>
                       {

                           try
                           {
                               PrintDialog printDialog = new PrintDialog();
                               FlowDocument flowDocument = new FlowDocument();
                               var appointments = obj as Appointments;


                               if (printDialog.ShowDialog() == true)
                               {


                                   Bold TalonRunBold = new Bold();
                                   Run TalonRun = new Run("Талон №: " + appointments.NumOrder);
                                   // TalonRun.FontWeight
                                   TalonRunBold.Inlines.Add(TalonRun);
                                   Paragraph p = new Paragraph();
                                   // p.Margin =  0;
                                   //p.Inlines.Add(Properties.Resources.shczrl);
                                   p.Inlines.Add(TalonRunBold);
                                   p.FontSize = 14;

                                   flowDocument.Blocks.Add(p);

                                   //System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                                   //BitmapImage bimg = new BitmapImage();
                                   //bimg.BeginInit();
                                   //bimg.UriSource = new Uri("shczrl.png", UriKind.Relative);
                                   //bimg.EndInit();
                                   //image.Source = bimg;
                                   //flowDocument.Blocks.Add(new BlockUIContainer(image));





                                   //Run TalonTextRun = new Run("\nІм'я пацієнта:\n" + CutName(appointments.Pacient.ToString()) + "\nІм'я лікаря:\n" + CutName(SelectedDocNames.docName) + "\nЧас прийому:\n" + appointments.TimeAppomination + "\nДата прийому:\n" + DateDoctorAcepting.ToShortDateString());

                                   Bold Bold1 = new Bold();
                                   Run Run1 = new Run("Ім'я пацієнта:\n");
                                   Bold1.Inlines.Add(Run1);

                                   Run Run2 = new Run(CutName(appointments.Pacient.ToString()));

                                   Bold Bold3 = new Bold();
                                   Run Run3 = new Run("\nІм'я лікаря:\n");
                                   Bold3.Inlines.Add(Run3);

                                   Run Run4 = new Run(CutName(SelectedDocNames.docName));

                                   Bold Bold5 = new Bold();
                                   Run Run5 = new Run("\nЧас прийому:\n");
                                   Bold5.Inlines.Add(Run5);

                                   Run Run6 = new Run(appointments.TimeAppomination);

                                   Bold Bold7 = new Bold();
                                   Run Run7 = new Run("\nДата прийому:\n");
                                   Bold7.Inlines.Add(Run7);

                                   Run Run8 = new Run(DateDoctorAcepting.ToShortDateString());

                                   p = new Paragraph();
                                   p.Inlines.Add(Bold1);
                                   p.Inlines.Add(Run2);
                                   p.Inlines.Add(Bold3);
                                   p.Inlines.Add(Run4);
                                   p.Inlines.Add(Bold5);
                                   p.Inlines.Add(Run6);
                                   p.Inlines.Add(Bold7);
                                   p.Inlines.Add(Run8);
                                   // p.Inlines.Add(  );

                                   p.FontSize = 12;

                                   flowDocument.Blocks.Add(p);

                                   flowDocument.PageHeight = printDialog.PrintableAreaHeight;
                                   flowDocument.PageWidth = printDialog.PrintableAreaWidth;
                                   flowDocument.PagePadding = new Thickness(0);
                                   flowDocument.Blocks.Add(p);
                                   IDocumentPaginatorSource idpSource = flowDocument;
                                   printDialog.PrintDocument(idpSource.DocumentPaginator, "Талон №" + appointments.NumOrder);
                               }

                               //printData = "\nІм'я пацієнта:\n" +  CutName(obj.ToString()) + "\nІм'я лікаря:\n" + CutName(docName) + "\nДата прийому: "+ /*+ dateTimePicker1.Text +*/ "\nЧас прийому: " /*+ comboBox2.SelectedItem.ToString()*/;
                               //printTalonString = "Талон №" ;
                               //printDialog1.ShowDialog();

                               //PrintDocument def = new PrintDocument();
                               //def.PrintPage += new PrintPageEventHandler(PRD);
                               //def.DocumentName = "Друк талону №" ;
                               //def.PrinterSettings = printDialog1..PrinterSettings;
                               //def.Print();
                               //def.Dispose();

                               flowDocument = null;
                           }
                           catch (Exception) { }



                       }));
            }

        }

        //private void CutNameHelper(string DGV, out string p,out string i ,out string b)
        //{

        //}

        public string CutName(string DGV)
        {
            string PIBout = "";
            string[] PIBprint = null;
            string pL, iL, bL;
            try
            {
                PIBprint = DGV.Split(new char[] { ' ' });
                if (PIBprint.Length <= 1)
                {
                    PIBprint = DGV.Split(new char[] { ' ' });
                }
            }
            catch { }
            try
            {
                //Encoding source = Encoding(DGV);
                //Encoding UTF8 = Encoding.UTF8;
                //byte[] encod = Encoding.Convert(source, UTF8, source.GetBytes(DGV));
                //string encoded = UTF8.GetString(encod);

                pL = "";
                iL = "";
                bL = "";

                // PIBprint = DGV.Split(new char[] { ' ' });
                if (PIBprint.Length <= 1)
                {
                    pL = PIBprint[0];

                }
                if (PIBprint.Length < 3 && PIBprint.Length > 1)
                {
                    pL = PIBprint[0];
                    iL = PIBprint[1];

                }
                if (PIBprint.Length >= 3)
                {
                    pL = PIBprint[0];
                    iL = PIBprint[1];
                    bL = PIBprint[2];


                }


            }
            catch (Exception)
            {

                // PIBprint = DGV.Split(new char[] { ' ' });

                pL = "";
                iL = "";
                bL = "";
                if (PIBprint.Length <= 1)
                {
                    pL = PIBprint[0];

                }
                if (PIBprint.Length < 3 && PIBprint.Length > 1)
                {
                    pL = PIBprint[0];
                    iL = PIBprint[1];

                }
                if (PIBprint.Length >= 3)
                {
                    pL = PIBprint[0];
                    iL = PIBprint[1];
                    bL = PIBprint[2];


                }
            }

            //   string PIBout = "";
            try
            {
                if (pL.Length + iL.Length <= 20) { PIBout = pL + " " + iL + "\n"; } else { PIBout = pL + "\n" + iL; }
                if (iL.Length + pL.Length > 20) { PIBout += "\n" + bL; } else { PIBout += bL; }
                return PIBout;
            }
            catch (Exception) { }
            // PIBout = pL + " " + iL + " " + bL;
            pL = null; iL = null; bL = null;

            return PIBout;
        }
        //void PRD(object sender, PrintPageEventArgs e)
        //{

        //    Font talonFont = new Font(FontFamily.GenericSansSerif, 14);
        //    Font mainFont = new Font(FontFamily.GenericSansSerif, 12);
        //    Graphics g = e.Graphics;
        //    g.DrawImage(Properties.Resources.shczrl, 1, 1);
        //    g.DrawString(printTalonString, mainFont, new SolidBrush(Color.Black), 0, Properties.Resources.shczrl.Height + 5);
        //    g.DrawString(printData, talonFont, new SolidBrush(Color.Black), 0, Properties.Resources.shczrl.Height + 15);
        //    g.Dispose();

        //}
        //string printData;
        //string printTalonString;

        private Appointments _SSelectedUser;
        public Appointments SSelectedUser
        {
            get
            {
                return _SSelectedUser;
            }
            set
            {
                _SSelectedUser = value;

                // MessageBox.Show(_SSelectedUser.IDUser);
                //CardPages CP = new CardPages();

                // KARTA.SUser = _SSelectedUser;
                //  MainWindow mw = new MainWindow();
                //  mw.page1.DataContext = localDB.karta(_SSelectedUser.IDUser);
                ////  MessageBox.Show(_SSelectedUser.IDUser);
                //CP.sSelectedUser = _SSelectedUser;
                //  CP.KARTA = localDB.karta(_SSelectedUser.IDUser);

                //  MainWindow MW = new MainWindow();

                //CP.KARTA= localDB.karta(_SSelectedUser.IDUser);

                //   CP._SSelectedUser = _SSelectedUser;
                //  KARTA = localDB.karta(_SSelectedUser.IDUser);
                //      CP.SSelectedUser = _SSelectedUser;
                //_SSelectedUser.IDUser;
                // MessageBox.Show(_SSelectedUser.IDUser);
            }
        }

        public string teststring { get; set; }//тесовый стринг

        /// <summary>
        /// умный поиск по комбобоксу
        /// </summary>
        public string ComboboxText
        {
            get
            {
                return comboboxtext;
            }
            set
            {

                if (value != "")
                {
                    comboboxtext = value;
                    //ComboBoxDropDown = true;
                    //IsTextSearchEnabled = false;
                    var FiltredUsers = from Users in OneTimeUsers where Users.Contains(value) || Users.Contains(value.ToUpper()) || Users.Contains(value.ToLower()) select Users;
                    ObservableCollection<string> temps = new ObservableCollection<string>();
                    foreach (var a in FiltredUsers)
                    {
                        temps.Add(a);
                    }
                    Users = temps;
                    if (!Users.Contains(value))
                    {
                        //if (value.Length > 0)
                        //{
                            SelectedUser = new Users { userFIO = value, userId = "007", userMail = "registratura@coworking.com", userPhone = "8-800-555-35-35" };
                        //}
                        //else
                        //{
                        //    SelectedUser = new Users { userFIO = "Регістратура", userId = "007", userMail = "registratura@coworking.com", userPhone = "8-800-555-35-35" };
                        //}
                    }


                }
                else
                {
                    comboboxtext = value;
                    //ComboBoxDropDown = false;
                    // IsTextSearchEnabled = false;
                    SelectedUser = new Users { userFIO = "Регістратура", userId = "007", userMail = "registratura@coworking.com", userPhone = "8-800-555-35-35" };
                    Users = OneTimeUsers;
                }
            }
        }

        /// <summary>
        /// Выбор юзера по выбраному фио из комбобокса
        /// </summary>
        public string SelectedFIO
        {
            get
            {
                return selectedFIO;
            }
            set
            {
                if (value != "")
                {
                    selectedFIO = value;
                    comboboxtext = value;
                    //Возможно нужна проверка на полное совпадение если таково бредусмотрено базой
                    foreach (var a in ListOfUsers)
                    {
                        if (a.userFIO == value)
                        {
                            SelectedUser = a;
                        }
                    }
                }
                else
                {
                    SelectedUser = new Users { userFIO = "Регістратура", userId = "007", userMail = "registratura@coworking.com", userPhone = "8-800-555-35-35" };

                }
              



            }
        }

        public bool ComboBoxDropDown { get; set; } = false;

        public bool IsTextSearchEnabled { get; set; } = false;
        /// <summary>
        /// При изменении состояния чек бокса при True-отображаеться росписнаие по талонам при false - росписание по времени
        /// </summary>
        public bool TimeHour
        {
            get { return timehour; }
            set
            {
                timehour = value;

                RefreshDocTimes();

                //}
            }
        }

        public Times SelectedTime { get; set; }//выбраное время

        public DateTime DateDoctorAcepting
        {
            get
            {
                return dateDoctorAcepting;
            }
            set
            {
                try
                {
                    dateDoctorAcepting = value;
                    int i = 0;
                    RefreshDocTimes();
                    Appointments = con.GetAppointments(SelectedDocNames.docID, value);


                }
                catch { }

            }
        }


        //       public string teststring { get; set; }
        //     public ObservableCollection<Times> DoctorTimes { get; set; }
        private DoctorsList _SelectedSpecf;
        public DoctorsList SelectedSpecf
        {
            get { return _SelectedSpecf; }
            set
            {
                _SelectedSpecf = value;

                try
                {
                    ListOfDocNames = con.GetDoctrosNames(value.idspecf.ToString());
                }
                catch
                {
                }


                WorkingDays = new ObservableCollection<DateTime>();


                DoctorTimes = new List<Times>();

            }
        }
        private DocNames _SelectedDocNames;
        public DocNames SelectedDocNames
        {
            get { return _SelectedDocNames; }
            set
            {
                _SelectedDocNames = value;
                //synhronyze.SynhronyzeAll();
                //CheckConnection();
                //подавляем екзепшены так как при выборе специальности DocNames становиться null
                try
                {
                    WorkingDays = con.GetListOfWorkingDays(Convert.ToInt32(value.docID));
                    Appointments = con.GetAppointments(SelectedDocNames.docID, DateDoctorAcepting);
                    //Otemp = con.GetListOfWorkingDays(Convert.ToInt32(value.docID));
                    if (con.CheckDoctorList(SelectedDocNames.docTimeId))
                    {
                        TimeHour = value.docBool;
                        RefreshDocTimes();
                    }
                    else
                    {
                        DoctorTimes.Clear();
                        if (SelectedDocNames != null || con.CheckDoctorList(SelectedDocNames.docTimeId))
                        {
                            MessageBox.Show("Для лікаря " + SelectedDocNames.docName + " графік прийому відсутній", "Прийом відсутній", MessageBoxButton.OK, MessageBoxImage.Information);
                            edDaysMethod();
                        }
                    }
                    //if (SelectedDocNames.docTimeId == "0" && SelectedDocNames.docTimeId == null || WorkingDays.Contains(DateDoctorAcepting)==false)


                    //TimeHour = value.docBool; // присваивать значение с статуса врача
                    // КОСТІЛЬ ПЕРЕДЕЛАТЬ ИЗМЕНИТЬ СЧИТІВАНЬЕ ЛИСТА С СПЕЦИФИКАЦИЯМИ И ВРАЧАМИ (Спросить у ИЛЬИ)

                }
                catch (Exception e)
                {
                    //розкоментить для отладки
                    //Екзепшен возникает при выборе специализации при этом SelectedDocNames становиться равный null
                    //MessageBox.Show(e.ToString());
                }
            }
        }


        #endregion

        #region Helpers object
        //conBD con = new conBD();

        #endregion



        //public regViewModel()
        //{
        //    // KARTA = new CardPageOne { Name = "aaaaaaaaaa", Sername = "bbbbbbbbbbb" };
        //    //CheckConnection();
        //     DateDoctorAcepting = DateTime.Today;
        //    ListOfSpecf = con.getList();
        //    ListOfUsers = con.GetUsers();


        //    // DateDoctorAcepting = DateTime.Parse("2017-07-07");


        //    Users = OneTimeUsers;


        //    foreach (var a in ListOfUsers)
        //    {
        //        OneTimeUsers.Add(a.userFIO);
        //    }
        //    DoctorTimes = new List<Times>();
        //    try
        //    {
        //        DoctorTimes = con.getDocTimes(SelectedDocNames.docID, SelectedDocNames.docTimeId, DateDoctorAcepting);
        //        OneTimeDoctorTimes = DoctorTimes;
        //    }
        //    catch { }
        //  //  localDB.save2("473", "SUG+", "AL+", "Inf+");

        //}


        //  public DoctorsList.DocNames sas { get; set; }
        //  WPF_Hospital.MainWindow a = new WPF_Hospital.MainWindow();


        #region Helpers method and command





        /// <summary>
        /// Метод для обновления росписания врача с проверкой на робочи/не робочий день
        /// </summary>



        public void RefreshDocTimes()
        {
            //await RefreshAsync();

            try
            {

                //if (con.CheckDoctorList(SelectedDocNames.docTimeId))
                //{
                //if (TimeHour == true)
                //{
                //    try
                //    {

                //        if (WorkingDays.Contains(DateDoctorAcepting) == true)
                //        {
                //            List<Times> temp = new List<Times>();
                //            DoctorTimes = con.getDocTimes(SelectedDocNames.docID,  DateDoctorAcepting);
                //            foreach (var a in DoctorTimes)
                //            {
                //                i++;
                //                temp.Add(new Times { Time = a.Time, Status = a.Status, Label = "Talon №" + i.ToString(), TimeProperties = a.TimeProperties });
                //            }
                //            DoctorTimes = temp;
                //        }
                //        else
                //        {
                //            DoctorTimes = new List<Times>();
                //            DoctorTimes.Add(new Times { Label = "Не робочій день", Status = "Red" });
                //        }
                //    }

                //    catch (Exception)
                //    {
                //        MessageBox.Show("Лікар не вибраний");
                //    }
                //}
                //else
                //{
                WorkingDays = con.GetListOfWorkingDays(Convert.ToInt32(_SelectedDocNames.docID));
                if (WorkingDays.Contains(DateDoctorAcepting) == true)
                {
                    DoctorTimes = con.getDocTimes(SelectedDocNames.docID, DateDoctorAcepting);
                }
                else
                {
                    DoctorTimes = new List<Times>();
                    DoctorTimes.Add(new Times { Label = "Не робочій день", Status = "Red" });
                }

                //}

            }
            catch (Exception e)
            {
                //Розкомнетить для отладки
                //MessageBox.Show(e.ToString());
            }
        }


        private RelayCommand _conf;

        public RelayCommand Conf
        {
            get
            {
                return _conf ??
                  (_conf = new RelayCommand(obj =>
                  {
                      try
                      {

                          if (SelectedUser == null)
                          {
                             
                                  SelectedUser = new Users { userFIO = "Регістратура", userId = "007", userMail = "registratura@coworking.com", userPhone = "8-800-555-35-35" };
                             
                          }
                          string mess = "Ви дійсно хочете записати на прийом пацієнта " + SelectedUser.userFIO + " до лікаря " + SelectedDocNames.docName;
                          string capt = "Запис на прийом до лікаря " + SelectedDocNames.docName;

                          if (SelectedTime.Status == "Green")
                          {
                              var result = MessageBox.Show(mess, capt, MessageBoxButton.YesNo, MessageBoxImage.Question);

                              if (result == MessageBoxResult.No) { }
                              if (result == MessageBoxResult.Yes)
                              {
                                  //запись на прием хере!

                                  string temp1 = ComboboxText;
                                  string[] temp = SelectedTime.Time.Split(new char[] { ':' });
                                  

                                  con.INsertTheApointment(SelectedUser.userId, Convert.ToInt32(SelectedDocNames.docID), SelectedUser.userFIO, SelectedUser.userPhone, SelectedUser.userMail,
                                      SelectedSpecf.specf, SelectedDocNames.docName, SelectedDocNames.docEmail, DateDoctorAcepting, temp[0], temp[1], SelectedDocNames.docCab);
                                  Appointments = con.GetAppointments(SelectedDocNames.docID, DateDoctorAcepting);
                                  DoctorTimes = con.getDocTimes(SelectedDocNames.docID, DateDoctorAcepting);
                                  OneTimeDoctorTimes = DoctorTimes;
                                  //SelectedUser = new Users();


                              }

                          }
                          //Appointments = con.GetAppointments(SelectedDoctor.docID, DateDoctorAcepting);
                          // teststring = SelectedDoctor.docID;
                          else
                          {
                              MessageBox.Show("Час зайнято");
                          }
                      }
                      catch (Exception e)
                      {
                         //   MessageBox.Show(e.ToString());
                          MessageBox.Show("Перевірте правильність введення данних:\nВибране ПІБ пацієнта\nВибрані дата та час прийому", "Неправильні дані",MessageBoxButton.OK,MessageBoxImage.Hand);
                      }

                  }));
            }
        }




        string S_LastName { get; set; }
        string S_FirstName { get; set; }
        DateTime S_DateBorn { get; set; }
        private RelayCommand _SearchUsers;
        public RelayCommand SearchUsers
        {
            get
            {
                return _SearchUsers ??
                  (_SearchUsers = new RelayCommand(obj =>
                  {

                      // localDB.karta(S_FirstName, S_LastName, S_DateBorn);


                      //SearchUsersCard SearchView = new SearchUsersCard();
                      //selectedSearchVM sSVM = new selectedSearchVM();


                      //SearchView.DataContext = sSVM;
                      //// sSVM = SelectedDocNames;

                      //ObservableCollection<Times> BackUPdocTimes = new ObservableCollection<Times>(); // не менять на лист, ибо не будет обновлятся вью расписания



                      //try { SearchView.ShowDialog(); }
                      //catch { }




                  }));
            }
        }
        public void edDaysMethod()
        {
            editDays daysEditing = new editDays();

            if (SelectedDocNames != null)
            {
                EditDayViewModel VMEditDays = new EditDayViewModel(SelectedDocNames);
                daysEditing.DataContext = VMEditDays;



                try
                {
                    daysEditing.ShowDialog();
                    RefreshDocTimes();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Лікар не вибраний", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        private RelayCommand _editDays;
        public RelayCommand editDays
        {
            get
            {
                return _editDays ??
                  (_editDays = new RelayCommand(obj =>
                  {
                      try
                      {
                          edDaysMethod();
                      }
                      catch
                      {

                      }


                  }));
            }
        }

        public void confUserMethod()
        {
            ConfirmUser _ConfirmUser = new ConfirmUser();


           
            _ConfirmUser.DataContext = new ConfirmUserViewModel();



            try

            {
                _ConfirmUser.ShowDialog();
                Users.Clear();
                ListOfUsers = con.GetUsers();
                Users = OneTimeUsers;
                foreach (var a in ListOfUsers)
                {
                    OneTimeUsers.Add(a.userFIO);
                }
            }
            catch { }

        }

        private RelayCommand _confUser;
        public RelayCommand confUser
        {
            get
            {
                return _confUser ??
                       (_confUser = new RelayCommand(obj =>
                       {
                           try
                           {
                               confUserMethod();
                               
                           }
                           catch
                           {

                           }


                       }));
            }
        }


        /// <summary>
        /// команда на биндинг события для изменения сосотояния чекбокса с ЗАНЕСЕНИЕМ ИЗМЕЕНИЙ В БАЗУ
        /// </summary>
        private RelayCommand _CheckBoxChenged;
        public RelayCommand CheckBoxChenged
        {
            get
            {
                return _CheckBoxChenged ??
                  (_CheckBoxChenged = new RelayCommand(obj =>
                  {

                      //SelectedDocNames.docBool = TimeHour;                   
                      // ListOfDocNames = con.GetDoctrosNames(SelectedSpecf.idspecf.ToString());
                      try
                      {
                          con.InsertTalonTime(Convert.ToInt32(SelectedDocNames.docID), TimeHour);

                      }
                      catch
                      {

                      }




                  }));
            }
        }



        #endregion


        //}
        #region ICommand region for Selected Item in treView
        //   private static object _selectedItem = null;
        // This is public get-only here but you could implement a public setter which
        // also selects the item.
        // Also this should be moved to an instance property on a VM for the whole tree, 
        // otherwise there will be conflicts for more than one tree.

        //private ICommand _selectedItemChangedCommand;
        //public ICommand SelectedItemChangedCommand
        //{
        //    get
        //    {
        //        DoctorsList.DocNames SelectedDoctor = new DoctorsList.DocNames();
        //        if (_selectedItemChangedCommand == null)
        //            teststring = SelectedDoctor.docID;
        //        _selectedItemChangedCommand = new RelayCommand(args => SelectedItemChanged(args));

        //        return _selectedItemChangedCommand;
        //    }
        //}
        public string docIdBackup { get => docIdPrivate; set { } }
        private string docIdPrivate;
        public void docIdBehind(DocNames s)
        {
            docIdBackup = s.docID;
            docIdPrivate = s.docID;

            // return null;
        }

        //public void SelectedItemChanged(object args)

        //{


        //    SelectedItem = args;
        //    try
        //    {

        //        DateDoctorAcepting = DateTime.Today;
        //        DoctorsList.DocNames SelectedDoctor = new DoctorsList.DocNames();
        //        SelectedDoctor = (DoctorsList.DocNames)args;
        //        //    Appointments = con.GetAppointments(SelectedDoctor.docID, DateTime.Today);
        //        //IDLik = SelectedDoctor.docID;

        //        teststring = SelectedDoctor.docID;
        //        //  docIdBackup = (string)SelectedItem;
        //        // docIdBackup = teststring;
        //    }
        //    catch { }



        //}

        //public object SelectedItem
        //{
        //    get { return _selectedItem; }
        //    private set
        //    {
        //        if (_selectedItem != value)
        //        {
        //            //  DoctorTimes = con.getDocTimes( , DateDoctorAcepting);
        //            _selectedItem = value;
        //        }

        //    }
        //}


        //private bool _isSelected;
        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set
        //    {
        //        if (_isSelected != value)
        //        {
        //            _isSelected = value;
        //            this.OnPropertyChanged("IsSelected");
        //            if (_isSelected)
        //            {
        //                SelectedItem = this;
        //            }
        //        }
        //    }
        //}
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

