using ExamenP3HerreraDilan.ViewModels;

namespace ExamenP3HerreraDilan.Views
{
    public partial class PaginaBusquedaDHerrera : ContentPage
    {
        public PaginaBusquedaDHerrera()
        {
            InitializeComponent();
            BindingContext = new BusquedaViewModelDHerrera();
        }
    }
}
