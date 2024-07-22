using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RunWebAppGroup.Helpers;
using RunWebAppGroup.Interfaces;

namespace RunWebAppGroup.Services
{
    public class PhotoService : IPhotoService
    {

        //Declara un campo privado y de solo lectura para la instancia
        //de Cloudinary.

        private readonly Cloudinary _cloudinary;


        //Define el constructor que recibe la configuración de
        //Cloudinary.
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            //Crea una cuenta de Cloudinary usando las configuraciones
            //proporcionadas.

            var acc = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret);

            // Inicializa el campo _cloudinary con una nueva instancia
            // de Cloudinary.

            _cloudinary = new Cloudinary(acc);

        }

        //Define un método asíncrono que sube una foto y devuelve el
        //resultado de la carga.
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {

            //Crea una instancia de ImageUploadResult para almacenar
            //el resultado de la carga.

            var uploadResult = new ImageUploadResult();

            //Verifica si el archivo tiene contenido.

            if (file.Length > 0)
            {

                //Abre un stream para leer el contenido del archivo.

                using (var stream = file.OpenReadStream())
                {

                    //Crea parámetros de carga, incluyendo la transformación
                    //de la imagen.

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    //Sube la imagen a Cloudinary de manera asíncrona.
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            return uploadResult;
        }

        //Define un método asíncrono que elimina una foto por su publicId.
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {

            //Crea parámetros de eliminación usando el publicId.
            var deleteParams = new DeletionParams(publicId);

            //Elimina la foto en Cloudinary de manera asíncrona
            //y almacena el resultado.

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }

    }
}