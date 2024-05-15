using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace TestAutocadJptMVVMGetPolylineLenght
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private double _lineLength;


        public double LineLength
        {
            get { return _lineLength; }
            set
            {
                _lineLength = value;
                OnPropertyChanged(nameof(LineLength));
            }
        }

        public ICommand GetLineLengthCommand => new RelayCommand(GetLineLength);

        
        public void GetLineLength()
        {
            
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            PromptEntityOptions opts = new PromptEntityOptions("\nSelect a line:");
            opts.SetRejectMessage("\nSelected object is not a line.");
           // opts.AddAllowedClass(typeof(Line), true);

            PromptEntityResult res = ed.GetEntity(opts);
            if (res.Status != PromptStatus.OK) return;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                var line = tr.GetObject(res.ObjectId, OpenMode.ForRead) as Polyline;
                if (line != null)
                {
                    LineLength = line.Length/1000;
                }

                tr.Commit();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
