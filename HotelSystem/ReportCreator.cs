using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace HotelSystem
{
    public class ReportCreator
    {
        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            IEnumerable<PropertyDescriptor> propertyList = props.Cast<PropertyDescriptor>();
            var header = propertyList.Select(p => p.DisplayName);
            var headerString = string.Join("\t", header);

            //foreach (PropertyDescriptor prop in props)
            //{
                

            //    output.Write(prop.DisplayName); // header
            //    output.Write("\t"); // too many tabs
            //}

            output.WriteLine(headerString);



            foreach (T item in data)
            {
                var value = propertyList.Select(p => p.Converter.ConvertToString(p.GetValue(item)));
                var valueList = string.Join("\t", value);

                output.WriteLine(valueList);
            }


            //foreach (T item in data)
            //{
            //    foreach (PropertyDescriptor prop in props)
            //    {
            //        output.Write(prop.Converter.ConvertToString(prop.GetValue(item)));
            //        output.Write("\t"); // too many tabs
            //    }
            //    output.WriteLine();
            //}
        }
    }
}
