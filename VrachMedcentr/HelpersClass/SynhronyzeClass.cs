using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.ComponentModel;

namespace VrachMedcentr
{
    class SynhronyzeClass : INotifyPropertyChanged
    {
        //база сайта
        conBD conWeb = new conBD();
        //conBD conLocal = new conBD("shostka.mysql.ukraine.com.ua", "shostka_medcen", "shostka_medcen", "n5t7jzqv");
        //типа "локальная" база
        public conBD conLocal; /*= new conBD("shostka.mysql.ukraine.com.ua", "shostka_medcen", "shostka_medcen", "n5t7jzqv");*/



        public string InternetConnectionStatus { get; set; }//временное решение сделать бы визуальное оповещение
        #region Synhronyza Tables
        public string TextTest { get; set; } = "fasgagag";
        /// <summary>
        /// Функция синхронизации всех таблиц (визиваеться при старте програмы)
        /// </summary>
        public async void SynhronyzeAll()
        {
            TextTest = "Start";
            try
            {

                int a = 1;
                //await SynhronyzeTable("enx4w_ttfsp_dop", 1);
                //await SynhronyzeTable("ekfgq_ttfsp_dop", 2);
                //await SynhronyzeTable("ekfgq_users", 1);
                //await SynhronyzeTable("ekfgq_ttfsp_sprspec", 1);
                //await SynhronyzeTable("ekfgq_ttfsp_spec", 1);
               // await SynhronyzeTable("enx4w_ttfsp", 1);
                // await SynhronyzeTable("ekfgq_ttfsp", 2);

                Task.WaitAll();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                TextTest = "end";
            }

            int i = 0;

        }


        /// <summary>
        /// Функция синхронизации данных в дата тейблах базы данных
        /// </summary>
        /// <param name="_TableName">
        /// Название таблици которую хотим синхронизировать
        /// </param>
        /// <param name="_mod">
        /// 1-найти розличия между локальной и веб базами(простыми словами нахождение в веб базе того чего не хватает в локальной)
        /// 2-найти розличия между веб и локальной базами(простыми словами нахождение в локальной базе того чего не хватает в веб)
        /// </param>    
        public Task SynhronyzeTable(string _TableName, int _mod)
        {

            return Task.Run(async () =>
           {

               if (CheckConnection())
               {
                   // ТУТ МОГУТ БІТЬ ПРОБЛЕМЫ
                   string WebTableHash = await conWeb.GetTableHash(_TableName);
                   string LocalTableHash = await conLocal.GetTableHash(_TableName);


                   if (WebTableHash != LocalTableHash)
                   {

                       // ТУТ МОГУТ БІТЬ ПРОБЛЕМЫ
                       DataTable _Web = await AsyncGetTable(conWeb, _TableName);
                       DataTable _Local = await AsyncGetTable(conLocal, _TableName);

                       //Web = GetTable(conWeb);
                       //Local = GetTable(conLocal);


                       //Web = con.get3apTime();
                       //Local = conLocal.get3apTime();
                       List<DataRow> t = _Local.AsEnumerable().ToList<DataRow>();
                       List<DataRow> t1 = _Web.AsEnumerable().ToList<DataRow>();
                     //  Compare(_TableName, _mod, _Local, _Web);



                       //foreach (var a in t)
                       //{
                       //    DateTime temp = a.Field<MySqlDateTime>("date").GetDateTime();
                       //}
                       int i = 0;//маркер точки останова
                       InternetConnectionStatus = _TableName;
                   }


               }

               else
               {
                   MessageBox.Show("Проблеми з інтернет з'єднанням");
                   InternetConnectionStatus = "gsdgsdg";
               }
           });

        }

