using MvvmCross.Platform;
using System.Collections;
using Xamarin.Forms;

namespace TrackMe.Controls
{
    public class ExtendedPicker : Picker
    {
        public ExtendedPicker()
        {
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<ExtendedPicker, IList>(o => o.ItemsSource, default(IList),
                propertyChanged: OnItemsSourceChanged);

        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create<ExtendedPicker, object>(o => o.SelectedItem, default(object), propertyChanged: OnSelectedItemChanged);

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = bindable as ExtendedPicker;
            if (newValue != null && picker!=null)
            {
                picker.SelectedIndex = picker.Items.IndexOf(newValue.ToString());
            }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList) GetValue(ItemsSourceProperty); }
            set {  SetValue(ItemsSourceProperty, value);}
        }

        public string DisplayMember { get; set; }

        private static void OnItemsSourceChanged(BindableObject bindable, IList oldvalue, IList newvalue)
        {
            var picker = bindable as ExtendedPicker;

            if (picker != null)
            {
                picker.Items.Clear();
                if (newvalue == null) return;foreach (var item in newvalue)
                {
                    if (string.IsNullOrEmpty(picker.DisplayMember))
                    {
                        picker.Items.Add(item.ToString());
                    }
                    else
                    {
                        var type = item.GetType();
                        var prop = type.GetProperty(picker.DisplayMember);
                        picker.Items.Add(prop.GetValue(item).ToString());
                    }
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, System.EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = ItemsSource[SelectedIndex];
            }
        }

        // temporarty property till https://bugzilla.xamarin.com/show_bug.cgi?id=36479 is fixed
        public bool IsEnabledWp
        {
            get
            {
                return (bool)this.GetValue(IsEnabledWpProperty);
            }
            set
            {
                this.SetValue(IsEnabledWpProperty, value);
            }
        }

        public static readonly BindableProperty IsEnabledWpProperty = BindableProperty.Create("IsEnabledWp", typeof(bool), typeof(ExtendedPicker), true);
    }
}
