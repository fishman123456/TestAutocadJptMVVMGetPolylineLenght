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
        private List<string> _polyLenght;


        public List<string> polyLenght
        {
            get { return _polyLenght; }
            set
            {
                _polyLenght = value;
                OnPropertyChanged(nameof(polyLenght));
            }
        }

        public ICommand GetLineLengthCommand => new RelayCommand(GetLineLength);

        
        public void GetLineLength()
        {
            
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            List<string> polyLenght = new List<string>();

            PromptSelectionOptions opts = new PromptSelectionOptions();
            opts.MessageForAdding = "Выберите обьекты: ";
            PromptSelectionResult res = ed.GetSelection(opts);
            // Do nothing if selection is unsuccessful

            if (res.Status != PromptStatus.OK)
                return ;
            SelectionSet selSet = res.Value;
            // добавляем в массив выбранные обьекты
            ObjectId[] idArray = selSet.GetObjectIds();

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId id in idArray)
                {
                    doc.LockDocument();
                    var ent = tr.GetObject(id, OpenMode.ForWrite) as Polyline;
                    if (ent != null )
                    {
                        polyLenght.Add(ent.Length.ToString());
                        LineLength += ent.Length;
                    }
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
