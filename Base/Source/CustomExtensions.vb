'Imports GlmSharp
Imports System.Globalization
Imports System.Runtime.CompilerServices

Public Module CustomExtensions
    ''' <summary>
    ''' Calcula la desviación estándar de cualquier IEnumerable de Double
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    <Extension>
    Public Function StandardDeviation(source As IEnumerable(Of Double)) As Double
        Dim average = source.Average
        Dim sum = source.Sum(Function(v) (v - average) ^ 2)
        Return Math.Sqrt(sum / source.Count)
    End Function

    ''' <summary>
    ''' Calcula la desviación estándar de cualquier IEnumerable, proyectando la lista a un IEnumerable de Double
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    <Extension>
    Public Function StandardDeviation(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Double)) As Double
        Dim list = source.Select(selector)
        Dim average = list.Average
        Dim sum = list.Sum(Function(v) (v - average) ^ 2)
        Return Math.Sqrt(sum / list.Count)
    End Function

    '''' <summary>
    '''' Convierte un IEnumerable de dvec2 a String. Las componentes se separan por comas y los distintos puntos por punto y coma.
    '''' </summary>
    '''' <param name="source"></param>
    '''' <returns></returns>
    '<Extension>
    'Public Function AsString(source As IEnumerable(Of dvec2)) As String
    '    Dim text As String = String.Empty
    '    For Each v In source
    '        text &= v.ToString(",", CultureInfo.InvariantCulture) & ";"
    '    Next
    '    Return text
    'End Function

    '''' <summary>
    '''' Convierte un IEnumerable de dvec3 a String. Las componentes se separan por comas y los distintos puntos por punto y coma.
    '''' </summary>
    '''' <param name="source"></param>
    '''' <returns></returns>
    '<Extension>
    'Public Function AsString(source As IEnumerable(Of dvec3)) As String
    '    Dim text As String = String.Empty
    '    For Each v In source
    '        text &= v.ToString(",", CultureInfo.InvariantCulture) & ";"
    '    Next
    '    Return text
    'End Function

    '''' <summary>
    '''' Convierte un IEnumerable de dvec4 a String. Las componentes se separan por comas y los distintos puntos por punto y coma.
    '''' </summary>
    '''' <param name="source"></param>
    '''' <returns></returns>
    '<Extension>
    'Public Function AsString(source As IEnumerable(Of dvec4)) As String
    '    Dim text As String = String.Empty
    '    For Each v In source
    '        text &= v.ToString(",", CultureInfo.InvariantCulture) & ";"
    '    Next
    '    Return text
    'End Function

    '''' <summary>
    '''' Convierte un String a una lista de dvec2. Las componentes deben estar separadas por comas y los distintos puntos por punto y coma.
    '''' </summary>
    '''' <param name="source"></param>
    '''' <returns></returns>
    '<Extension>
    'Public Function ToListOfdvec2(source As String) As List(Of dvec2)
    '    Dim vecs = source.Split({";"c}, StringSplitOptions.RemoveEmptyEntries)
    '    Dim list As New List(Of dvec2)
    '    For Each item In vecs
    '        Dim values = item.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
    '        Dim v As New dvec2(Double.Parse(values(0), CultureInfo.InvariantCulture),
    '                          Double.Parse(values(1), CultureInfo.InvariantCulture))
    '        list.Add(v)
    '    Next
    '    Return list
    'End Function

    '''' <summary>
    '''' Convierte un String a una lista de dvec3. Las componentes deben estar separadas por comas y los distintos puntos por punto y coma.
    '''' </summary>
    '''' <param name="source"></param>
    '''' <returns></returns>
    '<Extension>
    'Public Function ToListOfdvec3(source As String) As List(Of dvec3)
    '    Dim vecs = source.Split({";"c}, StringSplitOptions.RemoveEmptyEntries)
    '    Dim list As New List(Of dvec3)
    '    For Each item In vecs
    '        Dim values = item.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
    '        Dim v As New dvec3(Double.Parse(values(0), CultureInfo.InvariantCulture),
    '                          Double.Parse(values(1), CultureInfo.InvariantCulture),
    '                          Double.Parse(values(2), CultureInfo.InvariantCulture))
    '        list.Add(v)
    '    Next
    '    Return list
    'End Function

    '''' <summary>
    '''' Convierte un String a una lista de dvec4. Las componentes deben estar separadas por comas y los distintos puntos por punto y coma.
    '''' </summary>
    '''' <param name="source"></param>
    '''' <returns></returns>
    '<Extension>
    'Public Function ToListOfdvec4(source As String) As List(Of dvec4)
    '    Dim vecs = source.Split({";"c}, StringSplitOptions.RemoveEmptyEntries)
    '    Dim list As New List(Of dvec4)
    '    For Each item In vecs
    '        Dim values = item.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
    '        Dim v As New dvec4(Double.Parse(values(0), CultureInfo.InvariantCulture),
    '                          Double.Parse(values(1), CultureInfo.InvariantCulture),
    '                          Double.Parse(values(2), CultureInfo.InvariantCulture),
    '                          Double.Parse(values(3), CultureInfo.InvariantCulture))
    '        list.Add(v)
    '    Next
    '    Return list
    'End Function

End Module
