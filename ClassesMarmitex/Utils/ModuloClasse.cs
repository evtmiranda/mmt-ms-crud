using System;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace ClassesMarmitex
{
    public class ModuloClasse
    {
        /// <summary>
        /// Atributo utilizado para nomear os parâmetros para preenchimento dinâmico.
        /// </summary>
        /// <remarks>Izio</remarks>
        [AttributeUsage(AttributeTargets.Property)]
        public class AtributoPropriedade : Attribute
        {
            private string _NomeDoCampoNoDataReader;
            public string NomeDoCampoNoDataReader
            {
                get { return _NomeDoCampoNoDataReader; }
                set { _NomeDoCampoNoDataReader = value; }
            }

            private bool _NaoPopular;
            public bool NaoPopular
            {
                get { return _NaoPopular; }
                set { _NaoPopular = value; }
            }


        }

        /// <summary>
        /// Retorna um dicionario com as propriedades e os atributos customizados.
        /// </summary>
        /// <typeparam name="tPe">Classe que sera utilizada para obter as propriedades e atributos.</typeparam>
        /// <returns>Lista com as propriedades e os atributos customizados.</returns>
        /// <remarks>Izio</remarks>
        private Dictionary<PropertyInfo, AtributoPropriedade> ObterAtributoPropriedades<tPe>()
        {
            Dictionary<PropertyInfo, AtributoPropriedade> propriedadeAtributoClasse = new Dictionary<PropertyInfo, AtributoPropriedade>();

            foreach (var prop in typeof(tPe).GetProperties())
            {
                AtributoPropriedade atrProp = new AtributoPropriedade();

                foreach (var attr in prop.GetCustomAttributes(true))
                {
                    atrProp.NomeDoCampoNoDataReader = ((System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)attr).Name;

                    if (atrProp != null)
                    {
                        break;
                    }
                }

                //prop.Name = nomeAtributo.Name;

                propriedadeAtributoClasse.Add(prop, atrProp);
            }

            return propriedadeAtributoClasse;
        }

        /// <summary>
        /// Retorna a lista da classe parametrizada corretamente populada.
        /// </summary>
        /// <typeparam name="tPe">Classe que será retornada como lista.</typeparam>
        /// <param name="dtr">DataReader para obter os dados e preencher a lista com as classes.</param>
        /// <returns>Lista da classe tipada enviada.</returns>
        /// <remarks>Izio</remarks>
        public List<tPe> PreencheClassePorDataReader<tPe>(IDataReader dtr)
        {
            dynamic retornoFill = Activator.CreateInstance<List<tPe>>();

            while (dtr.Read())
            {
                dynamic retornoItem = Activator.CreateInstance<tPe>();

                foreach (var propAttr in ObterAtributoPropriedades<tPe>())
                {
                    var nomeColunaBuscar = propAttr.Key.Name;
                    bool naoPopularAtributo = false;

                    if (propAttr.Value != null)
                    {
                        nomeColunaBuscar = propAttr.Value.NomeDoCampoNoDataReader;
                        naoPopularAtributo = propAttr.Value.NaoPopular;
                    }

                    if (!naoPopularAtributo)
                    {
                        var propVal = dtr[nomeColunaBuscar];

                        if (!object.ReferenceEquals(propVal, DBNull.Value))
                        {
                            try
                            {
                                dynamic propValConvertida = Convert.ChangeType(propVal, propAttr.Key.PropertyType);
                                propAttr.Key.SetValue(retornoItem, propValConvertida, null);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Coluna Busca: {0}, Atributo: {1}, Exceção: {2}", nomeColunaBuscar, propAttr.Key.Name, ex.ToString()));
                            }
                        }
                    }
                }
                retornoFill.Add(retornoItem);
            }
            return retornoFill;
        }

        /// <summary>
        /// Convert uma lista generic em um arquivo CSV
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="path"></param>
        public System.IO.StreamWriter WriteCSV<T>(IEnumerable<T> items, string path)
        {
            var writer = new System.IO.StreamWriter(path);
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);
            using (writer)
            {
                writer.WriteLine(string.Join("; ", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join("; ", props.Select(p => p.GetValue(item, null))));
                }
            }

            return writer;
        }
    }
}
