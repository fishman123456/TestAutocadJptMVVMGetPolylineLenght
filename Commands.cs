using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;

namespace TestAutocadJptMVVMGetPolylineLenght
{
    public class Commands
    {
        static PaletteSet _ps = null;

        [CommandMethod("JSP")]
        public void JavaScriptPalette()
        {
            if (_ps == null)
            {
                _ps = new PaletteSet(
                  "JavaScript Demo",
                  new Guid("61135FFD-EE9A-4A5D-B3E1-993AB17E93BD")
                );
            }

            if (_ps.Count != 0)
            {
                _ps[0].PaletteSet.Remove(0);
            }

            var p =
              _ps.Add(
                "JavaScript Test",
                new Uri(
                  //"http://mywebsite.com/files/" +
                  "https://www.keanw.com/2014/05/javascript-in-autocad-revision-clouds-using-paperjs.html"+
                  "TransientPalette.html"
                )
              );

            _ps.Visible = true;
        }
    }
}



