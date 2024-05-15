using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAutocadJptMVVMGetPolylineLenght
{
    public class Model
    {
        [CommandMethod("U84JPTLenght")]
        public void ModelStart()
        {
            WindowUser window = new WindowUser();
            window.Show();
        }
    }
}
