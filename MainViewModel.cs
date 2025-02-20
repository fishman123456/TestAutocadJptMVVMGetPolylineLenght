﻿using System;
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
using System.Collections.ObjectModel;

namespace TestAutocadJptMVVMGetPolylineLenght
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // длина полилинии
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
        // слой  полилинии

        private string _layerName;
        public string LayerName
        {
            get { return _layerName; }
            set
            {
                _layerName = value;
                OnPropertyChanged(nameof(LayerName));
            }
        }
        
        public ICommand GetLineLengthCommand => new RelayCommand(GetLineLength);

        public List<string> PoliList;
        public void GetLineLength()
        {
            
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
           PoliList = new List<string>();
            // палитра автокад
            //Application.activedocument.addPalette("Sample Palette", "C:/capture.html");
            
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
                    var ent = tr.GetObject(id, OpenMode.ForWrite) as Polyline3d;
                    if (ent != null )
                    {
                        PoliList.Add(ent.Length.ToString());
                        LineLength += ent.Length;
                        LayerName = ent.Layer.ToString();
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
