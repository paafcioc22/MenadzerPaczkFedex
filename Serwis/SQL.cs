using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MenadżerPaczek.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Collections;
using System.Reflection;

namespace MenadżerPaczek.Serwis
{
    public class SQL : ConnToBase
    {
        private ObservableCollection<MM> listaDoZatwierdzenia;
        private List<Paczki> listaPaczek;

        public ObservableCollection<MM> GetListaMM()
        {
            listaDoZatwierdzenia = new ObservableCollection<MM>();
            var querystring = $@" 
              select   trn_numerpelny, trn_numernr, cast(TrN_DataDok as date)DataDok  , zr.mag_symbol, trn_opis,dcl.Mag_GIDNumer magdcl, zr.Mag_GIDNumer magzrd
              from cdn.tranag
	            join cdn.magazyny dcl on dcl.mag_magid=TrN_MagDocId 
	            join cdn.magazyny zr on zr.mag_magid=TrN_MagZrdId 
              where TrN_TypDokumentu=312 and TrN_DataDok>'20190910'
              and trn_bufor=0 and trn_magzrdid=1 ";//and trn_Stan = 1

            using (SqlConnection connection = new SqlConnection(sqlconn))
            {
                connection.Open();
                using (SqlCommand command2 = new SqlCommand(querystring, connection))
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaDoZatwierdzenia.Add(
                            new MM
                            {
                                Trn_DokumentObcy = Convert.ToString(reader["trn_numerpelny"]),
                                Mag_Kod = reader["mag_symbol"].ToString(),
                                Data = reader["DataDok"].ToString(),
                                Opis = reader["trn_opis"].ToString(),
                                Trn_Numer = reader["trn_numernr"].ToString(),
                                magdcl = Convert.ToInt32(reader["magdcl"]),
                                magzrd = Convert.ToInt32(reader["magzrd"]),
                                

                            });
                    }

                }

            }
            return listaDoZatwierdzenia;
        }



        public List<T> GetListaZlecen<T>() where T : new()
        {
            ArrayList rowList = new ArrayList();
            List<T> res = new List<T>();

            var querystring = $@"  
                    select  fmm_gidnumer, count(distinct fmm_elenumer)liczbapaczek, fmm_nrlistu, max(fmm_datazlecenia) datautwrz
                    from cdn.pc_fedexmm
                    group by fmm_gidnumer,fmm_nrlistu ";//and trn_Stan = 1

            using (SqlConnection connection = new SqlConnection(sqlconnXL))
            {
                connection.Open();
                using (SqlCommand command2 = new SqlCommand(querystring, connection))
                using (SqlDataReader r = command2.ExecuteReader())
                {
                    //while (reader.Read())
                    //{
                    //    object[] values = new object[reader.FieldCount];
                    //    reader.GetValues(values);
                    //    rowList.Add(values);
                    //}
                    object[] oo;
                    //DataTable dt = rs.GetSchemaTable();

                    while (r.Read())
                    {
                        int index = 0;
                        T t = new T();

                        //foreach (DataColumn col in dt.Columns)
                        //{
                        //    t.GetType().GetProperty(col.ColumnName).SetValue(t, rs.GetString(index++), null);
                        //}


                        for (int inc = 0; inc < r.FieldCount; inc++)
                        {
                            Type type = t.GetType();
                            PropertyInfo prop = type.GetProperty(r.GetName(inc));
                            prop.SetValue(t, Convert.ChangeType(r.GetValue(inc), prop.PropertyType), null);
                        }


                        res.Add(t);
                    };

                     

                }

            }
            return res;
        }


        public List<Paczki> GetListaPaczek(int v)
        {

            listaPaczek = new List<Paczki>();
            var querystring = $@" 
              select    * from cdn.pc_fedexmm where fmm_gidnumer={v}";//and trn_Stan = 1

            using (SqlConnection connection = new SqlConnection(sqlconnXL))
            {
                connection.Open();
                using (SqlCommand command2 = new SqlCommand(querystring, connection))
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaPaczek.Add(
                            new Paczki
                            {
                                Fmm_GidNumer = Convert.ToString(reader["Fmm_GidNumer"]),
                                Fmm_EleNumer = Convert.ToString(reader["Fmm_EleNumer"]),
                                Fmm_NrListu = reader["Fmm_NrListu"].ToString(),
                                Fmm_NrPaczki = reader["Fmm_NrPaczki"].ToString(),
                                Fmm_NazwaPaczki = reader["Fmm_NazwaPaczki"].ToString(),
                                Fmm_Elmenty = reader["Fmm_Elmenty"].ToString(),
                                Fmm_DataZlecenia = reader["Fmm_DataZlecenia"].ToString(),
                                Fmm_MagDcl = reader["Fmm_MagDcl"].ToString(),

                            });
                    }

                }

            }
            return listaPaczek;
        }

        public int GetLastGidNUmer()
        {
            var query = $@" select IsNull(MAX(fmm_gidnumer), 0)+1 lastgid from cdn.pc_fedexmm";


            int lastGid = 0;


            using (SqlConnection connection = new SqlConnection(sqlconnXL))
            {
                connection.Open();
                using (SqlCommand command2 = new SqlCommand(query, connection))
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastGid = Convert.ToInt32(reader["lastgid"]);
                    }
                }
            }

            return lastGid;
        }


        public bool DodajPaczke2Sql(ObservableCollection<MM> checkMMToPaczkaList, int gidnr)
        {
            bool wykonano = false;
            if (checkMMToPaczkaList.Count > 0)
            {

                var query = $@"declare @id int  = (select IsNull(MAX(Fmm_EleNumer), 0) from cdn.pc_fedexmm
                            where fmm_gidnumer={gidnr})+1
                    insert into cdn.pc_fedexmm values " + Environment.NewLine;

                string insert = "";

                foreach (var i in checkMMToPaczkaList)
                {
                    insert += $"({gidnr},@id,'','','{checkMMToPaczkaList[0].Trn_DokumentObcy}','{i.Trn_Numer}',getdate(),'ms'),";//+ Environment.NewLine;
                }
                query += insert.Substring(0, insert.Length - 1);

                using (SqlConnection connection = new SqlConnection(sqlconnXL))
                {
                    connection.Open();
                    using (SqlCommand command2 = new SqlCommand(query, connection))
                    {
                        command2.ExecuteNonQuery();
                        wykonano = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie dodano elementów do paczki");
            }
            return wykonano;
        }


    }
}
