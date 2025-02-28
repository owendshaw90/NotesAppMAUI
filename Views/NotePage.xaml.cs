namespace Notes.Views;

using Notes.ViewModels;
public partial class NotePage : ContentPage
{
	public NotePage()
	{
		this.BindingContext = new NoteViewModel();
		InitializeComponent();
	}
}