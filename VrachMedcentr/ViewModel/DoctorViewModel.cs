using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace VrachMedcentr
{
    class DoctorViewModel
    {
        #region Helpers object

        conBD con = new conBD();

        #endregion
        #region Private Variables

        private DocNames _SelectedDoc;

        #endregion

        #region Publick Variables

        public DocNames SelectedDoc
        {
            get
            {
                return _SelectedDoc;
            }

            set
            {
                _SelectedDoc = value;
                ToDayAppoinments = con.GetAppointments(SelectedDoc.docID, new DateTime(2017, 7, 6));
            }
        
        }
        public ObservableCollection<Appointments> ToDayAppoinments { get; set; }


        #endregion

        #region Constructor

        public DoctorViewModel()
        {
            //ToDayAppoinments = con.GetAppointments(SelectedDoc.docID, new DateTime(2017, 7, 6));
        }

        #endregion

    }
}