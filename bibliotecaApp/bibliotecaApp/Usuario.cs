using System;
using System.Collections.Generic;
using System.Text;

namespace bibliotecaApp
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Username { get; set; }
        public string Contrasena { get; set; }
        public Boolean Es_admin { get; set; }
    }
}
