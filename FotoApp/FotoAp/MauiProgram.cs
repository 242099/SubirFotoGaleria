using Microsoft.Maui.Media;

namespace FotoAp
{
    public partial class MainPage : ContentPage
    {
        private FileResult _photo;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnTakeCaptureClicked(object sender, EventArgs e)
        {
            try
            {
                // Capturar foto usando la cámara del dispositivo
                _photo = await MediaPicker.CapturePhotoAsync();

                if (_photo != null)
                {
                    // Mostrar la imagen en la interfaz de usuario de forma local
                    var stream = await _photo.OpenReadAsync();
                    capturedImage.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo capturar la foto: " + ex.Message, "OK");
            }
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            if (_photo == null)
            {
                await DisplayAlert("Advertencia", "Primero debes tomar una fotografía.", "OK");
                return;
            }

            try
            {
                using var content = new MultipartFormDataContent();
                using var stream = await _photo.OpenReadAsync();

                content.Add(new StreamContent(stream), "file", _photo.FileName);

                // IMPORTANTE: Si usas emulador Android, 'localhost' es 10.0.2.2. Ajusta el puerto según tu API.
                string url = "http://10.0.2.2:5000/api/fotografias/upload";

                using var client = new HttpClient();
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Fotografía guardada correctamente en SQL Server.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Fallo al subir la imagen al servidor.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Excepción", ex.Message, "OK");
            }
        }
    }
}