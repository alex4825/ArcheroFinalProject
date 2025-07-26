using Assets._Project.Develop.Runtime.UI.Core;
using System;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _screen;

        public MainMenuScreenPresenter(MainMenuScreenView screen)
        {
            _screen = screen;
        }

        public void Dispose()
        {

        }

        public void Initialize()
        {

        }
    }
}
