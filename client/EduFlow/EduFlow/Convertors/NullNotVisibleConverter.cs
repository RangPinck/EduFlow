using Avalonia.Data.Converters;
using EduFlowApi.DTOs.MaterialDTOs;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace EduFlow.Convertors
{
    public class NullNotVisibleConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is List<MaterialDTO> materials)
            {
                materials = value as List<MaterialDTO>;

                return materials.Count > 0;
            }

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
