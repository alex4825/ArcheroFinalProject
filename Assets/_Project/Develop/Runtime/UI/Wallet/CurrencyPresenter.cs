using Assets._Project.Develop.Runtime.Configs.Meta;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.UI.Wallet
{
    public class CurrencyPresenter
    {
        //Бизнес логика
        private readonly IReadonlyVariable<int> _currency;
        private readonly CurrencyTypes _currencyType;
        private readonly CurrencyIconsConfig _currencyIconsConfig;

        //Визуал
        private IconTextView _view;

        private IDisposable _disposable;

        public CurrencyPresenter(IReadonlyVariable<int> currency, CurrencyTypes currencyType, CurrencyIconsConfig currencyIconsConfig, IconTextView view)
        {
            _currency = currency;
            _currencyType = currencyType;
            _currencyIconsConfig = currencyIconsConfig;
            _view = view;
        }

        public IconTextView View => _view;

        public void Enable()
        {
            UpdateValue(_currency.Value);
            _view.SetIcon(_currencyIconsConfig.GetSpriteFor(_currencyType));

            _disposable = _currency.Subscribe(OnCurrencyChanged);
        }

        public void Disable()
        {
            _disposable.Dispose();
        }

        private void OnCurrencyChanged(int oldValue, int newValue) => UpdateValue(newValue);

        private void UpdateValue(int newValue) => _view.SetText(newValue.ToString());
    }
}
