using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;

namespace AppFotosMovil.ViewModels
{
    public partial class FotoViewModel : ObservableObject
    {
        [ObservableProperty]
        private ImageSource? _imagenPreview;

        [ObservableProperty]
        private string _mensajeEstado = "Selecciona una foto para comenzar";

        private FileResult? _fotoSeleccionada;

        [RelayCommand]
        private async Task SeleccionarFotoAsync()
        {
            try
            {
                // Abre el selector de archivos / galería del dispositivo
                _fotoSeleccionada = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Selecciona una imagen"
                });

                if (_fotoSeleccionada != null)
                {
                    var stream = await _fotoSeleccionada.OpenReadAsync();
                    ImagenPreview = ImageSource.FromStream(() => stream);
                    MensajeEstado = "Imagen lista para subir";
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Error al abrir galería: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task SubirFotoAsync()
        {
            if (_fotoSeleccionada == null)
            {
                MensajeEstado = "Primero debes seleccionar una foto.";
                return;
            }

            try
            {
                using var stream = await _fotoSeleccionada.OpenReadAsync();
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(stream), "file", _fotoSeleccionada.FileName);

                // URL con tu IP local exacta y el puerto de la API
                var url = "http://192.168.137.147:5188/api/fotos/upload";

                using var client = new HttpClient();
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    MensajeEstado = "¡Fotografía guardada con éxito en SQL Server!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MensajeEstado = $"Error [{(int)response.StatusCode}]: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                MensajeEstado = $"Fallo de red: {ex.Message}";
            }
        }
    }
}