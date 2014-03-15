
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace FlattyTweet.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider((ServiceLocatorProvider)(() => (IServiceLocator)SimpleIoc.Default));
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public static void Cleanup()
        {
        }
    }
}
