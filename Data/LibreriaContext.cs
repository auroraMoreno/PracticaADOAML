using PracticaADOAML.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaADOAML.Data
{
    #region CREAR LIBROS
    //CREAR LIBROS
    //CREATE PROCEDURE CREARLIBRO
    //(@IDLIBRO INT,
    //@TITULO NVARCHAR(30), 
    //@AUTOR NVARCHAR(30), 
    //@SINOPSIS NVARCHAR(40),
    //@IMAGEN NVARCHAR(30), 
    //@IDGENERO INT)
    //AS
    //INSERT INTO Libros VALUES(
    //@IDLIBRO,
    //@TITULO,
    //@AUTOR,
    //@SINOPSIS,
    //@IMAGEN,
    //@IDGENERO)
    //GO
    #endregion 

    #region CREAR GENEROS
    //CREAR GENEROS
    //    CREATE PROCEDURE CREARGENERO
    //(@IDGENERO INT, @GENERO NVARCHAR(30))
    //AS
    //INSERT INTO Generos VALUES(@IDGENERO, @GENERO)
    //GO


    #endregion
    #region UPDATE LIBRO
    //CREATE PROCEDURE UPDATELIBRO
    //(@IDLIBRO INT,
    //@TITULO NVARCHAR(30), 
    //@AUTOR NVARCHAR(30), 
    //@SINOPSIS NVARCHAR(40),
    //@IMAGEN NVARCHAR(30), 
    //@IDGENERO INT)
    //AS
    //UPDATE Libros SET
    //Titulo = @TITULO, 
    //Autor=@AUTOR, 
    //Sinopsis= @SINOPSIS,
    //Imagen = @IMAGEN,
    //IdGenero= @IDGENERO
    //WHERE IdLibro = @IDLIBRO
    //GO

    #endregion
    public class LibreriaContext
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        SqlDataAdapter adLibros;
        DataTable tablaLibros;
        SqlDataAdapter adGeneros;
        DataTable tablaGeneros;

        public LibreriaContext()
        {
            String cadena = @"Data Source=LAPTOP-KR2NL673\SQLAURORAMASTER;Initial Catalog=LIBRERIA;User ID=sa;Password=MCSD2020";
            this.cn = new SqlConnection(cadena);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            //libros
            this.tablaLibros = new DataTable();
            this.adLibros = new SqlDataAdapter("select * from libros", cadena);
            this.adLibros.Fill(this.tablaLibros);
            //generos
            this.tablaGeneros = new DataTable();
            this.adGeneros = new SqlDataAdapter("select * from generos", cadena);
            this.adGeneros.Fill(this.tablaGeneros);
        }

        //LIBRO:
        public List<Libro> GetLibros()
        {
            var consulta = from datos in this.tablaLibros.AsEnumerable() select datos;
            //var consulta2 = from librosDatos in this.tablaLibros.AsEnumerable()
            //                from generos in this.tablaGeneros.AsEnumerable()
            //                select librosDatos;

            List<Libro> libros = new List<Libro>();
            foreach(var l in consulta)
            {
                Libro libro = new Libro();
                libro.IdLibro = l.Field<int>("IdLibro");
                libro.Titulo = l.Field<String>("Titulo");
                libro.Autor = l.Field<String>("Autor");
                libro.Sinopsis = l.Field<String>("Sinopsis");
                libro.Imagen = l.Field<String>("Imagen");
                libro.IdGenero = l.Field<int>("IdGenero");
                libros.Add(libro);
            }
            return libros;
        }

        public Libro GetLibro(int idLibro)
        {
            var consulta = from datos in this.tablaLibros.AsEnumerable()
                           where datos.Field<int>("IdLibro") == idLibro
                           select datos; 

            var row = consulta.First();

            Libro libro = new Libro();
            libro.IdLibro = row.Field<int>("IdLibro");
            libro.Titulo = row.Field<String>("Titulo");
            libro.Autor = row.Field<String>("Autor");
            libro.Sinopsis = row.Field<String>("Sinopsis");
            libro.Imagen = row.Field<String>("Imagen");
            libro.IdGenero = row.Field<int>("IdGenero");

            return libro;

        }

        public int IdMaxLibro()
        {
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "select max(idLibro) + 1 as IdMax from Libros";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            int id = Convert.ToInt32(this.reader["IdMax"]);
            this.reader.Close();
            this.cn.Close();
            return id;
        }

        public int CrearLibro(String titulo, String autor, String sinopsis, String imagen, int idgenero)
        {
            int id = IdMaxLibro();
            this.com.Parameters.AddWithValue("@IDLIBRO", id);
            this.com.Parameters.AddWithValue("@TITULO", titulo);
            this.com.Parameters.AddWithValue("@AUTOR",autor);
            this.com.Parameters.AddWithValue("@SINOPSIS",sinopsis);
            this.com.Parameters.AddWithValue("@IMAGEN",imagen);
            this.com.Parameters.AddWithValue("@IDGENERO", idgenero);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "CREARLIBRO";
            this.cn.Open();
            int insertados = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return insertados;
        }


        public int UpdateLibro(Libro l)
        {
            this.com.Parameters.AddWithValue("@IDLIBRO", l.IdLibro);
            this.com.Parameters.AddWithValue("@TITULO", l.Titulo);
            this.com.Parameters.AddWithValue("@AUTOR", l.Autor);
            this.com.Parameters.AddWithValue("@SINOPSIS", l.Sinopsis);
            this.com.Parameters.AddWithValue("@IMAGEN", l.Imagen);
            this.com.Parameters.AddWithValue("@IDGENERO", l.IdGenero);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "UPDATELIBRO";
            this.cn.Open();
            int afectados = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return afectados;
        }

        //GENERO: 

        public List<Genero> GetGeneros()
        {
            var consulta = from datos in this.tablaGeneros.AsEnumerable() select datos;
            List<Genero> generos = new List<Genero>();
            foreach(var g in consulta)
            {
                Genero genero = new Genero();
                genero.IdGenero = g.Field<int>("IdGenero");
                genero.GeneroNombre = g.Field<String>("Genero");
                generos.Add(genero);
            }
            return generos;
        }

        public Genero GetGenero(int idGenero)
        {
            var consulta = from datos in this.tablaGeneros.AsEnumerable()
                           where datos.Field<int>("IdGenero") == idGenero
                           select datos;
            var row = consulta.First();
            Genero genero = new Genero();
            genero.IdGenero = row.Field<int>("IdGenero");
            genero.GeneroNombre = row.Field<String>("Genero");
            return genero;
        }

        public List<Libro> BuscarLibrosGenero(int idGenero)
        {
            var consulta = from datos in this.tablaLibros.AsEnumerable()
                           where datos.Field<int>("IdGenero") == idGenero
                           select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Libro> libros = new List<Libro>();
                foreach (var l in consulta)
                {
                    Libro libro = new Libro();
                    libro.IdLibro = l.Field<int>("IdLibro");
                    libro.Titulo = l.Field<String>("Titulo");
                    libro.Autor = l.Field<String>("Autor");
                    libro.Sinopsis = l.Field<String>("Sinopsis");
                    libro.Imagen = l.Field<String>("Imagen");
                    libro.IdGenero = l.Field<int>("IdGenero");
                    libros.Add(libro);
                }
                return libros;
            }
        }

        public int GenerarIdGenero()
        {
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "select max(IdGenero) + 1 as IdMaxGenero from Generos";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            int id = Convert.ToInt32(this.reader["IdMaxGenero"]);
            this.reader.Close();
            this.cn.Close();
            return id;
        }
        public int CrearGenero(String genero)
        {
            int idMaxGenero = GenerarIdGenero();
            this.com.Parameters.AddWithValue("@IDGENERO", idMaxGenero);
            this.com.Parameters.AddWithValue("@GENERO", genero);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "CREARGENERO";
            this.cn.Open();
            int insertados = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return insertados;
        }

        public int EliminarGenero(int idGenero)
        {
            this.com.Parameters.AddWithValue("@idGenero", idGenero);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "delete from Generos where IdGenero=@idGenero";
            this.cn.Open();
            int afectados = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return afectados;
        }
    }
}
