using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API_LAB2.Models;
using Library_LAB2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_LAB2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecompressController : ControllerBase
    {
        private IHostingEnvironment _env;

        public DecompressController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("{algorithm}")]
        [HttpPost]
        public async Task<ActionResult> DecompressFile([FromForm] IFormFile file, string algorithm)
        {
            if(algorithm.Equals("huffman"))
            {
                try
                {
                    // Escribir archivo subido hacia el servidor para trabajar con ese
                    var path = _env.ContentRootPath;
                    path = Path.Combine(path, "Files");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string pathInfo = Path.Combine(path, "Info");
                    if (!Directory.Exists(pathInfo))
                    {
                        Directory.CreateDirectory(pathInfo);
                    }

                    if (System.IO.File.Exists($"{pathInfo}/archivoCargado.txt"))
                    {
                        System.IO.File.Delete($"{pathInfo}/archivoCargado.txt");
                    }

                    using var saver = new FileStream($"{pathInfo}/archivoCargado.txt", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saver);
                    saver.Close();

                    // Leer el archivo en el servidor para trabajar sobre él
                    using var fileWritten = new FileStream($"{pathInfo}/archivoCargado.txt", FileMode.OpenOrCreate);
                    using var reader = new BinaryReader(fileWritten);
                    var buffer = new byte[2000000];
                    while (fileWritten.Position < fileWritten.Length)
                    {
                        buffer = reader.ReadBytes(2000000);
                    }
                    reader.Close();
                    fileWritten.Close();

                    var nodeString = ByteGenerator.ConvertToString(buffer);

                    Huffman huff = new Huffman();
                    string cadena_descifrada = huff.Decompress(nodeString);


                    // Buscar el nombre original en el archivo Historial.txt
                    string pathHistorial = Path.Combine(pathInfo, "Historial.txt");
                    if (!System.IO.File.Exists(pathHistorial))
                    {
                        return StatusCode(500, "Internal server error");
                    }
                    string cadenaHistorial = System.IO.File.ReadAllText(pathHistorial);

                    string[] cadenas = cadenaHistorial.Split('/');
                    string nombreOriginal = "";

                    foreach (var item in cadenas)
                    {
                        if (item.Contains(file.FileName))
                        {
                            string[] valores = item.Split(';');
                            nombreOriginal = valores[0].Substring(0, valores[0].Length - 4);
                            break;
                        }
                    }

                    // Escribir el archivo {original}.txt en el servidor
                    using (var fs = new FileStream($"{path}/{nombreOriginal}.txt", FileMode.OpenOrCreate))
                    {
                        fs.Write(ByteGenerator.ConvertToBytes(cadena_descifrada), 0, cadena_descifrada.Length);
                    }

                    // Archivo a mandar de regreso
                    return PhysicalFile($"{path}/{nombreOriginal}.txt", "text/plain", $"{nombreOriginal}.txt");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            else if (algorithm.Equals("lzw"))
            {

            }
        }
    }
}