        /// <summary>
        /// Функция нахождения розличий между дата тейблами
        /// </summary>
        /// <param name="_mod">
        /// 1-найти розличия между локальной и веб базами(простыми словами нахождение в веб базе того чего не хватает в локальной)\r
        /// 2-найти розличия между веб и локальной базами(простыми словами нахождение в локальной базе того чего не хватает в веб)
        /// </param>
        private void Compare(string TableName, int _mod, DataTable Local, DataTable Web)
        {


            switch (TableName)
            {

                case "enx4w_ttfsp_dop":
                    if (_mod == 1)
                    {
                       
                        //        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                        //Any(rl => rl.Field<int>("id") == rw.Field<int>("id") && rl.Field<MySqlDateTime>("date").GetDateTime() == rw.Field<MySqlDateTime>("date").GetDateTime())).CopyToDataTable();
                        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                            Any(rl => rl.Field<string>("rfio") == rw.Field<string>("rfio")
                            && rl.Field<MySqlDateTime>("date").GetDateTime() == rw.Field<MySqlDateTime>("date").GetDateTime()
                            && rl.Field<string>("hours") == rw.Field<string>("hours")
                            && rl.Field<string>("minutes") == rw.Field<string>("minutes")
                            && rl.Field<string>("specializations_name") == rw.Field<string>("specializations_name"))).CopyToDataTable();
                        List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();
                        // conLocal.insert_enx4w_ttfsp_dop(Local);
                        Local.Clear();
                    }
                    if (_mod == 2)
                    {
                        Local = Local.AsEnumerable().Where(rw => !Web.AsEnumerable().
                            Any(rl => rl.Field<string>("rfio") == rw.Field<string>("rfio")
                            && rl.Field<MySqlDateTime>("date").GetDateTime() == rw.Field<MySqlDateTime>("date").GetDateTime()
                            && rl.Field<string>("hours") == rw.Field<string>("hours")
                            && rl.Field<string>("minutes") == rw.Field<string>("minutes")
                            && rl.Field<string>("specializations_name") == rw.Field<string>("specializations_name"))).CopyToDataTable();
                        List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();
                        //conWeb.insert_enx4w_ttfsp_dop(Local);
                        Local.Clear();
                    }
                    break;
                case "enx4w_ttfsp":
                    if (_mod == 1)
                    {
                        int i = 0;
                        //        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                        //Any(rl => rl.Field<int>("id") == rw.Field<int>("id") && rl.Field<MySqlDateTime>("date").GetDateTime() == rw.Field<MySqlDateTime>("date").GetDateTime())).CopyToDataTable();
                        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                            Any(rl => rl.Field<int>("idspec") == rw.Field<int>("idspec")
                            && rl.Field<int>("id") == rw.Field<int>("id")
                            && rl.Field<int>("iduser") == rw.Field<int>("iduser")
                            && rl.Field<MySqlDateTime>("dttime").GetDateTime() == rw.Field<MySqlDateTime>("dttime").GetDateTime()
                            && rl.Field<string>("hrtime") == rw.Field<string>("hrtime")
                            && rl.Field<string>("mntime") == rw.Field<string>("mntime"))).CopyToDataTable();
                        List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();
                        conLocal.insert_enx4w_ttfsp(Local);
                    }
                    if (_mod == 2)
                    {
                        Local = Local.AsEnumerable().Where(rw => !Web.AsEnumerable().
                            Any(rl => rl.Field<int>("idspec") == rw.Field<int>("idspec")
                            && rl.Field<int>("id") == rw.Field<int>("id")
                            && rl.Field<int>("iduser") == rw.Field<int>("iduser")
                            && rl.Field<MySqlDateTime>("dttime").GetDateTime() == rw.Field<MySqlDateTime>("dttime").GetDateTime()
                            && rl.Field<string>("hrtime") == rw.Field<string>("hrtime")
                            && rl.Field<string>("mntime") == rw.Field<string>("mntime"))).CopyToDataTable();
                    }
                    break;
                //работает косяки читай внутриinsert_enx4w_ttfsp_sprspec
                case "enx4w_ttfsp_sprspec":
                    if (_mod == 1)
                    {
                        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                            Any(rl => rl.Field<int>("id") == rw.Field<int>("id")
                            && rl.Field<string>("name") == rw.Field<string>("name")
                            && rl.Field<string>("desc") == rw.Field<string>("desc")
                            )).CopyToDataTable();
                        List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();
                        conLocal.insert_enx4w_ttfsp_sprspec(Local);
                        Local.Clear();
                    }
                    if (_mod == 2)
                    {
                        Local = Local.AsEnumerable().Where(rw => !Web.AsEnumerable().
                            Any(rl => rl.Field<string>("id") == rw.Field<string>("id")
                            && rl.Field<string>("name") == rw.Field<string>("name")
                            && rl.Field<string>("desc") == rw.Field<string>("desc"))).CopyToDataTable();
                        conWeb.insert_enx4w_ttfsp_sprspec(Local);
                        Local.Clear();
                    }
                    break;
                //где запрос лебовски?
                case "enx4w_ttfsp_spec":
                    if (_mod == 1)
                    {
                        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                            Any(rl => rl.Field<string>("idsprspec") == rw.Field<string>("idsprspec")
                            && rl.Field<string>("name") == rw.Field<string>("name"))).CopyToDataTable();
                        List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();

