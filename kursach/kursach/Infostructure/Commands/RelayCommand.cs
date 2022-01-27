using kursach.Infostructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace kursach.Infostructure.Commands
{
    internal class RelayCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public RelayCommand(Action<object> Execute, Func<object,bool> CanExecute = null)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if(_CanExecute != null)
            {
                return _CanExecute(parameter);
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            if(_Execute != null)
            {
                _Execute(parameter);
            }
        }
    }
}
