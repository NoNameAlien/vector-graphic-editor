using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace VecEditor.App.ViewModels
{
    public partial class MainWindowViewModel
    {
        public void Undo()
        {
            if (historyManager.CheckNotEmpty())
            {
                PrimitiveObjects.Clear();

                foreach (var obj in (historyManager.getPrewState()).PrimitiveObjects)
                {
                    PrimitiveObjects.Add(obj);
                }
                this.RaisePropertyChanged(nameof(PrimitiveObjects));
            }
        }
        public void Redo() 
        {
            if (historyManager.CheckNotEmpty())
            {
                PrimitiveObjects.Clear();

                foreach (var obj in (historyManager.getNextState()).PrimitiveObjects)
                {
                    PrimitiveObjects.Add(obj);
                }
                this.RaisePropertyChanged(nameof(PrimitiveObjects));
            }
        }
    }
}