                        Local.Clear();
                    }
                    if (_mod == 2)
                    {
                        Local = Local.AsEnumerable().Where(rw => !Web.AsEnumerable().
                            Any(rl => rl.Field<string>("idsprspec") == rw.Field<string>("idsprspec")
                            && rl.Field<string>("name") == rw.Field<string>("name"))).CopyToDataTable();

                        Local.Clear();
                    }
                    break;
                case "enx4w_ttfsp_sprtime":
                    if (_mod == 1)
                    {
                        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                            Any(rl => rl.Field<string>("name") == rw.Field<string>("name")
                            && rl.Field<string>("timehm") == rw.Field<string>("timehm"))).CopyToDataTable();
                        //conLocal.insert_enx4w_ttfsp_sprtime(Local);
                        //Local.Clear();
                    }
                    if (_mod == 2)
                    {
                        Local = Local.AsEnumerable().Where(rw => !Web.AsEnumerable().
                            Any(rl => rl.Field<string>("name") == rw.Field<string>("name")
                            && rl.Field<string>("timehm") == rw.Field<string>("timehm"))).CopyToDataTable();
                        //conWeb.insert_enx4w_ttfsp_sprtime(Local);
                        //Local.Clear();
                    }
                    break;
                case "enx4w_users":
                    if (_mod == 1)
                    {
                        try
                        {
                            Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                                Any(rl => rl.Field<string>("name") == rw.Field<string>("name")
                                && rl.Field<string>("username") == rw.Field<string>("username"))).CopyToDataTable();
                            List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();
                            conLocal.insert_enx4w_users(Local);
                            Local.Clear();
                        }
                        catch { }
                    }
                    if (_mod == 2)
                    {
                        Local = Local.AsEnumerable().Where(rw => !Web.AsEnumerable().
                            Any(rl => rl.Field<string>("name") == rw.Field<string>("name")
                            && rl.Field<string>("username") == rw.Field<string>("username"))).CopyToDataTable();
                        List<DataRow> t0 = Local.AsEnumerable().ToList<DataRow>();
                        conLocal.insert_enx4w_users(Local);
                        Local.Clear();
                    }

                    break;

                //Нужно потестить данную таблицу
                case "talon_time":
                    if (_mod == 1)
                    {
                        Local = Web.AsEnumerable().Where(rw => !Local.AsEnumerable().
                            Any(rl => rl.Field<int>("doctor_id") == rw.Field<int>("doctor_id")
                            && rl.Field<int>("parametr") == rw.Field<int>("parametr"))).CopyToDataTable();
                    }
                    if (_mod == 2)
                    {
                        Local = Local.AsEnumerable().Where(rw => !Web.AsEnumerable().
                            Any(rl => rl.Field<int>("doctor_id") == rw.Field<int>("doctor_id")
                            && rl.Field<int>("parametr") == rw.Field<int>("parametr"))).CopyToDataTable();
                    }

                    break;



            }




        }

        /// <summary>
        /// функция проверки наличия интернет соединения
        /// </summary>
        public bool CheckConnection()
        {
            IPStatus status = IPStatus.Unknown;
            try
            {
                status = new Ping().Send("google.com.ua").Status;
            }
            catch { }

            if (status == IPStatus.Success)
            {
                return true;
                //MessageBox.Show("Сервер работает");
            }
            else
            {
                return false;
                //MessageBox.Show("Сервер временно недоступен!");
            }
        }

        /// <summary>
        /// Функция получения Таблицы
        /// </summary>
        /// <param name="_WebTablName">Имя желаемой таблицы</param>
        /// <param name="_DBConnection">Параметры подключения(База данных локальная/веб)</param>
        /// <returns></returns>
        private Task<DataTable> AsyncGetTable(conBD _DBConnection, string TableName)
        {

            return Task.Run(async () =>
            {

                MySqlConnectionStringBuilder mysqlCSB;
                mysqlCSB = new MySqlConnectionStringBuilder();
                mysqlCSB.Server = _DBConnection.server;
                mysqlCSB.Database = _DBConnection.database;
                mysqlCSB.UserID = _DBConnection.UserID;
                mysqlCSB.Password = _DBConnection.Password;
                // mysqlCSB.ConvertZeroDateTime = true;
                mysqlCSB.AllowZeroDateTime = true;

                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = mysqlCSB.ConnectionString;
                MySqlCommand cmd = new MySqlCommand();

                // MessageBox.Show(con.State.ToString());



                await con.OpenAsync();


                //MessageBox.Show(con.State.ToString());

                cmd.CommandText = "SELECT * FROM " + TableName;

                //cmd.Parameters.AddWithValue("@TableName", _WebTablName);
                cmd.Connection = con;
                // cmd.Prepare();
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();

                MySqlDataReader reader = cmd.ExecuteReader();

                dt.Load(reader);

                await con.CloseAsync();
                return dt;

            });




        }

        #endregion


        private RelayCommand _test;
        public RelayCommand Test
        {
            get
            {
                return _test ??
                  (_test = new RelayCommand(obj =>
                  {
                      SynhronyzeAll();

                      int i = 0;
                  }));
            }
        }

        #region Interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
