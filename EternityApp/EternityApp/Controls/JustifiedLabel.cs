using System;
using Xamarin.Forms;

namespace EternityApp.Controls
{
    public class JustifiedLabel : Label
    {
        public static readonly BindableProperty JustifyTextProperty =
            BindableProperty.Create(
                propertyName: nameof(JustifyText),
                returnType: typeof(Boolean),
                declaringType: typeof(JustifiedLabel),
                defaultValue: false,
                defaultBindingMode: BindingMode.OneWay
         );

        public bool JustifyText
        {
            get { return (Boolean)GetValue(JustifyTextProperty); }
            set { SetValue(JustifyTextProperty, value); }
        }
    }
}
